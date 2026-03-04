import React, { useEffect, useState } from 'react';
import { auditClient } from '../../api/clients';
import type { AuditEntry } from '../../api/types/Audit';
import { DataTable, StatusBadge, Pagination } from '../shared';
import { 
    History, Search, Filter, Shield, 
    User, Fingerprint, ExternalLink, Calendar
} from 'lucide-react';

const AuditPage: React.FC = () => {
    const [data, setData] = useState<AuditEntry[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(true);
    const [entityFilter, setEntityFilter] = useState('');
    const [correlationIdFilter, setCorrelationIdFilter] = useState('');
    const pageSize = 15;

    const fetchLogs = async () => {
        setLoading(true);
        try {
            const response = await auditClient.getAuditLogs({ 
                page, 
                pageSize, 
                entity: entityFilter || undefined,
                correlationId: correlationIdFilter || undefined
            });
            setData(response.items);
            setTotalCount(response.totalCount);
        } catch (error) {
            console.error('Failed to fetch audit logs:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchLogs();
    }, [page]);

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        setPage(1);
        fetchLogs();
    };

    const columns = [
        { 
            header: 'Timestamp', 
            key: 'occurredAt',
            render: (a: AuditEntry) => (
                <div className="flex items-center gap-2 text-gray-400">
                    <Calendar className="w-4 h-4" />
                    <span className="font-mono text-[11px] font-bold">
                        {new Date(a.occurredAt).toLocaleString()}
                    </span>
                </div>
            )
        },
        { 
            header: 'Actor', 
            key: 'actorName',
            render: (a: AuditEntry) => (
                <div className="flex items-center gap-2">
                    <div className="w-6 h-6 rounded-lg bg-gray-100 flex items-center justify-center text-gray-400">
                        <User className="w-3.5 h-3.5" />
                    </div>
                    <span className="font-bold text-gray-900">{a.actorName}</span>
                </div>
            )
        },
        { 
            header: 'Action', 
            key: 'action',
            render: (a: AuditEntry) => <StatusBadge status={a.action} type="priority" />
        },
        { 
            header: 'Entity / Target', 
            key: 'entityName',
            render: (a: AuditEntry) => (
                <div>
                    <span className="text-[10px] font-black text-blue-600 uppercase tracking-widest block">{a.entityName}</span>
                    <span className="font-mono text-[10px] text-gray-400 font-bold">ID: {a.entityId}</span>
                </div>
            )
        },
        { 
            header: 'Notes', 
            key: 'notes',
            render: (a: AuditEntry) => (
                <p className="max-w-xs truncate text-xs text-gray-500 italic font-medium">
                    {a.notes || '---'}
                </p>
            )
        },
        { 
            header: 'Correlation ID', 
            key: 'correlationId',
            render: (a: AuditEntry) => (
                <div className="flex items-center gap-2 px-2 py-1 bg-gray-50 rounded-lg group cursor-pointer hover:bg-blue-50 transition-colors">
                    <Fingerprint className="w-3.5 h-3.5 text-gray-300 group-hover:text-blue-400" />
                    <span className="font-mono text-[9px] text-gray-400 group-hover:text-blue-600 font-bold truncate max-w-[80px]">
                        {a.correlationId}
                    </span>
                    <ExternalLink className="w-3 h-3 text-gray-300 opacity-0 group-hover:opacity-100" />
                </div>
            )
        }
    ];

    return (
        <div className="space-y-8 animate-in fade-in slide-in-from-bottom-2 duration-500">
            <header className="flex flex-col md:flex-row md:items-center justify-between gap-6">
                <div>
                    <div className="flex items-center gap-3 text-emerald-600 mb-2">
                        <Shield className="w-5 h-5" />
                        <span className="text-xs font-black uppercase tracking-[0.2em]">Immutable Ledger</span>
                    </div>
                    <h1 className="text-3xl font-black text-gray-900 tracking-tight">Evidence Audit Trail</h1>
                    <p className="text-gray-500 mt-1 font-medium">Comprehensive history of all system activities and compliance decisions.</p>
                </div>
                
                <div className="flex items-center gap-4 py-2 px-4 bg-gray-50 rounded-2xl border border-gray-100">
                    <History className="w-5 h-5 text-gray-400" />
                    <div>
                        <p className="text-xs font-black text-gray-400 uppercase tracking-widest">Total Records</p>
                        <p className="text-lg font-black text-gray-900">{totalCount.toLocaleString()}</p>
                    </div>
                </div>
            </header>

            {/* Filter Bar */}
            <form onSubmit={handleSearch} className="flex flex-col lg:flex-row gap-4 bg-white p-4 rounded-3xl border border-gray-100 shadow-sm">
                <div className="flex-1 relative">
                    <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                    <input 
                        type="text" 
                        value={correlationIdFilter}
                        onChange={(e) => setCorrelationIdFilter(e.target.value)}
                        placeholder="Filter by Trace/Correlation ID..." 
                        className="w-full pl-11 pr-4 py-3 bg-gray-50 border-none rounded-2xl text-sm outline-none focus:ring-2 focus:ring-blue-100 transition-all font-medium"
                    />
                </div>
                <div className="flex gap-2">
                    <select 
                        value={entityFilter}
                        onChange={(e) => setEntityFilter(e.target.value)}
                        className="bg-gray-50 px-6 py-3 rounded-2xl text-sm font-bold text-gray-600 outline-none border-none cursor-pointer"
                    >
                        <option value="">All Entities</option>
                        <option value="ComplianceState">Compliance States</option>
                        <option value="Case">Workflows / Cases</option>
                        <option value="Asset">Assets</option>
                        <option value="Policy">Insurance Policies</option>
                    </select>
                    <button type="submit" className="flex items-center gap-2 px-8 py-3 bg-gray-900 text-white rounded-2xl text-sm font-black uppercase hover:bg-blue-600 transition-all active:scale-95 shadow-xl shadow-gray-200">
                        <Filter className="w-4 h-4" /> Apply
                    </button>
                </div>
            </form>

            <div className="bg-white rounded-3xl border border-gray-100 shadow-sm overflow-hidden">
                <DataTable 
                    columns={columns} 
                    data={data} 
                    loading={loading} 
                />
            </div>

            <Pagination 
                page={page} 
                totalPages={Math.ceil(totalCount / pageSize)} 
                onPageChange={setPage} 
            />
        </div>
    );
};

export default AuditPage;
