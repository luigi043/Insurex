import React, { useEffect, useState } from 'react';
import { API } from '../../api/utils/api';
import { DashboardSummary } from '../../api/types/Dashboard';
import { ApiResponse } from '../../api/types/Common';

const DashboardPage: React.FC = () => {
  const [summary, setSummary] = useState<DashboardSummary | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchSummary = async () => {
      try {
        const response = await API.get<ApiResponse<DashboardSummary>>('/dashboard/summary');
        setSummary(response.data.data);
      } catch (error) {
        console.error('Failed to fetch dashboard summary:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchSummary();
  }, []);

  if (loading) return <div className="text-center p-12 animate-pulse text-gray-500">Loading Dashboard...</div>;

  const cards = [
    { title: 'Total Assets', value: summary?.totalAssets || 0, sub: `$${summary?.totalValue.toLocaleString()}`, color: 'blue' },
    { title: 'Uninsured', value: summary?.uninsuredAssets || 0, sub: `${summary?.uninsuredPercentage.toFixed(1)}% of total`, color: 'red' },
    { title: 'Adequate', value: summary?.adequatelyInsuredAssets || 0, sub: `$${summary?.adequatelyInsuredValue.toLocaleString()}`, color: 'green' },
    { title: 'Underinsured', value: summary?.underInsuredAssets || 0, sub: `$${summary?.underInsuredValue.toLocaleString()}`, color: 'yellow' },
  ];

  return (
    <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500">
      <header>
        <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Financial Dashboard</h1>
        <p className="text-gray-500 mt-2 font-medium">Compliance overview of your asset portfolio.</p>
      </header>

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {cards.map((card) => (
          <div key={card.title} className="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 hover:shadow-md transition-shadow duration-300">
            <h3 className="text-sm font-bold text-gray-500 uppercase tracking-widest">{card.title}</h3>
            <p className={`text-4xl font-black mt-2 text-${card.color}-600`}>{card.value}</p>
            <p className="text-sm font-medium text-gray-400 mt-1">{card.sub}</p>
          </div>
        ))}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <div className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 min-h-[400px] flex items-center justify-center text-gray-400 font-medium">
          Chart Integration Pending (Recharts/Chart.js)
        </div>
        <div className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 min-h-[400px] flex items-center justify-center text-gray-400 font-medium">
          Recent Alerts & Compliance Notifications
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
