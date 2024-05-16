using OrganizadorCat.Models;
using OrganizadorCat.ViewModels.Equipo;
using OrganizadorCat.ViewModels.Proyecto;
using Syncfusion.SfSkinManager;
using Syncfusion.UI.Xaml.Grid;
using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Lógica de interacción para ProyectoMainPage.xaml
    /// </summary>
    /// 
    public class CustomCopyPaste : GridCutCopyPaste
    {
        public CustomCopyPaste(SfDataGrid DataGrid)
            : base(DataGrid)
        {

        }
        protected override void CopyCell(object record, GridColumn column, ref System.Text.StringBuilder text)
        {

            if (this.dataGrid.View == null)
                return;

            object copyText = null;

            if (column is GridTemplateColumn)
            {
                copyText = "vacio";
            }
            else
            {
                copyText = this.dataGrid.View.GetPropertyAccessProvider().GetValue(record, column.MappingName);
            }
            
            var copyargs = this.RaiseCopyGridCellContentEvent(column, record, copyText);
            if (!copyargs.Handled)
            {
                if (this.dataGrid.Columns[leftMostColumnIndex] != column || text.Length != 0)
                    text.Append('\t');

                text.Append(copyargs.ClipBoardValue);
            }
        }
    }

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
        public ProyectoMainPage(ProyectoMainViewModel viewModelExt)
        {
            InitializeComponent();

            DataContext = viewModelExt;
            this.dataGrid.GridCopyPaste = new CustomCopyPaste(this.dataGrid);
            this.dataGrid.CopyGridCellContent += dataGrid_CopyGridCellContent;
            SfSkinManager.SetTheme(this, new Theme(themeName));
        }
    }
}
