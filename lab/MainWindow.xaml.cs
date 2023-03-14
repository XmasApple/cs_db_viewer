using System.Windows;

namespace lab
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ConnectDB_OnClick(object sender, RoutedEventArgs e)
        {
            var connectionDialog = new ConnectionDialog();
            connectionDialog.ShowDialog();
            if (DatabaseManager.Instance.Connection == null) return;
            UpdateButton.IsEnabled = true;
        }

        private void DisconnectDB_OnClick(object sender, RoutedEventArgs e)
        {
            DatabaseManager.Instance.DeleteConnection();
            UpdateButton.IsEnabled = false;

            MessageBox.Show("Disconnected from database");
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            var dataTable = DatabaseManager.Instance.GetTable("product");

            DataGrid.ItemsSource = dataTable?.DefaultView;
        }
    }
}