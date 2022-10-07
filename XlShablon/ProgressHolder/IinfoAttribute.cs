using System.ComponentModel;

namespace GH.XlShablon
{
    public class IinfoAttribute : DescriptionAttribute
    {
        private string _property;

        public IinfoAttribute(string description, string property) : base(description)
        {
            _property = property;
        }

        public string Property => _property;
    }
}
