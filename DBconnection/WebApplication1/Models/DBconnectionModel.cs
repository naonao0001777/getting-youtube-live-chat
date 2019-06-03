using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;

namespace WebApplication1.Models
{
    public class DBconnectionModel
    {
        public void connection()
        {
            SqlConnection connection = null;
            SqlCommand command = null;

            try
            {
                //接続文字列の取得
                var connectionString = ConfigurationManager.ConnectionStrings["sqlsvr"].ConnectionString;

                //var connection = new SqlConnection(connectionString);

                connection.Open();

                //var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"select * from  ";

                command.ExecuteNonQuery();
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
                throw;
            }
            finally
            {
                connection.Close();
            }

        }
    }
}