import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { policyClient } from '../../api/clients';
import { Save, X, Shield, User, Building, MapPin, Loader2, AlertCircle, ChevronRight, ChevronLeft } from 'lucide-react';

const AddPolicyPage: React.FC = () => {
  const navigate = useNavigate();
  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [formFields, setFormFields] = useState<any>(null);

  const [formData, setFormData] = useState<any>({
    iInsurance_Company_Id: '',
    vcPolicy_Number: '',
    iPolicy_Type_Id: '1', // 1 = Personal, 2 = Business
    iPolicy_Payment_Frequency_Type_Id: '',
    // Personal Holder
    personal: {
      iIdentification_Type_Id: '1',
      iPerson_Title_Id: '1',
      vcFirst_Names: '',
      vcSurname: '',
      vcIdentification_Number: '',
      vcContact_Number: '',
      vcAlternative_Contact_Number: '',
      vcEmail_Address: '',
      bPostalAddresSameAsPhysical: true,
      physical_Address: {
        vcBuilding_Unit: '',
        vcAddress_Line_1: '',
        vcAddress_Line_2: '',
        vcSuburb: '',
        vcCity: '',
        iProvince_Id: '1',
        vcPostal_Code: '',
      },
      postal_Address: {
        vcPOBox_Bag: '',
        vcPost_Office_Name: '',
        vcPost_Postal_Code: '',
      }
    },
    // Business Holder
    business: {
      vcBusiness_Name: '',
      vcBusiness_Registration_Number: '',
      vcBusiness_Contact_Fullname: '',
      vcBusiness_Contact_Number: '',
      vcBusiness_Contact_Alternative_Number: '',
      vcBusiness_Email_Address: '',
      bPostalAddresSameAsPhysical: true,
      physical_Address: {
        vcBuilding_Unit: '',
        vcAddress_Line_1: '',
        vcAddress_Line_2: '',
        vcSuburb: '',
        vcCity: '',
        iProvince_Id: '1',
        vcPostal_Code: '',
      },
      postal_Address: {
        vcPOBox_Bag: '',
        vcPost_Office_Name: '',
        vcPost_Postal_Code: '',
      }
    }
  });

  useEffect(() => {
    const loadFields = async () => {
      try {
        const fields = await policyClient.getFormFields();
        setFormFields(fields);
        setFormData((prev: any) => ({
          ...prev,
          iInsurance_Company_Id: fields.insuranceCompanies[0]?.value || '',
          iPolicy_Payment_Frequency_Type_Id: fields.paymentFrequencies[0]?.value || '',
        }));
      } catch (err) {
        console.error('Failed to load fields:', err);
      }
    };
    loadFields();
  }, []);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>) => {
    const { name, value } = e.target;
    if (name.includes('.')) {
      const [section, field] = name.split('.');
      if (field.includes('_Address.')) {
        const [addrType, addrField] = field.split('.');
        setFormData((prev: any) => ({
          ...prev,
          [section]: {
            ...prev[section],
            [addrType]: {
              ...prev[section][addrType],
              [addrField]: value
            }
          }
        }));
      } else {
        setFormData((prev: any) => ({
          ...prev,
          [section]: { ...prev[section], [field]: value }
        }));
      }
    } else {
      setFormData((prev: any) => ({ ...prev, [name]: value }));
    }
  };

  const handleCheckbox = (section: 'personal' | 'business') => {
    setFormData((prev: any) => ({
      ...prev,
      [section]: {
        ...prev[section],
        bPostalAddresSameAsPhysical: !prev[section].bPostalAddresSameAsPhysical
      }
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    setError(null);

    try {
      const isPersonal = formData.iPolicy_Type_Id === '1';
      const payload: any = {
        iInsurance_Company_Id: parseInt(formData.iInsurance_Company_Id),
        vcPolicy_Number: formData.vcPolicy_Number,
        iPolicy_Type_Id: parseInt(formData.iPolicy_Type_Id),
        iPolicy_Payment_Frequency_Type_Id: parseInt(formData.iPolicy_Payment_Frequency_Type_Id),
      };

      if (isPersonal) {
        payload.policy_Holder_Individual = {
          ...formData.personal,
          iIdentification_Type_Id: parseInt(formData.personal.iIdentification_Type_Id),
          iPerson_Title_Id: parseInt(formData.personal.iPerson_Title_Id),
          physical_Address: {
            ...formData.personal.physical_Address,
            iProvince_Id: parseInt(formData.personal.physical_Address.iProvince_Id)
          }
        };
        await policyClient.addPersonalPolicy(payload);
      } else {
        payload.policy_Holder_Business = {
          ...formData.business,
          physical_Address: {
            ...formData.business.physical_Address,
            iProvince_Id: parseInt(formData.business.physical_Address.iProvince_Id)
          }
        };
        await policyClient.addBusinessPolicy(payload);
      }

      navigate('/policies');
    } catch (err: any) {
      setError(err.response?.data?.message || 'Verification failed. Please check the required fields.');
    } finally {
      setLoading(false);
    }
  };

  if (!formFields) return <div className="flex items-center justify-center h-64"><Loader2 className="animate-spin" /></div>;

  const currentHolder = formData.iPolicy_Type_Id === '1' ? 'personal' : 'business';

  return (
    <div className="max-w-5xl mx-auto space-y-8 pb-12 animate-in fade-in slide-in-from-bottom-4 duration-700">
      <header className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-extrabold text-gray-900 tracking-tight">Create New Policy</h1>
          <div className="flex items-center gap-2 mt-2">
            {[1, 2, 3].map(s => (
              <div key={s} className={`h-1.5 w-12 rounded-full transition-all ${step >= s ? 'bg-blue-600' : 'bg-gray-200'}`} />
            ))}
            <span className="text-xs font-bold text-gray-400 ml-2 uppercase tracking-widest">Step {step} of 3</span>
          </div>
        </div>
        <button onClick={() => navigate('/policies')} className="p-2 text-gray-400 hover:text-gray-600 hover:bg-gray-100 rounded-full transition-all"><X /></button>
      </header>

      {error && (
        <div className="bg-red-50 border border-red-100 p-4 rounded-2xl flex items-start gap-3 animate-shake">
          <AlertCircle className="w-5 h-5 text-red-500 mt-0.5" />
          <p className="text-red-700 text-sm font-medium">{error}</p>
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-6">
        {/* STEP 1: Policy Info */}
        {step === 1 && (
          <section className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 space-y-6 animate-in slide-in-from-right-4 duration-300">
            <div className="flex items-center gap-3 mb-2">
              <div className="w-10 h-10 rounded-xl bg-blue-50 flex items-center justify-center text-blue-600"><Shield className="w-5 h-5" /></div>
              <h2 className="text-xl font-bold text-gray-900">General Information</h2>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700 ml-1">Policy Number</label>
                <input type="text" name="vcPolicy_Number" value={formData.vcPolicy_Number} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" required />
              </div>
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700 ml-1">Insurer</label>
                <select name="iInsurance_Company_Id" value={formData.iInsurance_Company_Id} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all">
                  {formFields.insuranceCompanies.map((c: any) => (<option key={c.value} value={c.value}>{c.label}</option>))}
                </select>
              </div>
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700 ml-1">Policy Type</label>
                <select name="iPolicy_Type_Id" value={formData.iPolicy_Type_Id} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all">
                  <option value="1">Personal</option>
                  <option value="2">Business</option>
                </select>
              </div>
              <div className="space-y-2">
                <label className="text-sm font-bold text-gray-700 ml-1">Payment Frequency</label>
                <select name="iPolicy_Payment_Frequency_Type_Id" value={formData.iPolicy_Payment_Frequency_Type_Id} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all">
                  {formFields.paymentFrequencies.map((f: any) => (<option key={f.value} value={f.value}>{f.label}</option>))}
                </select>
              </div>
            </div>
          </section>
        )}

        {/* STEP 2: Holder Info */}
        {step === 2 && (
          <section className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 space-y-6 animate-in slide-in-from-right-4 duration-300">
            <div className="flex items-center gap-3 mb-2">
              <div className="w-10 h-10 rounded-xl bg-amber-50 flex items-center justify-center text-amber-600">
                {formData.iPolicy_Type_Id === '1' ? <User className="w-5 h-5" /> : <Building className="w-5 h-5" />}
              </div>
              <h2 className="text-xl font-bold text-gray-900">
                {formData.iPolicy_Type_Id === '1' ? 'Personal Details' : 'Business Details'}
              </h2>
            </div>

            {formData.iPolicy_Type_Id === '1' ? (
              <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">Title</label>
                  <select name="personal.iPerson_Title_Id" value={formData.personal.iPerson_Title_Id} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all">
                    {formFields.personTitles.map((t: any) => (<option key={t.value} value={t.value}>{t.label}</option>))}
                  </select>
                </div>
                <div className="space-y-2 md:col-span-1">
                  <label className="text-sm font-bold text-gray-700 ml-1">First Names</label>
                  <input type="text" name="personal.vcFirst_Names" value={formData.personal.vcFirst_Names} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" required />
                </div>
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">Surname</label>
                  <input type="text" name="personal.vcSurname" value={formData.personal.vcSurname} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" required />
                </div>
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">ID Type</label>
                  <select name="personal.iIdentification_Type_Id" value={formData.personal.iIdentification_Type_Id} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all">
                    {formFields.identificationTypes.map((t: any) => (<option key={t.value} value={t.value}>{t.label}</option>))}
                  </select>
                </div>
                <div className="space-y-2 md:col-span-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">ID Number</label>
                  <input type="text" name="personal.vcIdentification_Number" value={formData.personal.vcIdentification_Number} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all uppercase" required />
                </div>
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">Email</label>
                  <input type="email" name="personal.vcEmail_Address" value={formData.personal.vcEmail_Address} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" />
                </div>
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">Contact Number</label>
                  <input type="text" name="personal.vcContact_Number" value={formData.personal.vcContact_Number} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" />
                </div>
              </div>
            ) : (
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">Company Name</label>
                  <input type="text" name="business.vcBusiness_Name" value={formData.business.vcBusiness_Name} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" required />
                </div>
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">Registration #</label>
                  <input type="text" name="business.vcBusiness_Registration_Number" value={formData.business.vcBusiness_Registration_Number} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all uppercase" />
                </div>
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">Contact Person</label>
                  <input type="text" name="business.vcBusiness_Contact_Fullname" value={formData.business.vcBusiness_Contact_Fullname} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" />
                </div>
                <div className="space-y-2">
                  <label className="text-sm font-bold text-gray-700 ml-1">Email</label>
                  <input type="email" name="business.vcBusiness_Email_Address" value={formData.business.vcBusiness_Email_Address} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" />
                </div>
              </div>
            )}
          </section>
        )}

        {/* STEP 3: Address */}
        {step === 3 && (
          <section className="bg-white p-8 rounded-3xl shadow-sm border border-gray-100 space-y-6 animate-in slide-in-from-right-4 duration-300">
            <div className="flex items-center gap-3 mb-2">
              <div className="w-10 h-10 rounded-xl bg-purple-50 flex items-center justify-center text-purple-600"><MapPin className="w-5 h-5" /></div>
              <h2 className="text-xl font-bold text-gray-900">Physical Address</h2>
            </div>
            <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
              <div className="space-y-2"><label className="text-sm font-bold text-gray-700 ml-1">Unit/Building</label><input type="text" name={`${currentHolder}.physical_Address.vcBuilding_Unit`} value={formData[currentHolder].physical_Address.vcBuilding_Unit} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" /></div>
              <div className="space-y-2 md:col-span-2"><label className="text-sm font-bold text-gray-700 ml-1">Address Line 1</label><input type="text" name={`${currentHolder}.physical_Address.vcAddress_Line_1`} value={formData[currentHolder].physical_Address.vcAddress_Line_1} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" required /></div>
              <div className="space-y-2"><label className="text-sm font-bold text-gray-700 ml-1">Suburb</label><input type="text" name={`${currentHolder}.physical_Address.vcSuburb`} value={formData[currentHolder].physical_Address.vcSuburb} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" /></div>
              <div className="space-y-2"><label className="text-sm font-bold text-gray-700 ml-1">City</label><input type="text" name={`${currentHolder}.physical_Address.vcCity`} value={formData[currentHolder].physical_Address.vcCity} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all" required /></div>
              <div className="space-y-2"><label className="text-sm font-bold text-gray-700 ml-1">Postal Code</label><input type="text" name={`${currentHolder}.physical_Address.vcPostal_Code`} value={formData[currentHolder].physical_Address.vcPostal_Code} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all uppercase" required /></div>
              <div className="space-y-2 md:col-span-1">
                <label className="text-sm font-bold text-gray-700 ml-1">Province</label>
                <select name={`${currentHolder}.physical_Address.iProvince_Id`} value={formData[currentHolder].physical_Address.iProvince_Id} onChange={handleChange} className="w-full px-4 py-3 bg-gray-50 border-none rounded-xl text-sm font-medium outline-none focus:ring-2 focus:ring-blue-100 transition-all">
                  {formFields.provinces.map((p: any) => (<option key={p.value} value={p.value}>{p.label}</option>))}
                </select>
              </div>
            </div>
            <div className="flex items-center gap-3 pt-4">
              <input type="checkbox" id="postalSame" checked={formData[currentHolder].bPostalAddresSameAsPhysical} onChange={() => handleCheckbox(currentHolder as any)} className="w-5 h-5 rounded-lg border-gray-200 text-blue-600 focus:ring-blue-500" />
              <label htmlFor="postalSame" className="text-sm font-bold text-gray-600 cursor-pointer">Postal address is same as physical</label>
            </div>
          </section>
        )}

        <div className="flex justify-between items-center pt-4">
          {step > 1 ? (
            <button type="button" onClick={() => setStep(step - 1)} className="flex items-center gap-2 px-6 py-3 rounded-2xl text-sm font-bold text-gray-600 bg-white border border-gray-200 hover:bg-gray-100 transition-all"><ChevronLeft className="w-4 h-4" /> Back</button>
          ) : <div />}

          <div className="flex gap-4">
            <button type="button" onClick={() => navigate('/policies')} className="px-8 py-3 rounded-2xl text-sm font-bold text-gray-400 hover:text-gray-600 transition-all">Cancel</button>
            {step < 3 ? (
              <button type="button" onClick={() => setStep(step + 1)} className="flex items-center gap-2 px-10 py-3 bg-gray-900 rounded-2xl text-sm font-bold text-white hover:bg-black transition-all active:scale-95">Next <ChevronRight className="w-4 h-4" /></button>
            ) : (
              <button type="submit" disabled={loading} className="flex items-center gap-2 px-10 py-3 bg-blue-600 rounded-2xl text-sm font-bold text-white shadow-xl shadow-blue-200 hover:bg-blue-700 transition-all active:scale-95 disabled:opacity-50">
                {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : <Save className="w-4 h-4" />} Create Policy
              </button>
            )}
          </div>
        </div>
      </form>
    </div>
  );
};

export default AddPolicyPage;
