import React, { useState, useEffect } from 'react';
import { api } from '../services/api';

export default function Orders({ 
  viewType, // 'history' or 'checkout'
  cartItems, 
  products, 
  onOrderPlaced, 
  onNavigate 
}) {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  // Checkout Form States
  const [addressForm, setAddressForm] = useState({
    region: '',
    city: '',
    street: '',
    postalCode: '',
    isDefault: true
  });
  const [submitting, setSubmitting] = useState(false);

  // Load Order History
  useEffect(() => {
    async function loadOrders() {
      if (viewType !== 'history') return;
      setLoading(true);
      setError('');
      try {
        const data = await api.orders.getList();
        // The API returns an array, sort by ID descending (newest first)
        if (Array.isArray(data)) {
          setOrders(data.sort((a, b) => b.id - a.id));
        } else {
          setOrders([]);
        }
      } catch (err) {
        // If 404, might mean no orders found, which is fine
        if (err.status === 404) {
          setOrders([]);
        } else {
          setError('Buyurtmalarni yuklashda xatolik: ' + err.message);
        }
      } finally {
        setLoading(false);
      }
    }
    loadOrders();
  }, [viewType]);

  // Handle Checkout Submit
  const handleCheckoutSubmit = async (e) => {
    e.preventDefault();
    if (!addressForm.region || !addressForm.city || !addressForm.street || !addressForm.postalCode) {
      alert('Iltimos yetkazib berish ma\'lumotlarini to\'liq kiriting.');
      return;
    }

    setSubmitting(true);
    setError('');

    try {
      const createdAddressId = await api.addresses.create({
        region: addressForm.region,
        city: addressForm.city,
        street: addressForm.street,
        postalCode: addressForm.postalCode,
        isDefault: addressForm.isDefault
      });

      const tables = cartItems.map(item => {
        const prod = products.find(p => p.id === item.productId);
        return {
          productId: item.productId,
          quantity: item.quantity,
          price: prod ? prod.price : 0
        };
      });

      const today = new Date().toISOString().split('T')[0];
      const orderDto = {
        orderDate: today,
        shippingAddressId: Number(createdAddressId),
        tables
      };

      await api.orders.create(orderDto);
      alert('Buyurtmangiz muvaffaqiyatli qabul qilindi!');
      onOrderPlaced();
    } catch (err) {
      alert('Buyurtma berishda xatolik yuz berdi: ' + err.message);
    } finally {
      setSubmitting(false);
    }
  };

  // Calculate Cart Totals
  const getCartTotal = () => {
    return cartItems.reduce((sum, item) => {
      const prod = products.find(p => p.id === item.productId);
      return sum + ((prod ? prod.price : 0) * item.quantity);
    }, 0);
  };

  if (viewType === 'checkout') {
    const totalPrice = getCartTotal();

    return (
      <div style={{ padding: '0 16px 48px 16px', maxWidth: '800px', margin: '0 auto' }} className="fade-in">
        <h1 style={{ fontFamily: 'var(--font-display)', fontSize: '2rem', fontWeight: 800, marginBottom: '24px' }}>
          Buyurtmani Rasmiylashtirish
        </h1>

        <div style={{ display: 'grid', gridTemplateColumns: '1.2fr 1fr', gap: '32px' }} className="checkout-grid">
          {/* Checkout Form */}
          <div className="glass-panel" style={{ padding: '32px' }}>
            <h2 style={{ fontFamily: 'var(--font-display)', fontSize: '1.2rem', marginBottom: '20px' }}>Yetkazib berish ma'lumotlari</h2>
            
            <form onSubmit={handleCheckoutSubmit}>
              <div className="form-group">
                <label>Viloyat*</label>
                <input 
                  type="text" 
                  className="form-input" 
                  value={addressForm.region}
                  onChange={(e) => setAddressForm(prev => ({ ...prev, region: e.target.value }))}
                  placeholder="Masalan: Toshkent viloyati"
                  required
                />
              </div>

              <div className="form-group">
                <label>Shahar / tumani*</label>
                <input 
                  type="text" 
                  className="form-input" 
                  value={addressForm.city}
                  onChange={(e) => setAddressForm(prev => ({ ...prev, city: e.target.value }))}
                  placeholder="Masalan: Toshkent"
                  required
                />
              </div>

              <div className="form-group">
                <label>Ko'cha / uy / xona*</label>
                <input 
                  type="text" 
                  className="form-input" 
                  value={addressForm.street}
                  onChange={(e) => setAddressForm(prev => ({ ...prev, street: e.target.value }))}
                  placeholder="Masalan: Chilonzor 12-uy"
                  required
                />
              </div>

              <div className="form-group">
                <label>Pocht kod</label>
                <input 
                  type="text" 
                  className="form-input" 
                  value={addressForm.postalCode}
                  onChange={(e) => setAddressForm(prev => ({ ...prev, postalCode: e.target.value }))}
                  placeholder="100000"
                />
              </div>

              <div className="form-group">
                <label>Qo'shimcha izohlar (ixtiyoriy)</label>
                <textarea 
                  className="form-input" 
                  rows="3"
                  placeholder="Kuryer uchun maxsus ko'rsatmalar..."
                  style={{ resize: 'vertical' }}
                />
              </div>

              <div className="form-group" style={{ marginBottom: '24px' }}>
                <label>To'lov usuli</label>
                <div style={{
                  padding: '16px',
                  backgroundColor: 'var(--bg-secondary)',
                  border: '1px solid var(--accent-indigo)',
                  borderRadius: 'var(--border-radius-sm)',
                  display: 'flex',
                  alignItems: 'center',
                  gap: '12px'
                }}>
                  <input type="radio" defaultChecked readOnly style={{ accentColor: 'var(--accent-indigo)' }} />
                  <div>
                    <span style={{ fontWeight: 600, display: 'block', fontSize: '0.9rem' }}>Naqd / Eshik oldida to'lash</span>
                    <span style={{ fontSize: '0.75rem', color: 'var(--text-muted)' }}>Mahsulotni qo'lingizga olgandan so'ng to'laysiz</span>
                  </div>
                </div>
              </div>

              <button 
                type="submit" 
                className="glow-btn"
                disabled={submitting || cartItems.length === 0}
                style={{ width: '100%', padding: '14px', fontSize: '1rem' }}
              >
                {submitting ? 'Yuborilmoqda...' : 'Buyurtma berish'}
              </button>
            </form>
          </div>

          {/* Cart Summary */}
          <div className="glass-panel" style={{ padding: '32px', height: 'fit-content' }}>
            <h2 style={{ fontFamily: 'var(--font-display)', fontSize: '1.2rem', marginBottom: '20px' }}>Sizning buyurtmangiz</h2>
            
            <div style={{ display: 'flex', flexDirection: 'column', gap: '16px', marginBottom: '24px' }}>
              {cartItems.map((item) => {
                const prod = products.find(p => p.id === item.productId);
                return (
                  <div key={item.productId} style={{ display: 'flex', justifyContent: 'space-between', fontSize: '0.9rem' }}>
                    <span style={{ color: 'var(--text-muted)', flex: 1, paddingRight: '12px' }}>
                      {prod ? prod.name : `Mahsulot #${item.productId}`} <strong style={{ color: 'var(--text-main)' }}>x{item.quantity}</strong>
                    </span>
                    <span style={{ fontWeight: 600 }}>
                      {((prod ? prod.price : 0) * item.quantity).toLocaleString('uz-UZ')} UZS
                    </span>
                  </div>
                );
              })}
            </div>

            <div style={{
              borderTop: '1px solid var(--border-color)',
              paddingTop: '20px',
              display: 'flex',
              justifyContent: 'space-between',
              alignItems: 'center'
            }}>
              <span style={{ fontWeight: 600 }}>Umumiy summa:</span>
              <span style={{ fontSize: '1.4rem', fontWeight: 800, color: 'var(--accent-teal)', fontFamily: 'var(--font-display)' }}>
                {totalPrice.toLocaleString('uz-UZ')} UZS
              </span>
            </div>
          </div>
        </div>

        <style>{`
          @media (max-width: 768px) {
            .checkout-grid {
              grid-template-columns: 1fr !important;
              gap: 24px !important;
            }
          }
        `}</style>
      </div>
    );
  }

  // --- ORDER HISTORY VIEW ---
  return (
    <div style={{ padding: '0 16px 48px 16px', maxWidth: '900px', margin: '0 auto' }} className="fade-in">
      <h1 style={{ fontFamily: 'var(--font-display)', fontSize: '2rem', fontWeight: 800, marginBottom: '24px' }}>
        Mening Buyurtmalarim
      </h1>

      {error && (
        <div style={{
          padding: '12px 16px',
          backgroundColor: 'var(--accent-rose-glow)',
          color: '#ff7675',
          borderRadius: 'var(--border-radius-sm)',
          border: '1px solid var(--accent-rose)',
          marginBottom: '24px'
        }}>
          {error}
        </div>
      )}

      {loading ? (
        <div style={{ textAlign: 'center', padding: '48px' }}>Yuklanmoqda...</div>
      ) : orders.length === 0 ? (
        <div className="glass-panel" style={{ padding: '64px 24px', textAlign: 'center', color: 'var(--text-muted)' }}>
          <svg style={{ width: '48px', height: '48px', opacity: 0.3, marginBottom: '16px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="1.5" d="M9 5H7a2 2 0 00-2 2v12a2 2 0 002 2h10a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2m-3 7h3m-3 4h3m-6-4h.01M9 16h.01" />
          </svg>
          <p style={{ fontSize: '1.1rem', fontWeight: 500, marginBottom: '16px' }}>Sizda hali buyurtmalar mavjud emas.</p>
          <button onClick={() => onNavigate('home')} className="glow-btn">Xaridlarni Boshlash</button>
        </div>
      ) : (
        <div style={{ display: 'flex', flexDirection: 'column', gap: '20px' }}>
          {orders.map((ord) => (
            <div 
              key={ord.id} 
              className="glass-panel" 
              style={{
                padding: '24px',
                border: '1px solid var(--border-color)',
                backgroundColor: 'var(--bg-secondary)'
              }}
            >
              {/* Order Header info */}
              <div style={{
                display: 'flex',
                justifyContent: 'space-between',
                alignItems: 'center',
                borderBottom: '1px solid var(--border-color)',
                paddingBottom: '16px',
                marginBottom: '16px',
                flexWrap: 'wrap',
                gap: '12px'
              }}>
                <div>
                  <span style={{ fontSize: '0.8rem', color: 'var(--text-muted)', display: 'block' }}>BUYURTMA ID</span>
                  <span style={{ fontWeight: 700, fontSize: '1rem' }}>#UZM-{ord.id}</span>
                </div>

                <div>
                  <span style={{ fontSize: '0.8rem', color: 'var(--text-muted)', display: 'block' }}>SANA</span>
                  <span style={{ fontWeight: 600 }}>{ord.orderDate || 'Bugun'}</span>
                </div>

                <div>
                  <span style={{ fontSize: '0.8rem', color: 'var(--text-muted)', display: 'block' }}>HOLAT</span>
                  {ord.orderStatusId === 1 ? (
                    <span className="badge badge-primary">Kutilmoqda</span>
                  ) : ord.orderStatusId === 2 ? (
                    <span className="badge badge-success">Yetkazildi</span>
                  ) : (
                    <span className="badge badge-primary">Bajarilmoqda</span>
                  )}
                </div>

                <div>
                  <span style={{ fontSize: '0.8rem', color: 'var(--text-muted)', display: 'block' }}>JAMI SUMMA</span>
                  <span style={{ fontWeight: 800, color: 'var(--accent-teal)', fontSize: '1.1rem' }}>
                    {ord.totalAmount.toLocaleString('uz-UZ')} UZS
                  </span>
                </div>
              </div>

              {/* Order Items list inside order */}
              {ord.tables && ord.tables.length > 0 ? (
                <div style={{ display: 'flex', flexDirection: 'column', gap: '12px' }}>
                  {ord.tables.map((item, idx) => {
                    const prod = products.find(p => p.id === item.productId);
                    return (
                      <div key={item.id || idx} style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', fontSize: '0.88rem' }}>
                        <span style={{ color: 'var(--text-muted)' }}>
                          {prod ? prod.name : `Mahsulot #${item.productId}`} <strong style={{ color: 'var(--text-main)' }}>x{item.quantity}</strong>
                        </span>
                        <span style={{ fontWeight: 600 }}>
                          {(item.price || (prod ? prod.price : 0) * item.quantity).toLocaleString('uz-UZ')} UZS
                        </span>
                      </div>
                    );
                  })}
                </div>
              ) : (
                <div style={{ fontSize: '0.85rem', color: 'var(--text-muted)' }}>
                  Mahsulot tafsilotlari yuklanmadi.
                </div>
              )}
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
