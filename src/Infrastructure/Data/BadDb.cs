using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data;

using System.Data;
using System.Data.SqlClient;

public static class BadDb
{
    private static string _connectionString = String.Empty;
    public static string ConnectionString
    {
        get => _connectionString;
        set => _connectionString = value ?? throw new ArgumentException(nameof(value));
    }


    public static int ExecuteNonQueryUnsafe(string sql)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static IDataReader ExecuteReaderUnsafe(string sql)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        conn.Open();
        return cmd.ExecuteReader();
    }
}
