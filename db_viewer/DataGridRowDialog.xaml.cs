using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace lab;

public partial class DataGridRowDialog : Window
{
    private DataGrid _dataGrid;
    private DataRowView? _dataRowView;
    private string _tableName;

    private List<Label> _labels = new();
    private List<TextBox> _textBoxes = new();

    public DataGridRowDialog(DataGrid dataGrid, DataRowView? dataRowView, string tableName)
    {
        _dataGrid = dataGrid;
        _dataRowView = dataRowView;
        _tableName = tableName;

        InitializeComponent();

        var columns = (from DataColumn c in _dataRowView.Row.Table.Columns select c.ColumnName).ToList();
        var values = (from DataColumn c in _dataRowView.Row.Table.Columns
            select _dataRowView.Row[c.ColumnName].ToString()!).ToList();

        for (var i = 0; i < columns.Count; i++)
        {
            var label = new Label
            {
                Content = columns[i]
            };
            _labels.Add(label);
        }

        for (var i = 0; i < columns.Count; i++)
        {
            var textBox = new TextBox
            {
                Text = values[i]
            };
            _textBoxes.Add(textBox);
        }

        for (var i = 0; i < _labels.Count; i++)
        {
            Grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            Grid.SetRow(_labels[i], i);
            Grid.SetColumn(_labels[i], 0);
            Grid.SetRow(_textBoxes[i], i);
            Grid.SetColumn(_textBoxes[i], 1);
            Grid.Children.Add(_labels[i]);
            Grid.Children.Add(_textBoxes[i]);
        }
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
        if (_dataRowView == null)
        {
            Close();
            return;
        }

        var cols = _labels.Select(label => label.Content.ToString()!).ToList();
        var oldValues = (from DataColumn c in _dataRowView.Row.Table.Columns select _dataRowView.Row[c.ColumnName].ToString()!).ToList();
        var newValues = _textBoxes.Select(textBox => textBox.Text).ToList();
        
        DatabaseManager.Instance.UpdateRow(_tableName, cols, oldValues, cols, newValues);
        Close();
    }
}