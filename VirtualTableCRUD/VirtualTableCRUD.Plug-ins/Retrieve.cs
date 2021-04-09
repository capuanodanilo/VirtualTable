using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Extensions;
using Microsoft.Xrm.Sdk.Data.Exceptions;
using Newtonsoft.Json;

namespace VirtualTableCRUD.Plug_ins
{
    public class Retrieve : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            Guid id = Guid.Empty;
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference)
            {
                EntityReference entityRef = (EntityReference)context.InputParameters["Target"];
                Entity e = new Entity("dc_person");
                string cmdString = "SELECT PersonId, FirstName, LastName, Email FROM person WHERE PersonId=@PersonId";
                SqlConnection connection = Connection.GetConnection();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = cmdString;
                    command.Parameters.AddWithValue("@PersonId", entityRef.Id);
                    connection.Open();
                    try
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                e.Attributes.Add("dc_personid", reader.GetGuid(0));
                                e.Attributes.Add("dc_firstname", reader.GetString(1));
                                e.Attributes.Add("dc_lastname", reader.GetString(2));
                                e.Attributes.Add("dc_email", reader.GetString(3));
                            }
                        }
                    }
                    finally
                    {
                        connection.Close();
                    } 
                }
                context.OutputParameters["BusinessEntity"] = e;
            }
        }
    }
}
