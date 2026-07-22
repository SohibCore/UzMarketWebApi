import React, { useState } from 'react';
import { api } from '../services/api';

export default function Auth({ onLoginSuccess }) {
  const [isLogin, setIsLogin] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  // Login Form States
  const [loginUser, setLoginUser] = useState('');
  const [loginPass, setLoginPass] = useState('');

  // Register Form States
  const [regData, setRegData] = useState({
    userName: '',
    password: '',
    fullName: '',
    shortName: '',
    pinfl: '',
    phoneNumber: '',
    address: '',
    dateOfBirth: '',
    passportSeries: '',
    email: ''
  });

  const handleLoginSubmit = async (e) => {
    e.preventDefault();
    if (!loginUser || !loginPass) {
      setError('Iltimos barcha maydonlarni to\'ldiring.');
      return;
    }

    setLoading(true);
    setError('');

    try {
      const data = await api.auth.login(loginUser, loginPass);
      onLoginSuccess(data);
    } catch (err) {
      setError(err.message || 'Kirishda xatolik yuz berdi. Parol yoki foydalanuvchi nomi noto\'g\'ri.');
    } finally {
      setLoading(false);
    }
  };

  const handleRegisterSubmit = async (e) => {
    e.preventDefault();
    // Validate required fields
    const requiredFields = ['userName', 'password', 'fullName', 'pinfl', 'phoneNumber', 'passportSeries', 'email'];
    for (const field of requiredFields) {
      if (!regData[field]) {
        setError(`Iltimos, '${field}' maydonini to'ldiring.`);
        return;
      }
    }

    setLoading(true);
    setError('');

    try {
      // Convert date from yyyy-MM-dd to dd.MM.yyyy format
      let formattedDate = '01.01.2000';
      if (regData.dateOfBirth) {
        const [year, month, day] = regData.dateOfBirth.split('-');
        formattedDate = `${day}.${month}.${year}`;
      }

      const formattedData = {
        ...regData,
        shortName: regData.shortName || regData.fullName.split(' ')[0] || 'User',
        address: regData.address || 'Uzbekistan',
        dateOfBirth: formattedDate
      };
      
      const data = await api.auth.register(formattedData);
      onLoginSuccess(data);
    } catch (err) {
      setError(err.message || 'Ro\'yxatdan o\'tishda xatolik yuz berdi.');
    } finally {
      setLoading(false);
    }
  };

  const handleRegChange = (e) => {
    const { name, value } = e.target;
    setRegData(prev => ({
      ...prev,
      [name]: value
    }));
  };

  return (
    <div style={{
      display: 'flex',
      alignItems: 'center',
      justifyContent: 'center',
      minHeight: 'calc(100vh - 120px)',
      padding: '20px'
    }} className="fade-in">
      <div 
        className="glass-panel" 
        style={{
          width: '100%',
          maxWidth: isLogin ? '460px' : '650px',
          padding: '40px',
          boxShadow: 'var(--shadow-lg)',
          border: '1px solid rgba(255, 255, 255, 0.1)',
          transition: 'max-width var(--transition-normal)'
        }}
      >
        {/* Toggle tabs */}
        <div style={{
          display: 'flex',
          justifyContent: 'center',
          gap: '24px',
          marginBottom: '32px',
          borderBottom: '1px solid var(--border-color)',
          paddingBottom: '12px'
        }}>
          <h2 
            onClick={() => { setIsLogin(true); setError(''); }}
            style={{
              fontFamily: 'var(--font-display)',
              fontSize: '1.4rem',
              fontWeight: 700,
              cursor: 'pointer',
              color: isLogin ? 'var(--text-main)' : 'var(--text-muted)',
              borderBottom: isLogin ? '3px solid var(--accent-indigo)' : 'none',
              paddingBottom: '8px',
              transition: 'var(--transition-fast)'
            }}
          >
            Kirish
          </h2>
          <h2 
            onClick={() => { setIsLogin(false); setError(''); }}
            style={{
              fontFamily: 'var(--font-display)',
              fontSize: '1.4rem',
              fontWeight: 700,
              cursor: 'pointer',
              color: !isLogin ? 'var(--text-main)' : 'var(--text-muted)',
              borderBottom: !isLogin ? '3px solid var(--accent-indigo)' : 'none',
              paddingBottom: '8px',
              transition: 'var(--transition-fast)'
            }}
          >
            Ro'yxatdan O'tish
          </h2>
        </div>

        {error && (
          <div style={{
            padding: '12px 16px',
            backgroundColor: 'var(--accent-rose-glow)',
            color: '#ff7675',
            borderRadius: 'var(--border-radius-sm)',
            border: '1px solid var(--accent-rose)',
            marginBottom: '24px',
            fontSize: '0.9rem',
            lineHeight: '1.4'
          }}>
            {error}
          </div>
        )}

        {isLogin ? (
          /* LOGIN FORM */
          <form onSubmit={handleLoginSubmit}>
            <div className="form-group">
              <label>Foydalanuvchi nomi (UserName)</label>
              <input 
                type="text" 
                className="form-input" 
                value={loginUser}
                onChange={(e) => setLoginUser(e.target.value)}
                placeholder="UserName kiriting"
                required
              />
            </div>
            
            <div className="form-group" style={{ marginBottom: '32px' }}>
              <label>Parol</label>
              <input 
                type="password" 
                className="form-input" 
                value={loginPass}
                onChange={(e) => setLoginPass(e.target.value)}
                placeholder="Parolni kiriting"
                required
              />
            </div>

            <button 
              type="submit" 
              className="glow-btn"
              disabled={loading}
              style={{ width: '100%', padding: '14px', fontSize: '1rem' }}
            >
              {loading ? 'Kirilmoqda...' : 'Tizimga Kirish'}
            </button>
          </form>
        ) : (
          /* REGISTER FORM */
          <form onSubmit={handleRegisterSubmit}>
            <div style={{
              display: 'grid',
              gridTemplateColumns: '1fr 1fr',
              gap: '20px',
              marginBottom: '24px'
            }} className="reg-grid">
              
              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Foydalanuvchi nomi*</label>
                <input 
                  type="text" 
                  name="userName"
                  className="form-input" 
                  value={regData.userName}
                  onChange={handleRegChange}
                  placeholder="UserName"
                  required
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Parol*</label>
                <input 
                  type="password" 
                  name="password"
                  className="form-input" 
                  value={regData.password}
                  onChange={handleRegChange}
                  placeholder="Kamida 6 ta belgi"
                  required
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>To'liq ism (F.I.SH)*</label>
                <input 
                  type="text" 
                  name="fullName"
                  className="form-input" 
                  value={regData.fullName}
                  onChange={handleRegChange}
                  placeholder="Foydalanuvchi To'liq ismi"
                  required
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Qisqa ism</label>
                <input 
                  type="text" 
                  name="shortName"
                  className="form-input" 
                  value={regData.shortName}
                  onChange={handleRegChange}
                  placeholder="Masalan: Sohib"
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>JShShIR (PINFL)*</label>
                <input 
                  type="text" 
                  name="pinfl"
                  maxLength="14"
                  className="form-input" 
                  value={regData.pinfl}
                  onChange={handleRegChange}
                  placeholder="14 xonali son"
                  required
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Telefon raqam*</label>
                <input 
                  type="text" 
                  name="phoneNumber"
                  className="form-input" 
                  value={regData.phoneNumber}
                  onChange={handleRegChange}
                  placeholder="+998 90 123 45 67"
                  required
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Email*</label>
                <input 
                  type="email" 
                  name="email"
                  className="form-input" 
                  value={regData.email}
                  onChange={handleRegChange}
                  placeholder="example@mail.com"
                  required
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Pasport seriya va raqami*</label>
                <input 
                  type="text" 
                  name="passportSeries"
                  maxLength="9"
                  className="form-input" 
                  value={regData.passportSeries}
                  onChange={handleRegChange}
                  placeholder="AA1234567"
                  required
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Tug'ilgan sana</label>
                <input 
                  type="date" 
                  name="dateOfBirth"
                  className="form-input" 
                  value={regData.dateOfBirth}
                  onChange={handleRegChange}
                />
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>Manzil</label>
                <input 
                  type="text" 
                  name="address"
                  className="form-input" 
                  value={regData.address}
                  onChange={handleRegChange}
                  placeholder="Toshkent sh., Yunusobod"
                />
              </div>

            </div>

            <button 
              type="submit" 
              className="glow-btn"
              disabled={loading}
              style={{ width: '100%', padding: '14px', fontSize: '1rem', marginTop: '12px' }}
            >
              {loading ? 'Yaratilmoqda...' : 'Ro\'yxatdan O\'tish'}
            </button>
          </form>
        )}
      </div>
      
      {/* Responsive styles override */}
      <style>{`
        @media (max-width: 600px) {
          .reg-grid {
            grid-template-columns: 1fr !important;
          }
        }
      `}</style>
    </div>
  );
}
