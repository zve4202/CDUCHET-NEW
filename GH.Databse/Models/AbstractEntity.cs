using GH.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GH.Database
{
    [DataContract]
    public class AbstractEntity : IEditableObject
    {
        #region Свойства и поля
        private object _savedCopy = null;
        [Bindable(BindableSupport.No)]
        public virtual bool HasChanges => WasChanged();
        #endregion

        #region перезаписанные методы


        #endregion

        #region Методы
        protected object GetSavedCopy()
        {
            return _savedCopy;
        }

        public virtual void BeginEdit()
        {
            if (_savedCopy != null)
                return;
            _savedCopy = this.MemberwiseClone();
        }

        public virtual void CancelEdit()
        {
            if (_savedCopy == null)
                return;

            var copyProps = _savedCopy.GetFullAccessOnlyProperties();
            foreach (var prop in this.GetFullAccessOnlyProperties())
            {
                var copyProp = copyProps.First(x => x.Name == prop.Name);
                prop.SetValue(this, copyProp.GetValue(_savedCopy));
            }
            EndEdit();
        }

        public virtual void CancelEdit(string group = null)
        {
            if (_savedCopy == null)
                return;

            var copyProps = _savedCopy.GetFullAccessOnlyProperties();
            foreach (var prop in this.GetFullAccessOnlyProperties().Where(x => group == null || x.GetCustomAttributes(true).OfType<DisplayAttribute>().Any(a => a.GroupName == group)))
            {
                var copyProp = copyProps.First(x => x.Name == prop.Name);
                prop.SetValue(this, copyProp.GetValue(_savedCopy));
            }
        }

        public virtual void EndEdit()
        {
            _savedCopy = null;
        }

        public virtual void Assigne(object entity)
        {
            if (entity == null || GetType() != entity.GetType())
                return;

            var entityProps = entity.GetFullAccessOnlyProperties();
            foreach (PropertyInfo prop in this.GetFullAccessOnlyProperties())
            {
                PropertyInfo entityProp = entityProps.First(x => x.Name == prop.Name);
                prop.SetValue(this, entityProp.GetValue(entity));
            }

            if (_savedCopy != null)
                _savedCopy = MemberwiseClone();
        }

        public virtual bool WasChanged(string group = null)
        {
            if (_savedCopy == null)
                return false;

            IEnumerable<PropertyInfo> copy_info = _savedCopy.GetFullAccessOnlyProperties();
            IEnumerable<PropertyInfo> this_info = this.GetFullAccessOnlyProperties()
                .Where(x => group == null ||
                x.GetCustomAttributes(true).OfType<DisplayAttribute>().Any(a => a.GroupName == group)
                );

            foreach (PropertyInfo prop_value in this_info)
            {
                if (prop_value.Name == nameof(HasChanges))
                    continue;

                PropertyInfo copy_value = copy_info.First(x => x.Name == prop_value.Name);

                object obj1 = prop_value.GetValue(this);
                object obj2 = copy_value.GetValue(_savedCopy);
                //if (obj1 is AbstractEntity editable && editable.HasChanges)
                //    return true;

                if (!Convert.Equals(obj1, obj2))
                    return true;

            }
            return false;
        }

        public virtual bool ValueChanged(string name)
        {
            if (_savedCopy == null)
                return false;
            var this_value = this.GetFullAccessOnlyProperties().FirstOrDefault(x => x.Name == name);
            if (this_value == null)
                return false;

            var copy_value = _savedCopy.GetFullAccessOnlyProperties().FirstOrDefault(x => x.Name == name);
            if (copy_value == null)
                return false;

            object obj1 = this_value.GetValue(this);
            object obj2 = copy_value.GetValue(_savedCopy);

            return !Convert.Equals(obj1, obj2);
        }

        public static PropertyInfo GetProperty(IEditableObject model, string name)
        {
            return model.GetFullAccessOnlyProperties().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
        }



        private string ValToSting(object val)
        {
            if (val == null)
                return "(Пусто)";

            Type type = val.GetType();

            if (type == typeof(int) || type == typeof(Nullable<int>))
                return string.Format("{0:N0}", val);

            if (type == typeof(double) || type == typeof(Nullable<double>))
                return string.Format("{0:f2}", val);

            if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
                return string.Format("{0:d}", val);

            return val.ToString();
        }

        public virtual object AsValue(string name)
        {
            var this_value = this.GetFullAccessOnlyProperties().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (this_value == null)
                return null;

            return this_value.GetValue(this);
        }

        public virtual void AsValue(string name, object value)
        {
            var this_value = this.GetFullAccessOnlyProperties().FirstOrDefault(x => x.Name.ToLower() == name.ToLower());

            if (this_value == null)
                return;

            this_value.SetValue(this, value);
        }

        public virtual int AsInteger(string name)
        {
            return (int)AsValue(name);
        }

        public virtual void AsInteger(string name, int value)
        {
            AsValue(name, value);
        }

        public virtual bool AsBoolean(string name)
        {
            return (bool)AsValue(name);
        }

        public virtual void AsBoolean(string name, bool value)
        {
            AsValue(name, value);
        }

        #endregion
    }

}

