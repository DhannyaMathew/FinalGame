using System;
using UnityEditor;
using UnityEngine;
using RenderPipeline = UnityEngine.Experimental.Rendering.RenderPipeline;

[ExecuteInEditMode]
public class LightManager : MonoBehaviour
{
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
        var type = FindObjectsOfType<Light>();
        foreach (var light in type)
        {
            if (Mathf.Abs(camera.transform.position.x - light.transform.position.x) < 300 &&
                Mathf.Abs(camera.transform.position.z - light.transform.position.z) < 300)
            {
                light.enabled = true;
            }
            else
            {
                light.enabled = false;
            }
        }
    }
}