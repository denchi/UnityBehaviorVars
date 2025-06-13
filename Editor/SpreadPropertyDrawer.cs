using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SpreadAttribute))]
public class SpreadDrawer : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // First get the attribute since it contains the range for the slider
        SpreadAttribute spread = attribute as SpreadAttribute;
        SerializedProperty propertySpread = property.serializedObject.FindProperty(spread.valueDeltaFieldName);

        if (propertySpread != null)
        {
            // Now draw the property as a Slider or an IntSlider based on whether it's a float or integer.
            if (property.propertyType == SerializedPropertyType.Float)
            {
                if (propertySpread.floatValue != 0)
                {
                    var rect = new Rect(position.x, position.y, position.width / 2, position.height);
                    EditorGUI.PrefixLabel(rect, label);

                    rect = new Rect(position.x + position.width / 2 + 6, position.y, position.width / 4 - 6, position.height);

                    EditorGUI.BeginChangeCheck();
                    var value = EditorGUI.FloatField(rect, property.floatValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        property.floatValue = value;
                        property.serializedObject.ApplyModifiedProperties();
                    }

                    rect = new Rect(position.x + position.width / 2 + 3 + position.width / 4, position.y, 10, position.height);
                    EditorGUI.PrefixLabel(rect, new GUIContent("±"));

                    rect = new Rect(position.x + position.width / 2 + 6 + position.width / 4 + 10, position.y, position.width / 4 - 12 - 5, position.height);

                    EditorGUI.BeginChangeCheck();
                    value = EditorGUI.FloatField(rect, propertySpread.floatValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        propertySpread.floatValue = Mathf.Abs(value);
                        propertySpread.serializedObject.ApplyModifiedProperties();
                    }
                }
                else
                {
                    var rect = new Rect(position.x, position.y, position.width / 2, position.height);
                    EditorGUI.PrefixLabel(rect, label);

                    rect = new Rect(position.x + position.width / 2 + 6, position.y, position.width / 4 - 6, position.height);

                    EditorGUI.BeginChangeCheck();
                    var value = EditorGUI.FloatField(rect, property.floatValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        property.floatValue = value;
                        property.serializedObject.ApplyModifiedProperties();
                    }

                    rect = new Rect(position.x + position.width / 2 + 6 + position.width / 4, position.y, position.width / 4 - 12 + 5, position.height);

                    EditorGUI.BeginChangeCheck();
                    var toggle = EditorGUI.ToggleLeft(rect, new GUIContent("Spread"), false);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (toggle)
                        {
                            propertySpread.floatValue = Mathf.Abs(property.floatValue) * 0.5f;
                            propertySpread.serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                if (propertySpread.intValue != 0)
                {
                    var rect = new Rect(position.x, position.y, position.width / 2, position.height);
                    EditorGUI.PrefixLabel(rect, label);

                    rect = new Rect(position.x + position.width / 2 + 6, position.y, position.width / 4 - 6, position.height);

                    EditorGUI.BeginChangeCheck();
                    var value = EditorGUI.IntField(rect, property.intValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        property.intValue = value;
                        property.serializedObject.ApplyModifiedProperties();
                    }

                    rect = new Rect(position.x + position.width / 2 + 3 + position.width / 4, position.y, 10, position.height);
                    EditorGUI.PrefixLabel(rect, new GUIContent("±"));

                    rect = new Rect(position.x + position.width / 2 + 6 + position.width / 4 + 10, position.y, position.width / 4 - 12 - 5, position.height);

                    EditorGUI.BeginChangeCheck();
                    value = EditorGUI.IntField(rect, propertySpread.intValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        propertySpread.intValue = Mathf.Abs(value);
                        propertySpread.serializedObject.ApplyModifiedProperties();
                    }
                }
                else
                {
                    var rect = new Rect(position.x, position.y, position.width / 2, position.height);
                    EditorGUI.PrefixLabel(rect, label);

                    rect = new Rect(position.x + position.width / 2 + 6, position.y, position.width / 4 - 6, position.height);

                    EditorGUI.BeginChangeCheck();
                    var value = EditorGUI.IntField(rect, property.intValue);
                    if (EditorGUI.EndChangeCheck())
                    {
                        property.intValue = value;
                        property.serializedObject.ApplyModifiedProperties();
                    }

                    rect = new Rect(position.x + position.width / 2 + 6 + position.width / 4, position.y, position.width / 4 - 12 + 5, position.height);

                    EditorGUI.BeginChangeCheck();
                    var toggle = EditorGUI.ToggleLeft(rect, new GUIContent("Spread"), false);
                    if (EditorGUI.EndChangeCheck())
                    {
                        if (toggle)
                        {
                            propertySpread.intValue = Mathf.RoundToInt(Mathf.Abs(property.intValue) * 0.5f);
                            propertySpread.serializedObject.ApplyModifiedProperties();
                        }
                    }
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, "Use Spread with float or int.");
            }
        }
        else
            EditorGUI.PropertyField(position, property, label);
    }
}