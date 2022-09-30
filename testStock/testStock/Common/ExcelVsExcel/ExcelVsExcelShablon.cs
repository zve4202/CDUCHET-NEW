namespace GH.XlShablon
{
    public class ExcelVsExcelShablon : XlShablon
    {
        public ExcelVsExcelShablon() : base()
        {
            RemoveNotUsedFields = false;
        }

        protected override FieldsMap getDataMap()
        {
            return new ExcelVsExcelDataMap(this);
        }
    }
}
