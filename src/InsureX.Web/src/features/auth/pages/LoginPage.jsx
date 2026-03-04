import React from 'react';

// South Africa (English) locale applied
const AccountLogin = () => {
  return (
    <div className="card-section" style={{ maxWidth: '400px', margin: '0 auto', marginTop: '40px' }}>
      <div className="card-header">
        <h2 className="card-title">Sign In</h2>
      </div>
      <div style={{ padding: '24px' }}>
        <p>Please enter your credentials to log in.</p>
        <div style={{ marginTop: '16px' }}>
          <label style={{ display: 'block', marginBottom: '8px' }}>Email Address</label>
          <input type="email" style={{ width: '100%', padding: '8px', border: '1px solid var(--border)', borderRadius: '4px' }} />
        </div>
        <div style={{ marginTop: '16px' }}>
          <label style={{ display: 'block', marginBottom: '8px' }}>Password</label>
          <input type="password" style={{ width: '100%', padding: '8px', border: '1px solid var(--border)', borderRadius: '4px' }} />
        </div>
        <button className="btn-primary" style={{ marginTop: '24px', width: '100%', justifyContent: 'center' }}>
          Log In
        </button>
      </div>
    </div>
  );
};

export default AccountLogin;
