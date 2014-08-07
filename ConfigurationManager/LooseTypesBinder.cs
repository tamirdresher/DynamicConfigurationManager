using System;
using System.Diagnostics;
using System.Reflection;
using Newtonsoft.Json.Serialization;

namespace ConfigurationManager
{
    public class LooseTypesBinder : DefaultSerializationBinder
    {


        public override Type BindToType(string assemblyName, string typeName)
        {
            try
            {
                return base.BindToType(assemblyName, typeName);
            }
            catch (Exception)
            {
                return GetTypeFromTypeNameKey(assemblyName, typeName);
            }
        }

        private  Type GetTypeFromTypeNameKey(string assemblyName, string typeName)
        {
            
            if (assemblyName == null)
            {
                return Type.GetType(typeName);
            }
            Assembly assembly = Assembly.LoadWithPartialName(assemblyName);
            if (assembly == null)
            {
                Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
                Assembly[] array = assemblies;
                for (int i = 0; i < array.Length; i++)
                {
                    Assembly assembly2 = array[i];
                    if (assembly2.GetName().Name == assemblyName)
                    {
                        assembly = assembly2;
                        break;
                    }
                }

            }
            if (assembly == null)
            {
                Trace.WriteLine(string.Format("Could not load assembly '{0}'.", assemblyName));
                return typeof (ErrorConfigurationNode);
            }
            Type type = assembly.GetType(typeName);
            if (type == null)
            {
                var generic=CreateGenericType(assemblyName, typeName);
                if (generic!=null)
                {
                    return generic;
                }
                Trace.WriteLine(string.Format("Could not find type '{0}' in assembly '{1}'.", typeName, assembly.FullName));
                return typeof(ErrorConfigurationNode);
                
            }
            return type;
        }

        private Type CreateGenericType(string assemblyName, string typeName)
        {
            if (typeName.Contains("[["))
            {
                var genericDefPos = typeName.IndexOf("[[");
                var genericTypeStr = typeName.Substring(0, genericDefPos);
                var genericType = GetTypeFromTypeNameKey(assemblyName, genericTypeStr);

                var name = typeName.Substring(genericDefPos + 2, typeName.Length - genericDefPos - 4);
                var startIndexOfAssembly = name.IndexOf(',') + 1;
                var genericTypeParam = GetTypeFromTypeNameKey(name.Substring(startIndexOfAssembly+1),
                    name.Substring(0, startIndexOfAssembly - 1));

                return genericType.MakeGenericType(genericTypeParam);
            }
            return null;
        }
    }
}