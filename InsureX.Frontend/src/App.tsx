import React from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import MainLayout from './components/layouts/MainLayout';
import DashboardPage from './components/pages/DashboardPage';
import LoginPage from './components/pages/LoginPage';

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Routes>
        {/* Public Routes */}
        <Route path="/login" element={<LoginPage />} />

        {/* Protected Routes */}
        <Route element={<MainLayout />}>
          <Route path="/dashboard" element={<DashboardPage />} />
          <Route path="/assets" element={<div className="p-8 text-gray-400 font-medium italic text-center">Assets Module - Coming Soon</div>} />
          <Route path="/compliance" element={<div className="p-8 text-gray-400 font-medium italic text-center">Compliance Module - Coming Soon</div>} />
          <Route path="/cases" element={<div className="p-8 text-gray-400 font-medium italic text-center">Case Management - Coming Soon</div>} />
        </Route>

        {/* Fallback */}
        <Route path="/" element={<Navigate to="/dashboard" replace />} />
        <Route path="*" element={<div className="flex items-center justify-center min-h-screen text-2xl font-bold text-gray-300">404 | Not Found</div>} />
      </Routes>
    </BrowserRouter>
  );
};

export default App;
