import React from 'react';

const AddNewPolicy: React.FC = () => {
  return (
    <div className="card-section" style={{ padding: '24px' }}>
      <h2 className="card-title" style={{ marginBottom: '16px' }}>Initialise New Policy</h2>
      <p>Create a new policy record. Ensure all terms comply with South African insurance regulations.</p>
      
      <form style={{ marginTop: '24px', maxWidth: '600px' }}>
        <div style={{ marginBottom: '16px' }}>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 500 }}>Policy Holder Name</label>
          <input type="text" style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid var(--border)' }} />
        </div>

        <div style={{ marginBottom: '16px' }}>
          <label style={{ display: 'block', marginBottom: '8px', fontWeight: 500 }}>Policy Type</label>
          <select style={{ width: '100%', padding: '8px', borderRadius: '4px', border: '1px solid var(--border)' }}>
            <option>Asset Protection</option>
            <option>Vehicle Coverage</option>
            <option>Commercial Liability</option>
          </select>
        </div>

        <button type="button" className="btn-primary">Save Policy Details</button>
      </form>
    </div>
  );
};

export default AddNewPolicy;
