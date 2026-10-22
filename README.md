# Behaviour Vars

Behaviour Vars provides named values and reusable conditions for Unity packages. `ILayer` exposes values; runtime code copies them into a `RuntimeValueCollection` for evaluation.

Package id: `com.deathbygravitystudio.behaviour.vars` (0.1.0). Assemblies: `Behaviour.Vars` and `Behaviour.Vars.Editor`. Runtime types live in `Behaviours` and `Behaviours.Runtime`.

## Features

- **Named values.** `Value` is a `ScriptableObject` with a name, a `ValueType`, and storage for a bool (`bValue`), int (`iValue`), float (`fValue`), or string (`sValue`). `runtimeObject` holds extra data, including an editor location via `GetLocation` / `SetLocation`.
- **Value types.** `ValueType` is `Bool`, `Integer`, `Float`, `String`, `Other`, `Array`, `Vector2`, `Vector3`, `MinMax2D`, `MinMax3D`, `Prefab`, and `None`.
- **Layers.** `ILayer` is a `ScriptableObject` (a class, not an interface) that owns `List<Value> values`, an `enabled` flag, and the `GameObject` the layer is bound to. `findValue` looks a value up by name. `getFloat`, `getInt`, `getBool`, and `getString` return the stored field or a fallback when the name is missing. The matching setters write only when the name exists.
- **Triggers.** `ILayer.setTrigger` sets a bool value to true for a given number of frames. `update` counts those frames down and clears the bool. `RuntimeValueCollection.SetTrigger` holds the bool true for three frames on a coroutine started on the `MonoBehaviour` you pass in.
- **Conditions.** A `Condition` asset points at a `Value`, an `Operation` (`Is`, `IsNot`, `IsGreater`, `IsGreaterOrEqual`, `IsLess`, `IsLessOrEqual`), a typed constant, and a `nextOperand` of `And` or `Or` for the following condition.
- **Runtime copy.** `RuntimeValueCollection.InitWithLayer` copies a layer's values into `RuntimeValueData` entries, indexed by name and by `name.GetHashCode()`. It sets `type` and copies the bool, integer, float, string, and `Other` payloads. Other `ValueType` values are still created, with their type set and no payload copied. `InitWithLayerAsync` copies those same payloads and yields once per value, and does not assign `type`. Setters that take a name log a warning and return when the name is missing. Getters that take a name assert when it is missing. The hash overloads assert when the hash is missing. `SetObject` / `GetObject` use the untyped `pValue` field. `SetFloat`, `SetInt`, `SetBool`, `SetString`, and `SetObject` raise `ValueChanged` with the value name. `SetTrigger` does not.
- **Condition checks.** `RuntimeValueData.compare` evaluates a `RuntimeConditionData` for bool, integer, float, string, and `Other`. `Value.Compare` and `RuntimeValueData.compare` also compare two floats with an `Operation`.
- **External storage.** `IValueStorage` is the get/set surface (float, int, bool, string) used when another system saves or loads runtime values.
- **Runtime data attribute.** `RuntimeDataAttribute` marks a class with the runtime-data type to construct. `createRuntimeDataFromAttribute` instantiates that type.
- **Enum lookup.** `ValueEnumAttribute` tags an enum with a name. `FindAttributeForPath` searches the executing assembly for a match.
- **Binary write.** `Value.Write`, `Condition.Write`, and `ILayer.Write` write their fields with a `BinaryWriter`. There is no matching reader in this package.
- **Editor values.** `ValueUtils` creates and removes `Value` and `Condition` sub-assets, clones a value list onto a parent asset, and imports or exports an `ILayer`'s bool, int, float, and string values as a Newtonsoft `JArray`. `ValueGUI` draws layer values, runtime values, and a condition row.
- **Editor canvas.** `GenericWindow<T>` is an `EditorWindow` with pan, zoom, selection, a grid background, and a properties pane. `EditorBehaviourGUI` supplies the shared styles, node colors, icon lookup, and line, arrow, and bezier drawing used by the sibling editor windows. `EditorIcons` caches built-in editor icons. `EditorZoomArea` (namespace `VNKit`) scales a GUI rect around a pivot. `Lines/Colored Blended` is the line shader.
- **Inspector helpers.** `SpreadAttribute` plus `SpreadDrawer` draws a float or int and, when the named delta field is non-zero, a ± field stored on that other serialized property. `BTAnimatorParamAttribute` carries a `ValueType` for a property drawer defined in the behaviour-tree package.

## Installation

Unity Package Manager, git URL:

```
https://github.com/denchi/UnityBehaviorVars.git
```

From the Package Manager window choose **Add package from git URL** and paste that URL, or add this to `Packages/manifest.json`:

```json
{
  "dependencies": {
    "com.deathbygravitystudio.behaviour.vars": "https://github.com/denchi/UnityBehaviorVars.git"
  }
}
```

The package root is the repository root (`package.json` sits next to `Runtime/` and `Editor/`).

## Quick start

Create a layer, add a float value, copy it into a runtime collection, then test it with a condition. This is the same path the HFSM, tree, and dialog packages use: asset values stay on the `ILayer`, and gameplay code reads and writes the `RuntimeValueCollection`.

