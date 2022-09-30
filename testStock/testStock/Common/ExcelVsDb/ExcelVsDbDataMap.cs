namespace GH.XlShablon
{
    public class ExcelVsDbDataMap : FieldsMap
    {
        public ExcelVsDbDataMap(XlShablon shablon) : base(shablon)
        {
        }

        protected override void CreateMap()
        {
            //SET @barcode = < VARCHAR(13) >;
            Add(new BarcodeParam(Shablon));
        }
    }
}
