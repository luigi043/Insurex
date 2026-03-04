import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import { useAuthStore } from '../../stores/authStore';
import NavigationBar from './NavigationBar';

const MainLayout: React.FC = () => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  if (!isAuthenticated) {
    // Redirect to login if not authenticated
    // Note: /login route will be defined in App.tsx
    return <Navigate to="/login" replace />;
  }

  return (
    <div className="flex h-screen bg-gray-50 text-gray-900 font-sans">
      <NavigationBar />
      
      <main className="flex-1 overflow-auto">
        <div className="container mx-auto px-6 py-8">
          <Outlet />
        </div>
      </main>
    </div>
  );
};

export default MainLayout;
