import React from 'react';
import { Outlet, Link, useLocation } from 'react-router-dom';

const Layout: React.FC = () => {
  const location = useLocation();

  const navItems = [
    { path: '/', label: 'Overview', icon: '📊' },
    { path: '/assets', label: 'Assets', icon: '🏢' },
    { path: '/policies', label: 'Policies', icon: '📄' },
    { path: '/policy-management/new', label: 'New Policy', icon: '➕' },
    { path: '/compliance', label: 'Compliance', icon: '🛡️' },
    { path: '/cases', label: 'Cases', icon: '⚖️' },
    { path: '/billing', label: 'Billing', icon: '💳' },
    { path: '/reporting', label: 'Reporting', icon: '📈' },
    { path: '/admin', label: 'Admin Panel', icon: '⚙️' },
    { path: '/account/login', label: 'Login', icon: '🔐' },
  ];

  return (
    <div className="app-container">
      {/* Sidebar */}
      <aside className="sidebar">
        <div className="sidebar-header">
          {/* Logo can be imported from the assets copied over */}
          <div style={{ background: 'var(--primary)', color: 'white', fontWeight: 'bold', padding: '6px 12px', borderRadius: '8px' }}>IX</div>
          <span className="sidebar-title">InsureX</span>
        </div>
        
        <nav className="sidebar-nav">
          {navItems.map((item) => (
            <Link 
              key={item.path} 
              to={item.path}
              className={`nav-item ${location.pathname === item.path ? 'active' : ''}`}
            >
              <i className="icon">{item.icon}</i>
              <span>{item.label}</span>
            </Link>
          ))}
        </nav>
      </aside>

      {/* Main Content */}
      <main className="main-content">
        {/* Top Header */}
        <header className="top-header">
          <h1 className="page-title">
            {navItems.find(i => i.path === location.pathname)?.label || 'Dashboard'}
          </h1>
          
          <div className="user-profile">
            <span style={{ fontSize: '0.875rem', fontWeight: 500 }}>Admin User</span>
            <div className="avatar">A</div>
          </div>
        </header>

        {/* Content Area */}
        <div className="content-area">
          <Outlet />
        </div>
      </main>
    </div>
  );
};

export default Layout;
