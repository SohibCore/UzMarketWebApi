import React from 'react';

export default function CartDrawer({
  isOpen,
  onClose,
  cartItems,
  products,
  onUpdateQty,
  onRemoveItem,
  onCheckout,
  onClearCart
}) {
  if (!isOpen) return null;

  // Map cart item ProductId to rich product data
  const richItems = cartItems.map(item => {
    const prod = products.find(p => p.id === item.productId);
    return {
      ...item,
      name: prod ? prod.name : `Mahsulot #${item.productId}`,
      price: prod ? prod.price : 0,
      stock: prod ? prod.stockQuantity : 0,
      image: prod && prod.tables && prod.tables.length > 0 
        ? prod.tables.find(t => t.mainPic)?.imageUrl || prod.tables[0].imageUrl
        : 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=200&auto=format&fit=crop&q=60'
    };
  });

  const totalPrice = richItems.reduce((sum, item) => sum + (item.price * item.quantity), 0);

  return (
    <div style={{
      position: 'fixed',
      top: 0,
      left: 0,
      width: '100vw',
      height: '100vh',
      backgroundColor: 'rgba(0,0,0,0.6)',
      backdropFilter: 'blur(4px)',
      zIndex: 1000,
      display: 'flex',
      justifyContent: 'flex-end',
      animation: 'fadeIn 0.2s ease-out'
    }}>
      {/* Click outside to close */}
      <div 
        onClick={onClose} 
        style={{ flex: 1 }} 
      />

      {/* Drawer content */}
      <div 
        className="glass-panel" 
        style={{
          width: '100%',
          maxWidth: '460px',
          height: '100%',
          borderRadius: 0,
          borderLeft: '1px solid var(--border-color)',
          display: 'flex',
          flexDirection: 'column',
          boxShadow: 'var(--shadow-lg)',
          animation: 'slideInRight 0.3s cubic-bezier(0.16, 1, 0.3, 1)',
          background: 'var(--bg-secondary)'
        }}
      >
        {/* Drawer Header */}
        <div style={{
          padding: '24px',
          borderBottom: '1px solid var(--border-color)',
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between'
        }}>
          <h2 style={{ fontFamily: 'var(--font-display)', fontSize: '1.4rem' }}>Xarid Savati</h2>
          <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
            {cartItems.length > 0 && (
              <button 
                onClick={onClearCart}
                style={{
                  background: 'transparent',
                  border: 'none',
                  color: '#ff7675',
                  fontSize: '0.85rem',
                  cursor: 'pointer',
                  fontWeight: 500
                }}
              >
                Bo'shatish
              </button>
            )}
            <button 
              onClick={onClose}
              className="sec-btn"
              style={{ padding: '8px', borderRadius: '50%' }}
            >
              <svg style={{ width: '20px', height: '20px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M6 18L18 6M6 6l12 12" />
              </svg>
            </button>
          </div>
        </div>

        {/* Drawer Items List */}
        <div style={{
          flex: 1,
          overflowY: 'auto',
          padding: '24px',
          display: 'flex',
          flexDirection: 'column',
          gap: '16px'
        }}>
          {richItems.length === 0 ? (
            <div style={{
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              justifyContent: 'center',
              height: '100%',
              color: 'var(--text-muted)',
              gap: '16px'
            }}>
              <svg style={{ width: '64px', height: '64px', opacity: 0.3 }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M16 11V7a4 4 0 00-8 0v4M5 9h14l1 12H4L5 9z" />
              </svg>
              <p style={{ fontSize: '1.05rem', fontWeight: 500 }}>Sizning savatingiz bo'sh.</p>
              <button 
                onClick={onClose} 
                className="glow-btn"
                style={{ padding: '10px 20px', fontSize: '0.9rem' }}
              >
                Xarid qilish
              </button>
            </div>
          ) : (
            richItems.map((item) => (
              <div 
                key={item.id || item.productId}
                style={{
                  display: 'flex',
                  gap: '16px',
                  padding: '16px',
                  borderRadius: 'var(--border-radius-sm)',
                  backgroundColor: 'var(--bg-tertiary)',
                  border: '1px solid var(--border-color)',
                  position: 'relative'
                }}
              >
                {/* Product image */}
                <img 
                  src={item.image} 
                  alt={item.name}
                  style={{
                    width: '70px',
                    height: '70px',
                    objectFit: 'cover',
                    borderRadius: '8px',
                    backgroundColor: 'rgba(255,255,255,0.05)'
                  }}
                />

                {/* Info and adjustments */}
                <div style={{ flex: 1, display: 'flex', flexDirection: 'column', justifyContent: 'space-between' }}>
                  <div>
                    <h4 style={{ fontSize: '0.95rem', fontWeight: 600, color: 'var(--text-main)', marginBottom: '4px' }}>
                      {item.name}
                    </h4>
                    <span style={{ color: 'var(--accent-teal)', fontWeight: 700, fontSize: '0.9rem' }}>
                      {item.price.toLocaleString('uz-UZ')} UZS
                    </span>
                  </div>

                  {/* Quantity manager */}
                  <div style={{ display: 'flex', alignItems: 'center', gap: '12px', marginTop: '8px' }}>
                    <div style={{ 
                      display: 'flex', 
                      alignItems: 'center', 
                      backgroundColor: 'rgba(0,0,0,0.2)', 
                      borderRadius: '16px',
                      border: '1px solid var(--border-color)'
                    }}>
                      <button 
                        onClick={() => onUpdateQty(item, item.quantity - 1)}
                        style={{
                          background: 'transparent',
                          border: 'none',
                          color: 'var(--text-main)',
                          padding: '4px 10px',
                          cursor: 'pointer',
                          fontWeight: 700
                        }}
                      >
                        -
                      </button>
                      <span style={{ fontSize: '0.85rem', fontWeight: 600, minWidth: '24px', textAlign: 'center' }}>
                        {item.quantity}
                      </span>
                      <button 
                        onClick={() => onUpdateQty(item, item.quantity + 1)}
                        disabled={item.quantity >= item.stock}
                        style={{
                          background: 'transparent',
                          border: 'none',
                          color: item.quantity >= item.stock ? 'var(--text-muted)' : 'var(--text-main)',
                          padding: '4px 10px',
                          cursor: item.quantity >= item.stock ? 'not-allowed' : 'pointer',
                          fontWeight: 700
                        }}
                      >
                        +
                      </button>
                    </div>
                    {item.quantity >= item.stock && (
                      <span style={{ fontSize: '0.7rem', color: 'var(--accent-rose)' }}>Limit</span>
                    )}
                  </div>
                </div>

                {/* Delete button */}
                <button
                  onClick={() => onRemoveItem(item)}
                  style={{
                    position: 'absolute',
                    top: '16px',
                    right: '16px',
                    background: 'transparent',
                    border: 'none',
                    color: 'var(--text-muted)',
                    cursor: 'pointer'
                  }}
                  title="O'chirish"
                >
                  <svg style={{ width: '18px', height: '18px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" />
                  </svg>
                </button>
              </div>
            ))
          )}
        </div>

        {/* Drawer Footer Summary */}
        {cartItems.length > 0 && (
          <div style={{
            padding: '24px',
            borderTop: '1px solid var(--border-color)',
            display: 'flex',
            flexDirection: 'column',
            gap: '16px'
          }}>
            <div style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
              <span style={{ color: 'var(--text-muted)', fontSize: '0.95rem' }}>Jami qiymat:</span>
              <span style={{
                fontSize: '1.4rem',
                fontWeight: 800,
                color: 'var(--text-main)',
                fontFamily: 'var(--font-display)'
              }}>
                {totalPrice.toLocaleString('uz-UZ')} UZS
              </span>
            </div>

            <button 
              onClick={onCheckout}
              className="glow-btn"
              style={{
                width: '100%',
                padding: '14px',
                fontSize: '1rem',
                borderRadius: 'var(--border-radius-md)'
              }}
            >
              Xaridni Rasmiylashtirish
            </button>
          </div>
        )}
      </div>
    </div>
  );
}
