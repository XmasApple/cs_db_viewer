using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;

namespace lab;

public class DatabaseManager
{
    public SqlConnection? Connection { get; private set; }

    private static DatabaseManager? _instance;

    public static DatabaseManager Instance => _instance ??= new DatabaseManager();

    private static T TryExecute<T>(Func<T> method, T errorReturn)
    {
        try
        {
            return method();
        }
        catch (Exception exception)
        {
            MessageBox.Show($"Big Problem:\n{exception}");
            return errorReturn;
        }
    }

    public bool CreateConnection(string connectionString) => TryExecute(() =>
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
            Connection.Close();
            return true;
        },
        false);

    public void DeleteConnection() => Connection = null;

    public bool OpenConnection() => TryExecute(() =>
        {
            if (Connection is { State: ConnectionState.Closed }) Connection.Open();

            return true;
        },
        false);


    public bool CloseConnection() => TryExecute(() =>
        {
            if (Connection is { State: ConnectionState.Open }) Connection.Close();

            return true;
        },
        false);

    public DataTable? GetTable(string tableName) =>
        TryExecute(() =>
            {
                var commandText = $"SELECT * FROM {tableName}";

                var command = new SqlCommand(commandText, Connection);

                var dateTable = new DataTable();
                OpenConnection();
                dateTable.Load(command.ExecuteReader());
                CloseConnection();
                return dateTable;
            },
            null);

    public bool InsertValue(string tableName, List<string> cols, List<string> values) =>
        TryExecute(() =>
            {
                values = values.Select(val => $"'{val}'").ToList();
                var commandText =
                    $"INSERT INTO {tableName} " +
                    $"({string.Join(", ", cols)}) " +
                    $"VALUES ({string.Join(", ", values)});";


                using var insertCommand = Connection?.CreateCommand();
                if (insertCommand == null) throw new NullReferenceException();
                insertCommand.CommandText = commandText;

                insertCommand.Connection.Open();
                insertCommand.ExecuteNonQuery();
                insertCommand.Connection.Close();
                return true;
            },
            false);

    public bool DeleteRow(string tableName, List<string> cols, List<string> values) => TryExecute(
        () =>
        {
            using var insertCommand = Connection?.CreateCommand();
            if (insertCommand == null) throw new NullReferenceException();

            var compareString = string.Join(
                " AND ",
                cols.Zip(values, (col, val) => $"{col} = '{val}'"));

            insertCommand.CommandText = $"DELETE FROM {tableName} WHERE {compareString}";

            insertCommand.Connection.Open();
            insertCommand.ExecuteNonQuery();
            insertCommand.Connection.Close();
            return true;
        },
        false
    );

    public bool UpdateRow(string tableName, List<string> cols, List<string> oldValues, List<string> setCols,
        List<string> newValues) => TryExecute(() =>
        {
            using var insertCommand = Connection?.CreateCommand();
            if (insertCommand == null) throw new NullReferenceException();

            var compareString = string.Join(
                " AND ",
                cols.Zip(oldValues, (col, val) => $"{col} = '{val}'"));

            var setStrings = new List<string>();
            foreach (var (col, value) in setCols
                         .Zip(newValues)
                         .Where(pair => pair.Second != ""))
                setStrings.Add($"{col} = '{value}'");

            insertCommand.CommandText =
                $"UPDATE {tableName} " +
                $"SET {string.Join(", ", setStrings)} " +
                $"WHERE {compareString}";
            
            MessageBox.Show(insertCommand.CommandText);

            insertCommand.Connection.Open();
            insertCommand.ExecuteNonQuery();
            insertCommand.Connection.Close();

            return true;
        },
        false);
}