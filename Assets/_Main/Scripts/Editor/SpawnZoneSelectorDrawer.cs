#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpawnZoneSelectorAttribute))]
public class SpawnZoneSelectorDrawer : PropertyDrawer
{
    private string[] zoneNames;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (zoneNames == null || zoneNames.Length == 0)
        {
            var zones = GameObject.FindGameObjectsWithTag("SpawnZone");
            List<string> names = new List<string>();
            foreach (var zone in zones)
            {
                names.Add(zone.name);
            }
            zoneNames = names.ToArray();
        }

        int index = Mathf.Max(0, System.Array.IndexOf(zoneNames, property.stringValue));
        index = EditorGUI.Popup(position, label.text, index, zoneNames);
        if (index >= 0 && index < zoneNames.Length)
        {
            property.stringValue = zoneNames[index];
        }
    }
}
#endif
