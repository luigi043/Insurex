import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './components/layouts/MainLayout';
import AddUserPage from './components/pages/AddUserPage';
import RegisterTenantPage from './components/pages/RegisterTenantPage';

import DashboardPage from './components/pages/DashboardPage';
import AssetsPage from './components/pages/AssetsPage';
import AssetDetailPage from './components/pages/AssetDetailPage';
import CasesPage from './components/pages/CasesPage';
import CompliancePage from './components/pages/CompliancePage';
import AuditPage from './components/pages/AuditPage';
import UsersPage from './components/pages/UsersPage';
import TenantsPage from './components/pages/TenantsPage';
import LoginPage from './components/pages/LoginPage';
import PolicyPage from './components/pages/PolicyPage';
import BillingPage from './components/pages/BillingPage';
import ReportingPage from './components/pages/ReportingPage';
import AddAssetPage from './components/pages/AddAssetPage';
import AddPolicyPage from './components/pages/AddPolicyPage';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public Routes */}
        <Route path="/login" element={<LoginPage />} />

        {/* Protected Routes */}
        <Route element={<MainLayout />}>
          <Route path="dashboard" element={<DashboardPage />} />
          <Route path="assets" element={<AssetsPage />} />
          <Route path="assets/:id" element={<AssetDetailPage />} />
          <Route path="policies" element={<PolicyPage />} />
          <Route path="billing" element={<BillingPage />} />
          <Route path="reports" element={<ReportingPage />} />
          <Route path="assets/new" element={<AddAssetPage />} />
          <Route path="policies/new" element={<AddPolicyPage />} />
          <Route path="compliance" element={<CompliancePage />} />
          <Route path="cases" element={<CasesPage />} />
          <Route path="audit" element={<AuditPage />} />
          <Route path="users" element={<UsersPage />} />
          <Route path="users/new" element={<AddUserPage />} />
          <Route path="users/:id/edit" element={<AddUserPage />} />
          <Route path="tenants" element={<TenantsPage />} />
          <Route path="tenants/new" element={<RegisterTenantPage />} />
          <Route path="tenants/:id/edit" element={<RegisterTenantPage />} />
        </Route>

        {/* Fallback */}
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="*" element={<div className="flex items-center justify-center min-h-screen text-2xl font-bold text-gray-300">404 | Not Found</div>} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;

