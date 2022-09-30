namespace GH.XlShablon
{
    public class StockParam : FieldParam
    {
        public StockParam(XlShablon shablon) : base(shablon, "Qty", "qtyValue", typeof(int), 6, true)
        {
            ParamFunc = ParamFunctionType.ToSumm;
        }
    }
}
