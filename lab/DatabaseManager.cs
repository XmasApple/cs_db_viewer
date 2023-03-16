using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace lab;

public class DatabaseManager
{
    public SqlConnection? Connection
    {
        get => _connection;
    }

    private SqlConnection? _connection;

    private static DatabaseManager? _instance;

    public static DatabaseManager Instance
    {
        get { return _instance ??= new DatabaseManager(); }
    }

    public bool CreateConnection(string connectionString)
    {
        try
        {
            _connection = new SqlConnection(connectionString);
            _connection.Open();
            _connection.Close();
            return true;
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Big Problem: {exception}");
            return false;
        }
    }

    public void DeleteConnection()
    {
        _connection = null;
    }

    public bool OpenConnection()
    {
        try
        {
            if (_connection is { State: ConnectionState.Closed }) _connection.Open();

            return true;
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Big Problem: {exception}");
            return false;
        }
    }

    public bool CloseConnection()
    {
        try
        {
            if (_connection is { State: ConnectionState.Open }) _connection.Close();

            return true;
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Big Problem: {exception}");
            return false;
        }
    }

    public DataTable? GetTable(string tableName)
    {
        try
        {
            var commandText = $"SELECT * FROM {tableName}";

            var command = new SqlCommand(commandText, Connection);

            var dateTable = new DataTable();
            OpenConnection();
            dateTable.Load(command.ExecuteReader());
            CloseConnection();
            return dateTable;
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Big Problem: {exception}");
            return null;
        }
    }

    public bool InsertValue(string tableName, List<string> cols, List<SqlDbType> types, List<string> values)
    {
        try
        {
            var valuesParamStrings = string.Join(
                ", ",
                cols.Select(col => $"'@{col}'"));
            values = values.Select(val => $"'{val}'").ToList();
            var commandText =
                $"INSERT INTO {tableName} " +
                $"({string.Join(", ", cols)}) " +
                $"VALUES ({string.Join(", ", values)});";
                // $"VALUES ({valuesParamStrings});";
            

            using var insertCommand = Connection?.CreateCommand();
            if (insertCommand == null) throw new NullReferenceException();
            insertCommand.CommandText = commandText;
            // foreach (var (col, type, value) in cols.Zip(types, values))
            // {
            //     var parametr = new SqlParameter($"@{col}", value);
            //     insertCommand.Parameters.Add(parametr);
            //     // insertCommand.Parameters.Add($"@{col}", type).Value = value;
            // }
            MessageBox.Show(commandText);
            
            insertCommand.Connection.Open();
            insertCommand.ExecuteNonQuery();
            insertCommand.Connection.Close();
            return true;
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Big Problem: {exception}");
            return false;
        }
    }
}