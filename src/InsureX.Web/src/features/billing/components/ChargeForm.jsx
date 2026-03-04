import React from 'react';

const AdminBillingNewCharge = () => {
  return (
    <div className="card-section" style={{ padding: '24px' }}>
      <h2 className="card-title" style={{ marginBottom: '16px' }}>Create New Billing Charge</h2>
      <p>Enter billing details in ZAR (South African Rand). This will generate an invoice for the selected partner.</p>
      
      <form style={{ marginTop: '24px', maxWidth: '600px' }}>
        <div style={{ marginBottom: '16px' }}>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 500 }}>Partner / Organisation</label>
          <select style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid var(--border)' }}>
            <option>Select Partner</option>
            <option>Standard Bank</option>
            <option>Absa</option>
          </select>
        </div>

        <div style={{ marginBottom: '16px' }}>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 500 }}>Charge Amount (ZAR)</label>
          <input type="number" placeholder="R 0.00" style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid var(--border)' }} />
        </div>

        <button type="button" className="btn-primary">Generate Invoice</button>
      </form>
    </div>
  );
};

export default AdminBillingNewCharge;
