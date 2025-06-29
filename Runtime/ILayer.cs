using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace Behaviours
{
    public class ILayer : ScriptableObject
    {        
        public bool enabled;

        #if UNITY_EDITOR
        // Odin attributes removed, no effect
        #endif
        public List<Value> values;

        [HideInInspector]
        [System.NonSerialized]
        public List<Trigger> triggers;

        public class Trigger
        {
            public Value value;
            public string name;
            public int frames;
        }

        [System.NonSerialized]
        public GameObject gameObject;

        [System.NonSerialized]
        public GameObject _editorOnlyGameObject;

        public virtual void OnEnable()
        {
            if (values == null)
            {
                values = new List<Value>();
            }

            triggers = new List<Trigger>();
        }

        public virtual void init(GameObject gameObject)
        {
            this.gameObject = gameObject;
        }

        public virtual void start()
        {
            
        }

        public virtual void update(float dt)
        {
            if (triggers.Count > 0)
                updateTriggers();
        }

        public virtual void setEnabled(bool enable)
        {
            enabled = enable;
        }

        #if UNITY_EDITOR

        private void AddValue()
        {
            Value value = CreateInstance<Value>();
            {
                value.name = "bool_value";
                value.type = ValueType.Bool;
            }

            values.Add(value);
            
            EditorUtility.SetDirty(value);
            AssetDatabase.AddObjectToAsset(value, this);
            EditorUtility.SetDirty(this);
        }
        
        private void RemoveValue(int index)
        {
            var value = values[index];
            values.RemoveAt(index);
            
            DestroyImmediate(value, true);
            
            EditorUtility.SetDirty(this);
        }
        
        #endif
        
        #region VALUES

        public void setTrigger(string triggerName, int frames)
        {
            Trigger trigger = triggers.Find(x => x.name == triggerName);
            if (trigger == null)
            {
                trigger = new Trigger();

                trigger.name = triggerName;
                trigger.value = findValue(triggerName);

                if (trigger.value == null)
                {
                    return;
                }

                trigger.frames = frames;
                trigger.value.bValue = true;
                triggers.Add(trigger);
            }
            else
            {
                trigger.frames = frames;
                trigger.value.bValue = true;
            }
        }

        void updateTriggers()
        {
            List<Trigger> triggersToDelete = new List<Trigger>();
            foreach (Trigger trigger in triggers)
            {
                if (trigger.frames < 0)
                {
                    trigger.value.bValue = false;
                    triggersToDelete.Add(trigger);
                }
                else
                {
                    trigger.frames--;
                }
            }

            triggersToDelete.ForEach(delegate (Trigger trigger) { triggers.Remove(trigger); });
        }

        public Value findValue(string valueName)
        {
            return values.Find(x => x.name == valueName);
        }

        public float getFloat(string aname, float fallbackValue = 0)
        {
            Value v = findValue(aname);
            if (v)
            {
                return v.fValue;
            }
            else
            {
                return fallbackValue;
            }
        }

        public void setFloat(string aname, float value)
        {
            Value v = findValue(aname);
            if (v)
            {
                v.fValue = value;
            }
        }

        public int getInt(string aname, int fallbackValue = 0)
        {
            Value v = findValue(aname);
            if (v)
            {
                return v.iValue;
            }
            else
            {
                return fallbackValue;
            }
        }

        public void setInt(string aname, int value)
        {
            Value v = findValue(aname);
            if (v)
            {
                v.iValue = value;
            }
        }

        public bool getBool(string aname, bool fallbackValue = false)
        {
            Value v = findValue(aname);
            if (v)
            {
                return v.bValue;
            }
            else
            {
                return fallbackValue;
            }
        }

        public void setBool(string aname, bool value)
        {
            Value v = findValue(aname);
            if (v)
            {
                v.bValue = value;
            }
        }

        public string getString(string aname, string fallbackValue = "")
        {
            Value v = findValue(aname);
            if (v)
            {
                return v.sValue;
            }
            else
            {
                return fallbackValue;
            }
        }

        public void setString(string aname, string value)
        {
            Value v = findValue(aname);
            if (v)
            {
                v.sValue = value;
            }
        }

        #endregion

        #region SERIALIZATION

        public virtual void Write(BinaryWriter writer)
        {
            writer.Write(this.name);
            writer.Write(this.enabled);

            writer.Write(values.Count);
            foreach (var value in values)
            {
                value.Write(writer);
            }
        }

        #endregion
    }
}