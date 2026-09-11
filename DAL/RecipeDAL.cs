using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using SetupReportGenerator.Models;

namespace SetupReportGenerator.DAL
{
    public class RecipeDAL
    {
        private readonly DbHelper db = new DbHelper();

        public int InsertRecipe(RecipeHeader recipe)
        {
            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                string query = @"INSERT INTO RecipeHeader
                                (RecipeName, LineName, Model, BoardSide)
                                VALUES
                                (@RecipeName, @LineName, @Model, @BoardSide);

                                SELECT SCOPE_IDENTITY();";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@RecipeName", recipe.RecipeName);
                    cmd.Parameters.AddWithValue("@LineName", recipe.LineName);
                    cmd.Parameters.AddWithValue("@Model", recipe.Model);
                    cmd.Parameters.AddWithValue("@BoardSide", recipe.BoardSide);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }



        public List<RecipeHeader> GetReceiptNos(DateTime fromDate, DateTime toDate)
        {
            List<RecipeHeader> receipts = new List<RecipeHeader>();

            receipts.Add(new RecipeHeader
            {
                RecipeId = 0,
                RecipeName = "ALL"
            });

            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                string query = @"
            SELECT DISTINCT D.RecipeId
            FROM SetupDetail D
            INNER JOIN RecipeHeader H
                ON D.RecipeId = H.RecipeId
            WHERE CAST(H.ImportedDate AS DATE) BETWEEN @FromDate AND @ToDate
            ORDER BY D.RecipeId";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@FromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@ToDate", toDate.Date);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            receipts.Add(new RecipeHeader
                            {
                                RecipeId = Convert.ToInt32(reader["RecipeId"]),
                                RecipeName = reader["RecipeId"].ToString()
                            });
                        }
                    }
                }
            }

            return receipts;
        }

        public RecipeHeader GetRecipe(int recipeId)
        {
            RecipeHeader recipe = new RecipeHeader();

            using (SqlConnection con = db.GetConnection())
            {
                con.Open();

                string query = @"SELECT *
                         FROM RecipeHeader
                         WHERE RecipeId=@RecipeId";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@RecipeId", recipeId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    recipe.RecipeId = Convert.ToInt32(reader["RecipeId"]);
                    recipe.RecipeName = reader["RecipeName"].ToString();
                    recipe.LineName = reader["LineName"].ToString();
                    recipe.Model = reader["Model"].ToString();
                    recipe.BoardSide = reader["BoardSide"].ToString();
                }

                reader.Close();
            }

            return recipe;
        }

    }
}
