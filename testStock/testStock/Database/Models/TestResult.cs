using FluentNHibernate.Mapping;
using GH.Database;
using GH.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tester.Database
{

    public class TestResult : BaseEntity, ITestResult
    {
        public virtual int ScanType { get; set; }
        [UpdatableProperty(Caption = "Всего прих.")]
        public virtual int PrixQty { get; set; }
        [UpdatableProperty(Caption = "Остаток скл.")]
        public virtual int StockQty { get; set; }
        [UpdatableProperty(Caption = "Остаток реал.")]
        public virtual int OreaQty { get; set; }
        public virtual IEnumerable<PropertyInfo> GetScanTypeProperties()
        {
            string[] stok = { nameof(PrixQty), nameof(StockQty) };
            string orea = nameof(OreaQty);
            List<PropertyInfo> result = new List<PropertyInfo>();
            foreach (var item in this.GetNestedProperties())
            {
                switch (ScanType)
                {
                    case 0:
                        if (stok.Contains(item.Name))
                        {
                            result.Add(item);
                        }
                        break;
                    case 1:
                        if (item.Name == orea)
                        {
                            result.Add(item);
                        }
                        break;
                    case 2:
                        if (stok.Contains(item.Name) || item.Name == orea)
                        {
                            result.Add(item);
                        }
                        break;
                    default:
                        break;
                }
            }


            return result;
        }

    }

    public class TestResultMap : ClassMap<TestResult>
    {
        public TestResultMap()
        {
            Table("check_stock_orea_by_barcorde");
            Id(x => x.Id, "id");
            Map(x => x.ScanType, "scan_type");
            Map(x => x.PrixQty, "prix_qty");
            Map(x => x.StockQty, "stock_qty");
            Map(x => x.OreaQty, "orea_qty");
        }
    }
}