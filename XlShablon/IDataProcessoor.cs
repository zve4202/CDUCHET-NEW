using System.Windows.Forms;

namespace GH.XlShablon
{
    public interface IDataProcessor
    {
        void BeginProcess();
        void EndProcess();
        void SelectShablon(XlShablon shablon);
        void SelectResult();
        Control GetControl();
    }
}
