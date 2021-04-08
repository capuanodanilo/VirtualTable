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
    public static class Connection
    {
        public static SqlConnection GetConnection()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = "yoururl.database.windows.net";
                builder.UserID = "yourusername";
                builder.Password = "yourpassword";
                builder.InitialCatalog = "yourdb";
                SqlConnection connection = new SqlConnection(builder.ConnectionString);
                return connection;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
                throw;
            }
        }
    }
}
