using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace Infrastructure.Data;


public static class BadDb
{
    private static string _connectionString = String.Empty;
    public static string ConnectionString
    {
        get => _connectionString;
        set => _connectionString = value ?? throw new ArgumentException(nameof(value));
    }

    public static int ExecuteNonQuery(string sql) => ExecuteNonQuery(sql, Array.Empty<SqlParameter>());

    public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters is not null && parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }
        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static object? ExecuteScalar(string sql) => ExecuteScalar(sql, Array.Empty<SqlParameter>());

    public static object? ExecuteScalar(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters is not null && parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }
        conn.Open();
        return cmd.ExecuteScalar();
    }

    public static DataTable ExecuteQuery(string sql) => ExecuteQuery(sql, Array.Empty<SqlParameter>());

    public static DataTable ExecuteQuery(string sql, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(ConnectionString);
        using var cmd = new SqlCommand(sql, conn);
        if (parameters is not null && parameters.Length > 0)
        {
            cmd.Parameters.AddRange(parameters);
        }
        using var adapter = new SqlDataAdapter(cmd);
        var dt = new DataTable();
        adapter.Fill(dt);
        return dt;
    }
}
