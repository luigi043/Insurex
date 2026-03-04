import React, { useEffect, useState } from 'react';
import { API } from '../../api/utils/api';
import { DashboardSummary } from '../../api/types/Dashboard';
import { ApiResponse } from '../../api/types/Common';
import DashboardChart from '../shared/DashboardChart';
import { 
  ShieldCheck, 
  AlertCircle, 
  TrendingUp, 
  DollarSign, 
  Car, 
  Clock, 
  ChevronRight,
  RefreshCw
} from 'lucide-react';

const DashboardPage: React.FC = () => {
  const [summary, setSummary] = useState<DashboardSummary | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);

  const fetchSummary = async () => {
    setRefreshing(true);
    try {
      const response = await API.get<ApiResponse<DashboardSummary>>('/dashboard/summary');
      const chartsResponse = await API.get<ApiResponse<any>>('/dashboard/charts');
      
      const data = response.data.data;
      data.charts = chartsResponse.data.data;
      
      setSummary(data);
    } catch (error) {
      console.error('Failed to fetch dashboard data:', error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  };

  useEffect(() => {
    fetchSummary();
  }, []);

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center h-full min-h-[400px]">
        <RefreshCw className="w-10 h-10 text-blue-500 animate-spin mb-4" />
        <p className="text-gray-500 font-medium">Synchronizing Ledger Data...</p>
      </div>
    );
  }

  const statCards = [
    { 
      title: 'Total Assets', 
      value: summary?.totalAssets || 0, 
      sub: `$${summary?.totalValue?.toLocaleString()}`, 
      icon: <Car className="w-5 h-5" />,
      color: 'bg-blue-500', 
      textColor: 'text-blue-600' 
    },
    { 
      title: 'Uninsured', 
      value: summary?.uninsuredAssets || 0, 
      sub: `${summary?.uninsuredPercentage?.toFixed(1)}% Exposure`, 
      icon: <AlertCircle className="w-5 h-5" />,
      color: 'bg-red-500', 
      textColor: 'text-red-600' 
    },
    { 
      title: 'Adequately Insured', 
      value: summary?.adequatelyInsuredAssets || 0, 
      sub: `$${summary?.adequatelyInsuredValue?.toLocaleString()}`, 
      icon: <ShieldCheck className="w-5 h-5" />,
      color: 'bg-emerald-500', 
      textColor: 'text-emerald-600' 
    },
    { 
      title: 'Underinsured', 
      value: summary?.underInsuredAssets || 0, 
      sub: `$${summary?.underInsuredValue?.toLocaleString()}`, 
      icon: <TrendingUp className="w-5 h-5" />,
      color: 'bg-amber-500', 
      textColor: 'text-amber-600' 
    },
  ];

  return (
    <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header className="flex justify-between items-end">
        <div>
          <div className="flex items-center gap-2 mb-1">
            <span className="px-2.5 py-0.5 rounded-full bg-blue-100 text-blue-700 text-[10px] font-bold uppercase tracking-wider">Live Ledger</span>
            <span className="text-gray-300">|</span>
            <span className="text-xs text-gray-400 font-medium flex items-center gap-1">
              <Clock className="w-3 h-3" /> Last updated: {new Date().toLocaleTimeString()}
            </span>
          </div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Portfolio Summary</h1>
        </div>
        <button 
          onClick={fetchSummary}
          disabled={refreshing}
          className="flex items-center gap-2 px-4 py-2.5 bg-white border border-gray-200 rounded-xl text-sm font-bold text-gray-700 hover:bg-gray-50 hover:shadow-sm transition-all active:scale-95 disabled:opacity-50"
        >
          <RefreshCw className={`w-4 h-4 ${refreshing ? 'animate-spin' : ''}`} />
          Refresh
        </button>
      </header>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {statCards.map((card) => (
          <div key={card.title} className="bg-white p-6 rounded-3xl shadow-sm border border-gray-100 group hover:border-blue-200 transition-all duration-300">
            <div className="flex justify-between items-start">
              <div className={`p-2.5 rounded-2xl ${card.color} text-white shadow-lg shadow-${card.color.split('-')[1]}-200`}>
                {card.icon}
              </div>
              {/* Sparkline placeholder */}
              <div className="flex gap-1 items-end h-8">
                {[4, 7, 5, 8, 6].map((h, i) => (
                  <div key={i} className={`w-1 rounded-full ${card.color} opacity-${(i + 1) * 20}`} style={{ height: `${h * 10}%` }}></div>
                ))}
              </div>
            </div>
            <div className="mt-4">
              <h3 className="text-xs font-bold text-gray-400 uppercase tracking-widest leading-none">{card.title}</h3>
              <p className={`text-4xl font-black mt-2 tracking-tight ${card.textColor}`}>{card.value.toLocaleString()}</p>
              <p className="text-sm font-semibold text-gray-500 mt-1 flex items-center gap-1">
                {card.sub}
              </p>
            </div>
          </div>
        ))}
      </div>

      {/* Charts Zone */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        {summary?.charts && summary.charts.length > 0 ? (
          summary.charts.map((series, idx) => (
            <div key={idx} className="bg-white p-8 rounded-[2rem] shadow-sm border border-gray-100 hover:shadow-md transition-shadow">
              <DashboardChart series={series} type={idx % 2 === 0 ? 'bar' : 'pie'} />
            </div>
          ))
        ) : (
          <div className="col-span-2 bg-white p-12 rounded-[2.5rem] border border-dashed border-gray-200 flex flex-col items-center justify-center text-gray-400">
             <div className="w-16 h-16 bg-gray-50 rounded-full flex items-center justify-center mb-4">📈</div>
             <p className="font-semibold">No chart data available for current selection.</p>
          </div>
        )}
      </div>

      {/* Bottom Actions/Quick view */}
      <div className="bg-blue-900 rounded-[2.5rem] p-8 text-white relative overflow-hidden shadow-2xl">
        <div className="relative z-10 flex flex-col md:flex-row md:items-center justify-between gap-6">
          <div className="max-w-md">
            <h2 className="text-2xl font-bold mb-2">Detailed Compliance Ledger</h2>
            <p className="text-blue-200 text-sm font-medium">Export raw asset data or view deep-dive compliance trails for auditing purposes.</p>
          </div>
          <div className="flex gap-3">
            <button className="px-6 py-3 bg-white text-blue-900 font-bold rounded-2xl hover:bg-blue-50 transition-colors shadow-lg">
              View Asset List
            </button>
            <button className="px-6 py-3 bg-blue-800 text-white font-bold rounded-2xl hover:bg-blue-700 transition-colors border border-blue-700">
              Audit Logs
            </button>
          </div>
        </div>
        {/* Decorative background circle */}
        <div className="absolute -right-20 -bottom-20 w-80 h-80 bg-blue-800 rounded-full opacity-50 blur-3xl"></div>
      </div>
    </div>
  );
};

export default DashboardPage;
