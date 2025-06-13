using System.Collections.Generic;

namespace Behaviours.Runtime
{
    public class RuntimeValueData
    {
        public ValueType type;
        public string name;
        public int hash;

        public bool bValue;
        public int iValue;
        public float fValue;
        public string sValue;

        public object pValue;

        public List<RuntimeValueData> AsList
        {
            get
            {
                return pValue as List<RuntimeValueData>;
            }
        }

        public object ToObject()
        {
            switch (type)
            {
                case ValueType.Bool: return bValue;
                case ValueType.Float: return fValue;
                case ValueType.Integer: return iValue;
                case ValueType.String: return sValue;
                case ValueType.Other: return sValue;
                case ValueType.Array: return pValue;
            }
            return null;
        }

        public static bool compare(float left, Operation op, float right)
        {
            switch (op)
            {
                case Operation.Is: return left == right;
                case Operation.IsNot: return left != right;
                case Operation.IsLess: return left < right;
                case Operation.IsLessOrEqual: return left <= right;
                case Operation.IsGreater: return left > right;
                case Operation.IsGreaterOrEqual: return left >= right;
            }

            return false;
        }

        public bool compare(RuntimeConditionData c)
        {
            if (this.type == ValueType.Float)
            {
                #region FLOAT
                switch (c.operation)
                {
                    case Operation.Is: return this.fValue == c.fConstant;
                    case Operation.IsNot: return this.fValue != c.fConstant;
                    case Operation.IsLess: return this.fValue < c.fConstant;
                    case Operation.IsLessOrEqual: return this.fValue <= c.fConstant;
                    case Operation.IsGreater: return this.fValue > c.fConstant;
                    case Operation.IsGreaterOrEqual: return this.fValue >= c.fConstant;
                    default: return false;
                }
                #endregion
            }
            else if (this.type == ValueType.Bool)
            {
                #region BOOL
                switch (c.operation)
                {
                    case Operation.Is: return this.bValue == c.bConstant;
                    case Operation.IsNot: return this.bValue != c.bConstant;
                    default: return false;
                }
                #endregion
            }
            else if (this.type == ValueType.Integer)
            {
                #region INT
                switch (c.operation)
                {
                    case Operation.Is: return this.iValue == c.iConstant;
                    case Operation.IsNot: return this.iValue != c.iConstant;
                    case Operation.IsLess: return this.iValue < c.iConstant;
                    case Operation.IsLessOrEqual: return this.iValue <= c.iConstant;
                    case Operation.IsGreater: return this.iValue > c.iConstant;
                    case Operation.IsGreaterOrEqual: return this.iValue >= c.iConstant;
                    default: return false;
                }
                #endregion
            }
            else if (this.type == ValueType.String)
            {
                #region BOOL
                switch (c.operation)
                {
                    case Operation.Is: return this.sValue == c.sConstant;
                    case Operation.IsNot: return this.sValue != c.sConstant;
                    default: return false;
                }
                #endregion
            }
            else if (this.type == ValueType.Other)
            {
                #region BOOL
                switch (c.operation)
                {
                    case Operation.Is: return this.sValue == c.sConstant;
                    case Operation.IsNot: return this.sValue != c.sConstant;
                    default: return false;
                }
                #endregion
            }

            return false;
        }

        protected RuntimeValueData()
        {

        }

        public RuntimeValueData(string name)
        {
            this.name = name;
            this.hash = name.GetHashCode();
        }

        public RuntimeValueData(string name, ValueType type)
        {
            this.name = name;
            this.hash = name.GetHashCode();
            this.type = type;
        }
    }
}