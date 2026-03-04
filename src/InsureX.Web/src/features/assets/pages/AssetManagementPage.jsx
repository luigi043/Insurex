import React from 'react';

const Assets = () => {
  return (
    <div>
      <div className="card-section">
        <div className="card-header">
          <h2 className="card-title">Insured Assets Register</h2>
          <button className="btn-primary">+ Add New Asset</button>
        </div>
        <div className="table-responsive">
          <table>
            <thead>
              <tr>
                <th>Asset ID</th>
                <th>Description</th>
                <th>Location</th>
                <th>Value</th>
                <th>Status</th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td>AST-001</td>
                <td>Commercial Office Building A</td>
                <td>New York, NY</td>
                <td>$12,500,000</td>
                <td><span className="badge active">Insured</span></td>
              </tr>
              <tr>
                <td>AST-002</td>
                <td>Logistics Warehouse B</td>
                <td>Chicago, IL</td>
                <td>$8,200,000</td>
                <td><span className="badge active">Insured</span></td>
              </tr>
              <tr>
                <td>AST-003</td>
                <td>Fleet Vehicles (Class A)</td>
                <td>National</td>
                <td>$4,150,000</td>
                <td><span className="badge pending">Renewal Due</span></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default Assets;
