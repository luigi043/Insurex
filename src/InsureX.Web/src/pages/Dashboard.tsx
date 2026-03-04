import React from 'react';

const Dashboard: React.FC = () => {
  return (
    <div>
      <div className="dashboard-grid">
        <div className="stat-card">
          <div className="stat-info">
            <h3>Total Assets</h3>
            <div className="value">1,248</div>
            <div className="trend up">↑ 12% this month</div>
          </div>
          <div className="stat-icon primary">🏢</div>
        </div>

        <div className="stat-card">
          <div className="stat-info">
            <h3>Active Policies</h3>
            <div className="value">856</div>
            <div className="trend up">↑ 5% this month</div>
          </div>
          <div className="stat-icon success">📄</div>
        </div>

        <div className="stat-card">
          <div className="stat-info">
            <h3>Compliance Alerts</h3>
            <div className="value">24</div>
            <div className="trend danger">↓ action needed</div>
          </div>
          <div className="stat-icon warning">⚠️</div>
        </div>

        <div className="stat-card">
          <div className="stat-info">
            <h3>Open Cases</h3>
            <div className="value">12</div>
            <div className="trend down">↓ 3 closed today</div>
          </div>
          <div className="stat-icon danger">📋</div>
        </div>
      </div>

      <div className="card-section">
        <div className="card-header">
          <h2 className="card-title">Recent Activity</h2>
          <button className="btn-primary">View All</button>
        </div>
        <div className="table-responsive">
          <table>
            <thead>
              <tr>
                <th>Date</th>
                <th>Action</th>
                <th>Asset/Policy</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>Oct 24, 2024</td>
                <td>Policy Renewal</td>
                <td>Commercial Building A - POL-9932</td>
                <td><span className="badge active">Completed</span></td>
              </tr>
              <tr>
                <td>Oct 23, 2024</td>
                <td>Compliance Check</td>
                <td>Fleet Vehicles - Fleet-01</td>
                <td><span className="badge pending">Pending</span></td>
              </tr>
              <tr>
                <td>Oct 21, 2024</td>
                <td>Claim Filed</td>
                <td>Warehouse C - Roof Damage</td>
                <td><span className="badge expired">Investigating</span></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
