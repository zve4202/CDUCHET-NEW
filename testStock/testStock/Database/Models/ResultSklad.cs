using FluentNHibernate.Mapping;
using GH.Database;

namespace Tester.Database
{
    public class ResultSklad : BaseEntity
    {
        [UpdatableProperty(Caption = "Всего прих.")]
        public virtual int PrixQty { get; set; }
        [UpdatableProperty(Caption = "Остаток скл.")]
        public virtual int StockQty { get; set; }
    }

    public class ResultSkladMap : ClassMap<ResultSklad>
    {
        public ResultSkladMap()
        {
            Table("check_sklad_stock_by_barcorde");
            Id(x => x.Id, "id");
            Map(x => x.PrixQty, "prix_qty");
            Map(x => x.StockQty, "stock_qty");
        }
    }
}
