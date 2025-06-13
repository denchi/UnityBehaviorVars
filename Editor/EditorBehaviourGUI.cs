using UnityEngine;
using System.Collections.Generic;

using UnityEditor;

using System;
//using Labyrinth;
//using CharacterController = Labyrinth.CharacterController;

public class EditorBehaviourGUI
{
    #region STYLES

    static GUIStyle _styleHeader;
    public static GUIStyle labelHeader
    {
        get
        {
            if (_styleHeader == null)
            {
                _styleHeader = new GUIStyle(GUI.skin.label);
                {
                    _styleHeader.alignment = TextAnchor.MiddleLeft;
                    _styleHeader.fontStyle = FontStyle.Bold;
                    _styleHeader.fontSize = 30;
                    _styleHeader.normal.textColor = Color.gray;
                }
            }

            return _styleHeader;
        }
    }

    static GUIStyle _styleBack;
    public static GUIStyle buttonBack
    {
        get
        {
            if (_styleBack == null)
            {
                _styleBack = new GUIStyle(GUI.skin.button);
                {
                    _styleBack.imagePosition = ImagePosition.ImageOnly;
                    _styleBack.normal.background = null;
                    _styleBack.active.background = null;
                    _styleBack.focused.background = null;
                    _styleBack.hover.background = null;
                }
            }

            return _styleBack;
        }
    }

    static GUIStyle _styleTitle;
    public static GUIStyle labelStateTitle
    {
        get
        {
            if (_styleTitle == null)
            {
                _styleTitle = new GUIStyle(GUI.skin.label);
                {
                    _styleTitle.alignment = TextAnchor.UpperLeft;
                    _styleTitle.fontStyle = FontStyle.Bold;
                    _styleTitle.margin = new RectOffset(5, 10, 10, 10);
                    _styleTitle.imagePosition = ImagePosition.ImageLeft;
                }
            }

            return _styleTitle;
        }
    }

    static GUIStyle _styleScript;
    public static GUIStyle labelStateScript
    {
        get
        {
            if (_styleScript == null)
            {
                _styleScript = new GUIStyle(GUI.skin.label);
                {
                    _styleScript.alignment = TextAnchor.MiddleCenter;
                    _styleScript.fontStyle = FontStyle.Normal;
                }
            }

            return _styleScript;
        }
    }

    static GUIStyle _styleAttributeLeft;
    public static GUIStyle labelAttributeLeft
    {
        get
        {
            if (_styleAttributeLeft == null)
            {
                _styleAttributeLeft = new GUIStyle(GUI.skin.label);
                {
                    _styleAttributeLeft.alignment = TextAnchor.UpperLeft;
                    _styleAttributeLeft.fontStyle = FontStyle.Bold;
                }
            }

            return _styleAttributeLeft;
        }
    }

    static GUIStyle _styleAttributeRight;
    public static GUIStyle labelAttributeRight
    {
        get
        {
            if (_styleAttributeRight == null)
            {
                _styleAttributeRight = new GUIStyle(GUI.skin.label);
                {
                    _styleAttributeRight.alignment = TextAnchor.UpperRight;
                    _styleAttributeRight.fontStyle = FontStyle.Bold;
                }
            }

            return _styleAttributeRight;
        }
    }

    static GUIStyle _styleParam;
    public static GUIStyle labelStateParam
    {
        get
        {
            if (_styleParam == null)
            {
                _styleParam = new GUIStyle(GUI.skin.label);
                {
                    _styleParam.alignment = TextAnchor.MiddleRight;
                    _styleParam.fontStyle = FontStyle.Bold;
                }
            }

            return _styleParam;
        }
    }

    static GUIStyle _styleBox;
    public static GUIStyle boxState
    {
        get
        {
            //if (_styleBox == null)
            {
                _styleBox = new GUIStyle(GUI.skin.box);
                {
                    _styleBox.richText = true;
                    _styleBox.alignment = TextAnchor.UpperLeft;
                    _styleBox.fontStyle = FontStyle.Bold;
                    _styleBox.border = new RectOffset(7, 7, 40, 11);
                    _styleBox.imagePosition = ImagePosition.ImageLeft;
                    //_styleBox.normal.background = getTexture("text-back");
                    //_styleBox.normal.background = Texture2D.linearGrayTexture;
                    _styleBox.normal.background = Texture2D.whiteTexture;
                }
            }

            return _styleBox;
        }
    }

