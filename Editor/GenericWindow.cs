using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

public class GenericWindow<T> : EditorWindow where T: UnityEngine.Object, new()
{
    public Vector2 offset;
    public float scale = 1;
    public bool windowExpanded = true;


    [System.NonSerialized]
    public int windowWidth = 350;

    #region NONSERIALIZED

    [System.NonSerialized]
    public Texture2D windowIconTexture;

    [System.NonSerialized]
    public Rect windowRect;

    #endregion    

    #region MISC

    public static T1 CreateAsset<T1>(string assetPath) where T1 : ScriptableObject
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

    public void drawBackground(string header, Rect position, ref Vector2 offset)
    {
        Vector2 p = new Vector2(offset.x / 192, -offset.y / 192);
        Vector2 s = position.size / 192;

        var c = GUI.color;
        float k = 0.5f;
        GUI.color = new Color(k, k, k, 1);
        GUI.DrawTextureWithTexCoords(
            new Rect(0, 0, position.width, position.height), 
            EditorBehaviourGUI.getTexture("grid"), 
            //new Rect(
            //    new Vector2(p.x / scale, (p.y - position.size.y) / scale),
            //    new Vector2(s.x / scale, s.y / scale)
            //),
            new Rect(
                new Vector2(p.x, p.y),
                new Vector2(s.x, s.y)
            ),
            true
        );
        GUI.color = c;

        GUI.Label(new Rect(20, 20, position.width - 40, 50), GetViewTitle(), EditorBehaviourGUI.labelHeader);
    }

    public void drawStatusBar()
    {
        Rect statusBarRect = new Rect(0, position.size.y - 20, position.size.x, 20);
        EditorBehaviourGUI.FillRectangle(statusBarRect, Color.gray);
        GUI.Label(new Rect(statusBarRect.position.x + 2, statusBarRect.position.y + 2, 100, statusBarRect.size.y - 4), string.Format("{0}:{1}", offset.x, offset.y));        
        GUI.Label(new Rect(statusBarRect.position.x + 2 + 100 + 2, statusBarRect.position.y + 2, 150, statusBarRect.size.y - 4), string.Format("{0} object(s) selected", _selectedItems.Count));

        // GUI.Label(new Rect(statusBarRect.position.x + 2 + 100 + 2 + 150 + 2, statusBarRect.position.y + 2, 100, statusBarRect.size.y - 4), string.Format("{0}:{1}", Event.current.mousePosition.x + offset.x, Event.current.mousePosition.y + offset.y));
    }

    public void processCanvasInput(Event evt)
    {
        if (evt.type == EventType.MouseDown)
        {            
            Vector2 mp = evt.mousePosition;
            //Vector2 mp = GUIUtility.ScreenToGUIPoint(evt.mousePosition);
            //Vector2 mp = evt.mousePosition - position.position;
            //Debug.LogFormat("MOUSE: {0}", mp);
            OnCanvasMouseDown(evt);            
        }

        if (_mouseDownIndex != -1 || evt.button == 2)
        {
            if (evt.type == EventType.MouseDrag)
            {
                OnCanvasMouseDragged(evt);
            }
        }

        if (evt.type == EventType.ContextClick || evt.type == EventType.MouseUp && evt.button == 1)
        {
            OnContextMouseUp(evt);
        }

        if (evt.type == EventType.MouseUp)
        {
            OnCanvasMouseUp(evt);
        }

        if (evt.type == EventType.KeyUp)
        {
            if (evt.keyCode == KeyCode.Home)
            {
                scale = 1;
                offset = Vector2.zero;

                evt.Use();
            }
        }
    }

    public void DrawPropertiesWindow(int wndId)
    {
        float buttonHeight = 15;
        float buttonWidth = windowRect.width - 10;
        float buttonMargin = 5;

        // Draw opaque background for the properties window
        var rectangleColor = new Color(0.2f, 0.2f, 0.2f, 1);
        EditorBehaviourGUI.FillRectangle(new Rect(0, 0, windowRect.width, windowRect.height), rectangleColor);

        if (windowExpanded)
        {
            DoPropertiesWindowGUI(wndId);

            if (GUI.Button(new Rect((windowRect.width - buttonWidth) / 2, windowRect.height - buttonMargin - buttonHeight, buttonWidth, buttonHeight), "Hide"))
            {
                windowExpanded = false;
            }
        }
        else
        {
            if (GUI.Button(new Rect((windowRect.width - buttonWidth) / 2, windowRect.height - buttonMargin - buttonHeight, buttonWidth, buttonHeight), "Show"))
            {
                windowExpanded = true;
            }
        }
    }

    public virtual void DoPropertiesWindowGUI(int wndId)
    {

    }

    public int FindItemIndexAtPosition(Vector2 p)
    {
        int objectsCount = GetItemsCount();
        for (int i = objectsCount-1; i>= 0; --i )
        {
            Rect r = GetItemRect(i);
            if (r.Contains(p))
                return i;
        }
        return -1;
    }

