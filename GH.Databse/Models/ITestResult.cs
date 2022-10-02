using System.Collections.Generic;
using System.Reflection;

namespace GH.Database
{
    public interface ITestResult
    {
        IEnumerable<PropertyInfo> GetScanTypeProperties();
    }

}

