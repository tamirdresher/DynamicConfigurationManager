using System.Collections.Generic;

namespace DynamicConfigurationManager.ConfigurationProperties
{
    public class ListProperty<TListItem> : ConfigurationProperty<TListItem>
    {
        private List<TListItem> _listItems;
        IEnumerable<TListItem> _defaultItems;


        public ListProperty(string name, string description, TListItem[] listItems, TListItem defaultValue = default(TListItem))
            : base(name, description, defaultValue)
        {
            _defaultItems = new List<TListItem>(listItems);
        }

        protected virtual IEnumerable<TListItem> GetDefaultItems()
        {
            return _defaultItems;
        }

        public List<TListItem> ListItems
        {
            get { return _listItems??(_listItems = new List<TListItem>(GetDefaultItems())); }
            set { _listItems = value; }
        }
    }
}