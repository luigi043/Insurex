import React, { useEffect, useState } from 'react';
import { caseClient } from '../../api/clients';
import type { ComplianceCase } from '../../api/types/Case';
import { DataTable, StatusBadge, Pagination } from '../shared';
import { 
  ClipboardCheck, Search, Filter, AlertCircle, 
  CheckCircle2, X,
  History, User, Activity
} from 'lucide-react';

const CasesPage: React.FC = () => {
  const [data, setData] = useState<ComplianceCase[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const [selectedCase, setSelectedCase] = useState<ComplianceCase | null>(null);
  const [resolutionNote, setResolutionNote] = useState('');
  const [isResolving, setIsResolving] = useState(false);
  const pageSize = 10;

  const fetchCases = async () => {
    setLoading(true);
    try {
      const response = await caseClient.getCases({ page, pageSize });
      setData(response.items);
      setTotalCount(response.totalCount);
    } catch (error) {
      console.error('Failed to fetch cases:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchCases();
  }, [page]);

  const handleResolve = async () => {
    if (!selectedCase) return;
    setIsResolving(true);
    try {
      const response = await caseClient.resolveCase(selectedCase.id.toString(), resolutionNote);
      if (response.success) {
        setSelectedCase(null);
        setResolutionNote('');
        fetchCases(); // Refresh list
      }
    } catch (error) {
      console.error('Failed to resolve case:', error);
    } finally {
      setIsResolving(false);
    }
  };

  const columns = [
    { 
      header: 'Case #', 
      key: 'caseNumber',
      render: (c: ComplianceCase) => (
        <div className="flex items-center gap-3">
          <div className={`w-8 h-8 rounded-lg flex items-center justify-center font-black text-xs ${
            c.priority === 'High' ? 'bg-red-50 text-red-600' : 'bg-blue-50 text-blue-600'
          }`}>
            {c.caseNumber.substring(0, 2)}
          </div>
          <span className="font-bold text-gray-900">{c.caseNumber}</span>
        </div>
      )
    },
    { header: 'Case Title', key: 'title' },
    { 
      header: 'Priority', 
      key: 'priority',
      render: (c: ComplianceCase) => <StatusBadge status={c.priority} type="priority" />
    },
    { 
      header: 'Workflow Status', 
      key: 'status',
      render: (c: ComplianceCase) => <StatusBadge status={c.status} />
    },
    { 
      header: 'Opened At', 
      key: 'openedAt',
      render: (c: ComplianceCase) => <span className="text-gray-400 font-medium">{new Date(c.openedAt).toLocaleDateString()}</span>
    },
    { 
      header: 'Due Date', 
      key: 'dueAt',
      render: (c: ComplianceCase) => (
        <div className="flex items-center gap-2">
          {new Date(c.dueAt) < new Date() && c.status !== 'Resolved' && (
            <AlertCircle className="w-4 h-4 text-red-500" />
          )}
          <span className={`${new Date(c.dueAt) < new Date() && c.status !== 'Resolved' ? 'text-red-600 font-bold' : 'text-gray-600'}`}>
            {new Date(c.dueAt).toLocaleDateString()}
          </span>
        </div>
      )
    }
  ];

  return (
    <div className="space-y-8 animate-in fade-in duration-500">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Active Cases</h1>
          <p className="text-gray-500 mt-1 font-medium italic">Managing {totalCount} open compliance investigations and escalations.</p>
        </div>
        <div className="flex gap-2 p-1 bg-gray-100 rounded-xl">
          <button className="px-4 py-2 bg-white rounded-lg shadow-sm text-sm font-bold text-gray-900">Active</button>
          <button className="px-4 py-2 text-sm font-bold text-gray-500 hover:text-gray-700">Resolved</button>
        </div>
      </header>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <div className="bg-white p-6 rounded-3xl border border-gray-100 shadow-sm flex items-center gap-4">
          <div className="w-12 h-12 rounded-2xl bg-orange-50 flex items-center justify-center text-orange-600">
            <Activity className="w-6 h-6" />
          </div>
          <div>
            <p className="text-2xl font-black text-gray-900">12</p>
            <p className="text-xs text-gray-400 font-black uppercase">Open Cases</p>
          </div>
        </div>
        <div className="bg-white p-6 rounded-3xl border border-gray-100 shadow-sm flex items-center gap-4">
          <div className="w-12 h-12 rounded-2xl bg-red-50 flex items-center justify-center text-red-600">
            <AlertCircle className="w-6 h-6" />
          </div>
          <div>
            <p className="text-2xl font-black text-gray-900">4</p>
            <p className="text-xs text-gray-400 font-black uppercase">Critical</p>
          </div>
        </div>
        <div className="bg-white p-6 rounded-3xl border border-gray-100 shadow-sm flex items-center gap-4">
          <div className="w-12 h-12 rounded-2xl bg-emerald-50 flex items-center justify-center text-emerald-600">
            <CheckCircle2 className="w-6 h-6" />
          </div>
          <div>
            <p className="text-2xl font-black text-emerald-600">8</p>
            <p className="text-xs text-gray-400 font-black uppercase">Resolved Today</p>
          </div>
        </div>
        <div className="bg-white p-6 rounded-3xl border border-gray-100 shadow-sm flex items-center gap-4">
          <div className="w-12 h-12 rounded-2xl bg-blue-50 flex items-center justify-center text-blue-600">
            <History className="w-6 h-6" />
          </div>
          <div>
            <p className="text-2xl font-black text-gray-900">92%</p>
            <p className="text-xs text-gray-400 font-black uppercase">SLA Compliance</p>
          </div>
        </div>
      </div>

      {/* Filter Bar */}
      <div className="flex flex-col lg:flex-row gap-4 bg-white p-4 rounded-2xl border border-gray-100 shadow-sm">
        <div className="flex-1 relative">
          <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
          <input 
            type="text" 
            placeholder="Search cases by number or subject..." 
            className="w-full pl-11 pr-4 py-3 bg-gray-50 border-none rounded-xl text-sm outline-none focus:ring-2 focus:ring-blue-100 transition-all font-medium"
          />
        </div>
        <div className="flex gap-2">
          <select className="bg-gray-50 px-4 py-3 rounded-xl text-sm font-bold text-gray-600 outline-none border-none cursor-pointer">
            <option>All Priorities</option>
            <option>Critical</option>
            <option>High</option>
            <option>Normal</option>
          </select>
          <button className="flex items-center gap-2 px-6 py-3 bg-gray-900 text-white rounded-xl text-sm font-bold hover:bg-gray-800 transition-colors">
            <Filter className="w-4 h-4" /> Apply Filters
          </button>
        </div>
      </div>

      {/* Data Table */}
      <DataTable 
        columns={columns} 
        data={data} 
        loading={loading} 
        onRowClick={(c) => setSelectedCase(c)}
      />

      {/* Pagination */}
      <Pagination 
        page={page} 
        totalPages={Math.ceil(totalCount / pageSize)} 
        onPageChange={setPage} 
      />

      {/* Resolution Modal */}
      {selectedCase && (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4 animate-in fade-in duration-300">
          <div className="absolute inset-0 bg-gray-900/60 backdrop-blur-sm" onClick={() => !isResolving && setSelectedCase(null)}></div>
          
          <div className="relative bg-white w-full max-w-lg rounded-[2.5rem] shadow-2xl overflow-hidden animate-in zoom-in-95 duration-300">
            <div className="p-8 border-b border-gray-100 flex items-center justify-between">
              <div className="flex items-center gap-4">
                <div className="w-12 h-12 rounded-2xl bg-blue-600 flex items-center justify-center text-white shadow-lg shadow-blue-200">
                  <ClipboardCheck className="w-6 h-6" />
                </div>
                <div>
                  <h2 className="text-xl font-black text-gray-900 leading-tight">Resolve Case</h2>
                  <p className="text-xs text-gray-400 font-bold uppercase tracking-wider">{selectedCase.caseNumber}</p>
                </div>
              </div>
              <button 
                onClick={() => setSelectedCase(null)}
                disabled={isResolving}
                className="p-2 hover:bg-gray-100 rounded-xl transition-colors disabled:opacity-0"
              >
                <X className="w-6 h-6 text-gray-400" />
              </button>
            </div>

            <div className="p-8 space-y-6">
              <div className="bg-gray-50 p-4 rounded-2xl border border-gray-100">
                <h3 className="text-sm font-black text-gray-900 mb-1">{selectedCase.title}</h3>
                <p className="text-xs text-gray-500 font-medium leading-relaxed italic">
                  Case opened on {new Date(selectedCase.openedAt).toLocaleDateString()}. Initial investigation flagged this as {selectedCase.priority} priority.
                </p>
              </div>

              <div>
                <label className="text-[10px] font-black text-gray-400 uppercase tracking-widest block mb-1.5 ml-1">Resolution Summary</label>
                <textarea 
                  value={resolutionNote}
                  onChange={(e) => setResolutionNote(e.target.value)}
                  placeholder="Describe the outcome and remediation steps taken..."
                  className="w-full h-32 px-4 py-3 bg-gray-50 border-none rounded-2xl text-sm outline-none focus:ring-2 focus:ring-blue-100 transition-all font-medium placeholder:italic"
                  disabled={isResolving}
                />
              </div>

              <div className="flex gap-3">
                <button 
                  onClick={() => setSelectedCase(null)}
                  disabled={isResolving}
                  className="flex-1 py-4 bg-gray-50 text-gray-600 rounded-2xl text-xs font-black uppercase hover:bg-gray-100 transition-colors disabled:opacity-50"
                >
                  Cancel
                </button>
                <button 
                  onClick={handleResolve}
                  disabled={isResolving || !resolutionNote.trim()}
                  className="flex-[2] py-4 bg-blue-600 text-white rounded-2xl text-xs font-black uppercase shadow-xl shadow-blue-100 hover:bg-blue-700 transition-all active:scale-[0.98] disabled:opacity-50 disabled:grayscale"
                >
                  {isResolving ? 'Resolving...' : 'Confirm Resolution'}
                </button>
              </div>
            </div>
            
            <div className="px-8 py-4 bg-gray-50/50 border-t border-gray-50 flex items-center justify-center gap-2">
              <User className="w-3 h-3 text-gray-400" />
              <span className="text-[9px] text-gray-400 font-bold uppercase tracking-widest">Acting as System Administrator</span>
            </div>
          </div>
        </div>
      )}
    </div>
  );
};

export default CasesPage;
