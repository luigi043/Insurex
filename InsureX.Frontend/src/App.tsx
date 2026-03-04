import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './components/layouts/MainLayout';
import DashboardPage from './components/pages/DashboardPage';
import AssetsPage from './components/pages/AssetsPage';
import AssetDetailPage from './components/pages/AssetDetailPage';
import CasesPage from './components/pages/CasesPage';
import CompliancePage from './components/pages/CompliancePage';
import LoginPage from './components/pages/LoginPage';

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
          <Route path="compliance" element={<CompliancePage />} />
          <Route path="cases" element={<CasesPage />} />
        </Route>

        {/* Fallback */}
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="*" element={<div className="flex items-center justify-center min-h-screen text-2xl font-bold text-gray-300">404 | Not Found</div>} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;
