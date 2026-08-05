import React, { useState, useEffect } from 'react';
import { api } from '../services/api';

export default function AdminPanel({ products, onRefreshProducts }) {
  const [editingProduct, setEditingProduct] = useState(null); // null if adding new, or holds product object
  const [isFormOpen, setIsFormOpen] = useState(false);
  const [loading, setLoading] = useState(false);
  const [message, setMessage] = useState({ text: '', isError: false });
  const [categories, setCategories] = useState([]);
  const [categorySearch, setCategorySearch] = useState('');
  const [categoryDropdownOpen, setCategoryDropdownOpen] = useState(false);

  // Form Fields State
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    price: '',
    stockQuantity: '',
    categoryId: 0,
    imageUrl: '',
    imageUrls: [],
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
          const mappedCategories = normalized.map((cat) => ({
            id: cat.id ?? cat.categoryId ?? 0,
            name: cat.name ?? cat.title ?? 'Kategoriya'
          }));

          setCategories(mappedCategories);
          setFormData(prev => ({
            ...prev,
            categoryId: prev.categoryId && mappedCategories.some(cat => cat.id === prev.categoryId)
              ? prev.categoryId
              : mappedCategories[0]?.id ?? 0,
          }));
        } else {
          setCategories([]);
          setFormData(prev => ({ ...prev, categoryId: 0 }));
        }
      } catch (error) {
        console.warn('Kategoriya ro\'yxatini yuklashda xatolik:', error);
        setCategories([]);
        setFormData(prev => ({ ...prev, categoryId: 0 }));
      }
    }

    loadCategories();
  }, []);

  const sanitizeNumericValue = (value) => value.replace(/\D/g, '');

  const readImageAsDataUrl = (file) => new Promise((resolve, reject) => {
    const reader = new FileReader();

    reader.onload = () => {
      const sourceDataUrl = reader.result;
      if (typeof sourceDataUrl !== 'string' || !sourceDataUrl.startsWith('data:image')) {
        reject(new Error('Rasmni o‘qib bo‘lmadi.'));
        return;
      }

      const img = new Image();
      img.onload = () => {
        const maxDimension = 1400;
        let width = img.width;
        let height = img.height;

        if (width > height && width > maxDimension) {
          height = Math.round((height * maxDimension) / width);
          width = maxDimension;
        } else if (height > maxDimension) {
          width = Math.round((width * maxDimension) / height);
          height = maxDimension;
        }

        const canvas = document.createElement('canvas');
        canvas.width = width;
        canvas.height = height;

        const ctx = canvas.getContext('2d');
        ctx.drawImage(img, 0, 0, width, height);

        const mimeType = file.type.includes('png') ? 'image/png' : 'image/jpeg';
        const quality = mimeType === 'image/png' ? 1 : 0.78;
        resolve(canvas.toDataURL(mimeType, quality));
      };

      img.onerror = () => reject(new Error('Rasmni qayta ishlashda xatolik yuz berdi.'));
      img.src = sourceDataUrl;
    };

    reader.onerror = () => reject(new Error('Rasmni o‘qib bo‘lmadi.'));
    reader.readAsDataURL(file);
  });

  const handleInputChange = (e) => {
    const { name, value } = e.target;

    if (name === 'price' || name === 'stockQuantity') {
      setFormData(prev => ({
        ...prev,
        [name]: sanitizeNumericValue(value)
      }));
      return;
    }

    if (name === 'categoryId') {
      setFormData(prev => ({
        ...prev,
        [name]: Number(value)
      }));
      return;
    }

    setFormData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleCategorySearchChange = (e) => {
    const value = e.target.value;
    setCategorySearch(value);
    setCategoryDropdownOpen(true);

    if (!value) {
      const selected = categories.find((cat) => cat.id === formData.categoryId);
      setFormData((prev) => ({ ...prev, categoryId: selected?.id ?? 0 }));
    }
  };

  const handleCategorySelect = (catId) => {
    setFormData((prev) => ({ ...prev, categoryId: catId }));
    setCategorySearch(categories.find((cat) => cat.id === catId)?.name || '');
    setCategoryDropdownOpen(false);
  };

  const handleImageUpload = async (e) => {
    const files = Array.from(e.target.files || []);
    if (!files.length) return;

    if (files.length > 8) {
      setMessage({ text: 'Maksimal 8 ta rasm yuklash mumkin.', isError: true });
      e.target.value = '';
      return;
    }

    try {
      const results = (await Promise.all(files.map((file) => readImageAsDataUrl(file)))).filter(Boolean);
      if (!results.length) return;

      setFormData(prev => ({
        ...prev,
        imageUrls: [...prev.imageUrls, ...results],
        imageUrl: results[0] || prev.imageUrl,
      }));
      setMessage({ text: `${results.length} ta rasm yuklandi.`, isError: false });
    } catch (error) {
      setMessage({ text: error.message || 'Rasm yuklashda xatolik yuz berdi.', isError: true });
    } finally {
      e.target.value = '';
    }
  };

  const handleOpenAddForm = () => {
    setEditingProduct(null);
    setFormData({
      name: '',
      description: '',
      price: '',
      stockQuantity: '',
      categoryId: categories[0]?.id ?? 0,
      imageUrl: '',
      imageUrls: [],
    });
    setCategorySearch('');
    setCategoryDropdownOpen(false);
    setMessage({ text: '', isError: false });
    setIsFormOpen(true);
  };

  const handleOpenEditForm = (prod) => {
    setEditingProduct(prod);
    const existingImages = (prod.tables || prod.images || prod.items || []).map((img) => img.imageUrl || img.url || '').filter(Boolean);
    const mainImg = existingImages[0] || '';
    setFormData({
      name: prod.name,
      description: prod.description || '',
      price: String(prod.price ?? ''),
      stockQuantity: String(prod.stockQuantity ?? ''),
      categoryId: prod.categoryId ?? categories[0]?.id ?? 0,
      imageUrl: mainImg,
      imageUrls: existingImages,
    });
    setCategorySearch(categories.find((cat) => cat.id === (prod.categoryId ?? categories[0]?.id ?? 0))?.name || '');
    setCategoryDropdownOpen(false);
    setMessage({ text: '', isError: false });
    setIsFormOpen(true);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!formData.name || formData.price === '' || Number(formData.price) <= 0 || formData.stockQuantity === '' || Number(formData.stockQuantity) < 0 || !formData.categoryId) {
      setMessage({ text: 'Iltimos, maydonlarni to\'g\'ri to\'ldiring va kategoriya tanlang.', isError: true });
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
          items: formData.imageUrls.length > 0 ? formData.imageUrls.map((url, index) => ({
            imageUrl: url,
            mainPic: index === 0,
            productId: editingProduct.id,
            sortOrder: index + 1
          })) : []
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
          items: formData.imageUrls.length > 0 ? formData.imageUrls.map((url, index) => ({
            imageUrl: url,
            mainPic: index === 0,
            sortOrder: index + 1
          })) : []
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
        flexDirection: 'column',
        alignItems: 'center',
        justifyContent: 'center',
        textAlign: 'center',
        marginBottom: '32px',
        gap: '16px'
      }}>
        <div style={{ maxWidth: '720px' }}>
          <h1 style={{ fontFamily: 'var(--font-display)', fontSize: '2rem', fontWeight: 800, marginBottom: '8px' }}>
            Sotuvchi Boshqaruv Paneli
          </h1>
          <p style={{ color: 'var(--text-muted)', fontSize: '0.95rem', margin: 0 }}>
            Mahsulotlarni boshqaring, yangilarini qo‘shing va mavjudlarni tezkor ravishda tahrirlang.
          </p>
        </div>

        {!isFormOpen && (
          <button onClick={handleOpenAddForm} className="glow-btn" style={{ minWidth: '220px' }}>
            <svg style={{ width: '18px', height: '18px', marginRight: '6px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
              <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2.5" d="M12 4v16m8-8H4" />
            </svg>
            Yangi mahsulot qo‘shish
          </button>
        )}
      </div>

      {isFormOpen ? (
        /* CREATE / EDIT FORM VIEW */
        <div className="glass-panel" style={{ padding: '36px', maxWidth: '760px', margin: '0 auto', borderRadius: '24px' }}>
          <h2 style={{ fontFamily: 'var(--font-display)', fontSize: '1.7rem', fontWeight: 700, marginBottom: '24px', textAlign: 'center' }}>
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
            <div style={{ display: 'grid', gridTemplateColumns: '1fr 1fr', gap: '22px' }} className="form-grid">
              
              <div className="form-group" style={{ gridColumn: 'span 2', background: 'rgba(255,255,255,0.04)', padding: '16px', borderRadius: '16px', border: '1px solid rgba(255,255,255,0.08)' }}>
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

              <div className="form-group" style={{ gridColumn: 'span 2', background: 'rgba(255,255,255,0.04)', padding: '16px', borderRadius: '16px', border: '1px solid rgba(255,255,255,0.08)' }}>
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

              <div className="form-group" style={{ background: 'rgba(255,255,255,0.04)', padding: '16px', borderRadius: '16px', border: '1px solid rgba(255,255,255,0.08)' }}>
                <label>Narxi (UZS)*</label>
                <input 
                  type="text"
                  inputMode="numeric"
                  name="price"
                  className="form-input"
                  value={formData.price}
                  onChange={handleInputChange}
                  placeholder="Narxini kiriting"
                  required
                />
              </div>

              <div className="form-group" style={{ background: 'rgba(255,255,255,0.04)', padding: '16px', borderRadius: '16px', border: '1px solid rgba(255,255,255,0.08)' }}>
                <label>Ombordagi miqdor*</label>
                <input 
                  type="text"
                  inputMode="numeric"
                  name="stockQuantity"
                  className="form-input"
                  value={formData.stockQuantity}
                  onChange={handleInputChange}
                  placeholder="Mavjud dona soni"
                  required
                />
              </div>

              <div className="form-group" style={{ background: 'rgba(255,255,255,0.04)', padding: '16px', borderRadius: '16px', border: '1px solid rgba(255,255,255,0.08)' }}>
                <label>Kategoriya*</label>
                <div style={{ position: 'relative' }}>
                  <input
                    type="text"
                    className="form-input"
                    value={categorySearch}
                    onChange={handleCategorySearchChange}
                    onFocus={() => setCategoryDropdownOpen(true)}
                    placeholder="Kategoriya nomini yozing"
                  />
                  {categoryDropdownOpen && categories.length > 0 && (
                    <div style={{
                      position: 'absolute',
                      top: 'calc(100% + 6px)',
                      left: 0,
                      right: 0,
                      zIndex: 20,
                      background: 'var(--bg-secondary)',
                      border: '1px solid var(--border-color)',
                      borderRadius: '10px',
                      maxHeight: '220px',
                      overflowY: 'auto'
                    }}>
                      {categories
                        .filter((cat) => cat.name.toLowerCase().includes(categorySearch.toLowerCase()))
                        .map((cat) => (
                          <button
                            key={cat.id}
                            type="button"
                            onClick={() => handleCategorySelect(cat.id)}
                            style={{
                              display: 'block',
                              width: '100%',
                              textAlign: 'left',
                              padding: '10px 12px',
                              background: 'transparent',
                              border: 'none',
                              color: 'var(--text-main)',
                              cursor: 'pointer'
                            }}
                          >
                            {cat.name}
                          </button>
                        ))}
                    </div>
                  )}
                </div>
              </div>

              <div className="form-group" style={{ gridColumn: 'span 2', background: 'rgba(255,255,255,0.04)', padding: '16px', borderRadius: '16px', border: '1px solid rgba(255,255,255,0.08)' }}>
                <label>Rasm yuklash</label>
                <label htmlFor="product-images" style={{
                  display: 'inline-flex',
                  alignItems: 'center',
                  justifyContent: 'center',
                  gap: '8px',
                  padding: '12px 16px',
                  borderRadius: '12px',
                  border: '1px solid var(--accent-indigo)',
                  background: 'linear-gradient(135deg, rgba(95,39,205,0.18), rgba(0,210,211,0.12))',
                  color: 'var(--text-main)',
                  fontWeight: 600,
                  cursor: 'pointer',
                  transition: 'transform 0.2s ease'
                }}>
                  <svg style={{ width: '18px', height: '18px' }} fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M4 16v1a2 2 0 002 2h12a2 2 0 002-2v-1m-4-4l-4-4m0 0L8 8m4-4v10" />
                  </svg>
                  Rasm tanlash
                </label>
                <input
                  id="product-images"
                  type="file"
                  accept="image/*"
                  multiple
                  className="form-input"
                  onChange={handleImageUpload}
                  style={{ display: 'none' }}
                />
                {formData.imageUrls.length > 0 && (
                  <div style={{ display: 'grid', gridTemplateColumns: 'repeat(auto-fit, minmax(110px, 1fr))', gap: '10px', marginTop: '12px' }}>
                    {formData.imageUrls.map((url, index) => (
                      <img
                        key={`${url}-${index}`}
                        src={url}
                        alt={`Rasm ${index + 1}`}
                        style={{
                          width: '100%',
                          height: '110px',
                          objectFit: 'cover',
                          borderRadius: '10px',
                          border: '1px solid rgba(255,255,255,0.1)'
                        }}
                      />
                    ))}
                  </div>
                )}
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
                  const imageSources = (prod.tables || prod.images || prod.items || []).filter(Boolean);
                  const mainImage = imageSources.length > 0
                    ? (imageSources.find((item) => item.mainPic)?.imageUrl || imageSources.find((item) => item.imageUrl)?.imageUrl || imageSources.find((item) => item.url)?.url || imageSources[0]?.imageUrl || imageSources[0]?.url || imageSources[0])
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
                        {prod.categoryName || (prod.categoryId === 1 ? 'Smartfon' : prod.categoryId === 2 ? 'Kompyuter' : 'Kategoriya')}
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
