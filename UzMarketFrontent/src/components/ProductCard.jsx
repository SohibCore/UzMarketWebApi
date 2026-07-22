import React from 'react';

export default function ProductCard({ product, onAddToCart, onSelect }) {
  // Find main pic or fallback
  const mainImage = product.tables && product.tables.length > 0
    ? product.tables.find(t => t.mainPic)?.imageUrl || product.tables[0].imageUrl
    : 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=400&auto=format&fit=crop&q=60';

  const inStock = product.stockQuantity > 0;

  return (
    <div 
      className="glass-panel"
      style={{
        display: 'flex',
        flexDirection: 'column',
        height: '100%',
        overflow: 'hidden',
        transition: 'transform var(--transition-fast), box-shadow var(--transition-fast)',
        cursor: 'pointer',
        position: 'relative'
      }}
      onClick={() => onSelect(product.id)}
      onMouseEnter={(e) => {
        e.currentTarget.style.transform = 'translateY(-6px)';
        e.currentTarget.style.boxShadow = 'var(--shadow-lg), 0 0 20px rgba(95, 39, 205, 0.15)';
        e.currentTarget.style.borderColor = 'rgba(95, 39, 205, 0.3)';
      }}
      onMouseLeave={(e) => {
        e.currentTarget.style.transform = 'translateY(0)';
        e.currentTarget.style.boxShadow = 'var(--shadow-md)';
        e.currentTarget.style.borderColor = 'var(--border-color)';
      }}
    >
      {/* Product Image Wrapper */}
      <div style={{ position: 'relative', width: '100%', paddingTop: '80%', overflow: 'hidden', backgroundColor: 'rgba(255,255,255,0.02)' }}>
        <img 
          src={mainImage} 
          alt={product.name}
          style={{
            position: 'absolute',
            top: 0,
            left: 0,
            width: '100%',
            height: '100%',
            objectFit: 'cover',
            transition: 'transform 0.5s ease'
          }}
          onMouseEnter={(e) => {
            e.currentTarget.style.transform = 'scale(1.08)';
          }}
          onMouseLeave={(e) => {
            e.currentTarget.style.transform = 'scale(1.0)';
          }}
        />
        
        {/* Availability Badge */}
        <div style={{ position: 'absolute', top: '12px', left: '12px' }}>
          {inStock ? (
            <span className="badge badge-success">Mavjud</span>
          ) : (
            <span className="badge badge-danger">Tugagan</span>
          )}
        </div>
      </div>

      {/* Product Info */}
      <div style={{ padding: '16px', display: 'flex', flexDirection: 'column', flex: 1 }}>
        <h3 style={{
          fontSize: '1rem',
          fontWeight: 600,
          color: 'var(--text-main)',
          marginBottom: '8px',
          display: '-webkit-box',
          WebkitLineClamp: 2,
          WebkitBoxOrient: 'vertical',
          overflow: 'hidden',
          textOverflow: 'ellipsis',
          lineHeight: '1.4',
          height: '2.8em' // fix height for alignments
        }}>
          {product.name}
        </h3>

        <p style={{
          fontSize: '0.8rem',
          color: 'var(--text-muted)',
          marginBottom: '12px',
          display: '-webkit-box',
          WebkitLineClamp: 2,
          WebkitBoxOrient: 'vertical',
          overflow: 'hidden',
          textOverflow: 'ellipsis',
          height: '2.8em',
          lineHeight: '1.4'
        }}>
          {product.description || "Tavsif mavjud emas."}
        </p>

        {/* Pricing & Cart Action at bottom */}
        <div style={{ marginTop: 'auto', display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: '8px' }}>
          <div style={{ display: 'flex', flexDirection: 'column' }}>
            <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>Narxi</span>
            <span style={{ fontSize: '1.15rem', fontWeight: 800, color: 'var(--accent-teal)' }}>
              {product.price.toLocaleString('uz-UZ')} UZS
            </span>
          </div>

          <button
            onClick={(e) => {
              e.stopPropagation(); // prevent card click select trigger
              if (inStock) onAddToCart(product);
            }}
            disabled={!inStock}
            className={inStock ? "glow-btn" : "sec-btn"}
            style={{
              padding: '8px 12px',
              borderRadius: 'var(--border-radius-sm)',
              fontSize: '0.8rem',
              cursor: inStock ? 'pointer' : 'not-allowed',
              opacity: inStock ? 1 : 0.6
            }}
          >
            <svg style={{ width: '16px', height: '16px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M12 4v16m8-8H4" />
            </svg>
          </button>
        </div>
      </div>
    </div>
  );
}
