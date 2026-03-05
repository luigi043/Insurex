import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { assetClient, policyClient } from '../../api/clients';
import { Save, X, Car, AlertCircle, Loader2 } from 'lucide-react';

const AddAssetPage: React.FC = () => {
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [formFields, setFormFields] = useState<any>(null);

  const [formData, setFormData] = useState({
    iAsset_Type_Id: '',
    iFinancer_Id: '',
    vcFinance_Agrreement_Number: '',
    mAsset_Finance_Value: '',
    mAsset_Insurance_Value: '',
    vcVin_Number: '',
    vcRegistration_Number: '',
    iModel_Year: new Date().getFullYear().toString(),
    iVehicle_Make_Id: '',
    iVehicle_Model_Id: '',
    dtFinance_Start_Date: new Date().toISOString().split('T')[0],
    dtFinance_End_Date: new Date(new Date().setFullYear(new Date().getFullYear() + 5)).toISOString().split('T')[0],
  });

  useEffect(() => {
    const loadFields = async () => {
      try {
        const fields = await policyClient.getFormFields();
        setFormFields(fields);
        if (fields.assetTypes?.length > 0) {
          setFormData(prev => ({ ...prev, iAsset_Type_Id: fields.assetTypes[0].value }));
        }
      } catch (err) {
        console.error('Failed to load form fields:', err);
      }
    };
    loadFields();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    setFormData(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      // Map frontend string values to backend expected types
      const payload = {
        ...formData,
        iAsset_Type_Id: parseInt(formData.iAsset_Type_Id),
        mAsset_Finance_Value: parseFloat(formData.mAsset_Finance_Value),
        mAsset_Insurance_Value: parseFloat(formData.mAsset_Insurance_Value || '0'),
        iModel_Year: parseInt(formData.iModel_Year),
        iFinancer_Id: parseInt(formData.iFinancer_Id || '1'), // Default to 1 for now
        iVehicle_Make_Id: parseInt(formData.iVehicle_Make_Id || '1'),
        iVehicle_Model_Id: parseInt(formData.iVehicle_Model_Id || '1'),
        iAsset_Cover_Type_Id: 1, // Defaulting to Comprehensive
      };

      await assetClient.addAsset(payload);
      navigate('/assets');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to save asset. Please check the form and try again.');
    } finally {
      setLoading(false);
    }
  };

  if (!formFields) {
    return (
      <div className="flex items-center justify-center h-64">
        <Loader2 className="w-8 h-8 text-blue-500 animate-spin" />
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto space-y-8 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Register New Asset</h1>
          <p className="text-gray-500 mt-1">Add a new financial asset to the insurance ledger.</p>
        </div>
        <button 
          onClick={() => navigate('/assets')}
          className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-all"
        >
          <X className="w-6 h-6" />
        </button>
      </header>

      {error && (
        <div className="bg-red-50 border border-red-100 p-4 rounded-2xl flex items-start gap-3 animate-shake">
          <AlertCircle className="w-5 h-5 text-red-500 mt-0.5" />
          <p className="text-red-700 text-sm font-medium">{error}</p>
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-8">
        {/* Core Details */}
        <section className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 space-y-6">
          <div className="flex items-center gap-3 mb-2">
            <div className="w-10 h-10 rounded-xl bg-blue-50 flex items-center justify-center text-blue-600">
              <Car className="w-5 h-5" />
            </div>
            <h2 className="text-xl font-bold text-gray-900">Asset & Finance Details</h2>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">Asset Type</label>
              <select
                name="iAsset_Type_Id"
                value={formData.iAsset_Type_Id}
                onChange={handleChange}
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                required
              >
                {formFields.assetTypes.map((t: any) => (
                  <option key={t.value} value={t.value}>{t.label}</option>
                ))}
              </select>
            </div>

            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">Finance Agreement #</label>
              <input
                type="text"
                name="vcFinance_Agrreement_Number"
                value={formData.vcFinance_Agrreement_Number}
                onChange={handleChange}
                placeholder="AGR-999-000"
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">Finance Value (USD)</label>
              <input
                type="number"
                name="mAsset_Finance_Value"
                value={formData.mAsset_Finance_Value}
                onChange={handleChange}
                placeholder="0.00"
                step="0.01"
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                required
              />
            </div>

            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">Insurance Value (USD)</label>
              <input
                type="number"
                name="mAsset_Insurance_Value"
                value={formData.mAsset_Insurance_Value}
                onChange={handleChange}
                placeholder="0.00"
                step="0.01"
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
              />
            </div>
          </div>
        </section>

        {/* Vehicle Specifics */}
        <section className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 space-y-6">
          <h2 className="text-xl font-bold text-gray-900 border-l-4 border-blue-500 pl-4">Vehicle Information</h2>
          <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">VIN Number</label>
              <input
                type="text"
                name="vcVin_Number"
                value={formData.vcVin_Number}
                onChange={handleChange}
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all uppercase"
                required
              />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">Registration #</label>
              <input
                type="text"
                name="vcRegistration_Number"
                value={formData.vcRegistration_Number}
                onChange={handleChange}
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all uppercase"
                required
              />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">Model Year</label>
              <input
                type="number"
                name="iModel_Year"
                value={formData.iModel_Year}
                onChange={handleChange}
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
                required
              />
            </div>
          </div>

          <div className="grid grid-cols-1 md:grid-cols-2 gap-6 pt-4 border-t border-gray-50">
            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">Finance Start Date</label>
              <input
                type="date"
                name="dtFinance_Start_Date"
                value={formData.dtFinance_Start_Date}
                onChange={handleChange}
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
              />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-bold text-gray-700 ml-1">Finance End Date</label>
              <input
                type="date"
                name="dtFinance_End_Date"
                value={formData.dtFinance_End_Date}
                onChange={handleChange}
                className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all"
              />
            </div>
          </div>
        </section>

        <div className="flex justify-end gap-4 pt-4">
          <button
            type="button"
            onClick={() => navigate('/assets')}
            className="px-8 py-3 rounded-2xl text-sm font-bold text-gray-600 bg-white border border-gray-200 hover:bg-gray-50 transition-all"
          >
            Cancel
          </button>
          <button
            type="submit"
            disabled={loading}
            className="flex items-center gap-2 px-10 py-3 bg-blue-600 rounded-2xl text-sm font-bold text-white shadow-xl shadow-blue-200 hover:bg-blue-700 transition-all disabled:opacity-50 disabled:cursor-not-allowed active:scale-95"
          >
            {loading ? (
              <Loader2 className="w-4 h-4 animate-spin" />
            ) : (
              <Save className="w-4 h-4" />
            )}
            Save Asset
          </button>
        </div>
      </form>
    </div>
  );
};

export default AddAssetPage;
