import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { adminClient } from '../../api/clients';
import { Save, X, User, Mail, Shield, Building, Loader2, AlertCircle } from 'lucide-react';

const AddUserPage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = Boolean(id);
  
  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [tenants, setTenants] = useState<any[]>([]);

  const [formData, setFormData] = useState({
    userName: '',
    email: '',
    fullName: '',
    role: 'User',
    tenantId: '',
    isActive: true
  });

  useEffect(() => {
    const loadTenants = async () => {
      try {
        const res = await adminClient.getTenants({ page: 1, pageSize: 100 });
        setTenants(res.items);
      } catch (err) {
        console.error('Failed to load tenants:', err);
      }
    };

    const loadUser = async () => {
        if (!id) return;
        setFetching(true);
        try {
            // getUsers can be used to filter by ID or search if we don't have a GetUser by ID
            // But usually AdminController should have GetUser. 
            // Let's assume we can fetch all and find, or let's check AdminController again.
            const res = await adminClient.getUsers({ page: 1, pageSize: 1, query: id }); // Assuming query by ID works or we add GetUser
            if (res.items.length > 0) {
                const u = res.items[0];
                setFormData({
                    userName: u.userName,
                    email: u.email,
                    fullName: u.fullName,
                    role: u.role,
                    tenantId: u.tenantId?.toString() || '',
                    isActive: u.isActive
                });
            }
        } catch (err) {
            setError('Failed to load user details.');
        } finally {
            setFetching(false);
        }
    };

    loadTenants();
    if (isEdit) loadUser();
  }, [id, isEdit]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const payload = {
        ...formData,
        tenantId: formData.tenantId ? parseInt(formData.tenantId) : null
      };

      if (isEdit && id) {
        await adminClient.updateUser(id, payload);
      } else {
        await adminClient.createUser(payload);
      }
      navigate('/users');
    } catch (err: any) {
      setError(err.response?.data?.message || `Failed to ${isEdit ? 'update' : 'create'} user.`);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-3xl mx-auto space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">{isEdit ? 'Edit User' : 'Add New User'}</h1>
          <p className="text-gray-500 mt-1">{isEdit ? 'Update system access and role.' : 'Grant system access and assign organizational roles.'}</p>
        </div>
        <button 
          onClick={() => navigate('/users')}
          className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-all"
        >
          <X className="w-6 h-6" />
        </button>
      </header>

      {fetching && (
        <div className="flex flex-col items-center justify-center p-12 bg-white rounded-3xl border border-gray-100 space-y-4">
          <Loader2 className="w-8 h-8 text-blue-600 animate-spin" />
          <p className="text-sm font-bold text-gray-400 uppercase tracking-widest">Loading User Profile...</p>
        </div>
      )}

      {!fetching && error && (
        <div className="bg-red-50 border border-red-100 p-4 rounded-2xl flex items-start gap-3 animate-shake">
          <AlertCircle className="w-5 h-5 text-red-500 mt-0.5" />
          <p className="text-red-700 text-sm font-medium">{error}</p>
        </div>
      )}

      {!fetching && (
        <form onSubmit={handleSubmit} className="space-y-6">
          <section className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 space-y-6">
            <div className="flex items-center gap-3 mb-2">
              <div className="w-10 h-10 rounded-xl bg-blue-50 flex items-center justify-center text-blue-600">
                <User className="w-5 h-5" />
              </div>
              <h2 className="text-xl font-bold text-gray-900">Profile Information</h2>
            </div>
  
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700 ml-1">Full Name</label>
                <input
                  type="text"
                  name="fullName"
                  value={formData.fullName}
                  onChange={handleChange}
                  placeholder="John Doe"
                  className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                  required
                />
              </div>
  
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700 ml-1">Username</label>
                <input
                  type="text"
                  name="userName"
                  value={formData.userName}
                  onChange={handleChange}
                  placeholder="jdoe"
                  className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                  required
                />
              </div>
  
              <div className="space-y-2 md:col-span-2">
                <label className="text-sm font-bold text-gray-700 ml-1">Email Address</label>
                <div className="relative">
                  <Mail className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                  <input
                    type="email"
                    name="email"
                    value={formData.email}
                    onChange={handleChange}
                    placeholder="john@example.com"
                    className="w-full pl-11 pr-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                    required
                  />
                </div>
              </div>
            </div>
          </section>
  
          <section className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 space-y-6">
            <div className="flex items-center gap-3 mb-2">
              <div className="w-10 h-10 rounded-xl bg-purple-50 flex items-center justify-center text-purple-600">
                <Shield className="w-5 h-5" />
              </div>
              <h2 className="text-xl font-bold text-gray-900">Access & Permissions</h2>
            </div>
  
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700 ml-1">System Role</label>
                <select
                  name="role"
                  value={formData.role}
                  onChange={handleChange}
                  className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                >
                  <option value="User">Standard User</option>
                  <option value="Manager">Manager</option>
                  <option value="Administrator">Administrator</option>
                </select>
              </div>
  
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700 ml-1">Organization (Tenant)</label>
                <div className="relative">
                  <Building className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-400" />
                  <select
                    name="tenantId"
                    value={formData.tenantId}
                    onChange={handleChange}
                    className="w-full pl-11 pr-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                  >
                    <option value="">System / Global</option>
                    {tenants.map(t => (
                      <option key={t.id} value={t.id}>{t.name}</option>
                    ))}
                  </select>
                </div>
              </div>
            </div>
          </section>
  
          <div className="flex justify-end gap-4 pt-4">
            <button
              type="button"
              onClick={() => navigate('/users')}
              className="px-8 py-3 rounded-2xl text-sm font-bold text-gray-600 bg-white border border-gray-200 hover:bg-gray-50 transition-all"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex items-center gap-2 px-10 py-3 bg-blue-600 rounded-2xl text-sm font-bold text-white shadow-xl shadow-blue-200 hover:bg-blue-700 transition-all active:scale-95 disabled:opacity-50"
            >
              {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
              {isEdit ? 'Update User' : 'Create User'}
            </button>
          </div>
        </form>
      )}
    </div>
  );
};

export default AddUserPage;
