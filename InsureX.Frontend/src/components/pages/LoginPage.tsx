import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthStore } from '../../stores/authStore';

const LoginPage: React.FC = () => {
  const [clientId, setClientId] = useState('');
  const [clientSecret, setClientSecret] = useState('');
  const setToken = useAuthStore((state) => state.setToken);
  const navigate = useNavigate();

  const handleLogin = (e: React.FormEvent) => {
    e.preventDefault();
    // In a real app, we'd call the OAuth2 /token endpoint.
    // For this prototype, we'll simulate success if any credentials are provided.
    if (clientId && clientSecret) {
      // Fake token generation (in reality, the API would return a real JWT)
      const fakeToken = btoa(JSON.stringify({ 
        clientId, 
        tenantId: Math.floor(Math.random() * 1000), 
        scopes: ['compliance:read', 'audit:read'],
        exp: Math.floor(Date.now() / 1000) + 3600,
        iat: Math.floor(Date.now() / 1000)
      }));
      setToken(fakeToken);
      navigate('/dashboard');
    }
  };

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50 px-4">
      <div className="max-w-md w-full space-y-8 bg-white p-10 rounded-3xl shadow-xl border border-gray-100">
        <div className="text-center">
          <div className="mx-auto h-16 w-16 bg-blue-600 rounded-2xl flex items-center justify-center text-white text-3xl font-black shadow-lg">IX</div>
          <h2 className="mt-6 text-3xl font-extrabold text-gray-900 tracking-tight">InsureX Platform</h2>
          <p className="mt-2 text-sm font-medium text-gray-500">Sign in to your corporate account</p>
        </div>
        
        <form className="mt-8 space-y-6" onSubmit={handleLogin}>
          <div className="space-y-4">
            <div>
              <label className="text-xs font-bold text-gray-400 uppercase tracking-widest ml-1">Client ID</label>
              <input
                type="text"
                required
                className="mt-1 block w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all outline-none"
                placeholder="Enter Client ID"
                value={clientId}
                onChange={(e) => setClientId(e.target.value)}
              />
            </div>
            <div>
              <label className="text-xs font-bold text-gray-400 uppercase tracking-widest ml-1">Client Secret</label>
              <input
                type="password"
                required
                className="mt-1 block w-full px-4 py-3 rounded-xl border border-gray-200 focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all outline-none"
                placeholder="••••••••"
                value={clientSecret}
                onChange={(e) => setClientSecret(e.target.value)}
              />
            </div>
          </div>

          <button
            type="submit"
            className="w-full flex justify-center py-4 px-4 border border-transparent text-sm font-black rounded-xl text-white bg-blue-600 hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-blue-500 shadow-md transition-all active:scale-95"
          >
            Authenticate
          </button>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
