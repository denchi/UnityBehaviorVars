using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Behaviours.Runtime
{
    public class RuntimeValueCollection
    {
        public List<RuntimeValueData> Values => _values;

        public Action<string> ValueChanged;

        private const int TriggerFramesCount = 3;
        private List<RuntimeValueData> _values;
        private Dictionary<string, RuntimeValueData> _cachedStringValues;
        private Dictionary<int, RuntimeValueData> _cachedHashValues;

        #region OTHER

        public static int NameToHash(string valueName) => valueName?.GetHashCode() ?? -1;

        public void InitWithLayer(ILayer layer)
        {
            _cachedStringValues = new Dictionary<string, RuntimeValueData>();
            _cachedHashValues   = new Dictionary<int, RuntimeValueData>();
            _values             = new List<RuntimeValueData>();
            
            Debug.Assert(layer, "Layer is Null!");
            Debug.Assert(layer.values != null, "Layer values are Null!");

            foreach (var value in layer.values)
            {
                var runtimeValueData = new RuntimeValueData(value.name)
                {
                    type = value.type
                };
                
                switch (value.type)
                {
                    case ValueType.Bool: runtimeValueData.bValue = value.bValue; break;
                    case ValueType.Integer: runtimeValueData.iValue = value.iValue; break;
                    case ValueType.Float: runtimeValueData.fValue = value.fValue; break;
                    case ValueType.String: runtimeValueData.sValue = value.sValue; break;
                    case ValueType.Other: runtimeValueData.sValue = value.sValue; break;
                }

                _cachedHashValues[runtimeValueData.hash] = runtimeValueData;
                _cachedStringValues[runtimeValueData.name] = runtimeValueData;
                Values.Add(runtimeValueData);
            }
        }

        public IEnumerator InitWithLayerAsync(ILayer layer)
        {
            _cachedStringValues = new Dictionary<string, RuntimeValueData>();
            _cachedHashValues = new Dictionary<int, RuntimeValueData>();
            _values = new List<RuntimeValueData>();

            foreach (var value in layer.values)
            {
                var runtimeValueData = new RuntimeValueData(value.name);
                switch (value.type)
                {
                    case ValueType.Bool: runtimeValueData.bValue = value.bValue; break;
                    case ValueType.Integer: runtimeValueData.iValue = value.iValue; break;
                    case ValueType.Float: runtimeValueData.fValue = value.fValue; break;
                    case ValueType.String: runtimeValueData.sValue = value.sValue; break;
                    case ValueType.Other: runtimeValueData.sValue = value.sValue; break;
                }

                _cachedHashValues[runtimeValueData.hash] = runtimeValueData;
                _cachedStringValues[runtimeValueData.name] = runtimeValueData;
                _values.Add(runtimeValueData);

                yield return null;
            }
        }

        public RuntimeValueData GetRuntimeValue(string name)
        {
            return _cachedStringValues.TryGetValue(name, out var val) ? val : null;
        }

        private RuntimeValueData GetRuntimeValue(int hashCode)
        {
            return _cachedHashValues.TryGetValue(hashCode, out var val) ? val : null;
        }

        private IEnumerator SetTriggerCoroutine(RuntimeValueData value)
        {
            for (var i = 0; i < TriggerFramesCount; ++i)
            {
                value.bValue = true;
                yield return null;
                yield return new WaitForEndOfFrame();
            }
            value.bValue = false;
        }

        #endregion

        #region SETTER

        public void SetFloat(string name, float value)
        {
            var temp = GetRuntimeValue(name);
            if (temp == null)
            {
                Debug.LogWarning($"Runtime data has no value named '{name}'");
                return;
            }
            temp.fValue = value;
            ValueChanged?.Invoke(name);
        }

        public void SetInt(string name, int value)
        {
            var temp = GetRuntimeValue(name);
            if (temp == null)
            {
                Debug.LogWarning($"Runtime data has no value named '{name}'");
                return;
            }
            temp.iValue = value;
            ValueChanged?.Invoke(name);
        }

        public void SetBool(string name, bool value)
        {
            var temp = GetRuntimeValue(name);
            if (temp == null)
            {
                Debug.LogWarning($"Runtime data has no value named '{name}'");
                return;
            }
            temp.bValue = value;
            ValueChanged?.Invoke(name);
        }

        public void SetString(string name, string value)
        {
            var temp = GetRuntimeValue(name);
            if (temp == null)
            {
                Debug.LogWarning($"Runtime data has no value named '{name}'");
                return;
            }
            temp.sValue = value;
            ValueChanged?.Invoke(name);
        }

        public void SetObject(string name, object value)
        {
            var temp = GetRuntimeValue(name);
            if (temp == null)
            {
                Debug.LogWarning($"Runtime data has no value named '{name}'");
                return;
            }
            temp.pValue = value;
            ValueChanged?.Invoke(name);
        }

        public void SetTrigger(string name, MonoBehaviour component)
        {
            if (!component.gameObject.activeInHierarchy) 
                return;
            
            var temp = GetRuntimeValue(name);
            if (temp == null)
            {
                Debug.LogWarning($"Runtime data has no value named '{name}'");
                return;
            }
            component.StartCoroutine(SetTriggerCoroutine(temp));
        }

        #endregion

        #region GETTER

        public T GetObject<T>(string name) where T : class
        {
            var temp = GetRuntimeValue(name);
            Debug.Assert(temp != null, $"Runtime data has no value named '{name}'");       
            return (T)temp.pValue;
        }

        public object GetObject(string name)
        {
            var temp = GetRuntimeValue(name);
            Debug.Assert(temp != null, $"Runtime data has no value named '{name}'");       
            return temp.pValue;
        }

        public float GetFloat(string name)
        {
            var temp = GetRuntimeValue(name);
            Debug.Assert(temp != null, $"Runtime data has no value named '{name}'");       
            return temp.fValue;
        }

        public int GetInt(string name)
        {
            var temp = GetRuntimeValue(name);
            Debug.Assert(temp != null, $"Runtime data has no value named '{name}'");       
            return temp.iValue;
        }

        public bool GetBool(string name)
        {
            var temp = GetRuntimeValue(name);
            Debug.Assert(temp != null, $"Runtime data has no value named '{name}'");       
            return temp.bValue;
        }

        public string GetString(string name)
        {
            var temp = GetRuntimeValue(name);
            Debug.Assert(temp != null, $"Runtime data has no value named '{name}'");       
            return temp.sValue;
        }

        #endregion

        #region HASH CODES
        
        #region SETTER

        public void SetFloat(int hashCode, float value)
        {
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");       
            temp.fValue = value;
            ValueChanged?.Invoke(temp.name);
        }

        public void SetInt(int hashCode, int value)
        {
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");
            temp.iValue = value;
            ValueChanged?.Invoke(temp.name);
        }

        public void SetBool(int hashCode, bool value)
        {
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");
            temp.bValue = value;
            ValueChanged?.Invoke(temp.name);
        }

        public void SetString(int hashCode, string value)
        {
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");
            temp.sValue = value;
            ValueChanged?.Invoke(temp.name);
        }

        public void SetTrigger(int hashCode, MonoBehaviour component)
        {
            if (!component.gameObject.activeInHierarchy) 
                return;
            
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");
            component.StartCoroutine(SetTriggerCoroutine(temp));
        }

        #endregion

        #region GETTER

        public float GetFloat(int hashCode)
        {
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");
            return temp.fValue;
        }

        public int GetInt(int hashCode)
        {
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");
            return temp.iValue;
        }

        public bool GetBool(int hashCode)
        {
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");
            return temp.bValue;
        }

        public string GetString(int hashCode)
        {
            var temp = GetRuntimeValue(hashCode);
            Debug.Assert(temp != null, $"Runtime data has no value with hash code '{hashCode}'");
            return temp.sValue;
        }

        #endregion
        
        #endregion
    }
}