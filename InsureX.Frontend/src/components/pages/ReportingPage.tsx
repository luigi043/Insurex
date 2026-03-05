import React, { useState } from 'react';
import { reportClient } from '../../api/clients';
import { Download, FileText, ClipboardList, CheckCircle, AlertCircle } from 'lucide-react';

type ExportStatus = 'idle' | 'downloading' | 'done' | 'error';

interface ReportCard {
  id: string;
  title: string;
  description: string;
  icon: React.ReactNode;
  color: string;
  iconBg: string;
  action: () => Promise<void>;
}

const ReportingPage: React.FC = () => {
  const [statuses, setStatuses] = useState<Record<string, ExportStatus>>({});

  const setStatus = (id: string, status: ExportStatus) =>
    setStatuses((prev) => ({ ...prev, [id]: status }));

  const handleDownload = async (id: string, action: () => Promise<void>) => {
    setStatus(id, 'downloading');
    try {
      await action();
      setStatus(id, 'done');
      setTimeout(() => setStatus(id, 'idle'), 3000);
    } catch {
      setStatus(id, 'error');
      setTimeout(() => setStatus(id, 'idle'), 3000);
    }
  };

  const reports: ReportCard[] = [
    {
      id: 'audit',
      title: 'Audit Log Export',
      description: 'Download a full CSV export of the last 1 000 audit log entries, including timestamps, actors, entities, and notes.',
      icon: <ClipboardList className="w-6 h-6" />,
      color: 'text-blue-600',
      iconBg: 'bg-blue-500',
      action: reportClient.exportAuditLog,
    },
    {
      id: 'assets',
      title: 'Assets Portfolio Export',
      description: 'Download a CSV snapshot of your entire asset portfolio, including identifiers, types, compliance status, and borrower references.',
      icon: <FileText className="w-6 h-6" />,
      color: 'text-emerald-600',
      iconBg: 'bg-emerald-500',
      action: reportClient.exportAssets,
    },
  ];

  return (
    <div className="space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header>
        <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Reports & Exports</h1>
        <p className="text-sm text-gray-500 mt-1">Download CSV data exports for auditing and analysis</p>
      </header>

      {/* Report Cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        {reports.map((report) => {
          const status = statuses[report.id] || 'idle';
          return (
            <div
              key={report.id}
              className="bg-white rounded-3xl shadow-sm border border-gray-100 p-8 flex flex-col gap-6 hover:shadow-md transition-shadow duration-300"
            >
              <div className="flex items-start gap-4">
                <div className={`p-3 rounded-2xl ${report.iconBg} text-white shadow-lg flex-shrink-0`}>
                  {report.icon}
                </div>
                <div>
                  <h2 className={`text-xl font-extrabold ${report.color}`}>{report.title}</h2>
                  <p className="text-sm text-gray-500 mt-1 leading-relaxed">{report.description}</p>
                </div>
              </div>

              <button
                onClick={() => handleDownload(report.id, report.action)}
                disabled={status === 'downloading'}
                className={`w-full flex items-center justify-center gap-2 px-5 py-3 rounded-2xl font-bold text-sm transition-all duration-200 active:scale-95 ${
                  status === 'done'
                    ? 'bg-emerald-500 text-white'
                    : status === 'error'
                    ? 'bg-red-500 text-white'
                    : 'bg-gray-900 text-white hover:bg-gray-700'
                } disabled:opacity-60 disabled:cursor-not-allowed`}
              >
                {status === 'downloading' && (
                  <Download className="w-4 h-4 animate-bounce" />
                )}
                {status === 'done' && <CheckCircle className="w-4 h-4" />}
                {status === 'error' && <AlertCircle className="w-4 h-4" />}
                {status === 'idle' && <Download className="w-4 h-4" />}

                {status === 'downloading' ? 'Preparing download...' : status === 'done' ? 'Downloaded!' : status === 'error' ? 'Error — try again' : 'Download CSV'}
              </button>
            </div>
          );
        })}
      </div>

      {/* Info Banner */}
      <div className="bg-blue-50 border border-blue-100 rounded-3xl p-6 flex items-start gap-4">
        <div className="p-2.5 bg-blue-100 rounded-xl flex-shrink-0">
          <FileText className="w-5 h-5 text-blue-600" />
        </div>
        <div>
          <p className="text-sm font-bold text-blue-900">Exports are tenant-scoped</p>
          <p className="text-xs text-blue-600 mt-0.5">
            All exports only include data belonging to your organisation. Data is generated in real-time — no caching.
          </p>
        </div>
      </div>
    </div>
  );
};

export default ReportingPage;
