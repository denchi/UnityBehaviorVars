using System.Collections.Generic;

namespace Behaviours
{
    [System.AttributeUsage(System.AttributeTargets.Enum, AllowMultiple = true)]
    public class ValueEnumAttribute : System.Attribute
    {
        public string Name { get; set; }

        public ValueEnumAttribute(string name)
        {
            this.Name = name;
        }

        public static System.Type FindAttributeForPath(string name)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            foreach (System.Type type in assembly.GetTypes())
            {
                var attrs = type.GetCustomAttributes(typeof(ValueEnumAttribute), true);
                if (attrs.Length > 0)
                {
                    var attr = attrs[0] as ValueEnumAttribute;
                    if (attr.Name.Equals(name))
                    {
                        return type;
                    }
                }
            }
            return null;
        }

        public class EnumTypeInfo
        {
            public System.Type type;
            public System.Array values;
            public string[] names;
        }

        private static Dictionary<string, EnumTypeInfo> _cachedEnumTypeInfos = new Dictionary<string, EnumTypeInfo>();

        public static EnumTypeInfo FindCachedAttributeForPath(string name)
        {
            EnumTypeInfo e = null;
            if (_cachedEnumTypeInfos.TryGetValue(name, out e))
            {
                return e;
            }
            else
            {
                var en = FindAttributeForPath(name);
                if (en != null)
                {
                    _cachedEnumTypeInfos[name] = new EnumTypeInfo() { type = en, names = System.Enum.GetNames(en), values = System.Enum.GetValues(en) };
                    return e;
                }
                else
                {
                    _cachedEnumTypeInfos[name] = null;
                    return e;
                }
            }
        }
    }    
}
