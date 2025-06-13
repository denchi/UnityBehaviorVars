using UnityEditor;
using UnityEngine;

public class EditorIcons
{
    private static GUIContent _iconDelete;
    private static GUIContent _iconPaste;
    private static GUIContent _iconCopy;
    private static GUIContent _iconAdd;
    private static GUIContent _iconDublicate;
    private static GUIContent _iconPlay;
    private static GUIContent _iconNext;
    private static GUIContent _iconPrev;

    //

    public static GUIContent IconDelete => _iconDelete == null ? _iconDelete = EditorGUIUtility.IconContent("TreeEditor.Trash") : _iconDelete;
    public static GUIContent IconAdd  => _iconAdd == null ? _iconAdd = EditorGUIUtility.IconContent("d_P4_AddedLocal") : _iconAdd;
    public static GUIContent IconDublicate  => _iconDublicate == null ? _iconDublicate = EditorGUIUtility.IconContent("d_TreeEditor.Duplicate") : _iconDublicate;
    
    public static GUIContent IconPaste  => _iconPaste == null ? _iconPaste = EditorGUIUtility.IconContent("Clipboard") : _iconPaste;
    public static GUIContent IconCopy  => _iconCopy == null ? _iconCopy = EditorGUIUtility.IconContent("Clipboard") : _iconCopy;

    public static GUIContent IconPlay  => _iconPlay == null ? _iconPlay = EditorGUIUtility.IconContent("Animation.Play") : _iconPlay;

    public static GUIContent IconPrev  => _iconPrev == null ? _iconPrev = EditorGUIUtility.IconContent("tab_prev") : _iconPrev;
    public static GUIContent IconNext  => _iconNext == null ? _iconNext = EditorGUIUtility.IconContent("d_tab_next") : _iconNext;
}
