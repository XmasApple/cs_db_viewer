using System;
using System.Windows;
// mysql connector
using System.Data.SqlClient;
using System.Windows.Controls;

namespace lab;

public partial class ConnectionDialog : Window
{
    public ConnectionDialog()
    {
        InitializeComponent();
    }

    private void ConnectButton_Click(object sender, RoutedEventArgs e)
    {
        var databaseAddress = (TextBox)FindName("DatabaseAddressTextBox")!;
        var databaseName = (TextBox)FindName("DatabaseNameTextBox")!;
        var username = (TextBox)FindName("UserNameTextBox")!;
        var password = (PasswordBox)FindName("PasswordTextBox")!;

        var connectionString = 
            $"Server={databaseAddress.Text};" +
            $"Database={databaseName.Text};" +
            $"User ID={username.Text};" +
            $"Password={password.Password}";
        
        var connection = new SqlConnection(connectionString);
        try
        {
            connection.Open();
            MessageBox.Show("Connection successful!");
            DatabaseManager.Connection = connection;
            Close();

        }
        catch (Exception exception)
        {
            MessageBox.Show($"Big Problem: {exception}");
        }
    }
}