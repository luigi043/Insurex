import React, { useState, useEffect } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import { adminClient } from '../../api/clients';
import { Save, X, Building, Globe, ShieldCheck, Loader2, AlertCircle } from 'lucide-react';

const RegisterTenantPage: React.FC = () => {
  const navigate = useNavigate();
  const { id } = useParams<{ id: string }>();
  const isEdit = Boolean(id);

  const [loading, setLoading] = useState(false);
  const [fetching, setFetching] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [formData, setFormData] = useState({
    name: '',
    identifier: '',
    type: 'Financer',
    isActive: true
  });

  useEffect(() => {
    const loadTenant = async () => {
      if (!id) return;
      setFetching(true);
      try {
        const res = await adminClient.getTenants({ page: 1, pageSize: 1, query: id });
        if (res.items.length > 0) {
          const t = res.items[0];
          setFormData({
            name: t.name,
            identifier: t.identifier,
            type: t.type,
            isActive: t.isActive
          });
        }
      } catch (err) {
        setError('Failed to load entity details.');
      } finally {
        setFetching(false);
      }
    };
    if (isEdit) loadTenant();
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
      if (isEdit && id) {
        await adminClient.updateTenant(parseInt(id), formData);
      } else {
        await adminClient.createTenant(formData);
      }
      navigate('/tenants');
    } catch (err: any) {
      setError(err.response?.data?.message || `Failed to ${isEdit ? 'update' : 'register'} entity.`);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-3xl mx-auto space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">{isEdit ? 'Edit Entity' : 'Register New Entity'}</h1>
          <p className="text-gray-500 mt-1">{isEdit ? 'Update organizational details and status.' : 'Onboard new financial partners, insurers, or brokerage networks.'}</p>
        </div>
        <button 
          onClick={() => navigate('/tenants')}
          className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-all"
        >
          <X className="w-6 h-6" />
        </button>
      </header>

      {fetching && (
        <div className="flex flex-col items-center justify-center p-12 bg-white rounded-3xl border border-gray-100 space-y-4">
          <Loader2 className="w-8 h-8 text-gray-900 animate-spin" />
          <p className="text-sm font-bold text-gray-400 uppercase tracking-widest">Loading Entity Profile...</p>
        </div>
      )}

      {!fetching && error && (
        <div className="bg-red-50 border border-red-100 p-4 rounded-2xl flex items-start gap-3 animate-shake text-sm font-medium text-red-700">
          <AlertCircle className="w-5 h-5 text-red-500 mt-0.5" />
          {error}
        </div>
      )}

      {!fetching && (
        <form onSubmit={handleSubmit} className="space-y-6">
          <section className="bg-white p-10 rounded-3xl shadow-sm border border-gray-100 space-y-8">
            <div className="flex items-center gap-4">
              <div className="w-12 h-12 rounded-2xl bg-gray-900 flex items-center justify-center text-white shadow-lg">
                <Building className="w-6 h-6" />
              </div>
              <div>
                <h2 className="text-xl font-bold text-gray-900">Entity Details</h2>
                <p className="text-xs text-gray-400 font-bold uppercase tracking-widest mt-1">Core Identity & Network Key</p>
              </div>
            </div>
  
            <div className="space-y-6">
              <div className="space-y-2">
                <label className="text-xs font-black text-gray-400 uppercase tracking-[0.2em] ml-1">Legal Name</label>
                <input
                  type="text"
                  name="name"
                  value={formData.name}
                  onChange={handleChange}
                  placeholder="e.g. Standard Bank Group"
                  className="w-full px-6 py-4 bg-gray-50 border-none rounded-2xl text-sm font-bold outline-none focus:ring-4 focus:ring-blue-50 transition-all placeholder:text-gray-300"
                  required
                />
              </div>
  
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <label className="text-xs font-black text-gray-400 uppercase tracking-[0.2em] ml-1">Domain Identifier</label>
                  <div className="relative">
                    <Globe className="absolute left-5 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-300" />
                    <input
                      type="text"
                      name="identifier"
                      value={formData.identifier}
                      onChange={handleChange}
                      placeholder="standardbank.co.za"
                      className="w-full pl-12 pr-6 py-4 bg-gray-50 border-none rounded-2xl text-sm font-bold outline-none focus:ring-4 focus:ring-blue-50 transition-all placeholder:text-gray-300"
                      required
                    />
                  </div>
                </div>
  
                <div className="space-y-2">
                  <label className="text-xs font-black text-gray-400 uppercase tracking-[0.2em] ml-1">Classification</label>
                  <div className="relative">
                    <ShieldCheck className="absolute left-5 top-1/2 -translate-y-1/2 w-4 h-4 text-gray-300" />
                    <select
                      name="type"
                      value={formData.type}
                      onChange={handleChange}
                      className="w-full pl-12 pr-6 py-4 bg-gray-50 border-none rounded-2xl text-sm font-bold outline-none focus:ring-4 focus:ring-blue-50 transition-all appearance-none cursor-pointer"
                    >
                      <option value="Financer">Lending Institution</option>
                      <option value="Insurer">Underwriter / Insurer</option>
                      <option value="Broker">Brokerage Network</option>
                    </select>
                  </div>
                </div>
              </div>
            </div>
          </section>
  
          <div className="flex justify-end gap-4">
            <button
              type="button"
              onClick={() => navigate('/tenants')}
              className="px-10 py-4 rounded-2xl text-xs font-black uppercase text-gray-400 hover:text-gray-600 transition-all"
            >
              Go Back
            </button>
            <button
              type="submit"
              disabled={loading}
              className="flex items-center gap-3 px-12 py-4 bg-gray-900 rounded-2xl text-xs font-black uppercase text-white shadow-2xl shadow-gray-200 hover:bg-blue-600 transition-all active:scale-95 disabled:opacity-50"
            >
              {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />}
              {isEdit ? 'Update Entity' : 'Register Network Entity'}
            </button>
          </div>
        </form>
      )}
    </div>
  );
};

export default RegisterTenantPage;
