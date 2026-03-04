import React from 'react';

const AdminHome = () => {
  return (
    <div className="card-section" style={{ padding: '24px' }}>
      <h2 className="card-title" style={{ marginBottom: '16px' }}>Administration Dashboard</h2>
      <p>Welcome to the admin central control panel. Manage partner users, bulk imports, and system configurations.</p>
      
      <div className="dashboard-grid" style={{ marginTop: '24px' }}>
        <div className="stat-card">
          <div className="stat-info">
            <h3>Manage Partners</h3>
            <div className="value">12</div>
          </div>
          <div className="stat-icon primary">👥</div>
        </div>
        <div className="stat-card">
          <div className="stat-info">
            <h3>Bulk Imports</h3>
            <div className="value">3</div>
          </div>
          <div className="stat-icon warning">☁️</div>
        </div>
      </div>
    </div>
  );
};

export default AdminHome;
