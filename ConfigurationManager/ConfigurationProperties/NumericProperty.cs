using System;

namespace ConfigurationManager.ConfigurationProperties
{
    public class NumericProperty<TNumeric> : ConfigurationProperty<TNumeric> where TNumeric : struct,IComparable
    {
        public NumericProperty(string name,
            string description,
            TNumeric defaultValue = default(TNumeric),
            TNumeric? maximum = null,
            TNumeric? minimum = null,
            string unit="N/A")
            : base(name,description, defaultValue)
        {
            Maximum = maximum;
            Minimum = minimum;
            Unit = unit;
        }


        public TNumeric? Maximum { get; set; }
        public TNumeric? Minimum { get; set; }
        public string Unit { get; set; }
    }
}