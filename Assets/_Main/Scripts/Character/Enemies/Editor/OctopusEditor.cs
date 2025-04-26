using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OctopusEnemyCombat))]
public class OctopusEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        OctopusEnemyCombat combat = (OctopusEnemyCombat)target;

        if (GUILayout.Button("Previous Pattern"))
        {
            combat.selectedPatternIndex = Mathf.Max(0, combat.selectedPatternIndex - 1);
        }

        if (GUILayout.Button("Next Pattern"))
        {
            combat.selectedPatternIndex = Mathf.Min(combat.attackPatterns.Count - 1, combat.selectedPatternIndex + 1);
        }
    }
}
