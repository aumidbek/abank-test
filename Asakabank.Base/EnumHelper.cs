using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Asakabank.Base {
    public static class EnumHelper {
        public static string GetEnumDescription(Enum value) {
            var field = value.GetType().GetField(value.ToString());
            if (field == null)
                return value.ToString();
            var descriptionAttributeArray =
                (DescriptionAttribute[]) field.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return descriptionAttributeArray.Length > 0 ? descriptionAttributeArray[0].Description : value.ToString();
        }

        public static string ToDescription<TEnum>(this TEnum enumValue) where TEnum : struct {
            return GetEnumDescription((Enum) (ValueType) enumValue);
        }

        public static List<SourceForEnum<T>> GenerateSource<T>() where T : struct {
            return (from T obj in Enum.GetValues(typeof(T))
                select new SourceForEnum<T> {Name = GetEnumDescription((Enum) (ValueType) obj), Type = obj}).ToList();
        }

        public static List<SourceForEnum<byte>> GenerateSourceByte<T>() where T : struct {
            return (from T obj in Enum.GetValues(typeof(T))
                select new SourceForEnum<byte>
                    {Name = GetEnumDescription((Enum) (ValueType) obj), Type = Convert.ToByte(obj)}).ToList();
        }

        public static List<SourceForEnum<int>> GenerateSourceInt<T>() where T : struct {
            return (from T obj in Enum.GetValues(typeof(T))
                select new SourceForEnum<int>
                    {Name = GetEnumDescription((Enum) (ValueType) obj), Type = Convert.ToInt32(obj)}).ToList();
        }

        public static List<SourceForEnum<T>> GenerateSource<T>(T[] without) where T : struct {
            return (from T obj in Enum.GetValues(typeof(T))
                where !without.Contains(obj)
                select new SourceForEnum<T> {Name = GetEnumDescription((Enum) (ValueType) obj), Type = obj}).ToList();
        }
    }
}