using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace IAPR_Data.Utils;

/// <summary>
/// Bridge for legacy SqlHelper usage.
/// Modernized to use IConfiguration for connection strings.
/// </summary>
public static class SqlHelper
{
    private static string? _connectionString;

    public static void Initialize(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    private static string GetConn() => _connectionString ?? throw new InvalidOperationException("SqlHelper not initialized with connection string.");

    public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] parameters)
    {
        using var cmd = new SqlCommand(commandText, connection);
        cmd.CommandType = commandType;
        if (parameters != null) cmd.Parameters.AddRange(parameters);

        var ds = new DataSet();
        using var da = new SqlDataAdapter(cmd);
        da.Fill(ds);
        return ds;
    }

    public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
    {
        using var conn = new SqlConnection(GetConn());
        using var cmd = new SqlCommand(commandText, conn);
        cmd.CommandType = commandType;
        if (parameters != null) cmd.Parameters.AddRange(parameters);

        conn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] parameters)
    {
        var conn = new SqlConnection(GetConn());
        using var cmd = new SqlCommand(commandText, conn);
        cmd.CommandType = commandType;
        if (parameters != null) cmd.Parameters.AddRange(parameters);

        conn.Open();
        return cmd.ExecuteReader(CommandBehavior.CloseConnection);
    }
}







