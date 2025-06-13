using UnityEngine;
using System.Collections.Generic;

using UnityEditor;
using Newtonsoft.Json.Linq;

namespace Behaviours
{
    public class ValueUtils
    {
        #region CREATE

        public static Condition createCondition()
        {
            Condition condition = Condition.CreateInstance<Condition>();
            {
                condition.name = "Condition";

                condition.operation = Operation.Is;
                condition.value = null;
                condition.hideFlags = HideFlags.HideInHierarchy;
            }            

            return condition;
        }

        public static Value createValue()
        {
            Value value = Value.CreateInstance<Value>();
            {
                value.name = "Value";
            }

            return value;
        }

        public static Value createValue(ILayer layer, string valueName, ValueType valueType)
        {
            Value value = Value.CreateInstance<Value>();
            {
                value.name = valueName;
                value.type = valueType;
            }

            if (layer)
            {
                layer.values.Add(value);

                EditorUtility.SetDirty(value);
                AssetDatabase.AddObjectToAsset(value, layer);
                EditorUtility.SetDirty(layer);
            }

            return value;
        }

        #endregion        

        #region REMOVE        

        public static void removeCondition(Condition condition)
        {
            if (condition)
            {
                Object.DestroyImmediate(condition, true);
            }
        }        

        public static void removeValue(Value value, ILayer layer)
        {
            if (value)
            {
                if (layer)
                {
                    layer.values.Remove(value);
                }

                removeValue(value);
            }
        }

        public static void removeValue(Value value)
        {
            if (value)
            {
                Object.DestroyImmediate(value, true);
            }
        }

        #endregion

        #region MISC

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        static T1 CreateAsset<T1>(string assetPath) where T1 : ScriptableObject
        {
            T1 asset = ScriptableObject.CreateInstance<T1>();

            string assetFileName = AssetDatabase.GenerateUniqueAssetPath(System.IO.Path.GetDirectoryName(assetPath) + "/" + System.IO.Path.GetFileNameWithoutExtension(assetPath) + ".asset");

            AssetDatabase.CreateAsset(asset, assetFileName);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            return asset;
        }

        public static void exportToJSON(ILayer vars, JArray ja)
        {
            for (int i = 0; i < vars.values.Count; ++i)
            {
                JObject jc = new JObject();
                {
                    jc["name"] = new JValue(vars.values[i].name);
                    jc["type"] = new JValue((int)vars.values[i].type);

                    if (vars.values[i].type == ValueType.Bool)
                    {
                        jc["value"] = new JValue(vars.values[i].bValue);
                    }
                    else if (vars.values[i].type == ValueType.Integer)
                    {
                        jc["value"] = new JValue(vars.values[i].iValue);
                    }
                    else if (vars.values[i].type == ValueType.Float)
                    {
                        jc["value"] = new JValue(vars.values[i].fValue);
                    }
                    else
                    {
                        jc["value"] = new JValue(vars.values[i].sValue);
                    }
                }
                ja.Add(jc);
            }
        }

        public static void importFromJSON(ILayer vars, JArray ja, bool add = true)
        {
            if (!add)
            {
                while (vars.values.Count > 0)
                {
                    removeValue(vars.values[0], vars);
                }
            }

            for (int i = 0; i < ja.Count; ++i)
            {
                JObject jc = ja[i].ToObject<JObject>();
                {
                    ValueType type = (ValueType)jc["type"].ToObject<int>();

                    Value v = vars.findValue(jc["name"].ToObject<string>());
                    if (v == null)
                    {
                        vars.values[i] = createValue(vars, jc["name"].ToObject<string>(), type);
                    }
                    else
                    {
                        v = vars.values[i];
                    }

                    if (type == ValueType.Bool)
                    {
                        vars.values[i].bValue = jc["value"].ToObject<bool>();
                    }
                    else if (type == ValueType.Integer)
                    {
                        vars.values[i].iValue = jc["value"].ToObject<int>();
                    }
                    else if (type == ValueType.Float)
                    {
                        vars.values[i].fValue = jc["value"].ToObject<float>();
                    }
                    else
                    {
                        vars.values[i].sValue = jc["value"].ToObject<string>();
                    }

                    EditorUtility.SetDirty(vars.values[i]);
                }
            }
        }

        public static List<Value> cloneValues(List<Value> values, ScriptableObject parentAsset)
        {
            var newValues = new List<Value>();
            foreach (var value in values)
            {
                var newValue = Object.Instantiate(value);
                newValues.Add(newValue);
                AssetDatabase.AddObjectToAsset(newValue, parentAsset);
            }
            return newValues;
        }

        #endregion
    }    

    public class ValueGUI
    {
        static Vector2 scrollPos = Vector2.zero;

        #region MISC
        static Color __BeginBackColor_Color;
        public static void BeginBackColor(Color c)
        {
            __BeginBackColor_Color = GUI.backgroundColor;
            GUI.backgroundColor = c;
        }

