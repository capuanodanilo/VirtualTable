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
    public class RetrieveMultiple : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            EntityCollection collection = new EntityCollection();
            string cmdString = "SELECT PersonId, FirstName, LastName, Email FROM person";
            SqlConnection connection = Connection.GetConnection();
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = cmdString;
                connection.Open();
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Entity e = new Entity("dc_person");
                            e.Attributes.Add("dc_personid", reader.GetGuid(0));
                            e.Attributes.Add("dc_firstname", reader.GetString(1));
                            e.Attributes.Add("dc_lastname", reader.GetString(2));
                            e.Attributes.Add("dc_email", reader.GetString(3));
                            collection.Entities.Add(e);
                        }
                    }
                }
                finally
                {
                    connection.Close();
                }
                context.OutputParameters["BusinessEntityCollection"] = collection;
            }
        }
    }
}
