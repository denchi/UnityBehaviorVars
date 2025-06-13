using System.Globalization;
using UnityEngine;
using System.IO;
using Sirenix.OdinInspector;

namespace Behaviours
{
    public class Value : ScriptableObject
    {
        [HorizontalGroup("Value", order: -1)]
        [ShowInInspector]
        [HideLabel]
        public string Name
        {
            get => name;
            set => name = value; 
        }
        
        [HorizontalGroup("Value")]
        [HideLabel]
        public ValueType type;
        
        [ShowIf(nameof(IsValueBool))]
        [HorizontalGroup("Value", Width = 18)]
        [HideLabel]
        public bool bValue;
        
        [ShowIf(nameof(IsValueInt))]
        [HorizontalGroup("Value")]
        [HideLabel]
        public int iValue;
        
        [ShowIf(nameof(IsValueFloat))]
        [HorizontalGroup("Value")]
        [HideLabel]
        public float fValue;
        
        [ShowIf(nameof(IsValueString))]
        [HorizontalGroup("Value")]
        [HideLabel]
        public string sValue;
        
        public object runtimeObject;

        //
        
        private bool IsValueBool => type == ValueType.Bool;
        private bool IsValueInt => type == ValueType.Integer;
        private bool IsValueFloat => type == ValueType.Float;
        private bool IsValueString => type == ValueType.String;
        
        //
        
        public Vector2 GetLocation()
        {
            if (runtimeObject == null)
            {
                runtimeObject = Vector2.zero;
            }

            return (Vector2)runtimeObject;
        }

        public void SetLocation(Vector2 l)
        {
            runtimeObject = l;
        }
        
        public T GetExtraData<T>()
        {
            return (T) runtimeObject;
        }

        public void SetExtraData(object data)
        {
            runtimeObject = data;
        }

        public static bool Compare(float left, Operation op, float right)
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
        
        //

        private void OnEnable()
        {
            hideFlags = HideFlags.HideInHierarchy;
        }

        public override string ToString()
        {
            switch (type)
            {
                case ValueType.Bool: return bValue.ToString();
                case ValueType.Float: return fValue.ToString(CultureInfo.InvariantCulture);
                case ValueType.Integer: return iValue.ToString();
                case ValueType.String: return sValue;
                case ValueType.Other: return sValue;
            }
            
            return string.Empty;
        }

        #region SERIALIZATION

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.name);
            writer.Write((int)this.type);

            switch (this.type)
            {
                case ValueType.Bool:
                    writer.Write(this.bValue);
                    break;

                case ValueType.Float:
                    writer.Write(this.fValue);
                    break;

                case ValueType.Integer:
                    writer.Write(this.iValue);
                    break;

                default:
                    writer.Write(this.sValue);
                    break;
            }
        }

        #endregion
    }
}