```csharp
using Behaviours;
using Behaviours.Runtime;
using UnityEngine;

public class HealthGate : MonoBehaviour
{
    RuntimeValueCollection _values;

    void Start()
    {
        var layer = ScriptableObject.CreateInstance<ILayer>();

        var health = ScriptableObject.CreateInstance<Value>();
        health.Name = "health";
        health.type = ValueType.Float;
        health.fValue = 100f;
        layer.values.Add(health);

        _values = new RuntimeValueCollection();
        _values.InitWithLayer(layer);
        _values.ValueChanged += OnValueChanged;

        _values.SetFloat("health", 40f);
    }

    void OnValueChanged(string valueName)
    {
        if (valueName != "health")
            return;

        var health = _values.GetRuntimeValue("health");
        var belowHalf = new RuntimeConditionData
        {
            value = health,
            operation = Operation.IsLess,
            fConstant = 50f
        };

        if (health.compare(belowHalf))
            Debug.Log("health is below 50");
    }
}
```

`InitWithLayer` copies `fValue` (100). `SetFloat` writes the runtime copy and raises `ValueChanged`. `compare` then checks that runtime float against `fConstant`. The asset's `Value.fValue` is left at 100.

In editor code, `ValueUtils.createValue(layer, "health", ValueType.Float)` creates the `Value` and adds it to the layer asset. `ValueGUI.DrawLayerValues` draws the list.

## Sibling packages

Each package depends on `com.deathbygravitystudio.behaviour.vars` 0.1.0 and references the `Behaviour.Vars` assembly.

| Repository | Package | What it uses from this package |
| --- | --- | --- |
| [UnityBehaviorHFSM](https://github.com/denchi/UnityBehaviorHFSM) | `com.deathbygravitystudio.behaviour.hfsm` | `Layer` extends `ILayer`. `RuntimeLayerData` calls `RuntimeValueCollection.InitWithLayer`. Transitions store `Condition` assets and evaluate them with `RuntimeConditionData` and `RuntimeValueData.compare`, combined with `Operand`. `HFSMWindow` extends `GenericWindow<Node>` and draws with `EditorBehaviourGUI`, `EditorIcons`, `ValueGUI`, and `ValueUtils`. Cloning a transition uses `VarsUtils.Clone`. Services use `RuntimeDataAttribute`. |
| [UnityBehaviorTree](https://github.com/denchi/UnityBehaviorTree) | `com.deathbygravitystudio.behaviour.tree` | `Tree` extends `ILayer` (`CreateAssetMenu` **Behaviours/Tree**). `RuntimeTreeData.init` calls `InitWithLayer`. The `Behaviour` runner forwards `getFloat` / `setFloat` (and int, bool, string, plus `SetTrigger`) to that collection, by name or by hash. `BTWindow` extends `GenericWindow<Node>` and uses `EditorBehaviourGUI` and `ValueGUI.DrawLayerValues`. Tasks and services use `RuntimeDataAttribute`. `BTVarAttribute` stores a `ValueType`. The tree package's property drawer targets `BTAnimatorParamAttribute` from this package. |
| [UnityBehaviorGraph](https://github.com/denchi/UnityBehaviorGraph) | `com.deathbygravitystudio.behaviour.graph` | `Graph` extends `ILayer` (`CreateAssetMenu` **Behaviours/Graph**). Node inputs, outputs, and const/math tasks are tagged with `ValueType`. Runtime tasks carry `RuntimeValueData`, including `ValueType.Array` lists on `pValue`. `createWithGraph` copies `graph.values` into its own `List<RuntimeValueData>` instead of calling `InitWithLayer`. `GraphWindow` extends `GenericWindow<Node>`, draws with `EditorBehaviourGUI`, and calls `ValueGUI.DrawLayerValues`. |
| [UnityBehaviorDialogs](https://github.com/denchi/UnityBehaviorDialogs) | `com.deathbygravitystudio.behaviour.dialog` | `Dialog` extends `ILayer`. `RuntimeDialog.CreateRuntimeDialog` calls `InitWithLayer`. Option conditions are built with the HFSM helper `Utils.CreateRuntimeCondition` (the dialog package also depends on Behaviour HFSM) and combined with `Operand` in `EvaluateConditions`. `DialogActionSet` writes a runtime value with `SetInt`, `SetFloat`, `SetBool`, or `SetString`. `SaveRuntimeValues` / `LoadRuntimeValues` go through `IValueStorage`. The inspector draws vars with `ValueGUI.DrawLayerValues`. |

## Unity version

This repository does not declare a Unity version. `package.json` has no `unity` or `unityRelease` field, and there is no `ProjectSettings/ProjectVersion.txt`. The code uses `ScriptableObject`, `EditorWindow`, and assembly definitions without a version constraint.

The editor JSON import and export in `ValueUtils` references `Newtonsoft.Json.Linq`. `package.json` does not list that dependency.

## Licence

[MIT](LICENSE). Copyright (c) 2026 Denis Chiriac / Death By Gravity Studio.
