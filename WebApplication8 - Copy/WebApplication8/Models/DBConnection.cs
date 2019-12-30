using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class DBConnection
{
    static string DatabaseConnectionString = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
    public DBConnection() { }

    public static void SqlExecute(string sql)
    {
        using (SqlConnection conn = new SqlConnection(DatabaseConnectionString))
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Connection.Open();
            cmd.ExecuteNonQuery();
        }
    }

    public static object SqlReturn(string sql)
    {
        using (SqlConnection conn = new SqlConnection(DatabaseConnectionString))
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            object result = (object)cmd.ExecuteScalar();
            return result;
        }
        /*
            int myRequiredScalar = 0;
            object obj = new object();
            obj = SqlComm.SqlReturn("SELECT TOP 1 Col1 FROM Table1");
            if (obj != null) myRequiredScalar = (int)obj;
         */
    }

    public static DataTable SqlDataTable(string sql)
    {
        using (SqlConnection conn = new SqlConnection(DatabaseConnectionString))
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Connection.Open();
            DataTable TempTable = new DataTable();
            TempTable.Load(cmd.ExecuteReader());
            return TempTable;
        }
        /*
            int Col1Value = 0;
            DataTable dt = new DataTable();
            dt = SqlComm.SqlDataTable("SELECT * FROM a WHERE b");
            if (dt.Rows.Count != 0) 
            {
                Col1Value = (int)dt.Rows[2]["Col1"];
            }
            else
            {
                
            }   
         */
    }

    public static object SqlStoredProcedure1Param(string StoredProcedure, string PrmName1, object Param1)
    {
        using (SqlConnection conn = new SqlConnection(DatabaseConnectionString))
        {
            SqlCommand cmd = new SqlCommand(StoredProcedure, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter(PrmName1, Param1.ToString()));
            cmd.Connection.Open();
            object obj = new object();
            obj = cmd.ExecuteScalar();
            return obj;
        }
        //SqlComm.SqlStoredProcedure1Param("TheStoredProcedureName", "TheParameterName", TheParameterValue);
    }
}