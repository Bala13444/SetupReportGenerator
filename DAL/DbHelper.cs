using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace SetupReportGenerator.DAL
{
    public class DbHelper
    {
        private readonly string connectionString =
                                                   @"Server=DESKTOP-NU10J6D;
                                                   Database=SetupReportDB;
                                                   User Id=bala;
                                                   Password=Bala@134444;
                                                   TrustServerCertificate=True;";
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }
    }
}
