import React, { useState, useEffect } from 'react';
import { api } from '../services/api';

const FALLBACK_CATEGORIES = [
  { id: 1, name: 'Smartfonlar va Gadjetlar' },
  { id: 2, name: 'Kompyuter Texnikasi' },
  { id: 3, name: 'Maishiy Texnika' },
  { id: 4, name: 'Kiyim va Poyabzallar' },
  { id: 5, name: 'Kitoblar va Kanselyariya' },
];

export default function AdminPanel({ products, onRefreshProducts }) {
  const [editingProduct, setEditingProduct] = useState(null); // null if adding new, or holds product object
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState({ text: '', isError: false });
  const [categories, setCategories] = useState(FALLBACK_CATEGORIES);

  // Form Fields State
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    price: 0,
    stockQuantity: 0,
    categoryId: 1,
    supplierId: 1,
    imageUrl: '', // for main image quick add
  });

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
          setCategories(normalized.map((cat) => ({
            id: cat.id ?? cat.categoryId ?? 0,
            name: cat.name ?? cat.title ?? 'Kategoriya'
          })));
        }
      } catch (error) {
        console.warn('Kategoriya ro\'yxatini yuklashda xatolik:', error);
        setCategories(FALLBACK_CATEGORIES);
      }
    }

    loadCategories();
  }, []);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: name === 'price' || name === 'stockQuantity' || name === 'categoryId' || name === 'supplierId' 
        ? Number(value) 
        : value
    }));
  };

  const handleOpenAddForm = () => {
    setEditingProduct(null);
    setFormData({
      name: '',
      description: '',
      price: 0,
      stockQuantity: 0,
      categoryId: 1,
      supplierId: 1,
      imageUrl: '',
    });
    setMessage({ text: '', isError: false });
    setIsFormOpen(true);
  };

  const handleOpenEditForm = (prod) => {
    setEditingProduct(prod);
    const mainImg = prod.tables && prod.tables.length > 0
      ? prod.tables.find(t => t.mainPic)?.imageUrl || prod.tables[0].imageUrl
      : '';
    setFormData({
      name: prod.name,
      description: prod.description || '',
      price: prod.price,
      stockQuantity: prod.stockQuantity,
      categoryId: prod.categoryId,
      supplierId: prod.supplierId,
      imageUrl: mainImg,
    });
    setMessage({ text: '', isError: false });
    setIsFormOpen(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!formData.name || formData.price <= 0 || formData.stockQuantity < 0) {
      setMessage({ text: 'Iltimos, maydonlarni to\'g\'ri to\'ldiring.', isError: true });
      return;
    }

    setLoading(true);
    setMessage({ text: '', isError: false });

    try {
      if (editingProduct) {
        // Edit flow
        const updateDto = {
          id: editingProduct.id,
          name: formData.name,
          description: formData.description,
          price: formData.price,
          stockQuantity: formData.stockQuantity,
          categoryId: formData.categoryId,
          supplierId: formData.supplierId,
          tables: formData.imageUrl ? [
            {
              imageUrl: formData.imageUrl,
              mainPic: true,
              productId: editingProduct.id,
              sortOrder: 1
            }
          ] : []
        };
        await api.products.update(updateDto);
        setMessage({ text: 'Mahsulot muvaffaqiyatli tahrirlandi!', isError: false });
      } else {
        // Create flow
        const createDto = {
          name: formData.name,
          description: formData.description,
          price: formData.price,
          stockQuantity: formData.stockQuantity,
          categoryId: formData.categoryId,
          supplierId: formData.supplierId,
          tables: formData.imageUrl ? [
            {
              imageUrl: formData.imageUrl,
              mainPic: true,
              sortOrder: 1
            }
          ] : []
        };
        await api.products.create(createDto);
        setMessage({ text: 'Yangi mahsulot muvaffaqiyatli qo\'shildi!', isError: false });
      }
      
      // Reset & Refresh
      setTimeout(() => {
        setIsFormOpen(false);
        onRefreshProducts();
      }, 1500);
    } catch (err) {
      setMessage({ text: err.message || 'Xatolik yuz berdi.', isError: true });
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async (id) => {
    if (!window.confirm('Haqiqatan ham ushbu mahsulotni o\'chirmoqchimisiz?')) return;
    
    try {
      await api.products.delete(id);
      alert('Mahsulot o\'chirildi!');
      onRefreshProducts();
    } catch (err) {
      alert('O\'chirishda xatolik: ' + err.message);
    }
  };

  return (
    <div style={{ padding: '0 16px 48px 16px' }} className="fade-in">
      
      {/* Header section with CTA */}
      <div style={{
        display: 'flex',
        alignItems: 'center',
        justifyContent: 'space-between',
        marginBottom: '32px',
        flexWrap: 'wrap',
        gap: '16px'
      }}>
        <div>
          <h1 style={{ fontFamily: 'var(--font-display)', fontSize: '2rem', fontWeight: 800 }}>
            Sotuvchi Boshqaruv Paneli
          </h1>
          <p style={{ color: 'var(--text-muted)', fontSize: '0.9rem' }}>
            Do'kondagi mahsulotlarni boshqarish, yangilarini qo'shish va sotuv ko'rsatkichlarini nazorat qilish.
          </p>
        </div>

        {!isFormOpen && (
          <button onClick={handleOpenAddForm} className="glow-btn">
            <svg style={{ width: '18px', height: '18px', marginRight: '6px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M12 4v16m8-8H4" />
            </svg>
            Yangi mahsulot qo'shish
          </button>
        )}
      </div>

      {isFormOpen ? (
        /* CREATE / EDIT FORM VIEW */
        <div className="glass-panel" style={{ padding: '32px', maxWidth: '700px', margin: '0 auto' }}>
          <h2 style={{ fontFamily: 'var(--font-display)', fontSize: '1.4rem', marginBottom: '24px' }}>
            {editingProduct ? `Tahrirlash: ${editingProduct.name}` : 'Yangi Mahsulot Ma\'lumotlari'}
          </h2>

          {message.text && (
            <div style={{
              padding: '12px 16px',
              backgroundColor: message.isError ? 'var(--accent-rose-glow)' : 'rgba(0, 210, 211, 0.15)',
              color: message.isError ? '#ff7675' : 'var(--accent-teal)',
              borderRadius: 'var(--border-radius-sm)',
              border: `1px solid ${message.isError ? 'var(--accent-rose)' : 'var(--accent-teal)'}`,
              marginBottom: '24px',
              fontSize: '0.9rem'
            }}>
              {message.text}
            </div>
          )}

          <form onSubmit={handleSubmit}>
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '20px' }} className="form-grid">
              
              <div className="form-group" style={{ gridColumn: 'span 2' }}>
                <label>Mahsulot nomi*</label>
                <input 
                  type="text" 
                  name="name"
                  className="form-input"
                  value={formData.name}
                  onChange={handleInputChange}
                  placeholder="Masalan: iPhone 15 Pro Max"
                  required
                />
              </div>

              <div className="form-group" style={{ gridColumn: 'span 2' }}>
                <label>Batafsil tavsif</label>
                <textarea 
                  name="description"
                  className="form-input"
                  rows="3"
                  value={formData.description}
                  onChange={handleInputChange}
                  placeholder="Mahsulotning xususiyatlari, o'lchami, rangi va boshqalar..."
                  style={{ resize: 'vertical' }}
                />
              </div>

              <div className="form-group">
                <label>Narxi (UZS)*</label>
                <input 
                  type="number" 
                  name="price"
                  className="form-input"
                  value={formData.price}
                  onChange={handleInputChange}
                  placeholder="Narxini kiriting"
                  required
                />
              </div>

              <div className="form-group">
                <label>Ombordagi miqdor*</label>
                <input 
                  type="number" 
                  name="stockQuantity"
                  className="form-input"
                  value={formData.stockQuantity}
                  onChange={handleInputChange}
                  placeholder="Mavjud dona soni"
                  required
                />
              </div>

              <div className="form-group">
                <label>Kategoriya*</label>
                <select 
                  name="categoryId"
                  className="form-input"
                  value={formData.categoryId}
                  onChange={handleInputChange}
                >
                  {(categories.length > 0 ? categories : FALLBACK_CATEGORIES).map((cat) => (
                    <option key={cat.id} value={cat.id}>{cat.name}</option>
                  ))}
                </select>
              </div>

              <div className="form-group">
                <label>Yetkazib beruvchi ID (Supplier ID)*</label>
                <input 
                  type="number" 
                  name="supplierId"
                  className="form-input"
                  value={formData.supplierId}
                  onChange={handleInputChange}
                  required
                />
              </div>

              <div className="form-group" style={{ gridColumn: 'span 2' }}>
                <label>Rasm URL manzili (Image URL)</label>
                <input 
                  type="text" 
                  name="imageUrl"
                  className="form-input"
                  value={formData.imageUrl}
                  onChange={handleInputChange}
                  placeholder="https://example.com/rasm.jpg"
                />
              </div>

            </div>

            <div style={{ display: 'flex', gap: '16px', marginTop: '24px', justifyContent: 'flex-end' }}>
              <button 
                type="button" 
                onClick={() => setIsFormOpen(false)} 
                className="sec-btn"
                disabled={loading}
              >
                Bekor qilish
              </button>
              <button 
                type="submit" 
                className="glow-btn"
                disabled={loading}
              >
                {loading ? 'Saqlanmoqda...' : 'Saqlash'}
              </button>
            </div>
          </form>
        </div>
      ) : (
        /* PRODUCTS LIST VIEW */
        <div className="glass-panel" style={{ overflowX: 'auto', padding: '16px' }}>
          <table style={{
            width: '100%',
            borderCollapse: 'collapse',
            textAlign: 'left',
            fontSize: '0.9rem'
          }}>
            <thead>
              <tr style={{ borderBottom: '1px solid var(--border-color)', color: 'var(--text-muted)' }}>
                <th style={{ padding: '16px 12px' }}>Rasm</th>
                <th style={{ padding: '16px 12px' }}>Nomi</th>
                <th style={{ padding: '16px 12px' }}>Kategoriya</th>
                <th style={{ padding: '16px 12px' }}>Narxi</th>
                <th style={{ padding: '16px 12px' }}>Omborda</th>
                <th style={{ padding: '16px 12px', textAlign: 'right' }}>Amallar</th>
              </tr>
            </thead>
            <tbody>
              {products.length === 0 ? (
                <tr>
                  <td colSpan="6" style={{ padding: '32px', textMuted: true, textAlign: 'center' }}>
                    Mahsulotlar mavjud emas. Yangi mahsulot qo'shing.
                  </td>
                </tr>
              ) : (
                products.map((prod) => {
                  const mainImage = prod.tables && prod.tables.length > 0
                    ? prod.tables.find(t => t.mainPic)?.imageUrl || prod.tables[0].imageUrl
                    : 'https://images.unsplash.com/photo-1542291026-7eec264c27ff?w=100&auto=format&fit=crop&q=60';
                  
                  return (
                    <tr key={prod.id} style={{ borderBottom: '1px solid rgba(255,255,255,0.03)', transition: 'var(--transition-fast)' }} className="table-row">
                      <td style={{ padding: '12px' }}>
                        <img 
                          src={mainImage} 
                          alt={prod.name} 
                          style={{ width: '50px', height: '50px', objectFit: 'cover', borderRadius: '6px', backgroundColor: 'rgba(255,255,255,0.05)' }} 
                        />
                      </td>
                      <td style={{ padding: '12px', fontWeight: 600 }}>{prod.name}</td>
                      <td style={{ padding: '12px', color: 'var(--text-muted)' }}>
                        {prod.categoryId === 1 ? 'Smartfon' : prod.categoryId === 2 ? 'Kompyuter' : 'Kategoriya #' + prod.categoryId}
                      </td>
                      <td style={{ padding: '12px', color: 'var(--accent-teal)', fontWeight: 700 }}>
                        {prod.price.toLocaleString('uz-UZ')} UZS
                      </td>
                      <td style={{ padding: '12px' }}>
                        {prod.stockQuantity > 0 ? (
                          <span style={{ color: 'var(--accent-teal)' }}>{prod.stockQuantity} ta</span>
                        ) : (
                          <span style={{ color: 'var(--accent-rose)' }}>Tugagan</span>
                        )}
                      </td>
                      <td style={{ padding: '12px', textAlign: 'right' }}>
                        <div style={{ display: 'flex', gap: '8px', justifyContent: 'flex-end' }}>
                          <button 
                            onClick={() => handleOpenEditForm(prod)}
                            className="sec-btn"
                            style={{ padding: '8px 12px', fontSize: '0.8rem', borderRadius: '8px' }}
                          >
                            Tahrirlash
                          </button>
                          <button 
                            onClick={() => handleDelete(prod.id)}
                            className="sec-btn"
                            style={{ 
                              padding: '8px 12px', 
                              fontSize: '0.8rem', 
                              borderRadius: '8px', 
                              borderColor: 'rgba(255, 63, 108, 0.3)',
                              color: '#ff7675'
                            }}
                          >
                            O'chirish
                          </button>
                        </div>
                      </td>
                    </tr>
                  );
                })
              )}
            </tbody>
          </table>
        </div>
      )}

      <style>{`
        .table-row:hover {
          background-color: rgba(255, 255, 255, 0.02);
        }
        @media (max-width: 600px) {
          .form-grid {
            grid-template-columns: 1fr !important;
          }
          .form-grid > div {
            grid-column: span 1 !important;
          }
        }
      `}</style>
    </div>
  );
}
