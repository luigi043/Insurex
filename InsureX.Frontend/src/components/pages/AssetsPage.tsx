import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { assetClient } from '../../api/clients';
import type { Asset } from '../../api/types/Asset';
import { DataTable, StatusBadge, Pagination } from '../shared';
import { Car, Search, Filter, Download } from 'lucide-react';

const AssetsPage: React.FC = () => {
  const navigate = useNavigate();
  const [data, setData] = useState<Asset[]>([]);
  const [totalCount, setTotalCount] = useState(0);
  const [page, setPage] = useState(1);
  const [loading, setLoading] = useState(true);
  const pageSize = 10;

  const fetchAssets = async () => {
    setLoading(true);
    try {
      const response = await assetClient.getAssets({ page, pageSize });
      setData(response.items);
      setTotalCount(response.totalCount);
    } catch (error) {
      console.error('Failed to fetch assets:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchAssets();
  }, [page]);

  const columns = [
    { 
      header: 'Identifier / SKU', 
      key: 'assetIdentifier',
      render: (a: Asset) => (
        <div className="flex items-center gap-3">
          <div className="w-8 h-8 rounded-lg bg-gray-100 flex items-center justify-center text-gray-500">
            <Car className="w-4 h-4" />
          </div>
          <div>
            <p className="font-bold text-gray-900">{a.assetIdentifier}</p>
            <p className="text-[10px] text-gray-400 font-bold uppercase">{a.assetType}</p>
          </div>
        </div>
      )
    },
    { header: 'Registration', key: 'registrationNumber' },
    { 
      header: 'Value', 
      key: 'financedAmount',
      render: (a: Asset) => <span className="font-black text-gray-900">${a.financedAmount.toLocaleString()}</span>
    },
    { 
      header: 'Status', 
      key: 'status',
      render: (a: Asset) => <StatusBadge status={a.status} />
    },
    { 
      header: 'Compliance', 
      key: 'complianceStatus',
      render: (a: Asset) => <StatusBadge status={a.complianceStatus || 'Unknown'} />
    },
    {
      header: 'Loan End',
      key: 'loanEndDate',
      render: (a: Asset) => <span className="text-gray-400 font-medium">{new Date(a.loanEndDate).toLocaleDateString()}</span>
    }
  ];

  return (
    <div className="space-y-8 animate-in fade-in duration-500">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Assets Management</h1>
          <p className="text-gray-500 mt-1 font-medium italic">Tracking {totalCount} registered financial assets across the ledger.</p>
        </div>
        <div className="flex gap-3">
          <button 
            onClick={() => window.open(`${import.meta.env.VITE_API_BASE_URL}/reports/assets/export`, '_blank')}
            className="flex items-center gap-2 px-4 py-2 border border-gray-200 rounded-xl text-sm font-bold text-gray-700 bg-white hover:bg-gray-50 transition-all shadow-sm"
          >
            <Download className="w-4 h-4" /> Export CSV
          </button>
          <button 
            onClick={() => navigate('/assets/new')}
            className="flex items-center gap-2 px-6 py-2 bg-blue-600 rounded-xl text-sm font-bold text-white shadow-lg shadow-blue-200 hover:bg-blue-700 transition-all active:scale-95"
          >
            Register Asset
          </button>
        </div>
      </header>

      {/* Filter Bar */}
      <div className="flex flex-col lg:flex-row gap-4 bg-white p-4 rounded-2xl border border-gray-100 shadow-sm">
        <div className="flex-1 relative">
          <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
          <input 
            type="text" 
            placeholder="Search by identifier, reg number or reference..." 
            className="w-full pl-11 pr-4 py-3 bg-gray-50 border-none rounded-xl text-sm outline-none focus:ring-2 focus:ring-blue-100 transition-all"
          />
        </div>
        <div className="flex gap-2">
          <button className="flex items-center gap-2 px-4 py-3 bg-gray-50 text-gray-600 rounded-xl text-sm font-bold hover:bg-gray-100">
            <Filter className="w-4 h-4" /> Filters
          </button>
          <div className="h-full w-px bg-gray-100 mx-2"></div>
          <select className="bg-gray-50 px-4 py-3 rounded-xl text-sm font-bold text-gray-600 outline-none border-none cursor-pointer">
            <option>All Statuses</option>
            <option>Active</option>
            <option>Settled</option>
          </select>
        </div>
      </div>

      {/* Data Table */}
      <DataTable 
        columns={columns} 
        data={data} 
        loading={loading} 
        onRowClick={(a) => navigate(`/assets/${a.id}`)}
      />

      {/* Pagination */}
      <Pagination 
        page={page} 
        totalPages={Math.ceil(totalCount / pageSize)} 
        onPageChange={setPage} 
      />
    </div>
  );
};

export default AssetsPage;