    public void MoveObjectsRelative(Vector2 delta, params int[] indices)
    {
        int objectsCount = indices.Length;
        for (int i = objectsCount - 1; i >= 0; --i)
        {
            Rect r = GetItemRect(indices[i]);

            r.position += delta;

            if (Event.current.modifiers == EventModifiers.Shift)
            {
                r.position = new Vector2(Mathf.RoundToInt(r.position.x / 10) * 10, Mathf.RoundToInt(r.position.y / 10) * 10);
            }

            SetItemRect(indices[i], r);
        }
        Repaint();
    }

    public RectOffset rectOffset;

    public virtual Rect CalcClientRect()
    {
        float xmin = float.PositiveInfinity;
        float xmax = float.NegativeInfinity;
        float ymin = float.PositiveInfinity;
        float ymax = float.NegativeInfinity;

        int cnt = GetItemsCount();

        for (int i = 0; i < cnt; ++i)
        {
            Rect r = GetItemRect(i);
            if (r.xMin < xmin)
            {
                xmin = r.xMin;
            }
            if (r.xMax > xmax)
            {
                xmax = r.xMax;
            }
            if (r.yMin < ymin)
            {
                ymin = r.yMin;
            }
            if (r.yMax > ymax)
            {
                ymax = r.yMax;
            }
        }

        return Rect.MinMaxRect(xmin - rectOffset.left, ymin - rectOffset.top, xmax + rectOffset.right, ymax + rectOffset.bottom);
    }

    #endregion

    #region ITEMS

    public virtual int GetItemsCount()
    {
        return 0;
    }

    public virtual Rect GetItemRect(int index)
    {
        return new Rect();
    }

    public virtual void SetItemRect(int index, Rect newRect)
    {
        
    }

    public virtual string GetItemName(int index)
    {
        T item = GetItem(index);
        if (item)
        {
            return item.name;
        }
        else
        {
            return "ERROR!";
        }
    }

    public virtual void DrawItem(int index)
    {

    }

    public virtual T GetItem(int index)
    {
        return null;
    }

    public virtual int AddEmptyItem(Vector2 position)
    {
        return -1;
    }

    public virtual void RemoveItemAtIndex(int itemIndex)
    {
        removeSelectedItem(itemIndex);
    }

    public virtual string GetViewTitle()
    {
        return "Dummy View";
    }

    public virtual string GetWindowTitle()
    {
        return "Dummy Window";
    }

    #endregion    

    #region SELECTION

    public void setSelectedItem(params int[] items)
    {
        _selectedItems = new List<int>();
        if (items != null)
        {            
            addSelectedItem(items);
        }
        else
        {
            Repaint();
        }
    }

    public void addSelectedItem(params int[] items)
    {
        if (items != null)
        {
            for (int i = 0; i < items.Length; ++i)
            {
                if (items[i] != -1 && _selectedItems.Contains(items[i]) == false)                    
                    _selectedItems.Add(items[i]);
            }
        }

        Repaint();
    }

    public void clearSelection()
    {
        setSelectedItem(null);
    }

    public List<int> getSelectedItems()
    {
        return _selectedItems;
    }

    public bool isSelected(int index)
    {
        return _selectedItems.Contains(index);
    }

    public void removeSelectedItem(int index)
    {
        _selectedItems.Remove(index);
        Repaint();
    }

    List<int> _selectedItems = new List<int>();

    #endregion

    #region UNITY

    public virtual void OnEnable()
    {
        if (windowIconTexture == null)
        {
            if (System.IO.File.Exists(Application.dataPath + "\\Gizmos\\Behaviour Icon.png"))
            {
                windowIconTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Gizmos/Behaviour Icon.png");
            }
        }

        titleContent = new GUIContent(GetWindowTitle(), windowIconTexture);

        rectOffset = new RectOffset(500, 500, 500, 500);

        EditorApplication.playmodeStateChanged += _OnPlayModeChanged;
    }

    public virtual void OnDisable()
    {
        EditorApplication.playmodeStateChanged -= _OnPlayModeChanged;
    }

    void _OnPlayModeChanged()
    {
        OnPlayModeChanged(EditorApplication.isPlayingOrWillChangePlaymode);
    }

    public virtual void OnPlayModeChanged(bool isPlaying)
    {

    }

    public virtual void DrawBeforeItems()
    {

    }

    public virtual void drawAfterItems()
    {

    }

