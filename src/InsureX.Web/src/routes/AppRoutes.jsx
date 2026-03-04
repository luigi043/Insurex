import React from 'react';
import { Routes, Route } from 'react-router-dom';
import MainLayout from '../layouts/MainLayout';
import AuthLayout from '../layouts/AuthLayout';

// Pages
import AdminDashboard from '../features/admin/pages/AdminDashboard';
import AssetManagementPage from '../features/assets/pages/AssetManagementPage';
import PolicyManagementPage from '../features/policies/pages/PolicyManagementPage';
import AddPolicyPage from '../features/policies/pages/AddPolicyPage';
import LoginPage from '../features/auth/pages/LoginPage';
import ChargeForm from '../features/billing/components/ChargeForm';
import MonthlyReport from '../features/reports/components/reportTypes/MonthlyReport';

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<MainLayout />}>
        <Route index element={<AdminDashboard />} />
        <Route path="Assets" element={<AssetManagementPage />} />
        <Route path="Policies" element={<PolicyManagementPage />} />
        <Route path="PolicyManagement/AddNewPolicy.aspx" element={<AddPolicyPage />} />
        <Route path="Billing/AdminBillingNewCharge.aspx" element={<ChargeForm />} />
        <Route path="Admin/AdminMonthlyReport.aspx" element={<MonthlyReport />} />
        <Route path="Admin/AdminHome.aspx" element={<AdminDashboard />} />
      </Route>
      <Route path="/Account" element={<AuthLayout />}>
        <Route path="Login.aspx" element={<LoginPage />} />
      </Route>
    </Routes>
  );
};

export default AppRoutes;
