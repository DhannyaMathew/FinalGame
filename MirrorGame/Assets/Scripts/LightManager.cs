using System;
using UnityEditor;
using UnityEngine;
using RenderPipeline = UnityEngine.Experimental.Rendering.RenderPipeline;

[ExecuteInEditMode]
public class LightManager : MonoBehaviour
{
    [SerializeField] private AmbientLighting ambientLighting;
    [SerializeField] private Level[] levels;

    private void OnEnable()
    {
        RenderPipeline.beginCameraRendering += UpdateScene;
    }

    private void OnDisable()
    {
        RenderPipeline.beginCameraRendering -= UpdateScene;
    }

    private void UpdateScene(Camera camera)
    {
#if UNITY_EDITOR
        if (camera.cameraType != CameraType.SceneView || EditorApplication.isPlaying) return;
#else
        if (camera.cameraType != CameraType.SceneView) return;
#endif
        ambientLighting.transform.position = camera.transform.position;
        if (levels != null)
        {
            foreach (var level in levels)
            {
                if (Mathf.Abs(camera.transform.position.x - level.transform.position.x) < 300 &&
                    Mathf.Abs(camera.transform.position.z - level.transform.position.z) < 300)
                {
                    level.TurnOnDirectionalLight();
                    level.UpdateAmbientLights();
                }
                else
                {
                    level.TurnOffDirectionalLight();
                }
            }
        }
    }
}