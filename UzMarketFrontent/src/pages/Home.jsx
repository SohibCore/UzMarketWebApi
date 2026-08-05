import React, { useState, useEffect } from 'react';
import ProductCard from '../components/ProductCard';
import { api } from '../services/api';

const FALLBACK_CATEGORIES = [
  { id: 0, name: 'Barchasi', icon: '🛍️', parentId: 0 },
  { id: 1, name: 'Elektronika', icon: '📱', parentId: 0 },
  { id: 2, name: 'Smartfonlar', icon: '📱', parentId: 1 },
  { id: 3, name: 'Noutbuklar', icon: '💻', parentId: 1 },
  { id: 4, name: 'Televizorlar', icon: '📺', parentId: 1 },
  { id: 5, name: 'Kompyuter Texnikasi', icon: '💻', parentId: 0 },
  { id: 6, name: 'Maishiy Texnika', icon: '📺', parentId: 0 },
  { id: 7, name: 'Kiyim va Poyabzallar', icon: '👕', parentId: 0 },
  { id: 8, name: 'Kitoblar va Kanselyariya', icon: '📚', parentId: 0 }
];

const normalizeCategories = (rawCategories = []) => {
  const source = Array.isArray(rawCategories) && rawCategories.length ? rawCategories : FALLBACK_CATEGORIES;

  const normalized = source.map((cat, index) => ({
    id: cat.id ?? cat.categoryId ?? index + 1,
    name: cat.name ?? cat.title ?? 'Kategoriya',
    icon: cat.icon ?? '🛍️',
    parentId: cat.parentId ?? cat.parentCategoryId ?? cat.ParentId ?? 0,
    children: []
  }));

  const byId = new Map(normalized.map((cat) => [cat.id, cat]));
  normalized.forEach((cat) => {
    const parentId = Number(cat.parentId || 0);
    if (parentId && byId.has(parentId)) {
      byId.get(parentId).children.push(cat);
    }
  });

  return normalized;
};

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
  const [categoryQuery, setCategoryQuery] = useState('');

  const topLevelCategories = categories.filter((cat) => !cat.parentId || Number(cat.parentId) === 0);
  const selectedCategoryData = categories.find((cat) => cat.id === selectedCategory) || null;
  const selectedParentCategory = selectedCategoryData?.parentId && Number(selectedCategoryData.parentId) !== 0
    ? categories.find((cat) => cat.id === Number(selectedCategoryData.parentId)) || null
    : selectedCategoryData;
  const visibleSubCategories = selectedParentCategory?.children?.filter((child) =>
    child.name.toLowerCase().includes(categoryQuery.toLowerCase())
  ) || [];

  const handleCategorySelect = (categoryId) => {
    setSelectedCategory((current) => (current === categoryId ? 0 : categoryId));
  };

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
          setCategories(normalizeCategories(normalized));
        } else {
          setCategories(normalizeCategories(FALLBACK_CATEGORIES));
        }
      } catch (error) {
        console.warn('Kategoriyalarni yuklashda xatolik, fallback ishlatilmoqda:', error);
        setCategories(normalizeCategories(FALLBACK_CATEGORIES));
      }
    }

    loadCategories();
  }, []);

  useEffect(() => {
    let result = products;

    if (selectedCategory > 0) {
      const selectedIds = new Set();
      const selectedData = categories.find((cat) => cat.id === selectedCategory);

      if (selectedData) {
        if (selectedData.parentId && Number(selectedData.parentId) !== 0) {
          selectedIds.add(selectedData.id);
        } else {
          selectedIds.add(selectedData.id);
          selectedData.children.forEach((child) => selectedIds.add(child.id));
        }
      }

      result = result.filter((p) => selectedIds.has(p.categoryId));
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
  }, [products, selectedCategory, searchQuery, categories]);

  return (
    <div style={{ padding: '0 16px' }} className="fade-in">
      {/* Hero Banner Section */}
      <div 
        className="glass-panel" 
        style={{
          padding: '36px 40px',
          marginBottom: '32px',
          background: 'linear-gradient(135deg, rgba(95, 39, 205, 0.5) 0%, rgba(255, 63, 108, 0.18) 60%, rgba(0, 210, 211, 0.12) 100%)',
          borderRadius: '24px',
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'flex-start',
          gap: '14px',
          position: 'relative',
          overflow: 'hidden',
          boxShadow: '0 20px 60px rgba(95, 39, 205, 0.16)'
        }}
      >
        <div style={{
          position: 'absolute',
          top: '-20%',
          right: '-10%',
          width: '320px',
          height: '320px',
          borderRadius: '50%',
          background: 'var(--accent-teal)',
          opacity: 0.16,
          filter: 'blur(90px)'
        }} />

        <span className="badge badge-primary" style={{ padding: '6px 12px', fontSize: '0.85rem' }}>Yozgi chegirmalar!</span>
        
        <h1 style={{
          fontFamily: 'var(--font-display)',
          fontSize: '2.4rem',
          fontWeight: 800,
          lineHeight: '1.2',
          maxWidth: '620px',
          margin: 0
        }}>
          Tezkor yetkazib berish va <span className="text-gradient">arzon narxlar</span> bilan xarid qiling
        </h1>
        
        <p style={{
          color: 'var(--text-muted)',
          maxWidth: '560px',
          fontSize: '1rem',
          lineHeight: '1.6',
          margin: 0
        }}>
          UzMarket do‘konida eng yaxshi mahsulotlar, kafolat va qulay to‘lovlar bir joyda.
        </p>

        <button 
          onClick={() => {
            const el = document.getElementById('products-section');
            if (el) el.scrollIntoView({ behavior: 'smooth' });
          }}
          className="glow-btn" 
          style={{ marginTop: '6px' }}
        >
          Xaridni boshlash
        </button>
      </div>

      {/* Category Navigation Section */}
      <div style={{ marginBottom: '32px' }}>
        <div style={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: 'space-between',
          gap: '16px',
          marginBottom: '18px',
          flexWrap: 'wrap'
        }}>
          <div style={{ display: 'flex', alignItems: 'center', gap: '10px' }}>
            <h2 style={{
              fontFamily: 'var(--font-display)',
              fontSize: '1.35rem',
              fontWeight: 700,
              margin: 0,
              color: 'white'
            }}>
              Kategoriyalar
            </h2>
          </div>

          <div style={{
            display: 'flex',
            alignItems: 'center',
            gap: '8px',
            padding: '8px 12px',
            borderRadius: '999px',
            border: '1px solid rgba(255,255,255,0.08)',
            background: 'rgba(255,255,255,0.04)',
            minWidth: '280px',
            maxWidth: '360px',
            flex: 1
          }}>
            <svg style={{ width: '18px', height: '18px', color: 'var(--text-muted)' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 21l-4.35-4.35m1.85-5.15a7 7 0 11-14 0 7 7 0 0114 0z" />
            </svg>
            <input
              type="text"
              value={categoryQuery}
              onChange={(e) => setCategoryQuery(e.target.value)}
              placeholder="Kategoriya qidiring"
              className="form-input"
              style={{ border: 'none', background: 'transparent', boxShadow: 'none', padding: 0, minWidth: 0 }}
            />
          </div>
        </div>

        <div style={{
          display: 'flex',
          flexDirection: 'column',
          gap: '10px',
          alignItems: 'flex-start'
        }}>
          <div style={{
            display: 'flex',
            flexWrap: 'wrap',
            gap: '10px',
            justifyContent: 'flex-start'
          }}>
            {topLevelCategories
              .filter((cat) => cat.name.toLowerCase().includes(categoryQuery.toLowerCase()))
              .map((cat) => {
                const isActive = selectedCategory === cat.id;
                return (
                  <button
                    key={cat.id}
                    onClick={() => handleCategorySelect(cat.id)}
                    className="sec-btn"
                    style={{
                      padding: '10px 12px',
                      borderRadius: '999px',
                      display: 'flex',
                      alignItems: 'center',
                      gap: '8px',
                      border: isActive
                        ? '1px solid var(--accent-indigo)'
                        : '1px solid rgba(255,255,255,0.08)',
                      background: isActive
                        ? 'linear-gradient(135deg, rgba(95, 39, 205, 0.22), rgba(0, 210, 211, 0.12))'
                        : 'rgba(255,255,255,0.04)',
                      boxShadow: isActive ? '0 10px 30px rgba(95, 39, 205, 0.18)' : 'none',
                      textAlign: 'left',
                      color: 'white'
                    }}
                  >
                    <span style={{ fontSize: '1rem' }}>{cat.icon}</span>
                    <span style={{ fontWeight: 600, fontSize: '0.9rem' }}>{cat.name}</span>
                    {isActive && (
                      <span
                        onClick={(e) => {
                          e.stopPropagation();
                          setSelectedCategory(0);
                        }}
                        style={{
                          marginLeft: '4px',
                          display: 'inline-flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                          width: '18px',
                          height: '18px',
                          borderRadius: '50%',
                          background: 'rgba(255,255,255,0.14)',
                          color: 'white',
                          fontSize: '0.85rem',
                          lineHeight: 1
                        }}
                        aria-label={`Clear ${cat.name}`}
                      >
                        ×
                      </span>
                    )}
                  </button>
                );
              })}
          </div>

          {visibleSubCategories.length > 0 && (
            <div style={{
              display: 'flex',
              flexWrap: 'wrap',
              gap: '8px',
              justifyContent: 'flex-start'
            }}>
              {visibleSubCategories.map((child) => {
                const isActive = selectedCategory === child.id;
                return (
                  <button
                    key={child.id}
                    onClick={() => handleCategorySelect(child.id)}
                    style={{
                      padding: '8px 10px',
                      borderRadius: '999px',
                      display: 'flex',
                      alignItems: 'center',
                      gap: '6px',
                      border: isActive
                        ? '1px solid var(--accent-indigo)'
                        : '1px solid rgba(255,255,255,0.08)',
                      background: isActive
                        ? 'linear-gradient(135deg, rgba(95, 39, 205, 0.22), rgba(0, 210, 211, 0.12))'
                        : 'rgba(255,255,255,0.03)',
                      color: 'white',
                      fontSize: '0.86rem'
                    }}
                  >
                    <span>{child.icon}</span>
                    <span>{child.name}</span>
                    {isActive && (
                      <span
                        onClick={(e) => {
                          e.stopPropagation();
                          setSelectedCategory(0);
                        }}
                        style={{
                          marginLeft: '4px',
                          display: 'inline-flex',
                          alignItems: 'center',
                          justifyContent: 'center',
                          width: '16px',
                          height: '16px',
                          borderRadius: '50%',
                          background: 'rgba(255,255,255,0.14)',
                          color: 'white',
                          fontSize: '0.8rem',
                          lineHeight: 1
                        }}
                        aria-label={`Clear ${child.name}`}
                      >
                        ×
                      </span>
                    )}
                  </button>
                );
              })}
            </div>
          )}
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

        @keyframes pulseGlow {
          0%, 100% {
            box-shadow: 0 0 0 rgba(99, 102, 241, 0.12), 0 0 0 1px rgba(99, 102, 241, 0.2);
          }
          50% {
            box-shadow: 0 0 18px rgba(37, 99, 235, 0.34), 0 0 0 1px rgba(129, 140, 248, 0.72);
          }
        }
      `}</style>
    </div>
  );
}
