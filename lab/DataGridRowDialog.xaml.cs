using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace lab;

public partial class DataGridRowDialog : Window
{
    private DataGrid _dataGrid;
    private DataRowView? _dataRowView;
    private string _tableName;

    public DataGridRowDialog(DataGrid dataGrid, DataRowView? dataRowView, string tableName)
    {
        _dataGrid = dataGrid;
        _dataRowView = dataRowView;
        _tableName = tableName;
        InitializeComponent();
    }

    private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
    {
        var cols = new List<string>();
        var values = new List<string>();
        if (_dataRowView != null)
            foreach (DataColumn c in _dataRowView.Row.Table.Columns)
            {
                cols.Add(c.ColumnName);
                values.Add(_dataRowView.Row[c.ColumnName].ToString()!);
            }

        DatabaseManager.Instance.DeleteRow(_tableName, cols, values);
        Close();
    }

    private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
    {
        var cols = new List<string>();
        var oldValues = new List<string>();

        if (_dataRowView != null)
            foreach (DataColumn c in _dataRowView.Row.Table.Columns)
            {
                cols.Add(c.ColumnName);
                oldValues.Add(_dataRowView.Row[c.ColumnName].ToString()!);
            }

        var setCols = new List<string> { "Name", "Category" };
        var newValues = new List<string> { NameTextBox.Text, CategoryTextBox.Text };

        DatabaseManager.Instance.UpdateRow(_tableName, cols, oldValues, setCols, newValues);
        Close();
    }
}