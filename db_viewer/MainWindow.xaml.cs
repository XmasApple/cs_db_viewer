using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

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
            DataGrid.ItemsSource = null;

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

        private static string GetSavePath(string title, string filter, string ext)
        {
            var dlg = new SaveFileDialog
            {
                Title = title,
                Filter = filter,
                DefaultExt = ext,
                AddExtension = true
            };
            return dlg.ShowDialog() == true ? dlg.FileName : "";
        }
        
        private void ExportPDF_OnClick(object sender, RoutedEventArgs e)
        {
            var savePath = GetSavePath("Select PDF file", "PDF(*.pdf)|*.pdf", "pdf");
            if (savePath == "") return;
            var table = DatabaseManager.Instance.GetTable(TableName);
            if (table == null) return;
            Exporter.ExportToPdf(table, savePath);
            MessageBox.Show($"Table exported to file\n${savePath}");
        }

        private void ExportDOCX_OnClick(object sender, RoutedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}