using System;
using System.Data;
using System.Data.SqlClient;

namespace Infrastructure.Data;

using System.Collections;
using System.Collections.Generic;
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


    public static int ExecuteNonQueryUnsafe(string sql, IEnumerable<SqlParameter>? parameters = null)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        if (parameters is not null) 
        {
            cmd.Parameters.AddRange(new List<SqlParameter>(parameters).ToArray());
        }
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static IDataReader ExecuteReaderUnsafe(string sql, IEnumerable<SqlParameter>? parameters = null)
    {
        var conn = new SqlConnection(ConnectionString);
        var cmd = new SqlCommand(sql, conn);
        if (parameters is not null)
        {
            cmd.Parameters.AddRange(new List<SqlParameter>(parameters).ToArray());
        }
        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }
}
