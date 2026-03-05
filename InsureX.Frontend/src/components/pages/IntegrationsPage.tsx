import React, { useEffect, useState } from 'react';
import { integrationClient } from '../../api/clients';
import { 
  Webhook, Shield, Link, RefreshCw, Trash2, 
  Settings, Plus, X, Check, Copy, 
  Loader2, Info
} from 'lucide-react';

interface PartnerWebhookConfig {
  id: number;
  tenantId: number;
  targetUrl: string;
  secret: string;
  isActive: boolean;
  subscribedEvents: string;
  updatedAt: string;
}

const IntegrationsPage: React.FC = () => {
  const [configs, setConfigs] = useState<PartnerWebhookConfig[]>([]);
  const [loading, setLoading] = useState(true);
  const [isAdding, setIsAdding] = useState(false);
  const [newConfig, setNewConfig] = useState({ targetUrl: '', subscribedEvents: 'policy.confirmed' });
  const [copiedId, setCopiedId] = useState<string | null>(null);

  const fetchConfigs = async () => {
    setLoading(true);
    try {
      const data = await integrationClient.getWebhooks();
      setConfigs(data);
    } catch (error) {
      console.error('Failed to fetch integrations:', error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchConfigs();
  }, []);

  const handleCreate = async () => {
    try {
      await integrationClient.createWebhook({ 
        targetUrl: newConfig.targetUrl, 
        subscribedEvents: newConfig.subscribedEvents,
        isActive: true 
      });
      setIsAdding(false);
      setNewConfig({ targetUrl: '', subscribedEvents: 'policy.confirmed' });
      fetchConfigs();
    } catch (error) {
      alert('Failed to create integration.');
    }
  };

  const handleUpdate = async (id: number, data: Partial<PartnerWebhookConfig>) => {
    try {
      await integrationClient.updateWebhook(id, data);
      fetchConfigs();
    } catch (error) {
      alert('Failed to update integration.');
    }
  };

  const handleDelete = async (id: number) => {
    if (!confirm('Are you sure you want to delete this integration config?')) return;
    try {
      await integrationClient.deleteWebhook(id);
      fetchConfigs();
    } catch (error) {
      alert('Failed to delete integration.');
    }
  };

  const handleRotateSecret = async (id: number) => {
    if (!confirm('Rotating the secret will immediately break existing integrations until updated on the partner side. Proceed?')) return;
    try {
      await integrationClient.rotateSecret(id);
      fetchConfigs();
    } catch (error) {
      alert('Failed to rotate secret.');
    }
  };

  const copyToClipboard = (text: string, id: string) => {
    navigator.clipboard.writeText(text);
    setCopiedId(id);
    setTimeout(() => setCopiedId(null), 2000);
  };

  if (loading) {
    return (
      <div className="h-full flex items-center justify-center">
        <Loader2 className="w-12 h-12 text-blue-600 animate-spin" />
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto space-y-10 animate-in fade-in duration-500">
      <header className="flex flex-col md:flex-row md:items-center justify-between gap-6">
        <div>
          <h1 className="text-4xl font-black text-gray-900 tracking-tight">Partner Integrations</h1>
          <p className="text-gray-500 mt-2 font-medium">Manage webhook destinations, security secrets, and event subscriptions for external synchronisation.</p>
        </div>
        <button 
          onClick={() => setIsAdding(true)}
          className="flex items-center gap-2 px-6 py-3 bg-blue-600 text-white rounded-2xl font-bold shadow-xl shadow-blue-100 hover:bg-blue-700 transition-all active:scale-95"
        >
          <Plus className="w-5 h-5" /> New Endpoint
        </button>
      </header>

      {/* Integration List */}
      <div className="grid grid-cols-1 gap-8">
        {configs.length === 0 && !isAdding && (
          <div className="bg-white border-2 border-dashed border-gray-200 rounded-[2.5rem] p-20 text-center space-y-4">
            <div className="w-20 h-20 bg-gray-50 rounded-3xl flex items-center justify-center mx-auto text-gray-300">
              <Webhook className="w-10 h-10" />
            </div>
            <h3 className="text-xl font-bold text-gray-900">No active integrations</h3>
            <p className="text-gray-400 max-w-sm mx-auto">Connect your external ledger or insurer system to receive real-time updates via secure webhooks.</p>
            <button 
              onClick={() => setIsAdding(true)}
              className="px-6 py-2 bg-gray-900 text-white rounded-xl font-bold hover:bg-black transition-all"
            >
              Set up first integration
            </button>
          </div>
        )}

        {/* Adding Form */}
        {isAdding && (
          <div className="bg-blue-50 border-2 border-blue-200 rounded-[2.5rem] p-8 space-y-6 animate-in slide-in-from-top-4 duration-300">
            <div className="flex justify-between items-center">
              <h3 className="text-xl font-black text-blue-900">Configure Webhook Target</h3>
              <button onClick={() => setIsAdding(false)} className="p-2 hover:bg-blue-100 rounded-full text-blue-400"><X /></button>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-2">
                <label className="text-xs font-black text-blue-700 uppercase tracking-widest pl-1">Destination URL</label>
                <input 
                  type="url" 
                  placeholder="https://your-api.com/webhooks"
                  className="w-full px-5 py-4 bg-white border-none rounded-2xl shadow-sm outline-none focus:ring-2 focus:ring-blue-400 font-medium"
                  value={newConfig.targetUrl}
                  onChange={(e) => setNewConfig({ ...newConfig, targetUrl: e.target.value })}
                />
              </div>
              <div className="space-y-2">
                <label className="text-xs font-black text-blue-700 uppercase tracking-widest pl-1">Events Subscription</label>
                <select 
                  className="w-full px-5 py-4 bg-white border-none rounded-2xl shadow-sm outline-none focus:ring-2 focus:ring-blue-400 font-bold text-gray-700"
                  value={newConfig.subscribedEvents}
                  onChange={(e) => setNewConfig({ ...newConfig, subscribedEvents: e.target.value })}
                >
                  <option value="*">All Events (*)</option>
                  <option value="policy.confirmed">Policy Confirmations Only</option>
                  <option value="asset.imported">Asset Ingestions Only</option>
                  <option value="claim.approved">Claim Approvals Only</option>
                </select>
              </div>
            </div>
            <div className="flex justify-end gap-3 pt-2">
              <button onClick={() => setIsAdding(false)} className="px-6 py-3 font-bold text-blue-600 hover:bg-blue-100 rounded-xl">Cancel</button>
              <button onClick={handleCreate} className="px-8 py-3 bg-blue-600 text-white font-black rounded-xl shadow-lg shadow-blue-200 hover:bg-blue-700 transition-all">Enable Integration</button>
            </div>
          </div>
        )}

        {configs.map(config => (
          <div key={config.id} className="bg-white border border-gray-100 rounded-[2.5rem] shadow-sm hover:shadow-xl transition-all duration-300 overflow-hidden">
            <div className="p-8">
              <div className="flex flex-col lg:flex-row justify-between gap-8">
                <div className="flex-1 space-y-6">
                  <div className="flex items-center gap-4">
                    <div className={`p-4 rounded-2xl ${config.isActive ? 'bg-emerald-50 text-emerald-600' : 'bg-gray-50 text-gray-400'}`}>
                      <Link className="w-8 h-8" />
                    </div>
                    <div>
                      <div className="flex items-center gap-3">
                        <h4 className="text-2xl font-black text-gray-900 tracking-tight">{new URL(config.targetUrl).hostname}</h4>
                        <span className={`px-3 py-1 rounded-full text-[10px] font-black uppercase tracking-widest ${config.isActive ? 'bg-emerald-100 text-emerald-700' : 'bg-gray-100 text-gray-500'}`}>
                          {config.isActive ? 'Active' : 'Disabled'}
                        </span>
                      </div>
                      <p className="text-gray-400 font-medium mt-1 truncate max-w-md">{config.targetUrl}</p>
                    </div>
                  </div>

                  <div className="grid grid-cols-1 md:grid-cols-2 gap-8 pt-4">
                    <div className="p-6 bg-gray-50 rounded-3xl space-y-3 relative group">
                      <div className="flex justify-between items-center">
                        <label className="text-[10px] font-black text-gray-400 uppercase tracking-widest flex items-center gap-1.5">
                          <Shield className="w-3 h-3 text-blue-500" /> Webhook Secret (HMAC)
                        </label>
                        <button 
                          onClick={() => copyToClipboard(config.secret, `sec-${config.id}`)}
                          className="p-1.5 hover:bg-gray-200 rounded-lg text-gray-400 transition-colors"
                        >
                          {copiedId === `sec-${config.id}` ? <Check className="w-4 h-4 text-emerald-500" /> : <Copy className="w-4 h-4" />}
                        </button>
                      </div>
                      <p className="font-mono text-sm text-gray-900 font-bold overflow-hidden select-all pr-8">
                        {config.secret.substring(0, 16)}••••••••••••••••
                      </p>
                      <button 
                        onClick={() => handleRotateSecret(config.id)}
                        className="absolute right-4 bottom-4 p-2 bg-white text-gray-400 hover:text-blue-600 rounded-xl shadow-sm border border-gray-100 transition-all opacity-0 group-hover:opacity-100"
                        title="Rotate Secret"
                      >
                        <RefreshCw className="w-4 h-4" />
                      </button>
                    </div>

                    <div className="p-6 bg-gray-50 rounded-3xl space-y-3">
                      <label className="text-[10px] font-black text-gray-400 uppercase tracking-widest flex items-center gap-1.5">
                        <Webhook className="w-3 h-3 text-emerald-500" /> Data Subscriptions
                      </label>
                      <div className="flex flex-wrap gap-2">
                        {config.subscribedEvents.split(',').map(ev => (
                          <span key={ev} className="px-3 py-1 bg-white border border-gray-200 text-gray-600 rounded-lg text-[10px] font-black">
                            {ev.trim()}
                          </span>
                        ))}
                      </div>
                    </div>
                  </div>
                </div>

                <div className="lg:w-48 flex lg:flex-col justify-end gap-3">
                  <button 
                    onClick={() => handleUpdate(config.id, { isActive: !config.isActive })}
                    className={`flex-1 lg:flex-none flex items-center justify-center gap-2 px-4 py-3 rounded-2xl font-bold text-sm transition-all ${config.isActive ? 'bg-amber-50 text-amber-600 hover:bg-amber-100' : 'bg-emerald-50 text-emerald-600 hover:bg-emerald-100'}`}
                  >
                    {config.isActive ? <Settings className="w-4 h-4" /> : <Check className="w-4 h-4" />}
                    {config.isActive ? 'Disable' : 'Enable'}
                  </button>
                  <button 
                    onClick={() => handleDelete(config.id)}
                    className="flex-1 lg:flex-none flex items-center justify-center gap-2 px-4 py-3 bg-red-50 text-red-600 rounded-2xl font-bold text-sm hover:bg-red-100 transition-all"
                  >
                    <Trash2 className="w-4 h-4" /> Delete
                  </button>
                </div>
              </div>

              <div className="mt-8 pt-6 border-t border-gray-50 flex items-center justify-between text-[10px] font-bold text-gray-400 uppercase tracking-widest">
                <span>Last Synchronised: Never</span>
                <span>Config Updated: {new Date(config.updatedAt).toLocaleDateString()}</span>
              </div>
            </div>
          </div>
        ))}
      </div>

      {/* Security Banner */}
      <div className="bg-gray-900 rounded-[2.5rem] p-10 text-white flex flex-col md:flex-row items-center gap-8 shadow-2xl shadow-gray-200">
        <div className="w-20 h-20 bg-blue-600 rounded-3xl flex items-center justify-center shadow-2xl shadow-blue-500/20 shrink-0">
          <Shield className="w-10 h-10" />
        </div>
        <div className="space-y-4">
          <h3 className="text-2xl font-black tracking-tight">Enterprise Webhook Security</h3>
          <p className="text-gray-400 font-medium leading-relaxed">
            All outbound events are signed using <span className="text-white font-bold underline decoration-blue-500 underline-offset-4">HMAC-SHA256</span>. Partners must verify the <code className="text-xs bg-gray-800 px-2 py-1 rounded">X-InsureX-Signature</code> header using their shared secret and check the timestamp to prevent replay attacks.
          </p>
          <div className="flex gap-4 pt-2">
            <button className="flex items-center gap-2 text-xs font-black text-blue-400 hover:text-blue-300 transition-colors uppercase tracking-widest">
              <Info className="w-4 h-4" /> View Documentation
            </button>
            <div className="w-px h-4 bg-gray-800"></div>
            <button className="flex items-center gap-2 text-xs font-black text-emerald-400 hover:text-emerald-300 transition-colors uppercase tracking-widest">
              <Check className="w-4 h-4" /> Developer Guide
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default IntegrationsPage;
