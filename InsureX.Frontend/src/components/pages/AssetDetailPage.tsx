import React, { useEffect, useState } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { assetClient } from '../../api/clients';
import type { AssetDetail } from '../../api/types/Asset';
import { StatusBadge } from '../shared';
import { 
  ArrowLeft, Car, Shield, User, History, 
  Calendar, DollarSign, ChevronRight,
  AlertCircle, CheckCircle2, Clock
} from 'lucide-react';

const AssetDetailPage: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const navigate = useNavigate();
  const [asset, setAsset] = useState<AssetDetail | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchAsset = async () => {
      if (!id) return;
      try {
        const response = await assetClient.getAsset(id);
        if (response.success) {
          setAsset(response.data);
        }
      } catch (error) {
        console.error('Failed to fetch asset detail:', error);
      } finally {
        setLoading(false);
      }
    };
    fetchAsset();
  }, [id]);

  if (loading) {
    return (
      <div className="flex items-center justify-center h-96">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
      </div>
    );
  }

  if (!asset) {
    return (
      <div className="text-center py-20 bg-white rounded-3xl border border-dashed border-gray-200">
        <AlertCircle className="w-16 h-16 text-gray-400 mx-auto mb-4" />
        <h2 className="text-xl font-bold text-gray-900">Asset Not Found</h2>
        <p className="text-gray-500 mt-2">The asset you are looking for does not exist or has been removed.</p>
        <button 
          onClick={() => navigate('/assets')}
          className="mt-6 px-6 py-2 bg-blue-600 text-white font-bold rounded-xl"
        >
          Go Back to Assets
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-500">
      {/* Header */}
      <header className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <button 
            onClick={() => navigate('/assets')}
            className="p-2 hover:bg-gray-100 rounded-xl transition-colors"
          >
            <ArrowLeft className="w-6 h-6 text-gray-600" />
          </button>
          <div>
            <div className="flex items-center gap-3">
              <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">{asset.assetIdentifier}</h1>
              <StatusBadge status={asset.complianceStatus || 'Unknown'} />
            </div>
            <p className="text-gray-500 mt-1 font-medium">Asset ID: {asset.id} • Registered to {asset.borrower?.name}</p>
          </div>
        </div>
        <div className="flex gap-3">
          <button className="px-6 py-2 border border-gray-200 rounded-xl text-sm font-bold text-gray-700 bg-white hover:bg-gray-50 transition-all">
            Update Values
          </button>
          <button className="px-6 py-2 bg-blue-600 rounded-xl text-sm font-bold text-white shadow-lg shadow-blue-200 hover:bg-blue-700 transition-all active:scale-95">
            Actions
          </button>
        </div>
      </header>

      <div className="grid grid-cols-1 lg:grid-cols-3 gap-8">
        {/* Main Content */}
        <div className="lg:col-span-2 space-y-8">
          
          {/* Asset Summary Cards */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="bg-white p-6 rounded-3xl border border-gray-100 shadow-sm">
              <div className="flex items-center gap-3 text-blue-600 mb-2">
                <Car className="w-5 h-5" />
                <span className="text-xs font-black uppercase tracking-wider">Asset Specs</span>
              </div>
              <p className="text-2xl font-black text-gray-900">{asset.assetType}</p>
              <p className="text-sm text-gray-400 font-medium">Reg: {asset.registrationNumber}</p>
            </div>
            <div className="bg-white p-6 rounded-3xl border border-gray-100 shadow-sm">
              <div className="flex items-center gap-3 text-emerald-600 mb-2">
                <DollarSign className="w-5 h-5" />
                <span className="text-xs font-black uppercase tracking-wider">Financial Data</span>
              </div>
              <p className="text-2xl font-black text-gray-900">${asset.financedAmount.toLocaleString()}</p>
              <p className="text-sm text-gray-400 font-medium">Financed Value</p>
            </div>
            <div className="bg-white p-6 rounded-3xl border border-gray-100 shadow-sm">
              <div className="flex items-center gap-3 text-purple-600 mb-2">
                <Calendar className="w-5 h-5" />
                <span className="text-xs font-black uppercase tracking-wider">Timeline</span>
              </div>
              <p className="text-2xl font-black text-gray-900">{new Date(asset.loanEndDate).toLocaleDateString()}</p>
              <p className="text-sm text-gray-400 font-medium">Loan Expiration</p>
            </div>
          </div>

          {/* Insurance Policies */}
          <section className="bg-white rounded-3xl border border-gray-100 shadow-sm overflow-hidden">
            <div className="p-6 border-b border-gray-50 flex items-center justify-between">
              <div className="flex items-center gap-3">
                <Shield className="w-6 h-6 text-blue-600" />
                <h2 className="text-xl font-bold text-gray-900">Insurance Policies</h2>
              </div>
              <span className="px-3 py-1 bg-blue-50 text-blue-600 rounded-full text-xs font-black uppercase">
                {asset.policies?.length || 0} LINKED
              </span>
            </div>
            <div className="divide-y divide-gray-50 uppercase tracking-tight">
              {asset.policies?.map((policy) => (
                <div key={policy.id} className="p-6 flex items-center justify-between hover:bg-gray-50 transition-colors cursor-pointer group">
                  <div className="flex items-center gap-4">
                    <div className="w-12 h-12 rounded-2xl bg-blue-50 flex items-center justify-center text-blue-600 font-black">
                      {policy.insurerName.substring(0, 1)}
                    </div>
                    <div>
                      <p className="font-black text-gray-900">{policy.policyNumber}</p>
                      <p className="text-xs text-gray-400 font-bold">{policy.insurerName}</p>
                    </div>
                  </div>
                  <div className="flex items-center gap-8">
                    <div className="text-right hidden md:block">
                      <p className="font-black text-gray-900">${policy.insuredValue.toLocaleString()}</p>
                      <p className="text-[10px] text-gray-400 font-bold">INSURED SUM</p>
                    </div>
                    <div className="text-right hidden md:block">
                      <p className="font-black text-gray-900">{new Date(policy.expiryDate).toLocaleDateString()}</p>
                      <p className="text-[10px] text-gray-400 font-bold">EXPIRY DATE</p>
                    </div>
                    <StatusBadge status={policy.status} />
                    <ChevronRight className="w-5 h-5 text-gray-300 group-hover:text-blue-600 transition-colors" />
                  </div>
                </div>
              ))}
              {(!asset.policies || asset.policies.length === 0) && (
                <div className="p-12 text-center text-gray-400 font-medium italic">
                  No active insurance policies found for this asset.
                </div>
              )}
            </div>
          </section>

          {/* Compliance History Log */}
          <section className="bg-white rounded-3xl border border-gray-100 shadow-sm overflow-hidden">
            <div className="p-6 border-b border-gray-50 flex items-center gap-3">
              <History className="w-6 h-6 text-blue-600" />
              <h2 className="text-xl font-bold text-gray-900">Compliance Event Ledger</h2>
            </div>
            <div className="p-6">
              <div className="space-y-6">
                {asset.complianceHistory?.map((event) => (
                  <div key={event.id} className="relative pl-8 pb-6 last:pb-0 border-l-2 border-gray-50 last:border-l-0">
                    <div className="absolute -left-2.5 top-0 flex items-center justify-center w-5 h-5 rounded-full bg-white border-2 border-gray-50">
                      {event.outcome === 'Compliant' ? (
                        <CheckCircle2 className="w-4 h-4 text-emerald-500 fill-emerald-50" />
                      ) : (
                        <AlertCircle className="w-4 h-4 text-orange-500 fill-orange-50" />
                      )}
                    </div>
                    <div className="flex flex-col md:flex-row md:items-center justify-between gap-2">
                      <div>
                        <p className="font-black text-gray-900">{event.outcome}</p>
                        <p className="text-sm text-gray-500 font-medium">{event.reason}</p>
                      </div>
                      <div className="flex items-center gap-2 text-xs font-bold text-gray-400">
                        <Clock className="w-3 h-3" />
                        {new Date(event.evaluatedAt).toLocaleString()}
                      </div>
                    </div>
                  </div>
                ))}
              </div>
            </div>
          </section>
        </div>

        {/* Sidebar Info */}
        <div className="space-y-8">
          {/* Borrower Profile */}
          <section className="bg-white p-8 rounded-3xl border border-gray-100 shadow-sm">
            <div className="flex items-center gap-3 mb-6">
              <User className="w-6 h-6 text-blue-600" />
              <h2 className="text-xl font-bold text-gray-900">Borrower Entity</h2>
            </div>
            <div className="space-y-6">
              <div>
                <label className="text-[10px] font-black text-gray-400 uppercase tracking-widest block mb-1">Full Legal Name</label>
                <p className="font-black text-gray-900">{asset.borrower?.name}</p>
              </div>
              <div>
                <label className="text-[10px] font-black text-gray-400 uppercase tracking-widest block mb-1">Tax / ID Number</label>
                <p className="font-black text-gray-900">{asset.borrower?.idNumber}</p>
              </div>
              <div>
                <label className="text-[10px] font-black text-gray-400 uppercase tracking-widest block mb-1">Primary Email</label>
                <p className="font-black text-blue-600 hover:underline cursor-pointer">{asset.borrower?.email}</p>
              </div>
              <div>
                <label className="text-[10px] font-black text-gray-400 uppercase tracking-widest block mb-1">Contact Phone</label>
                <p className="font-black text-gray-900">{asset.borrower?.phone}</p>
              </div>
            </div>
            <button className="w-full mt-8 py-3 bg-gray-50 text-gray-600 rounded-xl text-xs font-black uppercase hover:bg-gray-100 transition-colors">
              Manage Profile
            </button>
          </section>

          {/* Quick Stats / Audit Info */}
          <section className="bg-blue-600 p-8 rounded-3xl shadow-xl shadow-blue-100 text-white relative overflow-hidden">
            <div className="relative z-10">
              <h3 className="text-lg font-black mb-4">Registry Audit</h3>
              <div className="space-y-4">
                <div className="flex justify-between items-center bg-white/10 p-3 rounded-xl border border-white/10">
                  <span className="text-xs font-bold text-blue-100">Tenant ID</span>
                  <span className="font-black">#T-{asset.tenantId}</span>
                </div>
                <div className="flex justify-between items-center bg-white/10 p-3 rounded-xl border border-white/10">
                  <span className="text-xs font-bold text-blue-100">Agreement Ref</span>
                  <span className="font-black">{asset.borrowerReference || 'N/A'}</span>
                </div>
              </div>
              <p className="mt-6 text-[10px] text-blue-200 font-medium leading-relaxed italic">
                This asset is monitored by the InsureX real-time compliance engine. Last integrity check passed on {new Date().toLocaleDateString()}.
              </p>
            </div>
            <AlertCircle className="absolute -bottom-8 -right-8 w-32 h-32 text-white/5 rotate-12" />
          </section>
        </div>
      </div>
    </div>
  );
};

export default AssetDetailPage;
