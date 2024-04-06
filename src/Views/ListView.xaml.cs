using CommunityToolkit.WinUI.Collections;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Data;
using Sungaila.SUBSTitute.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sungaila.SUBSTitute.Views
{
    public sealed partial class ListView : Page
    {
        public ListView()
        {
            this.InitializeComponent();
        }

        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            if (sender is not DataGrid dataGrid)
                return;

            if (dataGrid.DataContext is not MappingViewModel mappingViewModel)
                return;

            string? bindingPath;

            if (e.Column is DataGridBoundColumn boundColumn)
            {
                bindingPath = boundColumn.Binding.Path.Path;
            }
            else
            {
                bindingPath = e.Column.Tag as string;
            }

            if (string.IsNullOrEmpty(bindingPath))
                return;

            mappingViewModel.DrivesFilteredForDataGrid.SortDescriptions.Clear();
            
            foreach (var column in dataGrid.Columns)
            {
                if (column == e.Column)
                    continue;

                column.SortDirection = null;
            }

            var newDirection = e.Column.SortDirection == DataGridSortDirection.Descending
                ? DataGridSortDirection.Ascending
                : DataGridSortDirection.Descending;

            mappingViewModel.DrivesFilteredForDataGrid.SortDescriptions.Add(new SortDescription(bindingPath, newDirection == DataGridSortDirection.Descending ? SortDirection.Descending : SortDirection.Ascending));
            e.Column.SortDirection = newDirection;
        }
    }
}