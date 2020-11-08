
using UnityEngine;
using UnityEditor;
using UnityEngine.Experimental.UIElements;

public class editorWindow : EditorWindow
{

    Texture2D xSymbol;
    Texture2D oSymbol;
    Texture2D background;
    string assetBundleName = "Bundle Name";

    [MenuItem("Window/SettingsWindow")]
    public static void ShowWindow()
    {
        GetWindow<editorWindow>();
    }
    private void OnGUI()
    {
        //GUILayout.Label("this is a label");


        xSymbol = (Texture2D)EditorGUILayout.ObjectField("X Symbol", xSymbol, typeof(Texture2D), false);
        oSymbol = (Texture2D)EditorGUILayout.ObjectField("O Symbol", oSymbol, typeof(Texture2D), false);
        background = (Texture2D)EditorGUILayout.ObjectField("Background", background, typeof(Texture2D), false);
        assetBundleName = EditorGUILayout.TextField("Asset name", assetBundleName);

        //EditorGUILayout.RectField
        if (GUILayout.Button("Build asset bundle"))
        {
          
            AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(xSymbol)).assetBundleName = assetBundleName;
            AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(oSymbol)).assetBundleName = assetBundleName;
            AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(background)).assetBundleName = assetBundleName;

            BundleBuilder.Build();



            Debug.Log("save");
        }
    }


}

