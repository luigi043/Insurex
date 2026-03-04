import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import Assets from './pages/Assets';
import Policies from './pages/Policies';
import Compliance from './pages/Compliance';
import Cases from './pages/Cases';

// Imported Legacy Mappings
import AccountLogin from './pages/Account/Login';
import AdminHome from './pages/Admin/AdminHome';
import AdminBillingNewCharge from './pages/Billing/AdminBillingNewCharge';
import AddNewPolicy from './pages/PolicyManagement/AddNewPolicy';
import AdminMonthlyReport from './pages/Reporting/AdminMonthlyReport';

import './index.css';

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Layout />}>
          <Route index element={<Dashboard />} />
          <Route path="assets" element={<Assets />} />
          <Route path="policies" element={<Policies />} />
          <Route path="compliance" element={<Compliance />} />
          <Route path="cases" element={<Cases />} />
          
          <Route path="account/login" element={<AccountLogin />} />
          <Route path="admin" element={<AdminHome />} />
          <Route path="billing" element={<AdminBillingNewCharge />} />
          <Route path="policy-management/new" element={<AddNewPolicy />} />
          <Route path="reporting" element={<AdminMonthlyReport />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
