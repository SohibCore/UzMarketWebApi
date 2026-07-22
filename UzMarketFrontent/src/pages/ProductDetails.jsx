import React, { useState, useEffect } from 'react';
import { api } from '../services/api';

export default function ProductDetails({ productId, onAddToCart, onBack }) {
  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [quantity, setQuantity] = useState(1);
  const [activeImage, setActiveImage] = useState('');

  useEffect(() => {
    async function fetchProduct() {
      setLoading(true);
      setError('');
      try {
        const data = await api.products.get(productId);
        setProduct(data);
        
        // Set active image
        if (data.tables && data.tables.length > 0) {
          const mainPicObj = data.tables.find(t => t.mainPic) || data.tables[0];
          setActiveImage(mainPicObj.imageUrl);
        } else {
          setActiveImage('https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=600&auto=format&fit=crop&q=60');
        }
      } catch (err) {
        setError('Mahsulot ma\'lumotlarini yuklashda xatolik yuz berdi: ' + err.message);
      } finally {
        setLoading(false);
      }
    }

    if (productId) fetchProduct();
  }, [productId]);

  if (loading) {
    return (
      <div style={{
        display: 'flex',
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        padding: '100px 0',
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
        <p>Mahsulot tafsilotlari yuklanmoqda...</p>
      </div>
    );
  }

  if (error || !product) {
    return (
      <div className="glass-panel" style={{ padding: '40px', margin: '16px', textAlign: 'center' }}>
        <p style={{ color: 'var(--accent-rose)', marginBottom: '16px' }}>{error || 'Mahsulot topilmadi'}</p>
        <button onClick={onBack} className="glow-btn">Orqaga qaytish</button>
      </div>
    );
  }

  const inStock = product.stockQuantity > 0;

  return (
    <div style={{ padding: '0 16px 48px 16px' }} className="fade-in">
      {/* Back CTA */}
      <button 
        onClick={onBack} 
        className="sec-btn"
        style={{
          marginBottom: '24px',
          padding: '8px 16px',
          display: 'flex',
          alignItems: 'center',
          gap: '8px'
        }}
      >
        <svg style={{ width: '18px', height: '18px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
          <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M10 19l-7-7m0 0l7-7m-7 7h18" />
        </svg>
        Orqaga Qaytish
      </button>

      <div style={{
        display: 'grid',
        gridTemplateColumns: '1.2fr 1fr',
        gap: '40px',
        alignItems: 'start'
      }} className="details-grid">
        
        {/* Images Column */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}>
          {/* Main Pic Display */}
          <div 
            className="glass-panel"
            style={{
              padding: '16px',
              borderRadius: 'var(--border-radius-md)',
              overflow: 'hidden',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              backgroundColor: 'rgba(255,255,255,0.02)',
              position: 'relative'
            }}
          >
            <img 
              src={activeImage} 
              alt={product.name}
              style={{
                width: '100%',
                maxHeight: '450px',
                objectFit: 'contain',
                borderRadius: '8px'
              }}
            />
          </div>

          {/* Thumbnails list */}
          {product.tables && product.tables.length > 1 && (
            <div style={{
              display: 'flex',
              gap: '12px',
              overflowX: 'auto',
              paddingBottom: '8px'
            }}>
              {product.tables.map((img, idx) => (
                <div 
                  key={img.id || idx}
                  onClick={() => setActiveImage(img.imageUrl)}
                  style={{
                    width: '70px',
                    height: '70px',
                    borderRadius: '8px',
                    overflow: 'hidden',
                    cursor: 'pointer',
                    border: activeImage === img.imageUrl 
                      ? '2px solid var(--accent-indigo)' 
                      : '1px solid var(--border-color)',
                    opacity: activeImage === img.imageUrl ? 1 : 0.7,
                    transition: 'var(--transition-fast)',
                    backgroundColor: 'rgba(255,255,255,0.02)'
                  }}
                >
                  <img 
                    src={img.imageUrl} 
                    alt={`thumbnail-${idx}`}
                    style={{ width: '100%', height: '100%', objectFit: 'cover' }}
                  />
                </div>
              ))}
            </div>
          )}
        </div>

        {/* Content Column */}
        <div style={{ display: 'flex', flexDirection: 'column', gap: '24px' }}>
          <div>
            {/* Category tag */}
            <span className="badge badge-primary" style={{ marginBottom: '12px' }}>
              {product.categoryId === 1 ? 'Smartfon' : product.categoryId === 2 ? 'Kompyuter' : 'Kategoriya #' + product.categoryId}
            </span>

            <h1 style={{
              fontFamily: 'var(--font-display)',
              fontSize: '2rem',
              fontWeight: 800,
              lineHeight: '1.3',
              color: 'var(--text-main)',
              marginBottom: '12px'
            }}>
              {product.name}
            </h1>

            {/* Reviews and Ratings mock */}
            <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
              <div style={{ display: 'flex', color: '#ffb830' }}>
                {'★★★★★'.split('').map((s, i) => (
                  <span key={i} style={{ fontSize: '1.1rem' }}>{s}</span>
                ))}
              </div>
              <span style={{ fontSize: '0.85rem', color: 'var(--text-muted)' }}>(14 ta xaridor bahosi)</span>
            </div>
          </div>

          {/* Price Container */}
          <div 
            className="glass-panel"
            style={{
              padding: '20px 24px',
              backgroundColor: 'var(--bg-secondary)',
              border: '1px solid var(--border-color)',
              display: 'flex',
              flexDirection: 'column',
              gap: '4px'
            }}
          >
            <span style={{ fontSize: '0.8rem', color: 'var(--text-muted)' }}>Mahsulot narxi:</span>
            <span style={{ fontSize: '1.8rem', fontWeight: 800, color: 'var(--accent-teal)', fontFamily: 'var(--font-display)' }}>
              {product.price.toLocaleString('uz-UZ')} UZS
            </span>
          </div>

          {/* Description */}
          <div>
            <h3 style={{ fontSize: '1rem', fontWeight: 600, marginBottom: '8px' }}>Mahsulot haqida ma'lumot:</h3>
            <p style={{ color: 'var(--text-muted)', fontSize: '0.95rem', lineHeight: '1.6' }}>
              {product.description || "Ushbu mahsulot haqida batafsil ma'lumot kiritilmagan. UzMarket do'konining kafolatlangan va yuqori sifatli mahsulotlari ro'yxatiga kiradi."}
            </p>
          </div>

          {/* Stock state */}
          <div style={{ display: 'flex', alignItems: 'center', gap: '8px' }}>
            <span style={{ fontSize: '0.9rem', color: 'var(--text-muted)' }}>Holat:</span>
            {inStock ? (
              <span className="badge badge-success" style={{ fontWeight: 600 }}>
                Sotuvda bor ({product.stockQuantity} dona)
              </span>
            ) : (
              <span className="badge badge-danger" style={{ fontWeight: 600 }}>Tugagan</span>
            )}
          </div>

          {/* Quantity selector and Cart action */}
          {inStock && (
            <div style={{ display: 'flex', alignItems: 'center', gap: '16px', marginTop: '12px' }}>
              {/* Quantity */}
              <div 
                style={{
                  display: 'flex',
                  alignItems: 'center',
                  backgroundColor: 'var(--bg-secondary)',
                  border: '1px solid var(--border-color)',
                  borderRadius: '30px',
                  padding: '4px'
                }}
              >
                <button 
                  onClick={() => setQuantity(q => Math.max(1, q - 1))}
                  style={{
                    background: 'transparent',
                    border: 'none',
                    color: 'var(--text-main)',
                    width: '36px',
                    height: '36px',
                    cursor: 'pointer',
                    fontSize: '1.2rem',
                    fontWeight: 700
                  }}
                >
                  -
                </button>
                <span style={{ width: '36px', textAlign: 'center', fontWeight: 600, fontSize: '0.95rem' }}>
                  {quantity}
                </span>
                <button 
                  onClick={() => setQuantity(q => Math.min(product.stockQuantity, q + 1))}
                  style={{
                    background: 'transparent',
                    border: 'none',
                    color: 'var(--text-main)',
                    width: '36px',
                    height: '36px',
                    cursor: 'pointer',
                    fontSize: '1.2rem',
                    fontWeight: 700
                  }}
                >
                  +
                </button>
              </div>

              {/* Add to Cart button */}
              <button 
                onClick={() => {
                  onAddToCart(product, quantity);
                  setQuantity(1); // reset quantity selection
                }}
                className="glow-btn"
                style={{
                  flex: 1,
                  padding: '14px',
                  fontSize: '0.95rem',
                  borderRadius: '30px'
                }}
              >
                <svg style={{ width: '20px', height: '20px', marginRight: '8px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m0 0a2 2 0 100 4 2 2 0 000-4zm-8 2a2 2 0 11-4 0 2 2 0 014 0z" />
                </svg>
                Savatga qo'shish
              </button>
            </div>
          )}
        </div>
      </div>

      <style>{`
        @media (max-width: 800px) {
          .details-grid {
            grid-template-columns: 1fr !important;
            gap: 24px !important;
          }
        }
      `}</style>
    </div>
  );
}
