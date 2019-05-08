using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class Test
{

    public static void CreateAsset<T>() where T : ScriptableObject
    {
        
        T asset = ScriptableObject.CreateInstance<T>();
        
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/" + typeof(T).ToString() + ".asset");
        
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        
        AssetDatabase.SaveAssets();

        
        AssetDatabase.Refresh();

  
        EditorUtility.FocusProjectWindow();
        
        Selection.activeObject = asset;
    }



}

