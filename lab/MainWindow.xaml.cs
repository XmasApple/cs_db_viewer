using System;
using System.Windows;

namespace lab
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
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
        }

        private void DisconnectDB_OnClick(object sender, RoutedEventArgs e)
        {
            DatabaseManager.Connection?.Close();
            DatabaseManager.Connection = null;

            MessageBox.Show("Disconnected from database");
        }
    }
}