using System;
using System.Collections.Generic;
using System.Linq;

namespace DynamicConfigurationManager.ConfigurationProperties
{
    public class EnumProperty<TEnum> : ListProperty<TEnum>
        where TEnum : struct, IComparable, IConvertible, IFormattable
    {
        public EnumProperty(string name,string description, TEnum defaultValue = default(TEnum))
            : base(name,description, new TEnum[0],  defaultValue)
        {
            if (!typeof (TEnum).IsEnum)
            {
                throw new ArgumentException("EnumProperty cna only work with enum types", "TEnum");
            }
            
        }

        protected override IEnumerable<TEnum> GetDefaultItems()
        {
            return Enum.GetValues(typeof(TEnum)).OfType<TEnum>();
        }
    }
}