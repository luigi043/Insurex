import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { adminClient, reportClient } from '../../api/clients';
import type { User } from '../../api/types/Admin';
import { DataTable, StatusBadge, Pagination } from '../shared';
import { 
    Users, UserPlus, Search, 
    AtSign, ShieldCheck, Building2, Download
} from 'lucide-react';

const UsersPage: React.FC = () => {
    const navigate = useNavigate();
    const [data, setData] = useState<User[]>([]);
    const [totalCount, setTotalCount] = useState(0);
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(true);
    const [search, setSearch] = useState('');
    const pageSize = 10;

    const fetchUsers = async () => {
        setLoading(true);
        try {
            const response = await adminClient.getUsers({ 
                page, 
                pageSize,
                query: search || undefined
            });
            setData(response.items);
            setTotalCount(response.totalCount);
        } catch (error) {
            console.error('Failed to fetch users:', error);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, [page]);

    const handleSearch = (e: React.FormEvent) => {
        e.preventDefault();
        setPage(1);
        fetchUsers();
    };

    const handleExport = async () => {
        try {
            await reportClient.exportUsers();
        } catch (error) {
            console.error('Failed to export users:', error);
        }
    };

    const columns = [
        { 
            header: 'Full Name', 
            key: 'fullName',
            render: (u: User) => (
                <div className="flex items-center gap-3">
                    <div className="w-9 h-9 rounded-full bg-blue-100 flex items-center justify-center text-blue-600 font-black text-xs">
                        {u.fullName.split(' ').map(n => n[0]).join('')}
                    </div>
                    <div>
                        <p className="font-bold text-gray-900 leading-none">{u.fullName}</p>
                        <p className="text-[10px] text-gray-400 mt-1 font-mono uppercase tracking-tighter">ID: {u.id}</p>
                    </div>
                </div>
            )
        },
        { 
            header: 'Contact', 
            key: 'email',
            render: (u: User) => (
                <div className="space-y-1">
                    <div className="flex items-center gap-2 text-xs text-gray-600 font-medium">
                        <AtSign className="w-3 h-3 text-gray-400" />
                        {u.email}
                    </div>
                    <div className="flex items-center gap-2 text-[10px] text-gray-400 font-bold uppercase tracking-widest leading-none">
                        <Users className="w-3 h-3" />
                        {u.userName}
                    </div>
                </div>
            )
        },
        { 
            header: 'Organization', 
            key: 'tenantId',
            render: (u: User) => (
                <div className="flex items-center gap-2">
                    <Building2 className="w-3.5 h-3.5 text-gray-400" />
                    <span className="text-xs font-bold text-gray-700">
                        {u.tenantId ? `Tenant #${u.tenantId}` : 'System / Global'}
                    </span>
                </div>
            )
        },
        { 
            header: 'Access Role', 
            key: 'role',
            render: (u: User) => (
                <div className="flex items-center gap-2">
                    <ShieldCheck className="w-3.5 h-3.5 text-blue-500" />
                    <span className="text-xs font-black text-gray-900 uppercase tracking-tight">{u.role}</span>
                </div>
            )
        },
        { 
            header: 'Status', 
            key: 'isActive',
            render: (u: User) => <StatusBadge status={u.isActive ? 'Active' : 'Inactive'} type="compliance" />
        },
        {
            header: 'Actions',
            key: 'actions',
            render: (u: User) => (
                <button 
                    onClick={() => navigate(`/users/${u.id}/edit`)}
                    className="p-2 text-gray-400 hover:text-blue-600 hover:bg-blue-50 rounded-xl transition-all"
                    title="Edit User"
                >
                    <AtSign className="w-4 h-4" /> {/* Or Edit icon, using AtSign for now as it exists */}
                </button>
            )
        }
    ];

    return (
        <div className="space-y-8 animate-in fade-in slide-in-from-bottom-2 duration-500">
            <header className="flex flex-col md:flex-row md:items-center justify-between gap-6">
                <div>
                    <h1 className="text-3xl font-black text-gray-900 tracking-tight flex items-center gap-4">
                        <Users className="w-8 h-8 text-blue-600" />
                        User Management
                    </h1>
                    <p className="text-gray-500 mt-1 font-medium">Manage system access, roles, and organizational assignments.</p>
                </div>
                
                <div className="flex gap-3">
                    <button 
                        onClick={handleExport}
                        className="flex items-center gap-2 px-6 py-3 bg-white text-gray-700 border border-gray-200 rounded-2xl text-sm font-black uppercase hover:bg-gray-50 transition-all active:scale-95 shadow-sm"
                    >
                        <Download className="w-4 h-4" /> Export
                    </button>
                    <button 
                        onClick={() => navigate('/users/new')}
                        className="flex items-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-2xl text-sm font-black uppercase hover:bg-blue-700 transition-all active:scale-95 shadow-xl shadow-blue-100"
                    >
                        <UserPlus className="w-4 h-4" /> Add New User
                    </button>
                </div>
            </header>

            <div className="flex flex-col lg:flex-row gap-4">
                <form onSubmit={handleSearch} className="flex-1 relative">
                    <Search className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                    <input 
                        type="text" 
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                        placeholder="Search by name, email or username..." 
                        className="w-full pl-11 pr-4 py-4 bg-white border border-gray-100 rounded-3xl text-sm outline-none focus:ring-4 focus:ring-blue-50 transition-all font-medium shadow-sm"
                    />
                </form>
            </div>

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

export default UsersPage;
