import React, { useState, useEffect } from 'react';
import Navbar from './components/Navbar';
import CartDrawer from './components/CartDrawer';
import Home from './pages/Home';
import ProductDetails from './pages/ProductDetails';
import Auth from './pages/Auth';
import AdminPanel from './pages/AdminPanel';
import Orders from './pages/Orders';
import { api } from './services/api';
import './App.css';

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
    setCartLoading(true);
    try {
      const cartList = await api.cart.getList();
      const userCartList = Array.isArray(cartList)
        ? cartList.filter(c => c.userId === userId)
        : [];

      if (userCartList.length > 0) {
        const fullCart = await api.cart.get(userCartList[0].id);
        setActiveCart({
          id: fullCart.id,
          tables: fullCart.tables || []
        });
      } else {
        const newCart = await api.cart.create({ userId, tables: [] });
        setActiveCart({
          id: newCart.id,
          tables: []
        });
      }
    } catch (err) {
      if (err?.status !== 404 && err?.status !== 500) {
        console.error("Error loading user cart:", err);
      }

      try {
        const newCart = await api.cart.create({ userId, tables: [] });
        setActiveCart({
          id: newCart.id,
          tables: []
        });
      } catch (createErr) {
        console.error("Error creating user cart:", createErr);
      }
    } finally {
      setCartLoading(false);
    }
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

    if (!activeCart) {
      alert("Savat yuklanmoqda, iltimos kuting...");
      return;
    }

    const updatedTables = [...activeCart.tables];
    const existingIndex = updatedTables.findIndex(item => item.productId === product.id);

    if (existingIndex > -1) {
      // Increment quantity
      const newQty = (updatedTables[existingIndex].quantity || 1) + qty;
      if (newQty > product.stockQuantity) {
        alert(`Kechirasiz, omborda bor-yo'g'i ${product.stockQuantity} ta mahsulot mavjud.`);
        return;
      }
      updatedTables[existingIndex].quantity = newQty;
    } else {
      // Add new item
      updatedTables.push({
        productId: product.id,
        quantity: qty
      });
    }

    try {
      const response = await api.cart.update({
        id: activeCart.id,
        tables: updatedTables.map(t => ({
          cartId: activeCart.id,
          productId: t.productId,
          quantity: t.quantity
        }))
      });
      setActiveCart({
        id: response.id,
        tables: response.tables || []
      });
      setCartDrawerOpen(true); // Open drawer to show success
    } catch (err) {
      alert("Savatni yangilashda xatolik yuz berdi: " + err.message);
    }
  };

  // Update Item Quantity in Cart Drawer
  const handleUpdateQty = async (cartItem, newQty) => {
    if (!activeCart) return;

    if (newQty <= 0) {
      handleRemoveItem(cartItem);
      return;
    }

    const updatedTables = activeCart.tables.map(item => {
      if (item.productId === cartItem.productId) {
        return { ...item, quantity: newQty };
      }
      return item;
    });

    try {
      const response = await api.cart.update({
        id: activeCart.id,
        tables: updatedTables.map(t => ({
          cartId: activeCart.id,
          productId: t.productId,
          quantity: t.quantity
        }))
      });
      setActiveCart({
        id: response.id,
        tables: response.tables || []
      });
    } catch (err) {
      alert("Savatni yangilashda xatolik: " + err.message);
    }
  };

  // Remove Item from Cart
  const handleRemoveItem = async (cartItem) => {
    if (!activeCart) return;

    const updatedTables = activeCart.tables.filter(item => item.productId !== cartItem.productId);

    try {
      const response = await api.cart.update({
        id: activeCart.id,
        tables: updatedTables.map(t => ({
          cartId: activeCart.id,
          productId: t.productId,
          quantity: t.quantity
        }))
      });
      setActiveCart({
        id: response.id,
        tables: response.tables || []
      });
    } catch (err) {
      alert("Savatdan o'chirishda xatolik: " + err.message);
    }
  };

  // Clear Cart completely
  const handleClearCart = async () => {
    if (!activeCart) return;

    try {
      const response = await api.cart.update({
        id: activeCart.id,
        tables: []
      });
      setActiveCart({
        id: response.id,
        tables: []
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
            cartItems={activeCart ? activeCart.tables : []}
            onNavigate={setActivePage}
          />
        );
      case 'checkout':
        return (
          <Orders 
            viewType="checkout"
            products={products}
            cartItems={activeCart ? activeCart.tables : []}
            onOrderPlaced={handleOrderPlaced}
            onNavigate={setActivePage}
          />
        );
      default:
        return <div>Sahifa topilmadi</div>;
    }
  };

  const cartItemsCount = activeCart 
    ? activeCart.tables.reduce((sum, item) => sum + (item.quantity || 0), 0) 
    : 0;

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
        cartItems={activeCart ? activeCart.tables : []}
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
