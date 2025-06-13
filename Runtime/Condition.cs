using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using Sirenix.OdinInspector;

namespace Behaviours
{
    public class Condition : ScriptableObject
    {
        [HorizontalGroup("Value")]
        [ValueDropdown(nameof(Values))]
        [HideLabel]
        public Value value;
        
        [HorizontalGroup("Value")]
        [HideLabel]
        public Operation operation;

        [HorizontalGroup("Value")]
        [ShowIf(nameof(IsValueBool))]
        [HideLabel]
        public bool bConstant;
        
        [HorizontalGroup("Value")]
        [ShowIf(nameof(IsValueInt))]
        [HideLabel]
        public int iConstant;
        
        [HorizontalGroup("Value")]
        [ShowIf(nameof(IsValueFloat))]
        [HideLabel]
        public float fConstant;
        
        [HorizontalGroup("Value")]
        [ShowIf(nameof(IsValueString))]
        [HideLabel]
        public string sConstant;

        [HorizontalGroup("Value")]
        [HideLabel]
        public Operand nextOperand;

        private bool IsValueBool => value is { type: ValueType.Bool };
        private bool IsValueInt => value is { type: ValueType.Integer };
        private bool IsValueFloat => value is { type: ValueType.Float };
        private bool IsValueString => value is { type: ValueType.String };

        private IEnumerable Values
        {
            get
            {
                #if UNITY_EDITOR
                var thisPath = UnityEditor.AssetDatabase.GetAssetPath(this);
                var mainAsset = UnityEditor.AssetDatabase.LoadMainAssetAtPath(thisPath);
                if (mainAsset is ILayer layer)
                {
                    return layer.values.Select(v => new ValueDropdownItem(v.name, v));
                }
                #endif

                return Array.Empty<ValueDropdownItem>();
            }
        }

        void OnEnable()
        {
            hideFlags = HideFlags.HideInHierarchy;
        }

        public override string ToString()
        {
            return $"{name} = {value.name} {value.ToString()} {operation} {GetConstant()}";
        }

        private string GetConstant()
        {
            switch (value.type)
            {
                case ValueType.Bool: return bConstant.ToString();
                case ValueType.Float: return fConstant.ToString();
                case ValueType.Integer: return iConstant.ToString();
                case ValueType.String: return sConstant.ToString();
                case ValueType.Other: return sConstant.ToString();
            }
            return "";
        }

        public object ToObject()
        {
            switch (value.type)
            {
                case ValueType.Bool: return bConstant;
                case ValueType.Float: return fConstant;
                case ValueType.Integer: return iConstant;
                case ValueType.String: return sConstant;
                case ValueType.Other: return sConstant;
            }
            return "";
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(this.name);

            Debug.Assert(this.value, "Value is null");

            writer.Write(this.value.name);

            switch (this.value.type)
            {
                case ValueType.Bool:
                    writer.Write(this.bConstant);
                    break;

                case ValueType.Float:
                    writer.Write(this.fConstant);
                    break;

                case ValueType.Integer:
                    writer.Write(this.iConstant);
                    break;

                default:
                    writer.Write(this.sConstant);
                    break;
            }

            writer.Write((int)this.operation);
            writer.Write((int)this.nextOperand);
        }
    }
}
