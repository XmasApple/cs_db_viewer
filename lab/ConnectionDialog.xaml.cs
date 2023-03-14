﻿using System;
using System.Windows;
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
        var connectionString =
            $"Server={DatabaseAddressTextBox.Text};" +
            $"Database={DatabaseNameTextBox.Text};" +
            $"User ID={UserNameTextBox.Text};" +
            $"Password={PasswordTextBox.Password}";

        DatabaseManager.Instance.CreateConnection(connectionString);
        Close();
    }
}