        public static void EndBackColor()
        {
            GUI.backgroundColor = __BeginBackColor_Color;
        }
        #endregion

        public static int selectedIndex = 0;

        public static void DrawLayerValues(string label, ILayer layer)
        {
            //EditorGUILayout.BeginHorizontal();
            //{
            //    GUILayout.Button("Add");
            //    GUILayout.Button("Rem");
            //    GUILayout.Button("Up");
            //    GUILayout.Button("Down");
            //}
            //EditorGUILayout.EndHorizontal();

            if (layer == null)
                return;

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.Foldout(true, label);

                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                {
                    GUILayout.Label("Name", GUILayout.Width(100));
                    GUILayout.Label("Type", GUILayout.Width(60));
                    GUILayout.Label("Value");
                    GUILayout.Label(" ", GUILayout.Width(20));
                }
                EditorGUILayout.EndHorizontal();

                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                {
                    int idxToDelete = -1;
                    for (int i = 0; i < layer.values.Count; ++i)
                    {
                        EditorGUILayout.BeginHorizontal();
                        {
                            BeginBackColor(Color.yellow);
                            {
                                // DRAW VALUE NAME
                                EditorGUI.BeginChangeCheck();
                                layer.values[i].name = GUILayout.TextField(layer.values[i].name, GUILayout.Width(100));
                                if (EditorGUI.EndChangeCheck())
                                {
                                    EditorUtility.SetDirty(layer.values[i]);
                                }
                            }
                            EndBackColor();

                            // DRAW VALUE TYPE
                            EditorGUI.BeginChangeCheck();
                            layer.values[i].type = (ValueType)EditorGUILayout.EnumPopup(layer.values[i].type, GUILayout.Width(60));
                            if (EditorGUI.EndChangeCheck())
                            {
                                EditorUtility.SetDirty(layer.values[i]);
                            }

                            EditorGUI.BeginChangeCheck();
                            {
                                switch (layer.values[i].type)
                                {
                                    case ValueType.Integer:
                                        {
                                            var e = ValueEnumAttribute.FindCachedAttributeForPath(layer.values[i].name);
                                            if (e == null)
                                            {
                                                layer.values[i].iValue = EditorGUILayout.IntField(layer.values[i].iValue);
                                            }
                                            else
                                            {
                                                var newIdx = EditorGUILayout.Popup(layer.values[i].iValue, e.names);
                                                layer.values[i].iValue = (int)e.values.GetValue(newIdx);
                                            }
                                        }
                                        break;

                                    case ValueType.Float:
                                        {
                                            layer.values[i].fValue = EditorGUILayout.FloatField(layer.values[i].fValue);
                                        }
                                        break;

                                    case ValueType.Bool:
                                        {
                                            //layer.values[i].bValue = EditorGUILayout.Toggle(layer.values[i].bValue);
                                            layer.values[i].bValue = EditorGUILayout.Popup(layer.values[i].bValue ? 1 : 0, new string[] { "False", "True" }) > 0;
                                        }
                                        break;

                                    case ValueType.String:
                                        {
                                            layer.values[i].sValue = GUILayout.TextField(layer.values[i].sValue);
                                        }
                                        break;
                                }
                            }
                            if (EditorGUI.EndChangeCheck())
                            {
                                EditorUtility.SetDirty(layer.values[i]);
                            }

                            if (GUILayout.Button("-", GUILayout.Width(20)))
                            {
                                idxToDelete = i;
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }

                    if (idxToDelete != -1)
                    {
                        ValueUtils.removeValue(layer.values[idxToDelete], layer);
                    }

                }
                EditorGUILayout.EndScrollView();

                if (GUILayout.Button("Add Variable"))
                    {
                        ValueUtils.createValue(layer, "value", ValueType.Bool);
                    }

                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Import..."))
                    {
                        string filePath = EditorUtility.OpenFilePanel("Enter the vars file...", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "json");
                        if (string.IsNullOrEmpty(filePath) == false)
                        {
                            string json = System.IO.File.ReadAllText(filePath);
                            JArray ja = JArray.Parse(json);
                            ValueUtils.importFromJSON(layer, ja, true);
                        }
                    }
                    if (GUILayout.Button("Export..."))
                    {
                        string filePath = EditorUtility.SaveFilePanel("Enter the vars file...", System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), "vars", "json");
                        if (string.IsNullOrEmpty(filePath) == false)
                        {
                            JArray ja = new JArray();
                            ValueUtils.exportToJSON(layer, ja);
                            string json = ja.ToString();
                            System.IO.File.WriteAllText(filePath, json);
                        }
                    }
                }
                EditorGUILayout.EndHorizontal();                    

                if (GUILayout.Button("Sort by Name"))
                {
                    layer.values.Sort((x, y) => { return x.name.CompareTo(y.name); });
                    EditorUtility.SetDirty(layer);
                }

                if (GUILayout.Button("Select Asset"))
                {
                    EditorGUIUtility.PingObject(layer);
                }

                GUILayout.Space(15);               
            }
            EditorGUILayout.EndVertical();
        }