    static GUIStyle _styleNode;
    public static GUIStyle boxNode
    {
        get
        {
            if (_styleNode == null)
            {
                _styleNode = new GUIStyle(boxNodeBack);
                {
                    
                }
            }

            return _styleNode;
        }
    }

    static GUIStyle _styleNodeBack;
    public static GUIStyle boxNodeBack
    {
        get
        {
            if (_styleNodeBack == null)
            {
                _styleNodeBack = new GUIStyle(GUI.skin.box);
                {
                    _styleNodeBack.richText = true;
                    _styleNodeBack.alignment = TextAnchor.MiddleCenter;
                    _styleNodeBack.fontStyle = FontStyle.Bold;
                    _styleNodeBack.border = new RectOffset(7, 7, 40, 11);
                    _styleNodeBack.imagePosition = ImagePosition.ImageLeft;
                    _styleNodeBack.normal.background = Texture2D.whiteTexture;
                    _styleNodeBack.normal.textColor = Color.white;
                    _styleNodeBack.margin = new RectOffset(10, 10, 0, 0);
                }
            }

            return _styleNodeBack;
        }
    }
    
    static GUIStyle _styleNodeBackNoMargin;
    public static GUIStyle BoxNodeBackNoMargin
    {
        get
        {
            if (_styleNodeBackNoMargin == null)
            {
                _styleNodeBackNoMargin = new GUIStyle(boxNodeBack);
                {
                    _styleNodeBackNoMargin.margin = new RectOffset(0, 0, 0, 0);
                    _styleNodeBackNoMargin.padding = new RectOffset(0, 0, 0, 0);
                }
            }

            return _styleNodeBackNoMargin;
        }
    }

    static GUIStyle _styleBoxEmpty;
    public static GUIStyle boxEmptyState
    {
        get
        {
            if (_styleBoxEmpty == null)
            {
                _styleBoxEmpty = new GUIStyle(GUI.skin.box);
                {
                    _styleBoxEmpty.alignment = TextAnchor.MiddleCenter;
                    _styleBoxEmpty.fontStyle = FontStyle.Bold;
                    _styleBoxEmpty.border = new RectOffset(7, 11, 7, 11);
                    //_styleBoxEmpty.normal.background = getTexture("text-back-empty");
                    _styleBoxEmpty.normal.background = Texture2D.whiteTexture;
                }
            }

            return _styleBoxEmpty;
        }
    }

    static GUIStyle _styleLabelLeft;
    public static GUIStyle labelLeft
    {
        get
        {
            if (_styleLabelLeft == null)
            {
                _styleLabelLeft = new GUIStyle(GUI.skin.label);
                {
                    _styleLabelLeft.alignment = TextAnchor.MiddleLeft;
                }
            }

            return _styleLabelLeft;
        }
    }

    static GUIStyle _styleBoxGroup;
    public static GUIStyle boxGroupState
    {
        get
        {
            if (_styleBoxGroup == null)
            {
                _styleBoxGroup = new GUIStyle(GUI.skin.box);
                {
                    _styleBoxGroup.alignment = TextAnchor.MiddleCenter;
                    _styleBoxGroup.fontStyle = FontStyle.Bold;
                    _styleBoxGroup.border = new RectOffset(7 + 8, 11 + 8, 7 + 8, 11 + 8);
                    _styleBoxGroup.normal.background = getTexture("text-back-group");
                }
            }

            return _styleBoxGroup;
        }
    }

    #endregion

    #region RESOURCES

    public static Texture2D getTexture(string assetName)
    {
        if (textures.ContainsKey(assetName))
        {
            return textures[assetName];
        }
        return Texture2D.whiteTexture;
    }

    public static Texture2D GetTextureByFSMStateType(Type type)
    {
        string icon = "script";

        return getTexture(icon);
    }

    // public static Texture2D getTaskTexture(Type type)
    // {
    //     string icon = "script";
    //
    //     if (type == typeof(Behaviours.BT.Selector))
    //         icon = "question";
    //
    //     if (type == typeof(Behaviours.BT.Sequence))
    //         icon = "arrow";
    //
    //     return getTexture(icon);
    // }

