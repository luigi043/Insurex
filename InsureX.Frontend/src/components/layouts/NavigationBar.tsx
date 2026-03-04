import React from 'react';
import { Link, useLocation } from 'react-router-dom';
import { useAuthStore } from '../../stores/authStore';

const NavigationBar: React.FC = () => {
  const location = useLocation();
  const logout = useAuthStore((state) => state.logout);
  const user = useAuthStore((state) => state.user);

  const navItems = [
    { name: 'Dashboard', path: '/dashboard', icon: '📊' },
    { name: 'Assets', path: '/assets', icon: '🚗' },
    { name: 'Compliance', path: '/compliance', icon: '🛡️' },
    { name: 'Cases', path: '/cases', icon: '📁' },
  ];

  return (
    <aside className="w-64 bg-white border-r border-gray-200 flex flex-col h-full shadow-lg">
      <div className="p-6 border-b border-gray-100 flex items-center gap-3">
        <div className="w-10 h-10 bg-blue-600 rounded-lg flex items-center justify-center text-white font-bold text-xl shadow-md">
          IX
        </div>
        <span className="text-xl font-bold text-blue-900 tracking-tight">InsureX</span>
      </div>

      <nav className="flex-1 p-4 space-y-1">
        {navItems.map((item) => (
          <Link
            key={item.path}
            to={item.path}
            className={`flex items-center gap-3 px-4 py-3 rounded-xl transition-all duration-200 group ${
              location.pathname === item.path
                ? 'bg-blue-50 text-blue-700 font-semibold'
                : 'text-gray-600 hover:bg-gray-50 hover:text-blue-600'
            }`}
          >
            <span className="text-xl group-hover:scale-110 transition-transform">{item.icon}</span>
            {item.name}
          </Link>
        ))}
      </nav>

      <div className="p-4 border-t border-gray-100">
        <div className="mb-4 px-4 py-3 bg-gray-50 rounded-xl">
          <p className="text-xs font-semibold text-gray-500 uppercase tracking-widest">Logged in as</p>
          <p className="text-sm font-bold text-gray-900 truncate">{user?.clientId || 'N/A'}</p>
          <p className="text-[10px] text-gray-400 mt-0.5">Tenant ID: {user?.tenantId || 'System'}</p>
        </div>
        <button
          onClick={logout}
          className="w-full flex items-center justify-center gap-2 px-4 py-3 text-sm font-semibold text-red-600 hover:bg-red-50 rounded-xl transition-colors duration-200"
        >
          🚪 Logout
        </button>
      </div>
    </aside>
  );
};

export default NavigationBar;
