import React, { useEffect, useState } from 'react';
import { caseClient } from '../../api/clients';
import { ComplianceCase } from '../../api/types/Case';
import { DataTable, StatusBadge, Pagination } from '../shared';
import { Folder, Clock, AlertTriangle, CheckCircle2, ChevronRight } from 'lucide-react';

const CasesPage: React.FC = () => {
  const [data, setData] = useState<ComplianceCase[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
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

  const columns = [
    { 
      header: 'Case / Title', 
      key: 'caseNumber',
      render: (c: ComplianceCase) => (
        <div className="flex items-center gap-3">
          <div className={`w-8 h-8 rounded-lg flex items-center justify-center ${c.status === 'Open' ? 'bg-red-50 text-red-500' : 'bg-blue-50 text-blue-500'}`}>
            <Folder className="w-4 h-4" />
          </div>
          <div>
            <p className="font-bold text-gray-900">{c.caseNumber}</p>
            <p className="text-xs text-gray-400 font-medium truncate max-w-[200px]">{c.title}</p>
          </div>
        </div>
      )
    },
    { 
      header: 'Priority', 
      key: 'priority',
      render: (c: ComplianceCase) => (
        <div className="flex items-center gap-2">
          <AlertTriangle className={`w-3 h-3 ${c.priority === 'Critical' ? 'text-red-500' : 'text-amber-500'}`} />
          <StatusBadge status={c.priority} type="priority" />
        </div>
      )
    },
    { 
      header: 'Status', 
      key: 'status',
      render: (c: ComplianceCase) => <StatusBadge status={c.status} type="case" />
    },
    { 
      header: 'Due In',
      key: 'dueAt',
      render: (c: ComplianceCase) => (
        <div className="flex items-center gap-2 text-gray-400 font-medium italic">
          <Clock className="w-3 h-3" />
          {new Date(c.dueAt) < new Date() ? (
            <span className="text-red-500 font-bold uppercase text-[10px]">Overdue</span>
          ) : (
            <span>{Math.ceil((new Date(c.dueAt).getTime() - new Date().getTime()) / (1000 * 3600 * 24))} Days</span>
          )}
        </div>
      )
    },
    {
      header: 'Actions',
      key: 'actions',
      render: (c: ComplianceCase) => (
        <button className="p-2 hover:bg-gray-50 rounded-lg text-gray-400 hover:text-blue-600 transition-colors">
          <ChevronRight className="w-5 h-5" />
        </button>
      )
    }
  ];

  return (
    <div className="space-y-8 animate-in fade-in duration-500">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Workflow Orchestration</h1>
          <p className="text-gray-500 mt-1 font-medium italic">Active compliance cases requiring intervention.</p>
        </div>
        <div className="flex gap-4 items-center bg-gray-50 px-6 py-3 rounded-[1.5rem] border border-gray-100">
           <div className="flex items-center gap-2 border-r border-gray-200 pr-4">
              <span className="text-[10px] font-black uppercase text-gray-400 tracking-tighter">Open</span>
              <span className="text-xl font-black text-red-600">{data.filter(c => c.status === 'Open').length}</span>
           </div>
           <div className="flex items-center gap-2">
              <span className="text-[10px] font-black uppercase text-gray-400 tracking-tighter">SLA Warning</span>
              <span className="text-xl font-black text-amber-600">{data.filter(c => new Date(c.dueAt) < new Date()).length}</span>
           </div>
        </div>
      </header>

      {/* Tabs / Filter segment */}
      <div className="flex gap-6 border-b border-gray-100 px-2">
        {['All Cases', 'My Tasks', 'Critical Errors', 'Resolved'].map((tab, i) => (
          <button 
            key={tab} 
            className={`pb-4 px-2 text-sm font-bold tracking-tight transition-all relative ${i === 0 ? 'text-blue-600' : 'text-gray-400 hover:text-gray-600'}`}
          >
            {tab}
            {i === 0 && <div className="absolute bottom-0 left-0 right-0 h-1 bg-blue-600 rounded-t-full"></div>}
          </button>
        ))}
      </div>

      <DataTable columns={columns} data={data} loading={loading} />

      <Pagination 
        page={page} 
        totalPages={Math.ceil(totalCount / pageSize)} 
        onPageChange={setPage} 
      />
    </div>
  );
};

export default CasesPage;
