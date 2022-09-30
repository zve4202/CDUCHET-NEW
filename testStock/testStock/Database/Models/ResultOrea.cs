using FluentNHibernate.Mapping;
using GH.Database;

namespace Tester.Database
{
    public class ResultOrea : BaseEntity
    {
        [UpdatableProperty(Caption = "Остаток реал.")]
        public virtual int OreaQty { get; set; }
    }

    public class ResultOreaMap : ClassMap<ResultOrea>
    {
        public ResultOreaMap()
        {
            Table("check_orea_stock_by_barcorde");
            Id(x => x.Id, "id");
            Map(x => x.OreaQty, "orea_qty");
        }
    }
}
