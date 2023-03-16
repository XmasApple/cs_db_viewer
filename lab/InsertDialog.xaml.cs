using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace lab;

public partial class InsertDialog : Window
{
    public InsertDialog()
    {
        InitializeComponent();
    }

    private void InsertButton_Click(object sender, RoutedEventArgs e)
    {
        DatabaseManager.Instance.InsertValue(
            "product", new List<string> { "Name", "Category" },
            new List<SqlDbType> { SqlDbType.VarChar, SqlDbType.VarChar },
            new List<string> { NameTextBox.Text, CategoryTextBox.Text }
        );
        Close();
    }
}