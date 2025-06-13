using UnityEngine;

namespace Behaviours
{
    public class VarsUtils
    {
        static void checkAddToAsset(ScriptableObject target, string assetPath, UnityEngine.ScriptableObject parent = null)
        {
#if UNITY_EDITOR
            if (string.IsNullOrEmpty(assetPath) == false)
            {
                if (parent)
                {
                    Debug.LogFormat("Adding {0}:{1} to {2}", target.GetType().Name, target.name, assetPath);

                    UnityEditor.AssetDatabase.AddObjectToAsset(target, assetPath);

                    UnityEditor.EditorUtility.SetDirty(parent);
                }
                else
                {
                    Debug.LogFormat("Creating {0}:{1} at {2}", target.GetType().Name, target.name, assetPath);

                    UnityEditor.AssetDatabase.CreateAsset(target, assetPath);
                    // UnityEditor.AssetDatabase.SaveAssets();
                    // UnityEditor.AssetDatabase.Refresh();
                }
            }
#endif
        }

        public static Condition Clone(Condition original, string assetPath)
        {
            Condition condition = UnityEngine.Object.Instantiate<Condition>(original);
            {
                condition.name = "Copy of " + original.name;

#if UNITY_EDITOR
                if (string.IsNullOrEmpty(assetPath) == false)
                    UnityEditor.AssetDatabase.AddObjectToAsset(condition, assetPath);
#endif
            }
            return condition;
        }

    }
}
