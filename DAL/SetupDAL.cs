using Microsoft.Data.SqlClient;
using SetupReportGenerator.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetupReportGenerator.DAL
{
    public class SetupDAL
    {
        private readonly DbHelper db = new DbHelper();

        public void InsertSetupDetail(SetupDetail detail)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                string query = @"INSERT INTO SetupDetail
                                (RecipeId,
                                 MachineName,
                                 [Table],
                                 Track,
                                 PartNumber,
                                 ReferenceDesignator,
                                 FeederType)
                                VALUES
                                (@RecipeId,
                                 @MachineName,
                                 @Table,
                                 @Track,
                                 @PartNumber,
                                 @ReferenceDesignator,
                                 @FeederType)";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RecipeId",Convert.ToInt32(detail.RecipeId.ToString()));
                    cmd.Parameters.AddWithValue("@MachineName", detail.MachineName);
                    cmd.Parameters.AddWithValue("@Table", detail.Table);
                    cmd.Parameters.AddWithValue("@Track", detail.Track);
                    cmd.Parameters.AddWithValue("@PartNumber", detail.PartNumber);
                    cmd.Parameters.AddWithValue("@ReferenceDesignator", detail.ReferenceDesignator);
                    cmd.Parameters.AddWithValue("@FeederType", detail.FeederType);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetReport(DateTime fromDate, DateTime toDate, int recipeId)
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                SqlCommand cmd = new SqlCommand("usp_GetSetupReport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate", toDate.Date);
                cmd.Parameters.AddWithValue("@RecipeId", recipeId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            return dt;
        }

    }
}
