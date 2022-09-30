using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GH.Database
{
    public class BaseEntity : AbstractEntity
    {
        #region поля и свойства
        [UpdatableProperty(Key = true, Caption = "ID", ToolTip = "ID записи"), Editable(false), ReadOnly(true)]
        public virtual int Id { get; set; }

        [UpdatableProperty(Caption = "Наименование", ToolTip = "Наименование")]
        public virtual string Name { get; set; }
        #endregion

        #region перезаписанные методы
        public override bool Equals(object obj)
        {
            if (obj is BaseEntity objEntity)
                return (Id == 0 && base.Equals(obj)) || Id == objEntity.Id;

            return false;
        }

        public override int GetHashCode()
        {
            return 1877310944 + Id.GetHashCode();
        }

        public override void CancelEdit()
        {
            if (Id == 0)
                return;

            base.CancelEdit();
        }

        #endregion
    }
}
