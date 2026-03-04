import React, { useState } from 'react';
import { 
  ShieldCheck, Activity, Brain, 
  Settings, RefreshCw, BarChart3, AlertTriangle,
  ChevronRight, Play
} from 'lucide-react';

const CompliancePage: React.FC = () => {
  const [activeTab, setActiveTab] = useState<'rules' | 'logs' | 'health'>('rules');

  const rules = [
    { id: 'R-101', name: 'Identity Integrity', description: 'Validate borrower ID number against master registry.', status: 'Active', accuracy: '99.8%' },
    { id: 'R-202', name: 'Insured Value Corridor', description: 'Asset insured value must be > 90% of finance balance.', status: 'Active', accuracy: '98.5%' },
    { id: 'R-303', name: 'Policy Expiry Guard', description: 'Flag assets with policies expiring within 30 days.', status: 'Active', accuracy: '100%' },
    { id: 'R-404', name: 'Multi-Asset Linkage', description: 'Detect duplicate financing across different tenants.', status: 'Warning', accuracy: '94.2%' },
  ];

  const recentEvaluations = [
    { id: 4501, asset: 'VEH-9921', outcome: 'Compliant', latency: '42ms', timestamp: '2 mins ago' },
    { id: 4502, asset: 'VEH-4410', outcome: 'NonCompliant', latency: '38ms', timestamp: '5 mins ago' },
    { id: 4503, asset: 'PRP-2038', outcome: 'PendingReview', latency: '120ms', timestamp: '12 mins ago' },
  ];

  return (
    <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-6">
        <div>
          <div className="flex items-center gap-3 text-blue-600 mb-2">
            <Brain className="w-5 h-5" />
            <span className="text-xs font-black uppercase tracking-[0.2em]">Autonomous Engine</span>
          </div>
          <h1 className="text-4xl font-black text-gray-900 tracking-tight">Compliance Control</h1>
          <p className="text-gray-500 mt-2 font-medium">Real-time oversight of the InsureX automated evaluation engine.</p>
        </div>
        <div className="flex items-center gap-4 bg-white p-2 rounded-2xl border border-gray-100 shadow-sm">
          <div className="flex -space-x-2">
            {[1, 2, 3].map(i => (
              <div key={i} className="w-10 h-10 rounded-xl bg-gray-100 border-2 border-white flex items-center justify-center text-xs font-black text-gray-400">
                AI
              </div>
            ))}
          </div>
          <div className="pr-4 py-1 border-r border-gray-100">
            <p className="text-xs font-black text-gray-400 uppercase">Engine Status</p>
            <div className="flex items-center gap-2">
              <div className="w-2 h-2 rounded-full bg-emerald-500 animate-pulse"></div>
              <span className="text-sm font-black text-emerald-600">OPERATIONAL</span>
            </div>
          </div>
          <button className="px-6 py-3 bg-blue-600 rounded-xl text-white font-black text-xs uppercase tracking-wider shadow-lg shadow-blue-200 hover:bg-blue-700 transition-all">
            Retrain
          </button>
        </div>
      </header>

      {/* Engine Metrics */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white p-8 rounded-[2rem] border border-gray-100 shadow-sm relative overflow-hidden group">
          <BarChart3 className="absolute -right-4 -bottom-4 w-24 h-24 text-blue-50 opacity-0 group-hover:opacity-100 transition-opacity" />
          <p className="text-xs font-black text-gray-400 uppercase tracking-widest mb-4">Throughput (24h)</p>
          <div className="flex items-baseline gap-2">
            <h3 className="text-4xl font-black text-gray-900">42,801</h3>
            <span className="text-sm font-black text-emerald-500">+12%</span>
          </div>
          <p className="text-sm text-gray-400 font-medium mt-1">Total evaluations processed</p>
        </div>
        <div className="bg-white p-8 rounded-[2rem] border border-gray-100 shadow-sm relative overflow-hidden group">
          <Activity className="absolute -right-4 -bottom-4 w-24 h-24 text-orange-50 opacity-0 group-hover:opacity-100 transition-opacity" />
          <p className="text-xs font-black text-gray-400 uppercase tracking-widest mb-4">Avg Latency</p>
          <div className="flex items-baseline gap-2">
            <h3 className="text-4xl font-black text-gray-900">45ms</h3>
            <span className="text-sm font-black text-emerald-500">-2ms</span>
          </div>
          <p className="text-sm text-gray-400 font-medium mt-1">Rule execution response</p>
        </div>
        <div className="bg-white p-8 rounded-[2rem] border border-gray-100 shadow-sm relative overflow-hidden group">
          <AlertTriangle className="absolute -right-4 -bottom-4 w-24 h-24 text-red-50 opacity-0 group-hover:opacity-100 transition-opacity" />
          <p className="text-xs font-black text-gray-400 uppercase tracking-widest mb-4">Exception Rate</p>
          <div className="flex items-baseline gap-2">
            <h3 className="text-4xl font-black text-gray-900">0.08%</h3>
            <span className="text-sm font-black text-emerald-500">STABLE</span>
          </div>
          <p className="text-sm text-gray-400 font-medium mt-1">Manual review required</p>
        </div>
      </div>

      {/* Tabs */}
      <div className="flex gap-8 border-b border-gray-100 pb-px">
        {(['rules', 'logs', 'health'] as const).map(tab => (
          <button 
            key={tab}
            onClick={() => setActiveTab(tab)}
            className={`pb-4 text-xs font-black uppercase tracking-widest transition-all relative ${
              activeTab === tab ? 'text-blue-600' : 'text-gray-400 hover:text-gray-600'
            }`}
          >
            {tab}
            {activeTab === tab && (
              <div className="absolute bottom-0 left-0 right-0 h-1 bg-blue-600 rounded-t-full"></div>
            )}
          </button>
        ))}
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        <div className="lg:col-span-2 space-y-8">
          {activeTab === 'rules' && (
            <section className="bg-white rounded-3xl border border-gray-100 shadow-sm overflow-hidden">
              <div className="p-6 border-b border-gray-50 flex items-center justify-between">
                <h2 className="text-xl font-black text-gray-900">Evaluation Rulebook</h2>
                <button className="flex items-center gap-2 text-xs font-black text-blue-600 hover:opacity-70 transition-opacity uppercase tracking-widest">
                  <Settings className="w-4 h-4" /> New Rule
                </button>
              </div>
              <div className="divide-y divide-gray-50">
                {rules.map(rule => (
                  <div key={rule.id} className="p-6 flex items-center justify-between hover:bg-gray-50/50 transition-colors cursor-pointer group">
                    <div className="flex items-start gap-4">
                      <div className={`mt-1 w-10 h-10 rounded-xl flex items-center justify-center ${
                        rule.status === 'Active' ? 'bg-blue-50 text-blue-600' : 'bg-orange-50 text-orange-600'
                      }`}>
                        <ShieldCheck className="w-5 h-5" />
                      </div>
                      <div>
                        <p className="font-black text-gray-900">{rule.name}</p>
                        <p className="text-xs text-gray-400 font-medium mt-0.5">{rule.description}</p>
                      </div>
                    </div>
                    <div className="flex items-center gap-12 text-right">
                      <div className="hidden md:block">
                        <p className="text-xs font-black text-gray-900">{rule.accuracy}</p>
                        <p className="text-[10px] font-black text-gray-400 uppercase tracking-widest">CONFIDENCE</p>
                      </div>
                      <div className={`px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-widest border ${
                        rule.status === 'Active' ? 'bg-emerald-50 text-emerald-600 border-emerald-100' : 'bg-orange-50 text-orange-600 border-orange-100'
                      }`}>
                        {rule.status}
                      </div>
                      <ChevronRight className="w-5 h-5 text-gray-300 group-hover:text-blue-600 transition-colors" />
                    </div>
                  </div>
                ))}
              </div>
            </section>
          )}

          {activeTab === 'logs' && (
            <div className="bg-gray-900 p-8 rounded-[2.5rem] shadow-2xl text-white font-mono text-xs overflow-hidden relative">
              <div className="flex items-center gap-4 mb-6 border-b border-white/10 pb-4">
                <Activity className="w-4 h-4 text-emerald-400 animate-pulse" />
                <span className="font-bold text-emerald-400">LIVE EVALUATION STREAM</span>
              </div>
              <div className="space-y-3 opacity-90">
                <p><span className="text-gray-500">[14:22:01]</span> <span className="text-blue-400">ENGINE:</span> Processing webhook event 0xAE11... </p>
                <p><span className="text-gray-500">[14:22:01]</span> <span className="text-blue-400">RULES:</span> R-101 (PASS), R-202 (PASS), R-303 (FAIL)</p>
                <p><span className="text-gray-500">[14:22:01]</span> <span className="text-orange-400">WARN:</span> Policy Guard failure on Asset VEH-4410</p>
                <p><span className="text-gray-500">[14:22:01]</span> <span className="text-emerald-400">FINAL:</span> Emitting ComplianceOutcome.NonCompliant</p>
                <div className="h-4"></div>
                <p><span className="text-gray-500">[14:22:15]</span> <span className="text-blue-400">ENGINE:</span> Processing webhook event 0xBF90... </p>
                <p><span className="text-gray-500">[14:22:15]</span> <span className="text-emerald-400">FINAL:</span> Emitting ComplianceOutcome.Compliant (Latency 32ms)</p>
              </div>
              <div className="absolute top-4 right-8">
                <RefreshCw className="w-4 h-4 text-gray-600 animate-spin-slow" />
              </div>
            </div>
          )}
        </div>

        <div className="space-y-8">
          {/* Quick Action */}
          <section className="bg-gray-900 p-8 rounded-3xl text-white relative overflow-hidden group">
            <div className="relative z-10">
              <h3 className="text-xl font-black mb-4 flex items-center gap-3">
                <Play className="w-5 h-5 text-emerald-400 fill-emerald-400" />
                Dry Run
              </h3>
              <p className="text-sm text-gray-400 font-medium mb-8 leading-relaxed">
                Manually trigger an evaluation sequence against a specific asset using existing rule sets.
              </p>
              <button className="w-full py-4 bg-white text-gray-900 rounded-2xl text-xs font-black uppercase hover:bg-gray-100 transition-all active:scale-95">
                Start Simulation
              </button>
            </div>
            <Activity className="absolute -bottom-12 -right-12 w-48 h-48 text-white/5 -rotate-12 group-hover:scale-110 transition-transform duration-700" />
          </section>

          {/* Recent Evaluations Sidebar */}
          <section className="bg-white p-8 rounded-3xl border border-gray-100 shadow-sm">
            <div className="flex items-center justify-between mb-6">
              <h3 className="text-lg font-black text-gray-900">Recent Stream</h3>
              <span className="text-[10px] font-black text-gray-400 uppercase tracking-widest">Live</span>
            </div>
            <div className="space-y-6">
              {recentEvaluations.map(evalu => (
                <div key={evalu.id} className="flex items-start justify-between group cursor-pointer">
                  <div className="flex items-center gap-3">
                    <div className={`w-2 h-2 rounded-full mt-1.5 ${
                      evalu.outcome === 'Compliant' ? 'bg-emerald-500' : evalu.outcome === 'NonCompliant' ? 'bg-red-500' : 'bg-orange-500'
                    }`}></div>
                    <div>
                      <p className="text-sm font-black text-gray-900 group-hover:text-blue-600 transition-colors uppercase">{evalu.asset}</p>
                      <p className="text-[10px] text-gray-400 font-bold uppercase">{evalu.timestamp}</p>
                    </div>
                  </div>
                  <div className="text-right">
                    <p className="text-[10px] font-black text-gray-900 uppercase">{evalu.outcome}</p>
                    <p className="text-[10px] text-gray-400 font-medium">{evalu.latency}</p>
                  </div>
                </div>
              ))}
            </div>
            <button className="w-full mt-8 py-3 bg-gray-50 text-gray-600 rounded-xl text-xs font-black uppercase hover:bg-gray-100 transition-colors">
              View Audit Rail
            </button>
          </section>
        </div>
      </div>
    </div>
  );
};

export default CompliancePage;
