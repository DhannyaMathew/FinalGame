using UnityEditor;


[CustomEditor(typeof(AmbientLighting))]
public class AmbientLightEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ((AmbientLighting) target).Set();
    }
}