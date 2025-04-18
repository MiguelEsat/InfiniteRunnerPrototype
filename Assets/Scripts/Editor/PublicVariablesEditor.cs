using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

public class PublicVariablesEditor : EditorWindow
{
    private Dictionary<MonoBehaviour, List<FieldInfo>> scriptFields = new Dictionary<MonoBehaviour, List<FieldInfo>>();

    [MenuItem("Tools/Public Variables Editor")]
    public static void ShowWindow()
    {
        GetWindow<PublicVariablesEditor>("Public Variables Editor");
    }

    private void OnEnable()
    {
        ScanScene();
    }

    private void ScanScene()
    {
        scriptFields.Clear();
        MonoBehaviour[] scripts = FindObjectsOfType<MonoBehaviour>();

        foreach (MonoBehaviour script in scripts)
        {
            List<FieldInfo> fields = new List<FieldInfo>();
            FieldInfo[] allFields = script.GetType().GetFields(BindingFlags.Public 
                                                               | BindingFlags.Instance
                                                               | BindingFlags.NonPublic);

            foreach (FieldInfo field in allFields)
            {
                if (field.IsStatic)
                    continue;

                if (field.IsPublic || field.GetCustomAttribute<SerializeField>() != null)
                {
                    fields.Add(field);
                }
            }

            if (fields.Count > 0)
            {
                scriptFields[script] = fields;
            }
        }
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Actualizar Variables"))
        {
            ScanScene();
        }

        foreach (var entry in scriptFields)
        {
            MonoBehaviour script = entry.Key;
            List<FieldInfo> fields = entry.Value;

            EditorGUILayout.LabelField(script.gameObject.name + " - " + script.GetType().Name, EditorStyles.boldLabel);

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(script);
                if (value is int intValue)
                {
                    int newValue = EditorGUILayout.IntField(field.Name, intValue);
                    if (newValue != intValue) field.SetValue(script, newValue);
                }
                else if (value is float floatValue)
                {
                    float newValue = EditorGUILayout.FloatField(field.Name, floatValue);
                    if (newValue != floatValue) field.SetValue(script, newValue);
                }
                else if (value is string stringValue)
                {
                    string newValue = EditorGUILayout.TextField(field.Name, stringValue);
                    if (newValue != stringValue) field.SetValue(script, newValue);
                }
                else if (value is bool boolValue)
                {
                    bool newValue = EditorGUILayout.Toggle(field.Name, boolValue);
                    if (newValue != boolValue) field.SetValue(script, newValue);
                }
            }
        }
    }
}