    void OnGUI()
    {
        // DRAW BACKGROUND AND TITLE
        drawBackground("", position, ref offset);

        Event evt = Event.current;

        bool cancelCanvasInput = false;
        if (evt.type == EventType.MouseDown || evt.type == EventType.ContextClick || evt.type == EventType.MouseDrag)
        {
            if (windowRect.Contains(evt.mousePosition))
            {
                cancelCanvasInput = true;
            }
        }

        // DRAW VIEW
        Rect scrollWindowRect = new Rect(0, 20, position.width, position.height - 20 - 20);
        Rect scrollViewRect = CalcClientRect();
        //scrollViewRect.position = new Vector2(position.x, 0);
        offset = GUI.BeginScrollView(scrollWindowRect, offset, scrollViewRect, false, false);
        {            
            DrawBeforeItems();

            drawItems();

            drawAfterItems();

            if (!cancelCanvasInput)
                processCanvasInput(evt);
        }
        GUI.EndScrollView();        

        // DRAW STATUS BAR
        drawStatusBar();

        // GET NEW WINDOW RECT
        if (windowExpanded)
        {
            windowRect = new Rect(position.width - windowWidth - 20 + 3, 20, windowWidth, position.height - 20 - 20 - 20 + 4);
        }
        else
        {
            windowRect = new Rect(position.width - windowWidth - 20 + 3, 20, windowWidth, 40);
        }

        // DRAW WINDOW
        BeginWindows();        
        windowRect = GUI.Window(1, windowRect, DrawPropertiesWindow, "Properties");
        EndWindows();
    }    

    public virtual void drawItems()
    {
        // DRAW EACH OBJECT
        int objectsCount = GetItemsCount();
        for (int i = objectsCount - 1; i >= 0; --i)
        {
            DrawItem(i);
        }
    }

    public virtual bool RequiresUpdate { get => Application.isPlaying; }

    public virtual void Update()
    {
        if (RequiresUpdate)
        {
            Repaint();
        }
    }

    #endregion

    #region EVENTS

    protected Vector2 _mouseDownPosition;
    protected int _mouseDownIndex = -1;
    protected bool _mouseDragged = false;

    public virtual void OnItemMouseDown(Event evt, int index)
    {
        if ((evt.modifiers & EventModifiers.Control) != 0)
        {
            addSelectedItem(index);
        }
        else
        {
            setSelectedItem(index);
        }
    }    

    public virtual GenericMenu CreateMenu(int mouseDownIndex, Vector2 mousePosition)
    {
        if (mouseDownIndex != -1)
        {
            GenericMenu menu = new GenericMenu();
            {
                menu.AddItem(new GUIContent("Delete"), false, delegate ()
                {
                    List<int> selectedItemsCopy = new List<int>(_selectedItems);
                    for (int i = 0; i < selectedItemsCopy.Count; ++i)
                    {
                        RemoveItemAtIndex(selectedItemsCopy[i]);
                    }
                });
            }
            return menu;
        }
        else
        {
            GenericMenu menu = new GenericMenu();
            {
                menu.AddItem(new GUIContent("New"), false, delegate ()
                {
                    clearSelection();

                    int newIndex = AddEmptyItem(mousePosition);

                    setSelectedItem(newIndex);
                });
            }
            return menu;
        }
    }

    public int selectedFirstIndex
    {
        get
        {
            if (_selectedItems != null && _selectedItems.Count > 0)
            {
                return _selectedItems[0];
            }
            return -1;
        }
    }

    public virtual void OnCanvasMouseDown(Event evt)
    {
        _mouseDownPosition = evt.mousePosition;
        _mouseDragged = false;

        if (evt.button == 0 || evt.button == 1)
        {
            _mouseDownIndex = FindItemIndexAtPosition(evt.mousePosition);

            if (_mouseDownIndex != -1)
            {
                OnItemMouseDown(evt, _mouseDownIndex);
            }
            else
            {
                clearSelection();
            }

            evt.Use();
        }
    }

    public virtual void OnCanvasMouseUp(Event evt)
    {
        _mouseDownIndex = -1;
        _mouseDragged = false;
    }

    public virtual void OnContextMouseUp(Event evt)
    {
        if (evt.button == 1)
        {
            if (_mouseDragged == false)
            {
                GenericMenu menu = CreateMenu(selectedFirstIndex, evt.mousePosition);

                menu.ShowAsContext();

                evt.Use();
            }
        }       
    }

    public virtual void OnCanvasMouseDragged(Event evt)
    {
        if (evt.button == 2)
        {
            offset.x -= evt.delta.x;
            offset.y -= evt.delta.y;

            if (offset.x < 0)
                offset.x = 0;

            if (offset.y < 0)
                offset.y = 0;

            _mouseDragged = true;

            evt.Use();

            Repaint();
        }
        else if (_mouseDownIndex != -1)
        {
            Vector2 delta = evt.delta;

            _mouseDragged = true;

            evt.Use();

            MoveObjectsRelative(delta, _selectedItems.ToArray());
        }
    }

    public virtual void OnSelectionChange()
    {
        // OnEnable();
        Repaint();
    }

    #endregion
}