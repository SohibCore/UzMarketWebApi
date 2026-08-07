import React, { useState, useEffect, useRef } from 'react';
import { api } from '../services/api';

export default function Auth({ onLoginSuccess }) {
  const [isLogin, setIsLogin] = useState(true);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  
  // Registration Flow Step: 1 = PINFL Entry, 2 = Registration Form, 3 = OTP Verification
  const [regStep, setRegStep] = useState(1);
  const [pinflInput, setPinflInput] = useState('');
  const [autoFilled, setAutoFilled] = useState(false);

  // Login Form States
  const [loginUser, setLoginUser] = useState('');
  const [loginPass, setLoginPass] = useState('');
  const [showLoginPassword, setShowLoginPassword] = useState(false);
  const [showRegisterPassword, setShowRegisterPassword] = useState(false);

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
  const [pendingEmail, setPendingEmail] = useState('');
  const [verificationCode, setVerificationCode] = useState('');
  const [verificationInfo, setVerificationInfo] = useState('');

  // Professional OTP & Resend Timer States
  const [otp, setOtp] = useState(['', '', '', '', '', '']);
  const [timer, setTimer] = useState(60);
  const [canResend, setCanResend] = useState(false);
  const inputRefs = useRef([]);

  useEffect(() => {
    let interval = null;
    if (regStep === 3 && timer > 0) {
      interval = setInterval(() => {
        setTimer(prev => prev - 1);
      }, 1000);
    } else if (timer === 0) {
      setCanResend(true);
    }
    return () => clearInterval(interval);
  }, [regStep, timer]);

  const getMaskedEmail = (email) => {
    if (!email || !email.includes('@')) return email;
    const [name, domain] = email.split('@');
    if (name.length <= 2) return `${name[0]}*@${domain}`;
    return `${name[0]}${'*'.repeat(Math.min(name.length - 2, 5))}${name[name.length - 1]}@${domain}`;
  };

  // --- Step 1: PINFL Lookup Handler ---
  const handlePinflSubmit = async (e) => {
    e.preventDefault();
    const cleanedPinfl = pinflInput.trim();
    if (!cleanedPinfl || cleanedPinfl.length !== 14 || !/^\d{14}$/.test(cleanedPinfl)) {
      setError("Iltimos, 14 xonali JShShIR (PINFL) raqamingizni to'g'ri kiriting.");
      return;
    }

    setLoading(true);
    setError('');

    try {
      const personInfo = await api.uzasbo.getPersonInfo(cleanedPinfl);
      
      const fetchedName = personInfo?.name || personInfo?.shortName || '';
      const fetchedAddress = personInfo?.address || '';
      const fetchedPinfl = personInfo?.personalNum || cleanedPinfl;

      setRegData(prev => ({
        ...prev,
        pinfl: fetchedPinfl,
        fullName: fetchedName || prev.fullName,
        shortName: personInfo?.shortName || prev.shortName || (fetchedName ? fetchedName.split(' ')[0] : ''),
        address: fetchedAddress || prev.address
      }));

      setAutoFilled(true);
      setRegStep(2);
    } catch (err) {
      // Fallback if uzasbo endpoint fails: allow user to continue to step 2 with entered pinfl
      setRegData(prev => ({
        ...prev,
        pinfl: cleanedPinfl
      }));
      setAutoFilled(false);
      setRegStep(2);
    } finally {
      setLoading(false);
    }
  };

  const handleOtpChange = (index, value) => {
    const digit = value.replace(/\D/g, '');
    if (!digit && value !== '') return;

    const newOtp = [...otp];
    newOtp[index] = digit ? digit.slice(-1) : '';
    setOtp(newOtp);

    const fullCode = newOtp.join('');
    setVerificationCode(fullCode);

    if (digit && index < 5) {
      inputRefs.current[index + 1]?.focus();
    }
  };

  const handleOtpKeyDown = (index, e) => {
    if (e.key === 'Backspace' && !otp[index] && index > 0) {
      inputRefs.current[index - 1]?.focus();
    }
  };

  const handleOtpPaste = (e) => {
    e.preventDefault();
    const pastedData = e.clipboardData.getData('text').trim().replace(/\D/g, '').slice(0, 6);
    if (pastedData) {
      const newOtp = ['', '', '', '', '', ''];
      for (let i = 0; i < pastedData.length; i++) {
        newOtp[i] = pastedData[i];
      }
      setOtp(newOtp);
      setVerificationCode(newOtp.join(''));
      const targetIndex = Math.min(pastedData.length, 5);
      inputRefs.current[targetIndex]?.focus();
    }
  };

  const handleResendCode = async () => {
    if (!canResend || loading) return;
    setLoading(true);
    setError('');
    try {
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

      await api.auth.register(formattedData, regData.pinfl);
      setTimer(60);
      setCanResend(false);
      setOtp(['', '', '', '', '', '']);
      setVerificationCode('');
      setVerificationInfo('Yangi tasdiqlash kodi emailga yuborildi!');
    } catch (err) {
      setError(err.message || 'Kodni qayta yuborishda xatolik yuz berdi.');
    } finally {
      setLoading(false);
    }
  };

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

  const handleVerificationSubmit = async (e) => {
    e.preventDefault();
    const codeToVerify = verificationCode || otp.join('');
    if (!pendingEmail || codeToVerify.length < 6) {
      setError('Iltimos, 6 xonali tasdiqlash kodini to\'liq kiriting.');
      return;
    }

    setLoading(true);
    setError('');

    try {
      const data = await api.auth.verifyEmail(pendingEmail, codeToVerify);
      onLoginSuccess(data);
    } catch (err) {
      setError(err.message || 'Verifikatsiya kodi noto‘g‘ri yoki muddati tugagan.');
    } finally {
      setLoading(false);
    }
  };

  const handleRegisterSubmit = async (e) => {
    e.preventDefault();
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
      
      await api.auth.register(formattedData, regData.pinfl);
      setPendingEmail(regData.email);
      setRegStep(3); // Go to OTP verification step
      setOtp(['', '', '', '', '', '']);
      setVerificationCode('');
      setTimer(60);
      setCanResend(false);
      setVerificationInfo('Sizning emailingizga tasdiqlash kodi yuborildi.');
    } catch (err) {
      const message = err?.message || 'Ro\'yxatdan o\'tishda xatolik yuz berdi.';
      setError(message);
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

  const switchTab = (toLogin) => {
    setIsLogin(toLogin);
    setError('');
    setRegStep(1);
    setPinflInput('');
    setAutoFilled(false);
    setVerificationCode('');
    setVerificationInfo('');
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
          maxWidth: isLogin ? '560px' : regStep === 3 ? '620px' : regStep === 1 ? '580px' : '780px',
          padding: '60px',
          boxShadow: 'var(--shadow-lg)',
          border: '1px solid rgba(255, 255, 255, 0.1)',
          transition: 'max-width var(--transition-normal)'
        }}
      >
        {/* Toggle tabs */}
        {regStep !== 3 && (
          <div style={{
            display: 'flex',
            justifyContent: 'center',
            gap: '24px',
            marginBottom: '32px',
            borderBottom: '1px solid var(--border-color)',
            paddingBottom: '12px'
          }}>
            <h2 
              onClick={() => switchTab(true)}
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
              onClick={() => switchTab(false)}
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
        )}

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
            <form onSubmit={handleLoginSubmit} className="login-form fade-in">
              <div className="floating-label" style={{ marginBottom: '24px' }}>
                <input
                  type="text"
                  className="form-input glass-input"
                  value={loginUser}
                  onChange={(e) => setLoginUser(e.target.value)}
                  placeholder=" "
                  required
                />
                <label>Foydalanuvchi nomi (UserName)</label>
              </div>

              <div className="floating-label" style={{ marginBottom: '32px' }}>
                <input
                  type={showLoginPassword ? 'text' : 'password'}
                  className="form-input glass-input"
                  value={loginPass}
                  onChange={(e) => setLoginPass(e.target.value)}
                  placeholder=" "
                  style={{ paddingRight: '48px' }}
                  required
                />
                <label>Parol</label>
                <button
                  type="button"
                  onClick={() => setShowLoginPassword(prev => !prev)}
                  aria-label={showLoginPassword ? 'Parolni yashirish' : 'Parolni ko\'rsatish'}
                  title={showLoginPassword ? 'Parolni yashirish' : 'Parolni ko\'rsatish'}
                  style={{
                    position: 'absolute',
                    right: '12px',
                    top: '50%',
                    transform: 'translateY(-50%)',
                    width: '28px',
                    height: '28px',
                    display: 'inline-flex',
                    alignItems: 'center',
                    justifyContent: 'center',
                    background: 'transparent',
                    border: 'none',
                    color: 'var(--text-muted)',
                    cursor: 'pointer',
                    padding: 0
                  }}
                >
                  {showLoginPassword ? (
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                      <path d="M17.94 17.94A10.94 10.94 0 0 1 12 20C7 20 2.73 16.89 1 12a11.64 11.64 0 0 1 5.06-5.94"></path>
                      <path d="M9.9 4.24A10.69 10.69 0 0 1 12 4c5 0 9.27 3.11 11 8a11.64 11.64 0 0 1-2.22 3.42"></path>
                      <path d="M14.12 14.12a3 3 0 0 1-4.24-4.24"></path>
                      <path d="M1 1l22 22"></path>
                    </svg>
                  ) : (
                    <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                      <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
                      <circle cx="12" cy="12" r="3"></circle>
                    </svg>
                  )}
                </button>
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
        ) : regStep === 1 ? (
          /* STEP 1: PINFL CHECK STEP */
          <form onSubmit={handlePinflSubmit} className="fade-in">
            <div style={{ textAlign: 'center', marginBottom: '28px' }}>
              <div style={{
                width: '64px',
                height: '64px',
                borderRadius: '50%',
                background: 'linear-gradient(135deg, rgba(99, 102, 241, 0.2), rgba(168, 85, 247, 0.2))',
                border: '1px solid rgba(99, 102, 241, 0.4)',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                margin: '0 auto 16px auto',
                boxShadow: '0 0 20px rgba(99, 102, 241, 0.25)'
              }}>
                <svg width="30" height="30" viewBox="0 0 24 24" fill="none" stroke="var(--accent-indigo)" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z"></path>
                  <circle cx="12" cy="10" r="3"></circle>
                </svg>
              </div>

              <span style={{
                fontSize: '0.78rem',
                fontWeight: 700,
                color: 'var(--accent-indigo)',
                letterSpacing: '0.08em',
                textTransform: 'uppercase',
                background: 'rgba(99, 102, 241, 0.12)',
                padding: '4px 12px',
                borderRadius: '12px',
                border: '1px solid rgba(99, 102, 241, 0.25)',
                display: 'inline-block',
                marginBottom: '10px'
              }}>
                1-Qadam: Shaxsni Identifikatsiyalash
              </span>

              <h3 style={{
                fontFamily: 'var(--font-display)',
                fontSize: '1.35rem',
                fontWeight: 700,
                marginBottom: '8px',
                color: 'var(--text-main)'
              }}>
                JShShIR (PINFL) raqamingizni kiriting
              </h3>

              <p style={{ color: 'var(--text-muted)', fontSize: '0.88rem', lineHeight: '1.5' }}>
                Ro'yxatdan o'tishni boshlash uchun 14 xonali JShShIR raqamingizni kiriting.
              </p>
            </div>

            <div className="form-group" style={{ marginBottom: '28px' }}>
              <label>JShShIR (PINFL)*</label>
              <input
                type="text"
                maxLength="14"
                inputMode="numeric"
                className="form-input"
                value={pinflInput}
                onChange={(e) => setPinflInput(e.target.value.replace(/\D/g, ''))}
                placeholder=""
                style={{
                  letterSpacing: '0.12em',
                  fontSize: '1.1rem',
                  fontWeight: 600,
                  textAlign: 'center'
                }}
                required
                autoFocus
              />
            </div>

            <button
              type="submit"
              className="glow-btn"
              disabled={loading || pinflInput.length !== 14}
              style={{ width: '100%', padding: '14px', fontSize: '1rem' }}
            >
              {loading ? 'Ma\'lumotlar aniqlanmoqda...' : 'Davom etish →'}
            </button>
          </form>
        ) : regStep === 3 ? (
          /* STEP 3: PROFESSIONAL 6-DIGIT OTP VERIFICATION FORM */
          <form onSubmit={handleVerificationSubmit} className="fade-in">
            <div style={{ textAlign: 'center', marginBottom: '28px' }}>
              <div style={{
                width: '64px',
                height: '64px',
                borderRadius: '50%',
                background: 'linear-gradient(135deg, rgba(99, 102, 241, 0.2), rgba(168, 85, 247, 0.2))',
                border: '1px solid rgba(99, 102, 241, 0.4)',
                display: 'flex',
                alignItems: 'center',
                justifyContent: 'center',
                margin: '0 auto 16px auto',
                boxShadow: '0 0 20px rgba(99, 102, 241, 0.3)'
              }}>
                <svg width="28" height="28" viewBox="0 0 24 24" fill="none" stroke="var(--accent-indigo)" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                  <path d="M4 4h16c1.1 0 2 .9 2 2v12c0 1.1-.9 2-2 2H4c-1.1 0-2-.9-2-2V6c0-1.1.9-2 2-2z"></path>
                  <polyline points="22,6 12,13 2,6"></polyline>
                </svg>
              </div>

              <h3 style={{
                fontFamily: 'var(--font-display)',
                fontSize: '1.4rem',
                fontWeight: 700,
                marginBottom: '8px',
                color: 'var(--text-main)'
              }}>
                Emailni Tasdiqlash
              </h3>

              <p style={{ color: 'var(--text-muted)', fontSize: '0.9rem', marginBottom: '10px' }}>
                Tasdiqlash kodi quyidagi emailga yuborildi:
              </p>

              <div style={{
                display: 'inline-flex',
                alignItems: 'center',
                gap: '8px',
                padding: '6px 16px',
                borderRadius: '20px',
                background: 'rgba(99, 102, 241, 0.12)',
                border: '1px solid rgba(99, 102, 241, 0.25)',
                color: 'var(--accent-indigo)',
                fontSize: '0.95rem',
                fontWeight: 600
              }}>
                <span>{getMaskedEmail(pendingEmail)}</span>
              </div>
            </div>

            {/* 6 Digit OTP Inputs */}
            <div style={{ marginBottom: '28px' }}>
              <label style={{
                display: 'block',
                textAlign: 'center',
                marginBottom: '12px',
                fontSize: '0.82rem',
                fontWeight: 600,
                color: 'var(--text-muted)',
                textTransform: 'uppercase',
                letterSpacing: '0.05em'
              }}>
                6 xonali tasdiqlash kodi
              </label>
              
              <div 
                onPaste={handleOtpPaste}
                style={{
                  display: 'flex',
                  justifyContent: 'center',
                  gap: '10px'
                }}
              >
                {otp.map((digit, index) => (
                  <input
                    key={index}
                    ref={(el) => (inputRefs.current[index] = el)}
                    type="text"
                    inputMode="numeric"
                    maxLength="1"
                    value={digit}
                    onChange={(e) => handleOtpChange(index, e.target.value)}
                    onKeyDown={(e) => handleOtpKeyDown(index, e)}
                    style={{
                      width: '46px',
                      height: '54px',
                      textAlign: 'center',
                      fontSize: '1.4rem',
                      fontWeight: '700',
                      borderRadius: 'var(--border-radius-sm)',
                      border: digit ? '2px solid var(--accent-indigo)' : '1px solid var(--border-color)',
                      background: digit ? 'rgba(99, 102, 241, 0.1)' : 'rgba(255, 255, 255, 0.04)',
                      color: 'var(--text-main)',
                      boxShadow: digit ? '0 0 12px rgba(99, 102, 241, 0.3)' : 'none',
                      outline: 'none',
                      transition: 'all 0.2s ease'
                    }}
                  />
                ))}
              </div>
            </div>

            {/* Resend Timer / Action */}
            <div style={{
              textAlign: 'center',
              marginBottom: '24px',
              fontSize: '0.9rem',
              color: 'var(--text-muted)'
            }}>
              {!canResend ? (
                <p>Kodni qaytadan yuborish: <span style={{ color: 'var(--accent-indigo)', fontWeight: 600 }}>00:{timer < 10 ? `0${timer}` : timer}</span></p>
              ) : (
                <button
                  type="button"
                  onClick={handleResendCode}
                  disabled={loading}
                  style={{
                    background: 'none',
                    border: 'none',
                    color: 'var(--accent-indigo)',
                    cursor: 'pointer',
                    fontWeight: 600,
                    fontSize: '0.9rem',
                    textDecoration: 'underline'
                  }}
                >
                  Kodni qaytadan yuborish
                </button>
              )}
            </div>

            <button
              type="submit"
              className="glow-btn"
              disabled={loading || otp.join('').length < 6}
              style={{ width: '100%', padding: '14px', fontSize: '1rem', marginBottom: '16px' }}
            >
              {loading ? 'Tekshirilmoqda...' : 'Emailni Tasdiqlash'}
            </button>

            <div style={{ textAlign: 'center' }}>
              <button
                type="button"
                onClick={() => {
                  setRegStep(2);
                  setOtp(['', '', '', '', '', '']);
                  setVerificationCode('');
                  setError('');
                }}
                style={{
                  background: 'none',
                  border: 'none',
                  color: 'var(--text-muted)',
                  cursor: 'pointer',
                  fontSize: '0.88rem'
                }}
              >
                ← Ma'lumotlarni o'zgartirish
              </button>
            </div>
          </form>
        ) : (
          /* STEP 2: REGISTER FORM WITH AUTO-FILLED FIELDS */
          <form onSubmit={handleRegisterSubmit} className="fade-in">
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
                <div style={{ position: 'relative' }}>
                  <input 
                    type={showRegisterPassword ? 'text' : 'password'} 
                    name="password"
                    className="form-input" 
                    value={regData.password}
                    onChange={handleRegChange}
                    placeholder="Kamida 6 ta belgi (Masalan: Pass123!)"
                    style={{ paddingRight: '48px' }}
                    required
                  />
                  <button
                    type="button"
                    onClick={() => setShowRegisterPassword(prev => !prev)}
                    aria-label={showRegisterPassword ? 'Parolni yashirish' : 'Parolni ko\'rsatish'}
                    title={showRegisterPassword ? 'Parolni yashirish' : 'Parolni ko\'rsatish'}
                    style={{
                      position: 'absolute',
                      right: '12px',
                      top: '50%',
                      transform: 'translateY(-50%)',
                      width: '28px',
                      height: '28px',
                      display: 'inline-flex',
                      alignItems: 'center',
                      justifyContent: 'center',
                      background: 'transparent',
                      border: 'none',
                      color: 'var(--text-muted)',
                      cursor: 'pointer',
                      padding: 0
                    }}
                  >
                    {showRegisterPassword ? (
                      <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M17.94 17.94A10.94 10.94 0 0 1 12 20C7 20 2.73 16.89 1 12a11.64 11.64 0 0 1 5.06-5.94"></path>
                        <path d="M9.9 4.24A10.69 10.69 0 0 1 12 4c5 0 9.27 3.11 11 8a11.64 11.64 0 0 1-2.22 3.42"></path>
                        <path d="M14.12 14.12a3 3 0 0 1-4.24-4.24"></path>
                        <path d="M1 1l22 22"></path>
                      </svg>
                    ) : (
                      <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round">
                        <path d="M1 12s4-8 11-8 11 8 11 8-4 8-11 8-11-8-11-8z"></path>
                        <circle cx="12" cy="12" r="3"></circle>
                      </svg>
                    )}
                  </button>
                </div>
              </div>

              <div className="form-group" style={{ marginBottom: 0 }}>
                <label>To'liq ism (F.I.SH)*</label>
                <input 
                  type="text" 
                  name="fullName"
                  className="form-input" 
                  value={regData.fullName}
                  readOnly
                  placeholder="Foydalanuvchi To'liq ismi"
                  style={{ background: 'rgba(255, 255, 255, 0.03)', opacity: 0.85, cursor: 'not-allowed' }}
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
                  readOnly
                  style={{ background: 'rgba(255, 255, 255, 0.03)', opacity: 0.85, cursor: 'not-allowed', letterSpacing: '0.05em' }}
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
                  placeholder="+998901234567"
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
                  readOnly
                  placeholder="Toshkent sh., Yunusobod"
                  style={{ background: 'rgba(255, 255, 255, 0.03)', opacity: 0.85, cursor: 'not-allowed' }}
                />
              </div>

            </div>

            <div style={{ display: 'flex', gap: '12px', marginTop: '16px' }}>
              <button 
                type="button" 
                onClick={() => setRegStep(1)}
                className="form-input"
                style={{ width: '35%', cursor: 'pointer', textAlign: 'center' }}
              >
                ← PINFL ni o'zgartirish
              </button>

              <button 
                type="submit" 
                className="glow-btn"
                disabled={loading}
                style={{ width: '65%', padding: '14px', fontSize: '1rem' }}
              >
                {loading ? 'Yaratilmoqda...' : 'Ro\'yxatdan O\'tish'}
              </button>
            </div>
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
