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
    public class Update : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.Get<IPluginExecutionContext>();
            Guid id = Guid.Empty;
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity)
            {
                Entity entity = (Entity)context.InputParameters["Target"];
                string cmdString = "UPDATE person SET {0} WHERE PersonId=@PersonId";
                SqlConnection connection = Connection.GetConnection();
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.Parameters.AddWithValue("@PersonId", entity["dc_personid"]);
                    List<string> setList = new List<string>();
                    if (entity.Attributes.Contains("dc_firstname"))
                    {
                        command.Parameters.AddWithValue("@FirstName", entity["dc_firstname"]);
                        setList.Add("FirstName=@FirstName");
                    }
                    if (entity.Attributes.Contains("dc_lastname"))
                    {
                        command.Parameters.AddWithValue("@LastName", entity["dc_lastname"]);
                        setList.Add("LastName=@LastName");
                    }
                    if (entity.Attributes.Contains("dc_email"))
                    {
                        command.Parameters.AddWithValue("@Email", entity["dc_email"]);
                        setList.Add("Email=@Email");
                    }
                    command.CommandText = string.Format(cmdString, string.Join(",", setList)); connection.Open();
                    try
                    {
                        var numRecords = command.ExecuteNonQuery();
                        Console.WriteLine("updated {0} records", numRecords);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
