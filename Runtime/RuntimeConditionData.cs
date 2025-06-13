namespace Behaviours.Runtime
{
    public class RuntimeConditionData
    {
        public RuntimeValueData value;
        public Operation operation;

        public Operand nextOperand;

        public bool bConstant;
        public int iConstant;
        public float fConstant;
        public string sConstant;     
        
        public object getConstant(ValueType type)
        {
            switch (type)
            {
                case ValueType.Bool: return bConstant;
                case ValueType.Float: return fConstant;
                case ValueType.Integer: return iConstant;
                case ValueType.String: return sConstant;
            }
            return null;
        }
    }
}