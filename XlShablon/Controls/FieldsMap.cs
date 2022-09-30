using System.Collections.Generic;

namespace GH.XlShablon
{
    public class FieldsMap : List<FieldParam>
    {
        protected XlShablon Shablon;
        public FieldsMap(XlShablon shablon)
        {
            Shablon = shablon;
            CreateMap();
        }

        protected virtual void CreateMap() { }

        new public void Clear()
        {
            foreach (FieldParam item in ToArray())
            {
                if (item.ParamFunc == FieldParam.ParamFunctionType.OutSourceData)
                    Remove(item);
                else
                    item.Clear();
            }
        }

        internal bool MapIsReady(bool showDlg)
        {
            foreach (var item in this)
            {
                if (!item.Validate(showDlg))
                    return false;
            };

            return true;
        }
    }


}
