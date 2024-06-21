using Syncfusion.UI.Xaml.Grid;

namespace OrganizadorCat.Views.Proyecto
{
    /// <summary>
    /// Lógica de interacción para ProyectoMainPage.xaml
    /// </summary>
    /// 
    public partial class CustomCopyPaste : GridCutCopyPaste
    {
        public CustomCopyPaste(SfDataGrid DataGrid)
            : base(DataGrid)
        {

        }
        protected override void CopyCell(object record, GridColumn column, ref System.Text.StringBuilder text)
        {

            if (dataGrid.View == null || column == null || text == null)
            {
                return;
            }

            object obj = null;

            if (column is GridTemplateColumn)
            {
                obj = string.Empty;
            }
            else
            {                              
                obj = ((!dataGrid.GridCopyOption.HasFlag(GridCopyOption.IncludeFormat)) ? dataGrid.View.GetPropertyAccessProvider().GetValue(record, column.MappingName) : dataGrid.View.GetPropertyAccessProvider().GetFormattedValue(record, column));                                
            }
            GridCopyPasteCellEventArgs gridCopyPasteCellEventArgs = RaiseCopyGridCellContentEvent(column, record, obj);
            if (!gridCopyPasteCellEventArgs.Handled)
            {
                if (dataGrid.Columns[leftMostColumnIndex] != column || text.Length != 0)
                {
                    text.Append('\t');
                }

                text.Append(gridCopyPasteCellEventArgs.ClipBoardValue);
            }

            //    if (this.dataGrid.View == null)
            //        return;

            //    object copyText = null;

            //    if (column is GridTemplateColumn)
            //    {
            //        copyText = "vacio";
            //    }
            //    else
            //    {
            //        copyText = this.dataGrid.View.GetPropertyAccessProvider().GetValue(record, column.MappingName);
            //    }

            //    var copyargs = this.RaiseCopyGridCellContentEvent(column, record, copyText);
            //    if (!copyargs.Handled)
            //    {
            //        if (this.dataGrid.Columns[leftMostColumnIndex] != column || text.Length != 0)
            //            text.Append('\t');

            //        text.Append(copyargs.ClipBoardValue);
            //    }
            //}
        }
    }
}
