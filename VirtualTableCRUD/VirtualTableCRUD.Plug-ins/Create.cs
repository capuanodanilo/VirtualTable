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
    public class Create : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                Guid id = Guid.NewGuid();
                string cmdString = "INSERT INTO person (PersonId,FirstName,LastName,Email) VALUES (@PersonId,@FirstName,@LastName,@Email)";
                SqlConnection connection = Connection.GetConnection();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = cmdString;
                    command.Parameters.AddWithValue("@PersonId", id);
                    command.Parameters.AddWithValue("@FirstName", entity["dc_firstname"]);
                    command.Parameters.AddWithValue("@LastName", entity["dc_lastname"]);
                    command.Parameters.AddWithValue("@Email", entity["dc_email"]);
                    connection.Open();
                    try
                    {
                        var numRecords = command.ExecuteNonQuery();
                        Console.WriteLine("inserted {0} records", numRecords);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                context.OutputParameters["id"] = id;
            }
        }
    }
}
