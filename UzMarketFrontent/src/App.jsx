import React, { useState, useEffect, useRef } from 'react';
import Navbar from './components/Navbar';
import CartDrawer from './components/CartDrawer';
import Home from './pages/Home';
import ProductDetails from './pages/ProductDetails';
import Auth from './pages/Auth';
import AdminPanel from './pages/AdminPanel';
import Orders from './pages/Orders';
import { api } from './services/api';
import './App.css';

function normalizeCartItem(item) {
  return {
    id: item?.id ?? item?.Id ?? 0,
    productId: item?.productId ?? item?.ProductId,
    quantity: Number(item?.quantity ?? item?.Quantity ?? 1),
  };
}

function normalizeCart(cart) {
  if (!cart) return null;
  const items = cart.items || cart.Items || cart.tables || [];

  return {
    id: cart.id ?? cart.Id,
    userId: cart.userId ?? cart.UserId,
    statusId: cart.statusId ?? cart.StatusId,
    items: Array.isArray(items) ? items.map(normalizeCartItem) : [],
  };
}

export default function App() {
  // Navigation & Page routing
  const [activePage, setActivePage] = useState('home'); // home, auth, orders, admin, details, checkout
  const [selectedProductId, setSelectedProductId] = useState(null);
  const [searchQuery, setSearchQuery] = useState('');

  // Authentication State
  const [currentUser, setCurrentUser] = useState(null); // { id, userName, fullName }
  const [checkingAuth, setCheckingAuth] = useState(true);

  // Shop Data States
  const [products, setProducts] = useState([]);
  const [loadingProducts, setLoadingProducts] = useState(true);
  const [activeCart, setActiveCart] = useState(null); // { id, tables: [...] }
  const [cartLoading, setCartLoading] = useState(false);
  const [cartDrawerOpen, setCartDrawerOpen] = useState(false);
  const [isCartMutating, setIsCartMutating] = useState(false);
  const cartInitPromiseRef = useRef(null);
  const cartMutationPromiseRef = useRef(null);

  // 1. Check user auth session on mount
  useEffect(() => {
    let cancelled = false;

    async function initApp() {
      const authTask = (async () => {
        try {
          const user = await api.auth.me();
          if (cancelled) return;

          if (user && user.userId) {
            // Map backend User object properties
            const mappedUser = {
              id: Number(user.userId),
              userName: user.userName,
              fullName: user.fullName
            };
            setCurrentUser(mappedUser);
            await loadUserCart(mappedUser.id);
          }
        } catch (err) {
          if (!cancelled) {
            console.log("No active user session.");
          }
        }
      })();

      const productsTask = loadProducts();
      await Promise.allSettled([authTask, productsTask]);

      if (!cancelled) {
        setCheckingAuth(false);
      }
    }

    initApp();

    return () => {
      cancelled = true;
    };
  }, []);

  // Fetch product catalog
  const loadProducts = async () => {
    setLoadingProducts(true);
    try {
      const data = await api.products.getList();
      if (Array.isArray(data)) {
        setProducts(data);
      }
    } catch (err) {
      console.error("Error loading products:", err);
    } finally {
      setLoadingProducts(false);
    }
  };

  // Fetch or create user cart
  const loadUserCart = async (userId) => {
    if (cartInitPromiseRef.current) {
      return cartInitPromiseRef.current;
    }

    const pendingPromise = (async () => {
      setCartLoading(true);

      try {
        const cartList = await api.cart.getList({ id: userId });
        const userCartList = Array.isArray(cartList)
          ? cartList
              .map(normalizeCart)
              .filter(c => !c?.userId || Number(c.userId) === Number(userId))
              .sort((a, b) => Number(b.id || 0) - Number(a.id || 0))
          : [];

        let cartData = null;

        if (userCartList.length > 0) {
          const fullCart = await api.cart.get(userCartList[0].id);
          cartData = normalizeCart(fullCart) || userCartList[0];
        } else {
          const createRes = await api.cart.create({ userId, items: [] });
          const cartId = typeof createRes === 'object' ? (createRes.id ?? createRes.Id) : Number(createRes);
          cartData = {
            id: cartId,
            items: []
          };
        }

        setActiveCart(cartData);
        return cartData;
      } catch (err) {
        if (err?.status !== 404 && err?.status !== 500) {
          console.error("Error loading user cart:", err);
        }

        try {
          const createRes = await api.cart.create({ userId, items: [] });
          const cartId = typeof createRes === 'object' ? (createRes.id ?? createRes.Id) : Number(createRes);
          const cartData = {
            id: cartId,
            items: []
          };

          setActiveCart(cartData);
          return cartData;
        } catch (createErr) {
          console.error("Error creating user cart:", createErr);
          setActiveCart(null);
          return null;
        }
      } finally {
        setCartLoading(false);
        cartInitPromiseRef.current = null;
      }
    })();

    cartInitPromiseRef.current = pendingPromise;
    return pendingPromise;
  };

  // Login Handler
  const handleLoginSuccess = async (userData) => {
    const mappedUser = {
      id: Number(userData.userId),
      userName: userData.userName,
      fullName: userData.fullName
    };
    setCurrentUser(mappedUser);
    setActivePage('home');
    await loadUserCart(mappedUser.id);
  };

  // Logout Handler
  const handleLogout = async () => {
    try {
      await api.auth.logout();
    } catch (e) {
      console.error("Logout request error", e);
    }
    setCurrentUser(null);
    setActiveCart(null);
    setActivePage('home');
  };

  // Add to Cart Handler
  const handleAddToCart = async (product, qty = 1) => {
    if (!currentUser) {
      alert("Xarid qilish uchun avval tizimga kiring!");
      setActivePage('auth');
      return;
    }

    if (cartMutationPromiseRef.current) {
      return cartMutationPromiseRef.current;
    }

    const pendingMutation = (async () => {
      setIsCartMutating(true);
      try {
      let cart = activeCart;
      if (!cart?.id) {
        cart = await loadUserCart(currentUser.id);
      }

      if (!cart?.id) {
        alert("Savatni yaratib bo‘lmadi. Iltimos, qayta urinib ko‘ring.");
        return null;
      }

      const productId = product.id ?? product.Id;
      if (!productId) {
        alert("Mahsulot ID topilmadi. Sahifani yangilab qayta urinib ko'ring.");
        return null;
      }

      const stockQuantity = Number(product.stockQuantity ?? product.StockQuantity ?? 0);
      const currentItems = (cart.items || cart.tables || []).map(normalizeCartItem);
      const updatedItems = [...currentItems];
      const existingIndex = updatedItems.findIndex(item => Number(item.productId) === Number(productId));

      if (existingIndex > -1) {
        const newQty = (updatedItems[existingIndex].quantity || 1) + qty;
        if (stockQuantity > 0 && newQty > stockQuantity) {
          alert(`Kechirasiz, omborda bor-yo'g'i ${stockQuantity} ta mahsulot mavjud.`);
          return null;
        }
        updatedItems[existingIndex] = {
          ...updatedItems[existingIndex],
          quantity: newQty
        };
      } else {
        updatedItems.push({
          productId,
          quantity: qty
        });
      }

        await api.cart.update({
          id: cart.id,
          items: updatedItems.map(t => ({
            id: t.id || 0,
            productId: t.productId,
            quantity: t.quantity
          }))
        });

        setActiveCart({
          id: cart.id,
          items: updatedItems
        });
        setCartDrawerOpen(true);
        return updatedItems;
      } catch (err) {
        alert("Savatni yangilashda xatolik yuz berdi: " + err.message);
        return null;
      } finally {
        cartMutationPromiseRef.current = null;
        setIsCartMutating(false);
      }
    })();

    cartMutationPromiseRef.current = pendingMutation;
    return pendingMutation;
  };

  // Update Item Quantity in Cart Drawer
  const handleUpdateQty = async (cartItem, newQty) => {
    if (!activeCart) return;

    if (newQty <= 0) {
      handleRemoveItem(cartItem);
      return;
    }

    const currentItems = (activeCart.items || activeCart.tables || []).map(normalizeCartItem);
    const updatedItems = currentItems.map(item => {
      if (Number(item.productId) === Number(cartItem.productId)) {
        return { ...item, quantity: newQty };
      }
      return item;
    });

    try {
      await api.cart.update({
        id: activeCart.id,
        tables: updatedItems.map(t => ({
          id: t.id || null,
          productId: t.productId,
          quantity: t.quantity
        }))
      });

      setActiveCart({
        id: activeCart.id,
        items: updatedItems
      });
    } catch (err) {
      alert("Savatni yangilashda xatolik: " + err.message);
    }
  };

  // Remove Item from Cart
  const handleRemoveItem = async (cartItem) => {
    if (!activeCart) return;

    const currentItems = (activeCart.items || activeCart.tables || []).map(normalizeCartItem);
    const updatedItems = currentItems.filter(item => Number(item.productId) !== Number(cartItem.productId));

    try {
      await api.cart.update({
        id: activeCart.id,
        tables: updatedItems.map(t => ({
          id: t.id || null,
          productId: t.productId,
          quantity: t.quantity
        }))
      });

      setActiveCart({
        id: activeCart.id,
        items: updatedItems
      });
    } catch (err) {
      alert("Savatdan o'chirishda xatolik: " + err.message);
    }
  };

  // Clear Cart completely
  const handleClearCart = async () => {
    if (!activeCart) return;

    try {
      await api.cart.update({
        id: activeCart.id,
        tables: []
      });

      setActiveCart({
        id: activeCart.id,
        items: []
      });
    } catch (err) {
      alert("Savatni tozalashda xatolik: " + err.message);
    }
  };

  // Callback when checkout succeeds
  const handleOrderPlaced = async () => {
    await handleClearCart();
    setActivePage('orders');
  };

  // Router Swap rendering
  const renderPage = () => {
    if (checkingAuth) {
      return (
        <div style={{ textAlign: 'center', padding: '100px 0', color: 'var(--text-muted)' }}>
          Yuklanmoqda...
        </div>
      );
    }

    switch (activePage) {
      case 'home':
        return (
          <Home 
            products={products}
            loading={loadingProducts}
            searchQuery={searchQuery}
            onAddToCart={handleAddToCart}
            onSelectProduct={(id) => {
              setSelectedProductId(id);
              setActivePage('details');
            }}
          />
        );
      case 'details':
        return (
          <ProductDetails 
            productId={selectedProductId}
            onAddToCart={handleAddToCart}
            onBack={() => setActivePage('home')}
          />
        );
      case 'auth':
        return (
          <Auth 
            onLoginSuccess={handleLoginSuccess}
          />
        );
      case 'admin':
        return (
          <AdminPanel 
            products={products}
            onRefreshProducts={loadProducts}
          />
        );
      case 'orders':
        return (
          <Orders 
            viewType="history"
            products={products}
            cartItems={activeCart ? (activeCart.items || activeCart.tables || []) : []}
            onNavigate={setActivePage}
          />
        );
      case 'checkout':
        return (
          <Orders 
            viewType="checkout"
            products={products}
            cartItems={activeCart ? (activeCart.items || activeCart.tables || []) : []}
            onOrderPlaced={handleOrderPlaced}
            onNavigate={setActivePage}
          />
        );
      default:
        return <div>Sahifa topilmadi</div>;
    }
  };

  const cartItemsList = activeCart ? (activeCart.items || activeCart.tables || []) : [];
  const cartItemsCount = cartItemsList.reduce((sum, item) => sum + (item.quantity || 0), 0);

  return (
    <div style={{ minHeight: '100vh', display: 'flex', flexDirection: 'column' }}>
      {/* Header Navigation */}
      <Navbar 
        user={currentUser}
        cartCount={cartItemsCount}
        onCartToggle={() => setCartDrawerOpen(!cartDrawerOpen)}
        onNavigate={(page) => {
          setActivePage(page);
          setSearchQuery(''); // reset search when navigating pages
        }}
        activePage={activePage}
        searchQuery={searchQuery}
        onSearchChange={setSearchQuery}
        onLogout={handleLogout}
      />

      {/* Main Content Area */}
      <main style={{ flex: 1, maxWidth: '1200px', width: '100%', margin: '0 auto', paddingBottom: '40px' }}>
        {renderPage()}
      </main>

      {/* Footer */}
      <footer style={{
        padding: '24px',
        borderTop: '1px solid var(--border-color)',
        textAlign: 'center',
        color: 'var(--text-muted)',
        fontSize: '0.85rem',
        marginTop: 'auto',
        background: 'var(--bg-secondary)'
      }}>
        © 2026 UzMarket. Barcha huquqlar himoyalangan.
      </footer>

      {/* Slide-out Cart Drawer Overlay */}
      <CartDrawer 
        isOpen={cartDrawerOpen}
        onClose={() => setCartDrawerOpen(false)}
        cartItems={cartItemsList}
        products={products}
        onUpdateQty={handleUpdateQty}
        onRemoveItem={handleRemoveItem}
        onClearCart={handleClearCart}
        onCheckout={() => {
          setCartDrawerOpen(false);
          setActivePage('checkout');
        }}
      />
    </div>
  );
}