    static Dictionary<string, Texture2D> _textures;
    public static Dictionary<string, Texture2D> textures
    {
        get
        {
            if (_textures == null)
            {
                //Debug.LogFormat("Populating editor textures...");

                _textures = new Dictionary<string, Texture2D>();

                string[] guids = AssetDatabase.FindAssets("gear-icon-1833 t:Texture");
                if (guids.Length>0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    string dir = System.IO.Path.GetDirectoryName(path);
                     
                    string[] files = System.IO.Directory.GetFileSystemEntries(dir + "/", "*.png");
                    foreach (string filePath in files)
                    {
                        string assetPath = System.IO.Path.GetFileNameWithoutExtension(filePath);

                        //Debug.LogFormat("Loading '{0}' from '{1}'...", assetPath, filePath);
                        var texture = UnityEditor.AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
                        _textures[assetPath] = texture;                        
                    }
                }                 
            }
            return _textures;
        }
    }

    #endregion

    static Color _savedBackgroundColor;
    public static void BeginTint(Color color)
    {
        _savedBackgroundColor = GUI.backgroundColor;
        GUI.backgroundColor = color;
    }
    public static void EndTint()
    {
        GUI.backgroundColor = _savedBackgroundColor;
    }
    
    static Color _savedColor;
    public static void BeginColor(Color color)
    {
        _savedColor = GUI.color;
        GUI.color = color;
    }
    public static void EndColor()
    {
        GUI.color = _savedColor;
    }

    static bool _savedEnabled;
    public static void BeginEnable(bool enable)
    {
        _savedEnabled = GUI.enabled;
        GUI.enabled = enable;
    }
    public static void EndEnable()
    {
        GUI.enabled = _savedEnabled;
    }

    const float EPSILON = 0.001f;

    static bool IsPointOnLine(Vector2 linePointA, Vector2 linePointB, Vector2 point)
    {
        float a = (linePointB.y - linePointA.y) / (linePointB.x - linePointB.x);
        float b = linePointA.y - a * linePointA.x;
        if (Mathf.Abs(point.y - (a * point.x + b)) < EPSILON)
        {
            return true;
        }

        return false;
    }    

    #region DRAWING

    public static Color colorStateDefault = new Color(1.0f, 0.5f, 0.25f, 1);
    public static Color colorStateAny = new Color(0.5f, 0.25f, 0.5f, 1);
    public static Color colorStateExit = Color.cyan;
    public static Color colorGroupNormal = Color.green;
    public static Color colorStateNormal = new Color(0.8f, 0.8f, 0.8f, 1);    

    // public static Color getNodeBackgroundColor(Behaviours.HFSM.Node node, bool running, bool selected)
    // {
    //     Color color = colorStateNormal;
    //
    //     if (node)
    //     {
    //         if (node.state is Behaviours.HFSM.IComposedState)
    //         {
    //             color = colorGroupNormal;
    //         }
    //
    //         if (node.parent)
    //         {
    //             Behaviours.HFSM.IComposedState parent = node.parent.state as Behaviours.HFSM.IComposedState;
    //             if (parent)
    //             {
    //                 int idx = parent.FindIdxByNode(node);
    //
    //                 if (idx == parent.anyNodeIndex)
    //                 {
    //                     color = colorStateAny;
    //                 }
    //
    //                 if (idx == parent.defaultNodeIndex)
    //                 {
    //                     color = colorStateDefault;
    //                 }
    //
    //                 if (idx == parent.exitNodeIndex)
    //                 {
    //                     color = colorStateExit;
    //                 }
    //
    //                 if (running)
    //                     color = new Color(1, 0, 0, 1);
    //             }
    //         }
    //     }
    //
    //     if (selected)
    //     {
    //         color += new Color(0.5f, 0.5f, 0.5f, 0);
    //     }
    //
    //     return color;
    // }

