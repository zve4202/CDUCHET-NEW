namespace GH.XlShablon
{
    public class ExcelVsExcelDataMap : FieldsMap
    {
        public ExcelVsExcelDataMap(XlShablon shablon) : base(shablon)
        {
        }

        protected override void CreateMap()
        {
            //SET @barcode = < VARCHAR(13) >;
            Add(new BarcodeParam(Shablon));
            //SET @stock = < INT(6) >;
            Add(new StockParam(Shablon));
        }
    }
}
