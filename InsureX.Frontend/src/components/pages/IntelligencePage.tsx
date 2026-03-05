import React, { useEffect, useState } from 'react';
import { intelligenceClient } from '../../api/clients';
import { 
  XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, 
  AreaChart, Area, BarChart, Bar, Cell, PieChart, Pie
} from 'recharts';
import { 
  Brain, TrendingUp, ShieldAlert, Activity, ArrowUpRight, 
  Zap, Target, Loader2, Info
} from 'lucide-react';

const IntelligencePage: React.FC = () => {
  const [loading, setLoading] = useState(true);
  const [riskData, setRiskData] = useState<any[]>([]);
  const [trendData, setTrendData] = useState<any[]>([]);
  const [healthData, setHealthData] = useState<any[]>([]);

  const COLORS = ['#2563eb', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6'];

  useEffect(() => {
    const fetchData = async () => {
      setLoading(true);
      try {
        const [risk, trends, health] = await Promise.all([
          intelligenceClient.getRiskScore(),
          intelligenceClient.getTrends(),
          intelligenceClient.getTenantHealth()
        ]);
        setRiskData(risk.data || []);
        setTrendData(trends.data || []);
        setHealthData(health.data || []);
      } catch (error) {
        console.error('Failed to fetch intelligence data:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchData();
  }, []);

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center h-full min-h-[400px]">
        <Loader2 className="w-10 h-10 text-blue-600 animate-spin mb-4" />
        <p className="text-gray-500 font-bold uppercase tracking-widest text-xs">Processing Neural Insights...</p>
      </div>
    );
  }

  return (
    <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <div className="flex items-center gap-2 mb-1">
            <span className="px-2.5 py-0.5 rounded-full bg-indigo-100 text-indigo-700 text-[10px] font-bold uppercase tracking-wider">AI Powered</span>
            <span className="text-gray-300">|</span>
            <span className="text-xs text-gray-400 font-bold flex items-center gap-1 uppercase tracking-widest">
              Advanced Analytics Engine
            </span>
          </div>
          <h1 className="text-3xl font-black text-gray-900 tracking-tight flex items-center gap-3">
            <Brain className="w-8 h-8 text-indigo-600" />
            Intelligence Hub
          </h1>
        </div>
        <div className="flex items-center gap-3 bg-white p-1.5 rounded-2xl border border-gray-100 shadow-sm">
          <div className="px-4 py-2 bg-indigo-50 rounded-xl">
            <span className="text-xs font-black text-indigo-600 uppercase tracking-widest">Risk Index: 84/100</span>
          </div>
          <div className="w-10 h-10 rounded-xl bg-gray-900 flex items-center justify-center text-white shadow-lg cursor-help transition-transform hover:scale-105" title="Model Confidence">
            <Target className="w-5 h-5" />
          </div>
        </div>
      </header>

      {/* Hero Analytics Grid */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Compliance Trend Line Chart */}
        <div className="lg:col-span-2 bg-white p-8 rounded-[2.5rem] shadow-sm border border-gray-100 relative overflow-hidden group">
          <div className="flex items-center justify-between mb-8 cursor-default">
            <div>
              <h3 className="text-lg font-black text-gray-900 tracking-tight flex items-center gap-2">
                <TrendingUp className="w-5 h-5 text-emerald-500" />
                Compliance Trajectory
              </h3>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mt-1">12-Month Temporal Analysis</p>
            </div>
            <div className="flex items-center gap-2 text-emerald-600 bg-emerald-50 px-3 py-1 rounded-full text-xs font-black">
              <ArrowUpRight className="w-4 h-4" />
              +12.4%
            </div>
          </div>
          
          <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
              <AreaChart data={trendData}>
                <defs>
                  <linearGradient id="colorTrend" x1="0" y1="0" x2="0" y2="1">
                    <stop offset="5%" stopColor="#4f46e5" stopOpacity={0.1}/>
                    <stop offset="95%" stopColor="#4f46e5" stopOpacity={0}/>
                  </linearGradient>
                </defs>
                <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#f1f5f9" />
                <XAxis 
                  dataKey="name" 
                  axisLine={false} 
                  tickLine={false} 
                  tick={{ fill: '#94a3b8', fontSize: 10, fontWeight: 700 }} 
                />
                <YAxis 
                  axisLine={false} 
                  tickLine={false} 
                  tick={{ fill: '#94a3b8', fontSize: 10, fontWeight: 700 }}
                />
                <Tooltip 
                  contentStyle={{ borderRadius: '16px', border: 'none', boxShadow: '0 20px 25px -5px rgb(0 0 0 / 0.1)', fontWeight: 800 }}
                />
                <Area 
                  type="monotone" 
                  dataKey="TotalValue" 
                  stroke="#4f46e5" 
                  strokeWidth={4}
                  fillOpacity={1} 
                  fill="url(#colorTrend)" 
                  animationDuration={2000}
                />
              </AreaChart>
            </ResponsiveContainer>
          </div>
        </div>

        {/* Risk Score Donut */}
        <div className="bg-white p-8 rounded-[2.5rem] shadow-sm border border-gray-100 flex flex-col items-center justify-center text-center">
          <h3 className="text-lg font-black text-gray-900 tracking-tight mb-2">Exposure Distribution</h3>
          <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mb-6">Asset Health Classification</p>
          
          <div className="h-[250px] w-full relative">
            <div className="absolute inset-0 flex flex-col items-center justify-center z-10">
              <span className="text-4xl font-black text-gray-900">92%</span>
              <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Active</span>
            </div>
            <ResponsiveContainer width="100%" height="100%">
              <PieChart>
                <Pie
                  data={riskData.map(d => ({ name: d.label || d.Financer || 'Metric', value: d.value || d.AssetCount || 1 }))}
                  innerRadius={80}
                  outerRadius={105}
                  paddingAngle={8}
                  dataKey="value"
                  animationDuration={1500}
                >
                  {riskData.map((_, index) => (
                    <Cell key={`cell-${index}`} fill={COLORS[index % COLORS.length]} />
                  ))}
                </Pie>
                <Tooltip />
              </PieChart>
            </ResponsiveContainer>
          </div>
          
          <div className="grid grid-cols-2 gap-4 w-full mt-6">
            <div className="p-3 rounded-2xl bg-gray-50 border border-gray-100">
              <p className="text-[10px] font-bold text-gray-400 uppercase tracking-tighter">High Risk</p>
              <p className="text-lg font-black text-red-600">14</p>
            </div>
            <div className="p-3 rounded-2xl bg-gray-50 border border-gray-100">
              <p className="text-[10px] font-bold text-gray-400 uppercase tracking-tighter">Compliant</p>
              <p className="text-lg font-black text-emerald-600">842</p>
            </div>
          </div>
        </div>
      </div>

      {/* Secondary Row: Tenant Health & Insights */}
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-8">
        <div className="bg-white p-8 rounded-[2.5rem] shadow-sm border border-gray-100">
          <header className="flex items-center justify-between mb-8">
             <div>
                <h3 className="text-lg font-black text-gray-900 tracking-tight flex items-center gap-2">
                  <ShieldAlert className="w-5 h-5 text-amber-500" />
                  Tenant Integrity Matrix
                </h3>
                <p className="text-xs font-bold text-gray-400 uppercase tracking-widest mt-1">Cross-Entity Compliance Benchmarking</p>
             </div>
             <button className="p-2 hover:bg-gray-50 rounded-xl transition-colors">
               <Info className="w-4 h-4 text-gray-300" />
             </button>
          </header>

          <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
              <BarChart data={healthData.map(d => ({ name: d.Financer || 'NA', value: d.AssetCount || 0 }))}>
                <CartesianGrid strokeDasharray="3 3" vertical={false} stroke="#f1f5f9" />
                <XAxis 
                  dataKey="name" 
                  axisLine={false} 
                  tickLine={false} 
                  tick={{ fill: '#94a3b8', fontSize: 10, fontWeight: 700 }} 
                />
                <YAxis axisLine={false} tickLine={false} tick={{ fill: '#94a3b8', fontSize: 10, fontWeight: 700 }} />
                <Tooltip 
                   cursor={{ fill: '#f8fafc' }}
                   contentStyle={{ borderRadius: '16px', border: 'none', boxShadow: '0 20px 25px -5px rgb(0 0 0 / 0.1)' }}
                />
                <Bar 
                  dataKey="value" 
                  fill="#6366f1" 
                  radius={[8, 8, 0, 0]} 
                  barSize={32}
                  animationDuration={1800}
                />
              </BarChart>
            </ResponsiveContainer>
          </div>
        </div>

        <div className="space-y-6">
          <div className="bg-gradient-to-br from-indigo-600 to-indigo-900 p-8 rounded-[2.5rem] text-white shadow-2xl shadow-indigo-200 relative overflow-hidden">
             <Zap className="absolute -right-4 -top-4 w-32 h-32 text-white opacity-10 rotate-12" />
             <h3 className="text-xl font-bold mb-2">Neural Prediction</h3>
             <p className="text-indigo-100 text-sm font-medium mb-6">Based on current trends, we anticipate a 4.2% reduction in non-compliance cases for the next quarter.</p>
             <button className="px-6 py-3 bg-white text-indigo-900 font-black text-xs uppercase tracking-widest rounded-2xl hover:bg-indigo-50 transition-all shadow-xl">
               Run Simulation
             </button>
          </div>

          <div className="grid grid-cols-2 gap-6">
             <div className="bg-white p-6 rounded-3xl border border-gray-100 flex items-center gap-4">
               <div className="w-12 h-12 rounded-2xl bg-amber-50 flex items-center justify-center text-amber-600">
                 <Activity className="w-6 h-6" />
               </div>
               <div>
                 <p className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Model Drift</p>
                 <p className="text-lg font-black text-gray-900">0.02%</p>
               </div>
             </div>
             <div className="bg-white p-6 rounded-3xl border border-gray-100 flex items-center gap-4">
               <div className="w-12 h-12 rounded-2xl bg-indigo-50 flex items-center justify-center text-indigo-600">
                 <Target className="w-6 h-6" />
               </div>
               <div>
                 <p className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Accuracy</p>
                 <p className="text-lg font-black text-gray-900">99.1%</p>
               </div>
             </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default IntelligencePage;
