using OrganizadorCat.Models;
using OrganizadorCat.ViewModels;
using OrganizadorCat.ViewModels.Equipo;
using OrganizadorCat.ViewModels.Proyecto;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using Syncfusion.UI.Xaml.Grid.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OrganizadorCat.Views.Proyecto
{

    public partial class ProyectoMainPage : Page
    {
        public string themeName = App.Current.Properties["Theme"]?.ToString() != null ? App.Current.Properties["Theme"]?.ToString() : "Windows11Light";
       

        void dataGrid_GridCopyContent(object sender, GridCopyPasteEventArgs e)
        {

          
        }
        void dataGrid_CopyGridCellContent(object sender, GridCopyPasteCellEventArgs e)
        {

            if (e.Column.MappingName == "Acciones")
                e.Handled = true;
        }
        public ProyectoMainViewModel _instance { get; set; }
        public ProyectoMainPage(ProyectoMainViewModel viewModelExt)
        {
            InitializeComponent();
            _instance = viewModelExt;
            DataContext = _instance;
            this.dataGrid.GridCopyPaste = new CustomCopyPaste(this.dataGrid);          
            this.dataGrid.CopyGridCellContent += dataGrid_CopyGridCellContent;
            Theme t = new Theme(themeName);
            //this.dataGrid.CurrentCellValueChanged += AssociatedObject_CurrentCellValueChanged;
            //this.dataGrid.CurrentCellEndEdit += AssociatedObject_CurrentCellEndEdit;
            //this.dataGrid.ItemsSourceChanged += datagrid_ItemsSourceChanged;
            SfSkinManager.SetTheme(this, t);
            
        }

        void datagrid_ItemsSourceChanged(object sender, GridItemsSourceChangedEventArgs e)
        {
            if (this.dataGrid.View != null)
                this.dataGrid.View.RecordPropertyChanged += View_RecordPropertyChanged;
        }

        void View_RecordPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //if (e.PropertyName == "IsChecked")
            {
                var datarow = this.dataGrid.GetRowGenerator().Items.FirstOrDefault(row => row.RowIndex != -1 && row.RowData == sender);
                if (datarow != null)
                {
                    var rowcontrol = (datarow as IRowElement).Element;
                    BindingOperations.GetBindingExpression(rowcontrol, VirtualizingCellsControl.BackgroundProperty).UpdateTarget();
                }
            }
        }

        private void AssociatedObject_CurrentCellEndEdit(object? sender, CurrentCellEndEditEventArgs e)
        {
            
            
            if (e.RowColumnIndex.IsEmpty)
                return;
            else
                dataGrid.UpdateDataRow(e.RowColumnIndex.RowIndex);                
            
        }

        private void AssociatedObject_CurrentCellValueChanged(object? sender, CurrentCellValueChangedEventArgs e)
        {
        
        }

        //private void DatetimeProyectosDesde_DateTimeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{

        //}
    }
}
