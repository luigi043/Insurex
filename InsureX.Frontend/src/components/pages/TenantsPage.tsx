import React, { useEffect, useState } from 'react';
import { adminClient } from '../../api/clients';
import type { Tenant } from '../../api/types/Admin';
import { DataTable, StatusBadge, Pagination } from '../shared';
import { 
    Building, Plus, Search, 
    Hash, Tag, CalendarDays
} from 'lucide-react';

const TenantsPage: React.FC = () => {
    const [data, setData] = useState<Tenant[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const pageSize = 10;

    const fetchTenants = async () => {
        setLoading(true);
        try {
            const response = await adminClient.getTenants({ 
                page, 
                pageSize,
                query: search || undefined
            });
            setData(response.items);
            setTotalCount(response.totalCount);
        } catch (error) {
            console.error('Failed to fetch tenants:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchTenants();
    }, [page]);

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        setPage(1);
        fetchTenants();
    };

    const columns = [
        { 
            header: 'Organization Name', 
            key: 'name',
            render: (t: Tenant) => (
                <div className="flex items-center gap-4">
                    <div className="w-12 h-12 rounded-2xl bg-gray-50 flex items-center justify-center text-gray-400 group-hover:bg-blue-50 group-hover:text-blue-500 transition-colors">
                        <Building className="w-6 h-6" />
                    </div>
                    <div>
                        <p className="font-black text-gray-900 leading-tight tracking-tight uppercase text-sm">{t.name}</p>
                        <p className="text-[10px] text-gray-400 mt-1 font-bold tracking-widest uppercase">Member Since: {new Date(t.createdAt).toLocaleDateString()}</p>
                    </div>
                </div>
            )
        },
        { 
            header: 'System ID', 
            key: 'identifier',
            render: (t: Tenant) => (
                <div className="flex items-center gap-2 px-3 py-1.5 bg-gray-50 rounded-lg w-fit">
                    <Hash className="w-3.5 h-3.5 text-gray-400" />
                    <span className="font-mono text-[11px] font-bold text-gray-600">{t.identifier}</span>
                </div>
            )
        },
        { 
            header: 'Classification', 
            key: 'type',
            render: (t: Tenant) => (
                <div className="flex items-center gap-2">
                    <Tag className="w-3.5 h-3.5 text-blue-400" />
                    <span className="text-xs font-black text-gray-900 uppercase tracking-tight">{t.type}</span>
                </div>
            )
        },
        { 
            header: 'Health Status', 
            key: 'isActive',
            render: (t: Tenant) => (
                <div className="flex items-center gap-3">
                    <StatusBadge status={t.isActive ? 'Active' : 'Suspended'} type="compliance" />
                    {t.isActive && (
                        <div className="flex gap-1">
                            <div className="w-1.5 h-1.5 rounded-full bg-emerald-400 animate-pulse" />
                            <div className="w-1.5 h-1.5 rounded-full bg-emerald-400 animate-pulse delay-75" />
                        </div>
                    )}
                </div>
            )
        }
    ];

    return (
        <div className="space-y-8 animate-in fade-in slide-in-from-bottom-2 duration-500">
            <header className="flex flex-col md:flex-row md:items-center justify-between gap-6">
                <div>
                    <div className="flex items-center gap-3 text-blue-600 mb-2">
                        <Building className="w-5 h-5 font-black uppercase tracking-[0.2em]" />
                        <span className="text-xs font-black uppercase tracking-[0.2em]">Network Topology</span>
                    </div>
                    <h1 className="text-3xl font-black text-gray-900 tracking-tight">Organization Registry</h1>
                    <p className="text-gray-500 mt-1 font-medium">Global directory of financial entities, insurers, and brokerage networks.</p>
                </div>
                
                <button className="flex items-center gap-2 px-8 py-4 bg-gray-900 text-white rounded-2xl text-xs font-black uppercase hover:bg-blue-600 transition-all active:scale-95 shadow-2xl shadow-gray-200">
                    <Plus className="w-4 h-4" /> Register Entity
                </button>
            </header>

            <div className="flex flex-col lg:flex-row gap-4">
                <form onSubmit={handleSearch} className="flex-1 relative group">
                    <Search className="absolute left-6 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400 group-focus-within:text-blue-500 transition-colors" />
                    <input 
                        type="text" 
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                        placeholder="Search Registry by Name or Code..." 
                        className="w-full pl-14 pr-4 py-5 bg-white border border-gray-100 rounded-[2rem] text-sm outline-none focus:ring-4 focus:ring-blue-50 transition-all font-bold tracking-tight shadow-sm"
                    />
                </form>
            </div>

            <div className="bg-white rounded-[2.5rem] border border-gray-100 shadow-sm overflow-hidden group">
                <DataTable 
                    columns={columns} 
                    data={data} 
                    loading={loading} 
                />
            </div>

            <footer className="flex flex-col md:flex-row items-center justify-between gap-6 pt-4">
                <div className="flex items-center gap-4 text-gray-400">
                    <CalendarDays className="w-5 h-5" />
                    <span className="text-xs font-bold uppercase tracking-widest">Last Synced: {new Date().toLocaleTimeString()}</span>
                </div>
                <Pagination 
                    page={page} 
                    totalPages={Math.ceil(totalCount / pageSize)} 
                    onPageChange={setPage} 
                />
            </footer>
        </div>
    );
};

export default TenantsPage;
