import React, { useEffect, useState } from 'react';
import { billingClient } from '../../api/clients';
import { RefreshCw, Receipt, DollarSign, Plus, X, CheckCircle } from 'lucide-react';

interface ChargeForm {
  partnerId: string;
  chargeType: string;
  amount: string;
  description: string;
}

const CHARGE_TYPES = ['Premium', 'Ad-hoc', 'Penalty', 'Administration', 'Other'];

const BillingPage: React.FC = () => {
  const [invoices, setInvoices] = useState<any[]>([]);
  const [loading, setLoading] = useState(true);
  const [showModal, setShowModal] = useState(false);
  const [submitting, setSubmitting] = useState(false);
  const [submitted, setSubmitted] = useState(false);
  const [form, setForm] = useState<ChargeForm>({
    partnerId: '',
    chargeType: CHARGE_TYPES[0],
    amount: '',
    description: '',
  });

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

  const handleAddCharge = async (e: React.FormEvent) => {
    e.preventDefault();
    setSubmitting(true);
    try {
      await billingClient.addCharge(
        Number(form.partnerId),
        form.chargeType,
        Number(form.amount),
        form.description,
      );
      setSubmitted(true);
      setTimeout(() => {
        setSubmitted(false);
        setShowModal(false);
        setForm({ partnerId: '', chargeType: CHARGE_TYPES[0], amount: '', description: '' });
        fetchInvoices();
      }, 1500);
    } catch (error) {
      console.error('Failed to add charge:', error);
    } finally {
      setSubmitting(false);
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
      <header className="flex items-end justify-between">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Billing &amp; Invoices</h1>
          <p className="text-sm text-gray-500 mt-1">View partner invoices and manage charges</p>
        </div>
        <button
          onClick={() => setShowModal(true)}
          className="flex items-center gap-2 px-5 py-2.5 bg-blue-600 text-white text-sm font-bold rounded-xl hover:bg-blue-700 transition-colors shadow-md active:scale-95"
        >
          <Plus className="w-4 h-4" /> New Charge
        </button>
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

      {/* New Charge Modal */}
      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm animate-in fade-in duration-200">
          <div className="bg-white rounded-3xl shadow-2xl w-full max-w-md mx-4 p-8 relative animate-in zoom-in-95 duration-200">
            <button
              onClick={() => setShowModal(false)}
              className="absolute top-5 right-5 p-2 rounded-xl hover:bg-gray-100 transition-colors"
            >
              <X className="w-5 h-5 text-gray-500" />
            </button>
            <h2 className="text-xl font-extrabold text-gray-900 mb-1">New Charge</h2>
            <p className="text-sm text-gray-500 mb-6">Record a new billing charge against a partner</p>

            {submitted ? (
              <div className="flex flex-col items-center justify-center py-8 gap-3">
                <CheckCircle className="w-12 h-12 text-emerald-500" />
                <p className="font-bold text-emerald-700 text-lg">Charge added successfully!</p>
              </div>
            ) : (
              <form onSubmit={handleAddCharge} className="space-y-4">
                <div>
                  <label className="block text-xs font-bold text-gray-500 uppercase tracking-widest mb-1.5">Partner ID</label>
                  <input
                    type="number"
                    required
                    value={form.partnerId}
                    onChange={(e) => setForm({ ...form, partnerId: e.target.value })}
                    placeholder="e.g. 42"
                    className="w-full px-4 py-3 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                  />
                </div>
                <div>
                  <label className="block text-xs font-bold text-gray-500 uppercase tracking-widest mb-1.5">Charge Type</label>
                  <select
                    value={form.chargeType}
                    onChange={(e) => setForm({ ...form, chargeType: e.target.value })}
                    className="w-full px-4 py-3 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition bg-white"
                  >
                    {CHARGE_TYPES.map((ct) => (
                      <option key={ct} value={ct}>{ct}</option>
                    ))}
                  </select>
                </div>
                <div>
                  <label className="block text-xs font-bold text-gray-500 uppercase tracking-widest mb-1.5">Amount (R)</label>
                  <input
                    type="number"
                    step="0.01"
                    min="0"
                    required
                    value={form.amount}
                    onChange={(e) => setForm({ ...form, amount: e.target.value })}
                    placeholder="0.00"
                    className="w-full px-4 py-3 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition"
                  />
                </div>
                <div>
                  <label className="block text-xs font-bold text-gray-500 uppercase tracking-widest mb-1.5">Description</label>
                  <textarea
                    rows={3}
                    value={form.description}
                    onChange={(e) => setForm({ ...form, description: e.target.value })}
                    placeholder="Brief description of the charge..."
                    className="w-full px-4 py-3 border border-gray-200 rounded-xl text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 transition resize-none"
                  />
                </div>
                <button
                  type="submit"
                  disabled={submitting}
                  className="w-full py-3 bg-blue-600 text-white font-bold rounded-xl hover:bg-blue-700 transition-colors disabled:opacity-60 active:scale-95 mt-2"
                >
                  {submitting ? 'Saving...' : 'Add Charge'}
                </button>
              </form>
            )}
          </div>
        </div>
      )}
    </div>
  );
};

export default BillingPage;

