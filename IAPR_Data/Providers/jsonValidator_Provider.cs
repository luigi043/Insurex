using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Linq;
using IAPR_Data.Classes;
using System.Configuration;
using System.Net;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;
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
                {"name", new JsonSchema { Type = JsonSchemaType.String, Required = true,}},

                {"hobbies", new JsonSchema {Type = JsonSchemaType.Array, Required = true,
                    Items = new List<JsonSchema>
                           {new JsonSchema {Type = JsonSchemaType.String, Required=true},

                           } }},

            };
            #region jsonSchema
            // schema.Required. "street_address";

            //            string schemaJson = @"{
            //  '$schema': 'http://json-schema.org/draft-07/schema',
            //  '$id': 'http://example.com/example.json',
            //  'type': 'object',
            //  'title': 'The root schema',
            //  'description': 'The root schema comprises the entire JSON document.',
            //  'default': {},
            //  'examples': [
            //    {
            //      'trasactionId': '999993',
            //      'sourceIdentifier': '1234',
            //      'policies': [
            //        {
            //          'pNumber': 'POLOLM123456',
            //          'AffectedPeriod': 'February',
            //          'AffectedYear': 2020
            //        },
            //        {
            //          'pNumber': 'Py2323211',
            //          'AffectedPeriod': 'March',
            //          'AffectedYear': 2020
            //        }
            //      ]
            //    }
            //  ],
            //  'required': [
            //    'trasactionId',
            //    'sourceIdentifier',
            //    'policies'
            //  ],
            //  'properties': {
            //    'trasactionId': {
            //      '$id': '#/properties/trasactionId',
            //      'type': 'string',
            //      'title': 'The trasactionId schema',
            //      'description': 'An explanation about the purpose of this instance.',
            //      'default': '',
            //      'examples': [
            //        '999993'
            //      ]
            //    },
            //    'sourceIdentifier': {
            //      '$id': '#/properties/sourceIdentifier',
            //      'type': 'string',
            //      'title': 'The sourceIdentifier schema',
            //      'description': 'An explanation about the purpose of this instance.',
            //      'default': '',
            //      'examples': [
            //        '1234'
            //      ]
            //    },
            //    'policies': {
            //      '$id': '#/properties/policies',
            //      'type': 'array',
            //      'title': 'The policies schema',
            //      'description': 'An explanation about the purpose of this instance.',
            //      'default': [],
            //      'examples': [
            //        [
            //          {
            //            'pNumber': 'POLOLM123456',
            //            'AffectedPeriod': 'February',
            //            'AffectedYear': 2020
            //          },
            //          {
            //            'pNumber': 'Py2323211',
            //            'AffectedPeriod': 'March',
            //            'AffectedYear': 2020
            //          }
            //        ]
            //      ],
            //      'additionalItems': true,
            //      'items': {
            //        '$id': '#/properties/policies/items',
            //        'anyOf': [
            //          {
            //            '$id': '#/properties/policies/items/anyOf/0',
            //            'type': 'object',
            //            'title': 'The first anyOf schema',
            //            'description': 'An explanation about the purpose of this instance.',
            //            'default': {},
            //            'examples': [
            //              {
            //                'pNumber': 'POLOLM123456',
            //                'AffectedPeriod': 'February',
            //                'AffectedYear': 2020
            //              }
            //            ],
            //            'required': [
            //              'pNumber',
            //              'AffectedPeriod',
            //              'AffectedYear'
            //            ],
            //            'properties': {
            //              'pNumber': {
            //                '$id': '#/properties/policies/items/anyOf/0/properties/pNumber',
            //                'type': 'string',
            //                'title': 'The pNumber schema',
            //                'description': 'An explanation about the purpose of this instance.',
            //                'default': '',
            //                'examples': [
            //                  'POLOLM123456'
            //                ]
            //              },
            //              'AffectedPeriod': {
            //                '$id': '#/properties/policies/items/anyOf/0/properties/AffectedPeriod',
            //                'type': 'string',
            //                'title': 'The AffectedPeriod schema',
            //                'description': 'An explanation about the purpose of this instance.',
            //                'default': '',
            //                'examples': [
            //                  'February'
            //                ]
            //              },
            //              'AffectedYear': {
            //                '$id': '#/properties/policies/items/anyOf/0/properties/AffectedYear',
            //                'type': 'integer',
            //                'title': 'The AffectedYear schema',
            //                'description': 'An explanation about the purpose of this instance.',
            //                'default': 0,
            //                'examples': [
            //                  2020
            //                ]
            //              }
            //            },
            //            'additionalProperties': true
            //          }
            //        ]
            //      }
            //    }
            //  },
            //  'additionalProperties': true
            //}";

            //JsonSchema schema = JsonSchema.Parse(schemaJson);
            #endregion
            JObject jOBJNonpayment_Request = JObject.Parse(@"{'name': 'James'}");
            IList<string> messages;
            bool valid = jOBJNonpayment_Request.IsValid(schema, out messages);
        }

        public void Validate_Policy_NonPayment_Data(string nonPayment_Request, out C.Response res)
        {
            C.Response lRes = new C.Response();
            lRes.statusCode = 0;
            List<string> msg = new List<string>();
            // JsonSchema schema = JsonSchema.Parse(schemaJson);
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




                // JsonSchema schema = JsonSchema.Parse(schemaJson);
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
                // JsonSchema schema = JsonSchema.Parse(schemaJson);
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

                // JsonSchema schema = JsonSchema.Parse(schemaJson);
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
                // JsonSchema schema = JsonSchema.Parse(schemaJson);
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
            // JsonSchema schema = JsonSchema.Parse(schemaJson);
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
            // JsonSchema schema = JsonSchema.Parse(schemaJson);
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
            string filePath = string.Empty;

            switch (schemaName)
            {
                case "Non_Payment_Schema":
                    filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/JsonSchemas/" + schemaName + ".json"); ;// System.Web.HttpContext.Current.Server.MapPath(@"JsonSchemas\" + schemaName + ".json");
                    break;
                case "UpdateAssetFinanceValue_Schema":
                    filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/JsonSchemas/" + schemaName + ".json"); ;// System.Web.HttpContext.Current.Server.MapPath(@"JsonSchemas\" + schemaName + ".json");
                    break;
                case "UpdateAssetInsuredValue_Schema":
                    filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/JsonSchemas/" + schemaName + ".json"); ;// System.Web.HttpContext.Current.Server.MapPath(@"JsonSchemas\" + schemaName + ".json");
                    break;
                case "UpdateAssetCover_Schema":
                    filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/JsonSchemas/" + schemaName + ".json"); ;// System.Web.HttpContext.Current.Server.MapPath(@"JsonSchemas\" + schemaName + ".json");
                    break;
                case "RemoveAsset_Schema":
                    filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/JsonSchemas/" + schemaName + ".json"); ;// System.Web.HttpContext.Current.Server.MapPath(@"JsonSchemas\" + schemaName + ".json");
                    break;

                case "Policy_Status_Schema":
                    filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/JsonSchemas/" + schemaName + ".json"); ;// System.Web.HttpContext.Current.Server.MapPath(@"JsonSchemas\" + schemaName + ".json");
                    break;
                case "New_Asset_Vehicle_Schema":
                    filePath = System.Web.Hosting.HostingEnvironment.MapPath(@"/JsonSchemas/" + schemaName + ".json"); ;// System.Web.HttpContext.Current.Server.MapPath(@"JsonSchemas\" + schemaName + ".json");
                    break;

            }

            //JsonSchema schema = new JsonSchema();
            string json;
            using (var streamReader = new StreamReader(filePath, Encoding.UTF8))
            {
                json = streamReader.ReadToEnd();
            }
            JSchema schema = JSchema.Parse(json);

            //using (JsonTextReader reader = new JsonTextReader(file))
            //{
            //    schema = JsonSchema.Read(reader);
            //}
            return schema;
        }
    }
}
