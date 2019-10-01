using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ((Level) target).UpdateAmbientLights();
    }
}