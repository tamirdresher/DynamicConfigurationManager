using System.Dynamic;
using System.Linq;
using System.Text;

namespace DynamicConfigurationManager
{
    public class PathDescriber : DynamicObject
    {
        private readonly StringBuilder _strBuilder = new StringBuilder();

        public PathDescriber()
        {

        }
        public string Path { get { return _strBuilder.ToString(); } }
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = this;
            return AppendToPath(binder.Name);
        }

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            result = this;
            return AppendToPath(indexes.First().ToString());
        }

        private bool AppendToPath(string name)
        {
            if (_strBuilder.Length != 0)
            {
                _strBuilder.Append(".");
            }
            _strBuilder.Append(name);

            return true;
        }
    }


}