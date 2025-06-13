namespace Behaviours
{
    [System.AttributeUsage(System.AttributeTargets.Class)]
    public class RuntimeDataAttribute : System.Attribute
    {
        public System.Type runtimeDataType;

        public RuntimeDataAttribute(System.Type runtimeDataType)
        {
            this.runtimeDataType = runtimeDataType;
        }

        public static T createRuntimeDataFromAttribute<T>(System.Type serviceType) where T : class, new()
        {
            T runtimeData = null;
            object[] attributes = serviceType.GetCustomAttributes(typeof(RuntimeDataAttribute), true);
            if (attributes.Length > 0)
            {
                RuntimeDataAttribute attribute = attributes[attributes.Length - 1] as RuntimeDataAttribute;
                runtimeData = System.Activator.CreateInstance(attribute.runtimeDataType) as T;
            }
            return runtimeData;
        }
    }
}