using System.Linq;

namespace GH.XlShablon
{
    internal class LabelCodeParam : FieldParam
    {
        FieldParam _labelParam;
        internal FieldParam LabelParam
        {
            get
            {
                if (_labelParam == null)
                    _labelParam = FieldsMap.FirstOrDefault(x => x.Name == "label_name");
                return _labelParam;
            }
        }

        public override ExcelPanel ExcelPanel
        {
            get => base.ExcelPanel;
            set
            {
                if (base.ExcelPanel == value)
                    return;

                base.ExcelPanel = value;

                LabelParam.Required = ExcelPanel != null;
                LabelParam.Refresh();
            }
        }


        public LabelCodeParam(string caption, string name) : base(caption, name, typeof(string), 20, false)
        {
        }

    }


}
