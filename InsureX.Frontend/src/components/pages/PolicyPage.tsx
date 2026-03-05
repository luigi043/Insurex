import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { policyClient } from '../../api/clients';
import { RefreshCw, CheckCircle, XCircle, FileText, Eye, Plus } from 'lucide-react';

const PolicyPage: React.FC = () => {
  const navigate = useNavigate();
  const [transactions, setTransactions] = useState<any[]>([]);
  const [pending, setPending] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [activeTab, setActiveTab] = useState<'transactions' | 'pending'>('transactions');

  useEffect(() => {
    fetchData();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const [txRes, pendRes] = await Promise.all([
        policyClient.getTransactions(),
        policyClient.getPendingConfirmations()
      ]);
      setTransactions(txRes.data || []);
      setPending(pendRes.data || []);
    } catch (error) {
      console.error('Failed to fetch policy data:', error);
    } finally {
      setLoading(false);
    }
  };

  const handleConfirm = async (policyId: number, action: string) => {
    try {
      await policyClient.confirmPolicy(policyId, action);
      fetchData();
    } catch (error) {
      console.error('Failed to confirm policy:', error);
    }
  };

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center h-full min-h-[400px]">
        <RefreshCw className="w-10 h-10 text-blue-500 animate-spin mb-4" />
        <p className="text-gray-500 font-medium">Loading policy data...</p>
      </div>
    );
  }

  return (
    <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-4">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Policy Management</h1>
          <p className="text-sm text-gray-500 mt-1">View transactions and manage policy confirmations</p>
        </div>
        <button 
          onClick={() => navigate('/policies/new')}
          className="flex items-center gap-2 px-6 py-2 bg-blue-600 rounded-xl text-sm font-bold text-white shadow-lg shadow-blue-200 hover:bg-blue-700 transition-all active:scale-95"
        >
          <Plus className="w-4 h-4" /> New Policy
        </button>
      </header>

      {/* Tabs */}
      <div className="flex gap-2">
        <button
          onClick={() => setActiveTab('transactions')}
          className={`px-5 py-2.5 rounded-xl font-bold text-sm transition-all ${activeTab === 'transactions' ? 'bg-blue-600 text-white shadow-md' : 'bg-white text-gray-600 border border-gray-200 hover:bg-gray-50'}`}
        >
          <FileText className="w-4 h-4 inline mr-1.5" />Transactions ({transactions.length})
        </button>
        <button
          onClick={() => setActiveTab('pending')}
          className={`px-5 py-2.5 rounded-xl font-bold text-sm transition-all ${activeTab === 'pending' ? 'bg-amber-500 text-white shadow-md' : 'bg-white text-gray-600 border border-gray-200 hover:bg-gray-50'}`}
        >
          <Eye className="w-4 h-4 inline mr-1.5" />Pending Confirmations ({pending.length})
        </button>
      </div>

      {/* Transactions Table */}
      {activeTab === 'transactions' && (
        <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead className="bg-gray-50 text-gray-500 text-xs uppercase tracking-wider">
                <tr>
                  <th className="px-6 py-4 text-left">Policy #</th>
                  <th className="px-6 py-4 text-left">Customer</th>
                  <th className="px-6 py-4 text-left">Asset Type</th>
                  <th className="px-6 py-4 text-right">Finance Value</th>
                  <th className="px-6 py-4 text-left">Status</th>
                  <th className="px-6 py-4 text-left">Date</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {transactions.length > 0 ? transactions.map((tx, i) => (
                  <tr key={i} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 font-semibold text-blue-600">{tx.vcPolicy_Number || tx.PolicyNumber || '-'}</td>
                    <td className="px-6 py-4">{tx.vcCustomer_Name || tx.CustomerName || '-'}</td>
                    <td className="px-6 py-4">{tx.vcAsset_Type || tx.AssetType || '-'}</td>
                    <td className="px-6 py-4 text-right font-mono">R {Number(tx.dcFinance_Value || tx.FinanceValue || 0).toLocaleString('en-ZA', { minimumFractionDigits: 2 })}</td>
                    <td className="px-6 py-4"><span className="px-2.5 py-1 rounded-full bg-green-100 text-green-700 text-xs font-bold">{tx.vcStatus || tx.Status || '-'}</span></td>
                    <td className="px-6 py-4 text-gray-500">{tx.dtDate || tx.Date || '-'}</td>
                  </tr>
                )) : (
                  <tr><td colSpan={6} className="px-6 py-12 text-center text-gray-400">No transactions found</td></tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}

      {/* Pending Confirmations */}
      {activeTab === 'pending' && (
        <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
          <div className="overflow-x-auto">
            <table className="w-full text-sm">
              <thead className="bg-gray-50 text-gray-500 text-xs uppercase tracking-wider">
                <tr>
                  <th className="px-6 py-4 text-left">Policy #</th>
                  <th className="px-6 py-4 text-left">Customer</th>
                  <th className="px-6 py-4 text-right">Value</th>
                  <th className="px-6 py-4 text-center">Actions</th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100">
                {pending.length > 0 ? pending.map((item, i) => (
                  <tr key={i} className="hover:bg-gray-50 transition-colors">
                    <td className="px-6 py-4 font-semibold">{item.vcPolicy_Number || item.PolicyNumber || '-'}</td>
                    <td className="px-6 py-4">{item.vcCustomer_Name || item.CustomerName || '-'}</td>
                    <td className="px-6 py-4 text-right font-mono">R {Number(item.dcFinance_Value || item.FinanceValue || 0).toLocaleString('en-ZA', { minimumFractionDigits: 2 })}</td>
                    <td className="px-6 py-4 text-center space-x-2">
                      <button onClick={() => handleConfirm(item.iPolicy_Id || item.PolicyId, 'confirm')} className="px-3 py-1.5 bg-emerald-500 text-white rounded-lg text-xs font-bold hover:bg-emerald-600 transition-colors">
                        <CheckCircle className="w-3.5 h-3.5 inline mr-1" />Confirm
                      </button>
                      <button onClick={() => handleConfirm(item.iPolicy_Id || item.PolicyId, 'reject')} className="px-3 py-1.5 bg-red-500 text-white rounded-lg text-xs font-bold hover:bg-red-600 transition-colors">
                        <XCircle className="w-3.5 h-3.5 inline mr-1" />Reject
                      </button>
                    </td>
                  </tr>
                )) : (
                  <tr><td colSpan={4} className="px-6 py-12 text-center text-gray-400">No pending confirmations</td></tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      )}
    </div>
  );
};

export default PolicyPage;
