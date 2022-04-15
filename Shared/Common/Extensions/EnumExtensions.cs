using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDisplayName(this Enum enumValue)
        {
            if (enumValue == null)
            {
                return string.Empty;
            }

            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
        }

        public static T ToEnumValueByDisplayName<T>(string displayName)
            where T : Enum
        {
            var members = Enum.GetValues(typeof(T)).Cast<T>();
            var member = members.FirstOrDefault(m => m.ToDisplayName() == displayName);
            return member;
        }       
    }
}
