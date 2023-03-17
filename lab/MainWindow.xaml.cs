using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace lab
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private const string TableName = "product";

        private void ConnectDB_OnClick(object sender, RoutedEventArgs e)
        {
            var connectionDialog = new ConnectionDialog();
            connectionDialog.ShowDialog();
            if (DatabaseManager.Instance.Connection == null) return;
            UpdateButton.IsEnabled = true;
            InsertButton.IsEnabled = true;
            UpdateButton_Click(null!, null!);
        }

        private void DisconnectDB_OnClick(object sender, RoutedEventArgs e)
        {
            DatabaseManager.Instance.DeleteConnection();
            UpdateButton.IsEnabled = false;
            InsertButton.IsEnabled = false;

            MessageBox.Show("Disconnected from database");
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = DatabaseManager.Instance.GetTable(TableName);

            DataGrid.ItemsSource = dataTable?.DefaultView;
        }

        private void InsertButton_Click(object sender, RoutedEventArgs e)
        {
            var insertDialog = new InsertDialog();
            insertDialog.ShowDialog();
            UpdateButton_Click(null!, null!);
        }

        private void DataGrid_OnBeginningEdit(object? sender, DataGridBeginningEditEventArgs e)
        {
            var dataGridRowDialog = new DataGridRowDialog(
                DataGrid,
                e.Row.Item as DataRowView,
                TableName);
            dataGridRowDialog.ShowDialog();
            
            UpdateButton_Click(null!, null!);
        }
    }
}