import React, { useEffect, useState } from 'react';
import { billingClient } from '../../api/clients';
import { RefreshCw, Receipt, DollarSign } from 'lucide-react';

const BillingPage: React.FC = () => {
  const [invoices, setInvoices] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchInvoices();
  }, []);

  const fetchInvoices = async () => {
    setLoading(true);
    try {
      const res = await billingClient.getInvoices();
      setInvoices(res.data || []);
    } catch (error) {
      console.error('Failed to fetch invoices:', error);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center h-full min-h-[400px]">
        <RefreshCw className="w-10 h-10 text-blue-500 animate-spin mb-4" />
        <p className="text-gray-500 font-medium">Loading billing data...</p>
      </div>
    );
  }

  const totalAmount = invoices.reduce((sum, inv) => sum + Number(inv.dcAmount || inv.Amount || 0), 0);

  return (
    <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header>
        <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Billing & Invoices</h1>
        <p className="text-sm text-gray-500 mt-1">View partner invoices and manage charges</p>
      </header>

      {/* Summary Cards */}
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        <div className="bg-white p-6 rounded-3xl shadow-sm border border-gray-100">
          <div className="flex items-center gap-3">
            <div className="p-2.5 rounded-2xl bg-blue-500 text-white"><Receipt className="w-5 h-5" /></div>
            <div>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest">Total Invoices</p>
              <p className="text-3xl font-black text-blue-600">{invoices.length}</p>
            </div>
          </div>
        </div>
        <div className="bg-white p-6 rounded-3xl shadow-sm border border-gray-100">
          <div className="flex items-center gap-3">
            <div className="p-2.5 rounded-2xl bg-emerald-500 text-white"><DollarSign className="w-5 h-5" /></div>
            <div>
              <p className="text-xs font-bold text-gray-400 uppercase tracking-widest">Total Amount</p>
              <p className="text-3xl font-black text-emerald-600">R {totalAmount.toLocaleString('en-ZA', { minimumFractionDigits: 2 })}</p>
            </div>
          </div>
        </div>
      </div>

      {/* Invoices Table */}
      <div className="bg-white rounded-3xl shadow-sm border border-gray-100 overflow-hidden">
        <div className="overflow-x-auto">
          <table className="w-full text-sm">
            <thead className="bg-gray-50 text-gray-500 text-xs uppercase tracking-wider">
              <tr>
                <th className="px-6 py-4 text-left">Invoice #</th>
                <th className="px-6 py-4 text-left">Partner</th>
                <th className="px-6 py-4 text-left">Description</th>
                <th className="px-6 py-4 text-right">Amount</th>
                <th className="px-6 py-4 text-left">Status</th>
                <th className="px-6 py-4 text-left">Date</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-100">
              {invoices.length > 0 ? invoices.map((inv, i) => (
                <tr key={i} className="hover:bg-gray-50 transition-colors">
                  <td className="px-6 py-4 font-semibold text-blue-600">{inv.vcInvoice_Number || inv.InvoiceNumber || `INV-${i + 1}`}</td>
                  <td className="px-6 py-4">{inv.vcPartner_Name || inv.PartnerName || '-'}</td>
                  <td className="px-6 py-4 text-gray-500">{inv.vcDescription || inv.Description || '-'}</td>
                  <td className="px-6 py-4 text-right font-mono font-semibold">R {Number(inv.dcAmount || inv.Amount || 0).toLocaleString('en-ZA', { minimumFractionDigits: 2 })}</td>
                  <td className="px-6 py-4">
                    <span className={`px-2.5 py-1 rounded-full text-xs font-bold ${(inv.vcStatus || inv.Status || '').toLowerCase() === 'paid' ? 'bg-green-100 text-green-700' : 'bg-amber-100 text-amber-700'}`}>
                      {inv.vcStatus || inv.Status || 'Pending'}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-gray-500">{inv.dtDate || inv.Date || '-'}</td>
                </tr>
              )) : (
                <tr><td colSpan={6} className="px-6 py-12 text-center text-gray-400">No invoices found</td></tr>
              )}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
};

export default BillingPage;
