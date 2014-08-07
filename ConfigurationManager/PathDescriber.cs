using System.Dynamic;
using System.Text;

namespace ConfigurationManager
{
    class PathDescriber : DynamicObject
    {
        private StringBuilder _strBuilder;

        public PathDescriber()
        {

        }
        public string Path { get { return _strBuilder.ToString(); } }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {

            if (_strBuilder == null)
            {
                _strBuilder = new StringBuilder();

            }
            else
            {
                _strBuilder.Append(".");
            }
            _strBuilder.Append(binder.Name);
            result = this;
            return true;
        }
    }
}