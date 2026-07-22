import React, { useState, useEffect } from 'react';
import ProductCard from '../components/ProductCard';
import { api } from '../services/api';

const FALLBACK_CATEGORIES = [
  { id: 0, name: 'Barchasi', icon: '🛍️' },
  { id: 1, name: 'Smartfonlar va Gadjetlar', icon: '📱' },
  { id: 2, name: 'Kompyuter Texnikasi', icon: '💻' },
  { id: 3, name: 'Maishiy Texnika', icon: '📺' },
  { id: 4, name: 'Kiyim va Poyabzallar', icon: '👕' },
  { id: 5, name: 'Kitoblar va Kanselyariya', icon: '📚' }
];

export default function Home({ 
  products, 
  loading, 
  searchQuery, 
  onAddToCart, 
  onSelectProduct 
}) {
  const [selectedCategory, setSelectedCategory] = useState(0);
  const [filteredProducts, setFilteredProducts] = useState([]);
  const [categories, setCategories] = useState(FALLBACK_CATEGORIES);

  useEffect(() => {
    async function loadCategories() {
      try {
        const data = await api.categories.getList();
        const normalized = Array.isArray(data)
          ? data
          : Array.isArray(data?.items)
            ? data.items
            : [];

        if (normalized.length > 0) {
          setCategories(
            normalized.map((cat) => ({
              id: cat.id ?? cat.categoryId ?? 0,
              name: cat.name ?? cat.title ?? 'Kategoriya',
              icon: cat.icon ?? '🛍️'
            }))
          );
        }
      } catch (error) {
        console.warn('Kategoriyalarni yuklashda xatolik, fallback ishlatilmoqda:', error);
        setCategories(FALLBACK_CATEGORIES);
      }
    }

    loadCategories();
  }, []);

  useEffect(() => {
    let result = products;

    // Filter by category
    if (selectedCategory > 0) {
      result = result.filter(p => p.categoryId === selectedCategory);
    }

    // Filter by search query
    if (searchQuery) {
      const q = searchQuery.toLowerCase();
      result = result.filter(p => 
        p.name.toLowerCase().includes(q) || 
        (p.description && p.description.toLowerCase().includes(q))
      );
    }

    setFilteredProducts(result);
  }, [products, selectedCategory, searchQuery]);

  return (
    <div style={{ padding: '0 16px' }} className="fade-in">
      {/* Hero Banner Section */}
      <div 
        className="glass-panel" 
        style={{
          padding: '48px',
          marginBottom: '32px',
          background: 'linear-gradient(135deg, rgba(95, 39, 205, 0.45) 0%, rgba(255, 63, 108, 0.15) 100%)',
          borderRadius: 'var(--border-radius-lg)',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'flex-start',
          gap: '16px',
          position: 'relative',
          overflow: 'hidden'
        }}
      >
        <div style={{
          position: 'absolute',
          top: '-20%',
          right: '-10%',
          width: '300px',
          height: '300px',
          borderRadius: '50%',
          background: 'var(--accent-teal)',
          opacity: 0.15,
          filter: 'blur(80px)'
        }} />

        <span className="badge badge-primary" style={{ padding: '6px 12px' }}>Yozgi chegirmalar!</span>
        
        <h1 style={{
          fontFamily: 'var(--font-display)',
          fontSize: '2.8rem',
          fontWeight: 800,
          lineHeight: '1.2',
          maxWidth: '600px'
        }}>
          Milliy Bozordagi <span className="text-gradient">Eng Sifatli</span> Mahsulotlar
        </h1>
        
        <p style={{
          color: 'var(--text-muted)',
          maxWidth: '500px',
          fontSize: '1rem',
          lineHeight: '1.6'
        }}>
          UzMarket do'konida qulay to'lovlar, tezkor yetkazib berish xizmati va doimiy kafolat mavjud. Bugunoq o'z xaridingizni amalga oshiring!
        </p>

        <button 
          onClick={() => {
            const el = document.getElementById('products-section');
            if (el) el.scrollIntoView({ behavior: 'smooth' });
          }}
          className="glow-btn" 
          style={{ marginTop: '8px' }}
        >
          Xaridni boshlash
        </button>
      </div>

      {/* Category Navigation Section */}
      <div style={{ marginBottom: '32px' }}>
        <h2 style={{
          fontFamily: 'var(--font-display)',
          fontSize: '1.5rem',
          fontWeight: 700,
          marginBottom: '16px'
        }}>
          Kategoriyalar bo'yicha ko'rish
        </h2>
        
        <div style={{
          display: 'flex',
          gap: '12px',
          overflowX: 'auto',
          paddingBottom: '8px',
          scrollbarWidth: 'none' // Hide default scrollbar
        }}>
          {categories.map((cat) => (
            <button
              key={cat.id}
              onClick={() => setSelectedCategory(cat.id)}
              className="sec-btn"
              style={{
                padding: '10px 20px',
                borderRadius: 'var(--border-radius-lg)',
                whiteSpace: 'nowrap',
                display: 'flex',
                alignItems: 'center',
                gap: '8px',
                border: selectedCategory === cat.id 
                  ? '1px solid var(--accent-indigo)' 
                  : '1px solid var(--border-color)',
                background: selectedCategory === cat.id 
                  ? 'var(--accent-indigo-glow)' 
                  : 'var(--bg-secondary)',
                boxShadow: selectedCategory === cat.id
                  ? '0 4px 12px rgba(95, 39, 205, 0.2)'
                  : 'none'
              }}
            >
              <span>{cat.icon}</span>
              <span style={{ fontWeight: selectedCategory === cat.id ? 600 : 400 }}>{cat.name}</span>
            </button>
          ))}
        </div>
      </div>

      {/* Products Display Section */}
      <div id="products-section" style={{ scrollMarginTop: '100px', marginBottom: '48px' }}>
        <div style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          marginBottom: '24px'
        }}>
          <h2 style={{
            fontFamily: 'var(--font-display)',
            fontSize: '1.5rem',
            fontWeight: 700
          }}>
            {searchQuery ? `Qidiruv natijalari: "${searchQuery}"` : 'Barcha Mahsulotlar'}
          </h2>
          <span style={{ color: 'var(--text-muted)', fontSize: '0.9rem' }}>
            {filteredProducts.length} ta mahsulot topildi
          </span>
        </div>

        {loading ? (
          <div style={{
            display: 'flex',
            flexDirection: 'column',
            alignItems: 'center',
            justifyContent: 'center',
            padding: '80px 0',
            gap: '16px',
            color: 'var(--text-muted)'
          }}>
            <div style={{
              width: '40px',
              height: '40px',
              border: '3px solid rgba(255,255,255,0.05)',
              borderTopColor: 'var(--accent-indigo)',
              borderRadius: '50%',
              animation: 'spin 1s linear infinite'
            }} />
            <p>Mahsulotlar yuklanmoqda...</p>
          </div>
        ) : filteredProducts.length === 0 ? (
          <div 
            className="glass-panel"
            style={{
              padding: '64px 24px',
              textAlign: 'center',
              color: 'var(--text-muted)',
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
              justifyContent: 'center',
              gap: '16px'
            }}
          >
            <svg style={{ width: '48px', height: '48px', opacity: 0.4 }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M9.172 16.172a4 4 0 015.656 0M9 10h.01M15 10h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
            </svg>
            <p style={{ fontSize: '1.1rem', fontWeight: 500 }}>Hech qanday mahsulot topilmadi.</p>
            {(selectedCategory > 0 || searchQuery) && (
              <button 
                onClick={() => { setSelectedCategory(0); }} 
                className="sec-btn"
                style={{ padding: '8px 16px', fontSize: '0.85rem' }}
              >
                Filtrlarni tozalash
              </button>
            )}
          </div>
        ) : (
          <div style={{
            display: 'grid',
            gridTemplateColumns: 'repeat(auto-fill, minmax(260px, 1fr))',
            gap: '24px'
          }}>
            {filteredProducts.map((product) => (
              <ProductCard 
                key={product.id} 
                product={product} 
                onAddToCart={onAddToCart} 
                onSelect={onSelectProduct}
              />
            ))}
          </div>
        )}
      </div>

      <style>{`
        @keyframes spin {
          to { transform: rotate(360deg); }
        }
      `}</style>
    </div>
  );
}
