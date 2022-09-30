namespace GH.XlShablon
{
    public class ExcelVsDbShablon : XlShablon
    {
        public ExcelVsDbShablon() : base()
        {
            RemoveNotUsedFields = false;
        }

        protected override FieldsMap getDataMap()
        {
            return new ExcelVsDbDataMap(this);
        }
    }
}
