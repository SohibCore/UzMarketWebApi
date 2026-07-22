import React from 'react';

export default function Navbar({
  user,
  cartCount,
  onCartToggle,
  onNavigate,
  activePage,
  searchQuery,
  onSearchChange,
  onLogout
}) {
  return (
    <nav className="glass-panel" style={{
      position: 'sticky',
      top: '16px',
      margin: '0 16px 24px 16px',
      zIndex: 100,
      padding: '12px 24px',
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'space-between',
      gap: '20px',
      borderRadius: 'var(--border-radius-lg)',
      background: 'rgba(11, 15, 25, 0.7)',
      backdropFilter: 'blur(20px)',
      boxShadow: '0 8px 32px 0 rgba(0, 0, 0, 0.37)'
    }}>
      {/* Brand logo */}
      <div 
        onClick={() => onNavigate('home')} 
        style={{ 
          cursor: 'pointer',
          display: 'flex',
          alignItems: 'center',
          gap: '8px'
        }}
      >
        <span style={{
          fontSize: '1.8rem',
          fontWeight: 800,
          fontFamily: 'var(--font-display)',
          letterSpacing: '-1px'
        }} className="text-gradient">
          UzMarket
        </span>
      </div>

      {/* Search Bar - only show on home page */}
      {activePage === 'home' && (
        <div style={{ flex: '1', maxWidth: '500px', position: 'relative' }}>
          <input 
            type="text" 
            placeholder="Mahsulotlarni qidirish..." 
            value={searchQuery}
            onChange={(e) => onSearchChange(e.target.value)}
            style={{
              width: '100%',
              padding: '10px 16px 10px 42px',
              backgroundColor: 'rgba(255, 255, 255, 0.05)',
              border: '1px solid var(--border-color)',
              borderRadius: 'var(--border-radius-lg)',
              color: 'var(--text-main)',
              fontSize: '0.9rem',
              transition: 'var(--transition-fast)'
            }}
            className="search-input"
          />
          <svg 
            style={{
              position: 'absolute',
              left: '14px',
              top: '50%',
              transform: 'translateY(-50%)',
              width: '18px',
              height: '18px',
              color: 'var(--text-muted)'
            }}
            fill="none" 
            stroke="currentColor" 
            viewBox="0 0 24 24"
          >
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
          </svg>
        </div>
      )}

      {/* Navigation Options */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
        <button 
          onClick={() => onNavigate('home')}
          className="sec-btn"
          style={{
            padding: '8px 16px',
            fontSize: '0.9rem',
            border: activePage === 'home' ? '1px solid var(--accent-indigo)' : '1px solid transparent',
            background: activePage === 'home' ? 'var(--accent-indigo-glow)' : 'transparent'
          }}
        >
          Do'kon
        </button>

        {user ? (
          <>
            <button 
              onClick={() => onNavigate('orders')}
              className="sec-btn"
              style={{
                padding: '8px 16px',
                fontSize: '0.9rem',
                border: activePage === 'orders' ? '1px solid var(--accent-indigo)' : '1px solid transparent',
                background: activePage === 'orders' ? 'var(--accent-indigo-glow)' : 'transparent'
              }}
            >
              Buyurtmalarim
            </button>
            <button 
              onClick={() => onNavigate('admin')}
              className="sec-btn"
              style={{
                padding: '8px 16px',
                fontSize: '0.9rem',
                border: activePage === 'admin' ? '1px solid var(--accent-rose)' : '1px solid transparent',
                background: activePage === 'admin' ? 'var(--accent-rose-glow)' : 'transparent'
              }}
            >
              Sotuvchi Paneli
            </button>
          </>
        ) : null}
      </div>

      {/* User Actions & Cart */}
      <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
        {/* Shopping Cart button with badge count */}
        <button 
          onClick={onCartToggle}
          className="sec-btn"
          style={{
            position: 'relative',
            padding: '10px 18px',
            borderRadius: 'var(--border-radius-lg)',
            display: 'flex',
            alignItems: 'center',
            gap: '8px',
            background: 'var(--bg-tertiary)'
          }}
        >
          <svg style={{ width: '20px', height: '20px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
          </svg>
          <span style={{ fontSize: '0.9rem', fontWeight: 600 }}>Savat</span>
          {cartCount > 0 && (
            <span style={{
              position: 'absolute',
              top: '-6px',
              right: '-6px',
              background: 'var(--accent-rose)',
              color: 'white',
              borderRadius: '50%',
              width: '20px',
              height: '20px',
              fontSize: '0.75rem',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              fontWeight: 700,
              boxShadow: '0 0 10px rgba(255, 63, 108, 0.6)'
            }}>
              {cartCount}
            </span>
          )}
        </button>

        {/* User Auth Info / Button */}
        {user ? (
          <div style={{ display: 'flex', alignItems: 'center', gap: '12px' }}>
            <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'flex-end' }}>
              <span style={{ fontSize: '0.85rem', fontWeight: 600 }}>{user.fullName || user.userName}</span>
              <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>@{user.userName}</span>
            </div>
            <button 
              onClick={onLogout}
              className="sec-btn"
              style={{
                padding: '8px 12px',
                borderRadius: '50%',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center'
              }}
              title="Chiqish"
            >
              <svg style={{ width: '18px', height: '18px', color: '#ff7675' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M17 16l4-4m0 0l-4-4m4 4H7m6 4v1a3 3 0 01-3 3H6a3 3 0 01-3-3V7a3 3 0 013-3h4a3 3 0 013 3v1" />
              </svg>
            </button>
          </div>
        ) : (
          <button 
            onClick={() => onNavigate('auth')}
            className="glow-btn"
            style={{ padding: '10px 20px', fontSize: '0.9rem' }}
          >
            Kirish / Ro'yxatdan O'tish
          </button>
        )}
      </div>
    </nav>
  );
}
