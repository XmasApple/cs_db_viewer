using System;
using System.Data;
using System.Data.SqlClient;
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
            MessageBox.Show("Connection successful!");
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
            var commandText = $"select * from {tableName}";
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
}