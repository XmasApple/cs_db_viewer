using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace lab;

public partial class InsertDialog : Window
{
    
    private DataGrid _dataGrid;
    private string _tableName;
    
    private List<Label> _labels = new();
    private List<TextBox> _textBoxes = new();
    
    public InsertDialog(DataGrid dataGrid, string tableName)
    {
        _dataGrid = dataGrid;
        _tableName = tableName;
        InitializeComponent();
        
        // var columns = (from DataColumn c in _dataRowView.Row.Table.Columns select c.ColumnName).ToList();
        // var values = (from DataColumn c in _dataRowView.Row.Table.Columns
        //     select _dataRowView.Row[c.ColumnName].ToString()!).ToList();
        var columns = new List<string>();
        foreach (var c in _dataGrid.Columns) columns.Add(c.Header.ToString()!);

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
                Text = ""
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
        Grid.SetRow(InsertButton, _labels.Count);
    }

    private void InsertButton_Click(object sender, RoutedEventArgs e)
    {
        // DatabaseManager.Instance.InsertValue(
        //     "product", new List<string> { "Name", "Category" },
        //     new List<string> { NameTextBox.Text, CategoryTextBox.Text }
        // );
        var columns = _dataGrid.Columns.Select(c => c.Header.ToString()!).ToList();
        var values = _textBoxes.Select(t => t.Text).ToList();

        DatabaseManager.Instance.InsertValue(_tableName, columns, values);
        Close();
    }
}