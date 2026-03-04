import React from 'react';

const AdminMonthlyReport = () => {
  return (
    <div className="card-section" style={{ padding: '24px' }}>
      <h2 className="card-title" style={{ marginBottom: '16px' }}>Monthly Reporting Suite</h2>
      <p>Generate performance and billing reports for the current financial period.</p>
      
      <div style={{ marginTop: '24px', display: 'flex', gap: '16px' }}>
        <button className="btn-primary">Export to PDF</button>
        <button className="btn-primary" style={{ background: '#10B981' }}>Export to CSV</button>
      </div>

      <div className="table-responsive" style={{ marginTop: '32px' }}>
          <table>
            <thead>
              <tr>
                <th>Report Name</th>
                <th>Generated On</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>October 2024 Summary (ZAR)</td>
                <td>31-Oct-2024</td>
                <td><span className="badge active">Ready</span></td>
              </tr>
            </tbody>
          </table>
      </div>
    </div>
  );
};

export default AdminMonthlyReport;
