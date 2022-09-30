using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GH.Utils
{
    public static class TypeHelper
    {
        public static IEnumerable<PropertyInfo> GetNestedProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.DeclaringType.Name == type.Name);
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
        }

        public static IEnumerable<PropertyInfo> GetFullAccessOnlyProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                .Where(p => p.GetMethod != null && p.SetMethod != null);
        }

        public static IEnumerable<PropertyInfo> GetNestedProperties(this Object obj)
        {
            return obj.GetType().GetNestedProperties();
        }

        public static IEnumerable<PropertyInfo> GetAllProperties(this Object obj)
        {
            return obj.GetType().GetAllProperties();
        }

        public static IEnumerable<PropertyInfo> GetFullAccessOnlyProperties(this Object obj)
        {
            return obj.GetType().GetFullAccessOnlyProperties();
        }
    }
}
