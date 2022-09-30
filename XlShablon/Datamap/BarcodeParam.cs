using System;
using static GH.Utils.CheckHelper;

namespace GH.XlShablon
{
    public class BarcodeParam : FieldParam
    {
        public BarcodeParam(XlShablon shablon) : base(shablon, "EAN13", "barcodeValue", typeof(string), 13, true)
        {
            ParamFunc = ParamFunctionType.IsKey;
        }

        public override object ConvertValue(object val)
        {
            if (val != null)
            {
                return CheckBarcode(val.ToString());
            }
            return DBNull.Value;
        }

    }


}
