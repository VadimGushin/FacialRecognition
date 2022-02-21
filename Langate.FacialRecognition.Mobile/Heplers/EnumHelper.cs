using MvvmCross.Binding.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Langate.FacialRecognition.Mobile.Heplers
{
    public static class EnumHelper
    {
        public static IDictionary<string, string> GetCollectionByType(Type type)
        {
            var values = Enum.GetNames(type);
            var resultGenders = new Dictionary<string, string>();
            foreach (var value in values)
            {
                string description = GetDescription(Enum.Parse(type, value));
                resultGenders.Add(value, description);
            }
            return resultGenders;
        }

        private static string GetDescription<T>(this T source)
        {
            FieldInfo fieldInfo = source.GetType().GetField(source.ToString());

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            return source.ToString();
        }
    }
}
