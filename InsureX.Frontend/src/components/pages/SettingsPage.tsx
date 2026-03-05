import React, { useState } from 'react';
import { authClient } from '../../api/clients';
import { Shield, Key, AlertCircle, CheckCircle2, Loader2, Save } from 'lucide-react';

const SettingsPage: React.FC = () => {
  const [currentPassword, setCurrentPassword] = useState('');
  const [newPassword, setNewPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [loading, setLoading] = useState(false);
  const [success, setSuccess] = useState('');
  const [error, setError] = useState('');

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (newPassword !== confirmPassword) {
      setError('New passwords do not match.');
      return;
    }

    if (newPassword.length < 8) {
      setError('Password must be at least 8 characters long.');
      return;
    }

    setLoading(true);
    try {
      await authClient.changePassword(currentPassword, newPassword);
      setSuccess('Your password has been updated successfully.');
      setCurrentPassword('');
      setNewPassword('');
      setConfirmPassword('');
    } catch (err: any) {
      if (err.response?.data?.errors) {
        setError(err.response.data.errors.join(', '));
      } else {
        setError(err.response?.data?.message || 'Failed to update password. Please check your current password and try again.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="max-w-3xl mx-auto space-y-8 animate-in fade-in duration-500">
      <header>
        <h1 className="text-4xl font-black text-gray-900 tracking-tight">Account Settings</h1>
        <p className="text-gray-500 mt-2 font-medium">Manage your personal profile and security preferences.</p>
      </header>

      <div className="bg-white border border-gray-100 rounded-[2.5rem] shadow-sm overflow-hidden">
        <div className="p-8 lg:p-10 border-b border-gray-50">
          <div className="flex items-center gap-4 mb-8">
            <div className="w-12 h-12 bg-blue-50 text-blue-600 rounded-2xl flex items-center justify-center">
              <Shield className="w-6 h-6" />
            </div>
            <div>
              <h2 className="text-2xl font-black text-gray-900">Security</h2>
              <p className="text-gray-400 font-medium">Update your password securely.</p>
            </div>
          </div>

          <form onSubmit={handleSubmit} className="space-y-6 max-w-xl">
            {error && (
              <div className="p-4 bg-red-50 text-red-700 rounded-2xl flex items-center gap-3 text-sm font-bold animate-in slide-in-from-top-2">
                <AlertCircle className="w-5 h-5 flex-shrink-0" />
                <p>{error}</p>
              </div>
            )}
            
            {success && (
              <div className="p-4 bg-emerald-50 text-emerald-700 rounded-2xl flex items-center gap-3 text-sm font-bold animate-in slide-in-from-top-2">
                <CheckCircle2 className="w-5 h-5 flex-shrink-0" />
                <p>{success}</p>
              </div>
            )}

            <div className="space-y-2 relative">
              <label className="text-xs font-black text-gray-400 uppercase tracking-widest pl-1">Current Password</label>
              <div className="relative">
                <Key className="w-5 h-5 absolute left-4 top-1/2 -translate-y-1/2 text-gray-300" />
                <input
                  type="password"
                  required
                  value={currentPassword}
                  onChange={(e) => setCurrentPassword(e.target.value)}
                  className="w-full pl-12 pr-4 py-4 bg-gray-50 border-none rounded-2xl shadow-inner focus:ring-2 focus:ring-blue-400 focus:bg-white transition-all font-medium text-gray-900"
                  placeholder="Enter current password"
                />
              </div>
            </div>

            <div className="space-y-6 pt-4 border-t border-gray-50 mt-6 block">
              <div className="space-y-2">
                <label className="text-xs font-black text-gray-400 uppercase tracking-widest pl-1">New Password</label>
                <input
                  type="password"
                  required
                  value={newPassword}
                  onChange={(e) => setNewPassword(e.target.value)}
                  className="w-full px-5 py-4 bg-gray-50 border-none rounded-2xl shadow-inner focus:ring-2 focus:ring-blue-400 focus:bg-white transition-all font-medium text-gray-900"
                  placeholder="Minimum 8 characters"
                  minLength={8}
                />
              </div>

              <div className="space-y-2">
                <label className="text-xs font-black text-gray-400 uppercase tracking-widest pl-1">Confirm New Password</label>
                <input
                  type="password"
                  required
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  className="w-full px-5 py-4 bg-gray-50 border-none rounded-2xl shadow-inner focus:ring-2 focus:ring-blue-400 focus:bg-white transition-all font-medium text-gray-900"
                  placeholder="Re-enter new password"
                  minLength={8}
                />
              </div>
            </div>

            <div className="pt-6">
              <button
                type="submit"
                disabled={loading || !currentPassword || !newPassword || !confirmPassword}
                className="flex items-center gap-2 px-8 py-4 bg-gray-900 text-white rounded-2xl font-black shadow-xl shadow-gray-200 hover:bg-black transition-all disabled:opacity-50 disabled:cursor-not-allowed w-full sm:w-auto justify-center"
              >
                {loading ? <Loader2 className="w-5 h-5 animate-spin" /> : <Save className="w-5 h-5" />}
                Update Password
              </button>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default SettingsPage;