        public static void DrawRuntimeValues(string label, Runtime.RuntimeValueCollection col)
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                GUILayout.Label(label);
                int idxToDelete = -1;
                for (int i = 0; i < col.Values.Count; ++i)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        BeginBackColor(Color.yellow);
                        {
                            GUI.enabled = false;
                            GUILayout.TextField(col.Values[i].name, GUILayout.Width(100));
                            GUI.enabled = true;
                        }
                        EndBackColor();

                        // DRAW VALUE TYPE
                        GUI.enabled = false;
                        EditorGUILayout.EnumPopup(col.Values[i].type, GUILayout.Width(80));
                        GUI.enabled = true;

                        switch (col.Values[i].type)
                        {
                            case ValueType.Integer:
                                {
                                    var e = ValueEnumAttribute.FindCachedAttributeForPath(col.Values[i].name);
                                    if (e == null)
                                    {
                                        col.Values[i].iValue = EditorGUILayout.IntField(col.Values[i].iValue);
                                    }
                                    else
                                    {
                                        var newIdx = EditorGUILayout.Popup(col.Values[i].iValue, e.names);
                                        col.Values[i].iValue = (int)e.values.GetValue(newIdx);
                                    }
                                    //col._values[i].iValue = EditorGUILayout.IntField(col._values[i].iValue);
                                }
                                break;

                            case ValueType.Float:
                                {
                                    col.Values[i].fValue = EditorGUILayout.FloatField(col.Values[i].fValue);
                                }
                                break;

                            case ValueType.Bool:
                                {
                                    col.Values[i].bValue = EditorGUILayout.Toggle(col.Values[i].bValue);
                                }
                                break;

                            case ValueType.String:
                                {
                                    col.Values[i].sValue = GUILayout.TextField(col.Values[i].sValue);
                                }
                                break;
                        }

                        GUI.enabled = false;                                                
                        if (GUILayout.Button("-", GUILayout.Width(20)))
                        {
                            idxToDelete = i;
                        }
                        GUI.enabled = true;
                    }
                    EditorGUILayout.EndHorizontal();
                }

                GUI.enabled = false;
                if (GUILayout.Button("Add Variable"))
                {
                    
                }

                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Import..."))
                {
                    
                }
                if (GUILayout.Button("Export..."))
                {
                    
                }
                EditorGUILayout.EndHorizontal();
                GUI.enabled = true;
            }
            EditorGUILayout.EndVertical();
        }

        public static void DrawConditionValues(ILayer layer, Condition condition)
        {
            List<string> values = new List<string>();
            foreach (Value val in layer.values)
            {
                values.Add(val.name);
            }

            BeginBackColor(Color.yellow);
            {
                EditorGUI.BeginChangeCheck();
                int newIndex = EditorGUILayout.Popup(layer.values.IndexOf(condition.value), values.ToArray(), GUILayout.Width(100));
                if (EditorGUI.EndChangeCheck())
                {
                    condition.value = layer.values[newIndex];
                    EditorUtility.SetDirty(condition);
                }
            }
            EndBackColor();

            EditorGUI.BeginChangeCheck();
            condition.operation = (Operation)EditorGUILayout.EnumPopup(condition.operation, GUILayout.Width(50));
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(condition);
            }

            if (condition.value)
            {
                EditorGUI.BeginChangeCheck();

                const int w = 40;
                switch (condition.value.type)
                {
                    case ValueType.Integer:
                        {
                            //condition.iConstant = EditorGUILayout.IntField(condition.iConstant, GUILayout.Width(w));

                            if (condition.value)
                            {
                                var e = ValueEnumAttribute.FindCachedAttributeForPath(condition.value.name);
                                if (e == null)
                                {
                                    condition.iConstant = EditorGUILayout.IntField(condition.iConstant);
                                }
                                else
                                {
                                    var newIdx = EditorGUILayout.Popup(condition.iConstant, e.names);
                                    condition.iConstant = (int)e.values.GetValue(newIdx);
                                }
                            }
                            else
                            {
                                condition.iConstant = EditorGUILayout.IntField(condition.iConstant);
                            }
                        }
                        break;

                    case ValueType.Float:
                        {
                            condition.fConstant = EditorGUILayout.FloatField(condition.fConstant, GUILayout.Width(w));
                        }
                        break;

                    case ValueType.Bool:
                        {
                            condition.bConstant = EditorGUILayout.Toggle(condition.bConstant, GUILayout.Width(w));
                        }
                        break;

                    case ValueType.String:
                        {
                            condition.sConstant = GUILayout.TextField(condition.sConstant, GUILayout.Width(w));
                        }
                        break;
                }

                if (EditorGUI.EndChangeCheck())
                {
                    EditorUtility.SetDirty(condition);
                }
            }
        }
    }
}