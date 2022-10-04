using System.Data;
using System.Windows.Forms;

namespace GH.XlShablon
{
    public class ExcelVsExcelProcessor : DataProcessor
    {
        public ExcelVsExcelProcessor(Control control) : base(control)
        {
        }

        protected override void CalculateRow(DataRow row)
        {
            int idx = Shablon.DataMap.Count + ShablonIndex;

            if (ShablonIndex == Shablon.DataMap.Count - 1)
            {
                int qty1 = 0;
                int.TryParse(row[1].ToString(), out qty1);
                int qty2 = 0;
                int.TryParse(row[2].ToString(), out qty2);
                row.SetField<int>(3, qty1 - qty2);
                row.AcceptChanges();
            }

        }

        protected override bool AddCalculateFields()
        {
            DataColumn column = new DataColumn("sumOfQty", typeof(int));
            column.Caption = "Difference";
            column.DefaultValue = 0;
            ResultData.Columns.Add(column);
            int idx = Shablon.DataMap.Count + ShablonIndex;
            column.SetOrdinal(idx);
            return true;
        }

    }
}
