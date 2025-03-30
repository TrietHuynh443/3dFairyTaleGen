using System;
using SO;
using UnityEditor;
using UnityEngine;

public class EnvironmentManagerEditorGUI : EditorWindow
{
    private EnvironmentSO _environmentConfig;
    private int _selectedEnvironmentIndex;

    [MenuItem("Tools/Environment Manager")]
    public static void ShowWindow()
    {
        GetWindow<EnvironmentManagerEditorGUI>("Environment Manager");
    }

    private void OnEnable()
    {
        LoadEnvironments();
    }

    private void LoadEnvironments()
    {
        _environmentConfig = Resources.LoadAll<EnvironmentSO>("SO/Environment")[0];
        
        _selectedEnvironmentIndex = 0;
    }

    private void OnGUI()
    {
        GUILayout.Label("Environment Manager", EditorStyles.boldLabel);
        
        if (_environmentConfig != null)
        {
            _environmentConfig.environmentType = (EnvironmentType)EditorGUILayout.EnumPopup("Environment Type", _environmentConfig.environmentType);

            if (GUILayout.Button("Save"))
            {
                EditorUtility.SetDirty(_environmentConfig);
                AssetDatabase.SaveAssets();
            }
        }
    }
}

