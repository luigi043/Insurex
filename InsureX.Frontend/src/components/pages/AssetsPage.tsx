import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { assetClient } from '../../api/clients';
import type { Asset } from '../../api/types/Asset';
import { DataTable, StatusBadge, Pagination } from '../shared';
import { Car, Search, Filter, Download, Upload, AlertCircle, X, Loader2, CheckCircle2 } from 'lucide-react';

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

  const [importing, setImporting] = useState(false);
  const [showImportModal, setShowImportModal] = useState(false);
  const [importStatus, setImportStatus] = useState<{ type: 'success' | 'error', message: string } | null>(null);

  const handleFileUpload = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    if (!file) return;

    setImporting(true);
    setImportStatus(null);
    
    const reader = new FileReader();
    reader.onload = async (e) => {
      try {
        const text = e.target?.result as string;
        const lines = text.split('\n').filter(l => l.trim());
        const headers = lines[0].split(',').map(h => h.trim());
        
        const assets = lines.slice(1).map(line => {
          const values = line.split(',').map(v => v.trim());
          const obj: any = {};
          headers.forEach((h, i) => {
            // Mapping common CSV headers to BulkImportFromFinancer properties
            if (h === 'FinanceNumber') obj.vcFinance_Number = values[i];
            else if (h === 'IDNumber') obj.vcID_Business_Number = values[i];
            else if (h === 'CustomerType') obj.vcCustomer_Type_Description = values[i];
            else if (h === 'InsuranceCompany') obj.vcInsurance_Company = values[i];
            else if (h === 'PolicyNumber') obj.vcPolicy_Number = values[i];
            else if (h === 'AssetType') obj.vcAsset_Type_Description = values[i];
            else if (h === 'UniqueIdentifier') obj.vcAsset_Unique_Identifier = values[i];
          });
          return obj;
        });

        await assetClient.importAssets(assets);
        setImportStatus({ type: 'success', message: `${assets.length} assets successfully queued for ledger synchronization.` });
        fetchAssets();
      } catch (error) {
        setImportStatus({ type: 'error', message: 'Failed to process import. Please ensure CSV format compliance.' });
      } finally {
        setImporting(false);
      }
    };
    reader.readAsText(file);
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
            onClick={() => setShowImportModal(true)}
            className="flex items-center gap-2 px-4 py-2 border border-blue-200 rounded-xl text-sm font-bold text-blue-700 bg-blue-50 hover:bg-blue-100 transition-all shadow-sm"
          >
            <Upload className="w-4 h-4" /> Bulk Import
          </button>
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

      {/* Import Modal */}
      {showImportModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-gray-900/60 backdrop-blur-sm p-4 animate-in fade-in duration-300">
          <div className="bg-white rounded-[2.5rem] shadow-2xl w-full max-w-lg overflow-hidden border border-gray-100 scale-in-center animate-in zoom-in-95 duration-300">
            <div className="p-8">
              <div className="flex justify-between items-center mb-6">
                <h3 className="text-2xl font-black text-gray-900 tracking-tight">Bulk Asset Ingestion</h3>
                <button onClick={() => { setShowImportModal(false); setImportStatus(null); }} className="p-2 hover:bg-gray-100 rounded-full transition-colors">
                  <X className="w-6 h-6 text-gray-400" />
                </button>
              </div>

              {!importStatus ? (
                <div className="space-y-6">
                  <p className="text-gray-500 text-sm font-medium">Upload a CSV file containing your asset portfolio. Our intelligence engine will automatically validate and map the records to the global ledger.</p>
                  
                  <label className={`flex flex-col items-center justify-center w-full h-48 border-2 border-dashed rounded-[2rem] cursor-pointer transition-all ${importing ? 'bg-gray-50 border-gray-200 pointer-events-none' : 'bg-blue-50/30 border-blue-200 hover:bg-blue-50/50 hover:border-blue-400'}`}>
                    <div className="flex flex-col items-center justify-center pt-5 pb-6">
                      {importing ? (
                        <>
                          <Loader2 className="w-12 h-12 text-blue-600 animate-spin mb-4" />
                          <p className="text-sm font-bold text-blue-700 uppercase tracking-widest">Parsing Dataset...</p>
                        </>
                      ) : (
                        <>
                          <div className="w-16 h-16 bg-blue-600 rounded-2xl flex items-center justify-center text-white shadow-xl shadow-blue-200 mb-4 transition-transform group-hover:scale-110">
                            <Upload className="w-8 h-8" />
                          </div>
                          <p className="text-sm font-black text-gray-900 uppercase tracking-widest">Select CSV File</p>
                          <p className="text-xs text-gray-400 font-medium mt-1">or drag and drop here</p>
                        </>
                      )}
                    </div>
                    <input type="file" className="hidden" accept=".csv" onChange={handleFileUpload} disabled={importing} />
                  </label>

                  <div className="flex items-start gap-3 p-4 bg-amber-50 rounded-2xl border border-amber-100">
                    <AlertCircle className="w-5 h-5 text-amber-500 shrink-0 mt-0.5" />
                    <p className="text-xs text-amber-800 font-medium leading-relaxed">
                      Ensure your CSV includes required headers: <code className="bg-amber-100 px-1 rounded">FinanceNumber</code>, <code className="bg-amber-100 px-1 rounded">IDNumber</code>, <code className="bg-amber-100 px-1 rounded">PolicyNumber</code>.
                    </p>
                  </div>
                </div>
              ) : (
                <div className="text-center py-8 space-y-6">
                  <div className={`w-20 h-20 mx-auto rounded-full flex items-center justify-center shadow-xl ${importStatus.type === 'success' ? 'bg-emerald-100 text-emerald-600 shadow-emerald-100' : 'bg-red-100 text-red-600 shadow-red-100'}`}>
                    {importStatus.type === 'success' ? <CheckCircle2 className="w-10 h-10" /> : <AlertCircle className="w-10 h-10" />}
                  </div>
                  <div>
                    <h4 className={`text-xl font-black tracking-tight ${importStatus.type === 'success' ? 'text-emerald-900' : 'text-red-900'}`}>{importStatus.type === 'success' ? 'Ingestion Complete' : 'Import Failed'}</h4>
                    <p className="text-gray-500 text-sm font-medium mt-2 px-8">{importStatus.message}</p>
                  </div>
                  <button 
                    onClick={() => { setShowImportModal(false); setImportStatus(null); }}
                    className="w-full py-4 bg-gray-900 text-white font-bold rounded-2xl hover:bg-black transition-all shadow-lg active:scale-95"
                  >
                    Close
                  </button>
                </div>
              )}
            </div>
          </div>
        </div>
      )}

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
