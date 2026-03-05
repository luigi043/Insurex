using IAPR_Data.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using IAPR_Data.Classes;
using System.Configuration;
using System.Net;
using System.IO;
using System.Data;
using Microsoft.Data.SqlClient;
using IAPR_Data.Interfaces;
using C = IAPR_Data.Classes;

namespace IAPR_Data.Providers
{
    public class jsonValidator_Provider : ijsonValidator
    {
        public void Validate_2()
        {
            JsonSchema schema = new JsonSchema();
            schema.Type = JsonSchemaType.Object;

            schema.Properties = new Dictionary<string, JsonSchema>
            {
                { "trasactionId", new JsonSchema { Type = JsonSchemaType.String } },
                { "sourceIdentifier", new JsonSchema { Type = JsonSchemaType.String } },
                {
                    "policies", new JsonSchema
                    {
                        Type = JsonSchemaType.Array,
                        Items = new List<JsonSchema> { new JsonSchema { Type = JsonSchemaType.String } }
                    }
                },
            };
        }

        public void Validate_Policy_NonPayment_Data1(string nonPayment_Request)
        {
            JsonSchema schema = new JsonSchema();
            schema.Type = JsonSchemaType.Object;
            schema.Properties = new Dictionary<string, JsonSchema>
            {
                {"name", new JsonSchema { Type = JsonSchemaType.String, Required = true }},
                {"hobbies", new JsonSchema {Type = JsonSchemaType.Array, Required = true,
                    Items = new List<JsonSchema> { new JsonSchema {Type = JsonSchemaType.String, Required=true} } }}
            };

            JObject jOBJNonpayment_Request = JObject.Parse(@"{'name': 'James'}");
            IList<string> messages;
            bool valid = jOBJNonpayment_Request.IsValid(schema, out messages);
        }

        public void Validate_Policy_NonPayment_Data(string nonPayment_Request, out C.Response res)
        {
            C.Response lRes = new C.Response();
            lRes.statusCode = 0;
            List<string> msg = new List<string>();
            try
            {
                JSchema schema = GetJsonSchema("Non_Payment_Schema");
                JObject jOBJ = JObject.Parse(nonPayment_Request);
                IList<string> messages;
                bool valid = jOBJ.IsValid(schema, out messages);
                if (!valid)
                {
                    lRes.statusCode = 201;
                    foreach (string s in messages)
                    {
                        msg.Add(s);
                    }
                    lRes.supportMessages = msg;
                }
            }
            catch (Exception ex)
            {
                lRes.statusCode = 200;
                msg.Add(ex.Message);
                lRes.supportMessages = msg;
            }
            res = lRes;
        }

        public void Validate_Update_Asset_Finance_Value_Data(string updateAssetFinanceValue_Request, out C.Response res)
        {
            C.Response lRes = new C.Response();
            lRes.statusCode = 0;
            List<string> msg = new List<string>();
            try
            {
                JSchema schema = GetJsonSchema("UpdateAssetFinanceValue_Schema");
                JObject jOBJ = JObject.Parse(updateAssetFinanceValue_Request);
                IList<string> messages;
                bool valid = jOBJ.IsValid(schema, out messages);
                if (!valid)
                {
                    lRes.statusCode = 201;
                    foreach (string s in messages)
                    {
                        lRes.supportMessages.Add(s);
                    }
                }
            }
            catch (Exception ex)
            {
                lRes.statusCode = 200;
                msg.Add(ex.Message);
                lRes.supportMessages = msg;
            }
            res = lRes;
        }

        public void Validate_Update_Asset_Insured_Value_Data(string updateAssetInsuredValue_Request, out C.Response res)
        {
            C.Response lRes = new C.Response();
            List<string> msg = new List<string>();
            lRes.statusCode = 0;
            try
            {
                JSchema schema = GetJsonSchema("UpdateAssetInsuredValue_Schema");
                JObject jOBJ = JObject.Parse(updateAssetInsuredValue_Request);
                IList<string> messages;
                bool valid = jOBJ.IsValid(schema, out messages);
                if (!valid)
                {
                    lRes.statusCode = 201;
                    foreach (string s in messages)
                    {
                        msg.Add(s);
                    }
                    lRes.supportMessages = msg;
                }
            }
            catch (Exception ex)
            {
                lRes.statusCode = 200;
                msg.Add(ex.Message);
                lRes.supportMessages = msg;
            }
            res = lRes;
        }