    public static void FillRectangle(Rect rect, Color color)
    {
        Color c = GUI.color;
        GUI.color = color;
        GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width, rect.height), Texture2D.whiteTexture);
        GUI.color = c;
    }

    public static void DrawCurve(Vector2 start, Vector2 end, Color color, float width, float multiplier = 1)
    {
        Vector3 startPos = new Vector3(start.x, start.y, 0);
        Vector3 endPos = new Vector3(end.x, end.y, 0);

        Color shadowCol = new Color(0, 0, 0, 0.06f);

        Vector3 startTan = startPos + Vector3.right * Mathf.Abs(endPos.x - startPos.x) / 2 * multiplier;
        Vector3 endTan = endPos + Vector3.left * Mathf.Abs(endPos.x - startPos.x) / 2 * multiplier;

        if (endPos.y > startPos.y)
        {
            startTan = startPos + Vector3.up * Mathf.Abs(startPos.y - endPos.y) / 1 * multiplier;
            endTan = endPos + Vector3.down * Mathf.Abs(startPos.y - endPos.y) / 1 * multiplier;
        }

        for (int i = 0; i < 3; i++)
        {
            // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (width + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, width);
    }
    
    public static (Vector3 position, Vector3 direction) CalculateBezierPointAndDirection(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // Calculate the position on the curve
        Vector3 position = CalculateBezierPoint(t, p0, p1, p2, p3);

        // Calculate the derivative (tangent) at t
        Vector3 derivative = CalculateBezierTangent(t, p0, p1, p2, p3);

        // Normalize the derivative to get the direction
        Vector3 direction = derivative.normalized;

        return (position, direction);
    }

    public static Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        // The formula for a cubic Bezier curve
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        // Calculate point along Bezier curve
        Vector3 point = uuu * p0; // first term
        point += 3 * uu * t * p1; // second term
        point += 3 * u * tt * p2; // third term
        point += ttt * p3; // fourth term

        return point;
    }

    public static Vector3 CalculateBezierTangent(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        // The derivative of the cubic Bezier curve formula
        Vector3 tangent = 3 * uu * (p1 - p0) + 6 * u * t * (p2 - p1) + 3 * tt * (p3 - p2);

        return tangent;
    }

    public static void DrawCurveVertical(Vector2 start, Vector2 end, Color color, float width, float multiplier = 1)
    {
        Vector3 startPos = new Vector3(start.x, start.y, 0);
        Vector3 endPos = new Vector3(end.x, end.y, 0);

        Color shadowCol = new Color(0, 0, 0, 0.06f);

        float d = (endPos.y - startPos.y) / 3 * multiplier;

        Vector3 startTan = startPos + Vector3.up * d;
        Vector3 endTan = endPos + Vector3.down * d;

        for (int i = 0; i < 3; i++)
        {
            // Draw a shadow
            Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (width + 1) * 5);
        }

        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, width);
    }

    public static void DrawLine(Vector2 start, Vector2 end, Color color, float width)
    {
        //DrawCurve(start, end, color, width);

        Vector3 startPos = new Vector3(start.x, start.y, 0);
        Vector3 endPos = new Vector3(end.x, end.y, 0);

        Vector3 startTan = startPos + (startPos - endPos).normalized;
        Vector3 endTan = endPos + (endPos - startPos).normalized;

        //Color shadowCol = new Color(0, 0, 0, 0.06f);

        //for (int i = 0; i < 3; i++)
        //{
        //    // Draw a shadow
        //    Handles.DrawBezier(startPos, endPos, startTan, endTan, shadowCol, null, (width + 1) * 5);
        //}

        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, width);
    }

    public static void DrawArrowOnly(Vector2 start, Vector2 end, Color color, float width, int offset = 0, float multiplier = 1)
    {
        // Draws an arrow at 'start', pointing in the direction of (end - start)
        Vector2 dir = (end - start).normalized;
        float angleInDegrees = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float size = 8f; // was 12f, now 2x smaller

        // Center the arrow at 'start'
        Vector2 pivot = start;
        Vector2 topLeft = pivot - new Vector2(size, size) / 2f;

        Matrix4x4 matrixBackup = GUI.matrix;
        Color colorBackup = GUI.color;

        var texture = getTexture("arrow2");
        GUIUtility.RotateAroundPivot(angleInDegrees, pivot);
        GUI.color = color;
        GUI.DrawTexture(new Rect(topLeft, Vector2.one * size), texture);

        GUI.matrix = matrixBackup;
        GUI.color = colorBackup;
    }

    public static void DrawLineNoShadow(Vector2 start, Vector2 end, Color color, float width)
    {
        Vector3 startPos = new Vector3(start.x, start.y, 0);
        Vector3 endPos = new Vector3(end.x, end.y, 0);

        Vector3 startTan = startPos + (startPos - endPos).normalized;
        Vector3 endTan = endPos + (endPos - startPos).normalized;

        Handles.DrawBezier(startPos, endPos, startTan, endTan, color, null, width);
    }

    public static void DrawArrow(Vector2 p, float size, Color c, float width, float angle, float angle2 = 15)
    {
        Vector2 p1 = new Vector2(Mathf.Cos(angle + angle2), Mathf.Sin(angle + angle2)) * size;
        Vector2 p2 = new Vector2(Mathf.Cos(angle - angle2), Mathf.Sin(angle - angle2)) * size;

        DrawLineNoShadow(p, p - p1, c, width);
        DrawLineNoShadow(p, p - p2, c, width);
    }

    #endregion

    #region EDITOR

    public static void ValuesField(string label, ref List<Behaviours.Value> values, ScriptableObject target)
    {
        EditorGUILayout.BeginVertical(GUI.skin.box);
        {
            GUILayout.Label(label);

            int idxToDelete = -1;
            for (int i = 0; i < values.Count; ++i)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    Color bkcolor = GUI.backgroundColor;
                    GUI.backgroundColor = Color.yellow;

                    string oldName = values[i].name;
                    values[i].name = GUILayout.TextField(values[i].name, GUILayout.Width(100));
                    if (oldName != values[i].name)
                    {
                        EditorUtility.SetDirty(values[i]);
                    }

                    GUI.backgroundColor = bkcolor;

                    Behaviours.ValueType oldType = values[i].type;
                    values[i].type = (Behaviours.ValueType)EditorGUILayout.EnumPopup(values[i].type, GUILayout.Width(80));
                    if (oldType != values[i].type)
                    {
                        EditorUtility.SetDirty(values[i]);
                    }

                    switch (values[i].type)
                    {
                        case Behaviours.ValueType.Integer:
                            {
                                int oldValue = values[i].iValue;
                                values[i].iValue = EditorGUILayout.IntField(values[i].iValue);
                                if (oldValue != values[i].iValue)
                                {
                                    EditorUtility.SetDirty(values[i]);
                                }
                            }
                            break;

                        case Behaviours.ValueType.Float:
                            {
                                float oldValue = values[i].fValue;
                                values[i].fValue = EditorGUILayout.FloatField(values[i].fValue);
                                if (oldValue != values[i].fValue)
                                {
                                    EditorUtility.SetDirty(values[i]);
                                }
                            }
                            break;

                        case Behaviours.ValueType.Bool:
                            {
                                bool oldValue = values[i].bValue;
                                values[i].bValue = EditorGUILayout.Toggle(values[i].bValue);
                                if (oldValue != values[i].bValue)
                                {
                                    EditorUtility.SetDirty(values[i]);
                                }
                            }
                            break;

                        case Behaviours.ValueType.String:
                            {
                                string oldValue = values[i].sValue;
                                values[i].sValue = GUILayout.TextField(values[i].sValue);
                                if (oldValue != values[i].sValue)
                                {
                                    EditorUtility.SetDirty(values[i]);
                                }
                            }
                            break;                        
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
                Behaviours.Value v = values[idxToDelete];
                values.RemoveAt(idxToDelete);
                ScriptableObject.DestroyImmediate(v, true);

                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Add Variable"))
            {
                Behaviours.Value v = Behaviours.Value.CreateInstance<Behaviours.Value>();
                {
                    v.type = Behaviours.ValueType.Bool;
                    v.name = "New value";
                    v.hideFlags = HideFlags.HideInHierarchy;

                    v.bValue = false;

                    values.Add(v);
                }

                UnityEditor.EditorUtility.SetDirty(v);

                UnityEditor.AssetDatabase.AddObjectToAsset(v, target);

                UnityEditor.EditorUtility.SetDirty(target);
            }
        }
        EditorGUILayout.EndVertical();
    }

    #endregion
}