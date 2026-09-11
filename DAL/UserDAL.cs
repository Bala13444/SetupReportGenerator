using Microsoft.Data.SqlClient;
using System;

namespace SetupReportGenerator.DAL
{
    public class UserDAL
    {
        DbHelper db = new DbHelper();

        public bool RegisterUser(string username, string password)
        {
            SqlConnection con = db.GetConnection();

            con.Open();

            string query = "INSERT INTO Users(UserName,Password) VALUES(@UserName,@Password)";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@Password", password);

            int rows = cmd.ExecuteNonQuery();

            con.Close();

            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool LoginUser(string username, string password)
        {
            SqlConnection con = db.GetConnection();

            con.Open();

            string query = "SELECT COUNT(*) FROM Users WHERE UserName=@UserName AND Password=@Password";

            SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@UserName", username);
            cmd.Parameters.AddWithValue("@Password", password);

            int count = Convert.ToInt32(cmd.ExecuteScalar());

            con.Close();

            if (count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}