        public void Validate_Update_Asset_Cover_Data(string updateAssetCover_Request, out C.Response res)
        {
            C.Response lRes = new C.Response();
            List<string> msg = new List<string>();
            lRes.statusCode = 0;
            try
            {
                JSchema schema = GetJsonSchema("UpdateAssetCover_Schema");
                JObject jOBJ = JObject.Parse(updateAssetCover_Request);
                IList<string> messages;
                bool valid = jOBJ.IsValid(schema, out messages);
                if (!valid)
                {
                    lRes.statusCode = 201;
                    foreach (string s in messages)
                    {
                        msg.Add(s);
                    }
                    lRes.supportMessages = msg;
                }
            }
            catch (Exception ex)
            {
                lRes.statusCode = 200;
                msg.Add(ex.Message);
                lRes.supportMessages = msg;
            }
            res = lRes;
        }

        public void Validate_Update_Asset_Remove_Data(string removeAsset_Request, out C.Response res)
        {
            C.Response lRes = new C.Response();
            List<string> msg = new List<string>();
            lRes.statusCode = 0;
            try
            {
                JSchema schema = GetJsonSchema("RemoveAsset_Schema");
                JObject jOBJ = JObject.Parse(removeAsset_Request);
                IList<string> messages;
                bool valid = jOBJ.IsValid(schema, out messages);
                if (!valid)
                {
                    lRes.statusCode = 201;
                    foreach (string s in messages)
                    {
                        msg.Add(s);
                    }
                    lRes.supportMessages = msg;
                }
            }
            catch (Exception ex)
            {
                lRes.statusCode = 200;
                msg.Add(ex.Message);
                lRes.supportMessages = msg;
            }
            res = lRes;
        }

        public void Validate_Policy_Status_Data(string policyStatus_Request, out C.Response res)
        {
            C.Response lRes = new C.Response();
            lRes.statusCode = 0;
            List<string> msg = new List<string>();
            try
            {
                JSchema schema = GetJsonSchema("Policy_Status_Schema");
                JObject jOBJ = JObject.Parse(policyStatus_Request);
                IList<string> messages;
                bool valid = jOBJ.IsValid(schema, out messages);
                if (!valid)
                {
                    lRes.statusCode = 201;
                    foreach (string s in messages)
                    {
                        msg.Add(s);
                    }
                    lRes.supportMessages = msg;
                }
            }
            catch (Exception ex)
            {
                lRes.statusCode = 200;
                msg.Add(ex.Message);
                lRes.supportMessages = msg;
            }
            res = lRes;
        }

        #region AssetManagement
        public void Validate_New_Asset_Vehicle(string New_Asset_Vehicle, out C.Response res)
        {
            C.Response lRes = new C.Response();
            lRes.statusCode = 0;
            List<string> msg = new List<string>();
            try
            {
                JSchema schema = GetJsonSchema("New_Asset_Vehicle_Schema");
                JObject jOBJ = JObject.Parse(New_Asset_Vehicle);
                IList<string> messages;
                bool valid = jOBJ.IsValid(schema, out messages);
                if (!valid)
                {
                    lRes.statusCode = 201;
                    lRes.statusMessage = "Error";
                    msg.Add("Schema Validation Failure");
                    foreach (string s in messages)
                    {
                        msg.Add(s);
                    }
                    lRes.supportMessages = msg;
                }
            }
            catch (Exception ex)
            {
                lRes.statusCode = 200;
                msg.Add(ex.Message);
                lRes.supportMessages = msg;
            }
            res = lRes;
        }
        #endregion

        private JSchema GetJsonSchema(string schemaName)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JsonSchemas", schemaName + ".json");
            
            string json;
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                json = streamReader.ReadToEnd();
            }
            JSchema schema = JSchema.Parse(json);
            return schema;
        }
    }
}


