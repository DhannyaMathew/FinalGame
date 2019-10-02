using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private GameObject mirror;
    [SerializeField] private float hitOffset = 0.1f;

    private bool _hasMirror = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _hasMirror)
        {
            _hasMirror = false;
            RaycastHit hit;
            var ray = GameManager.MainCamera.Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position + Vector3.up,
                ray.direction, out hit,
                Mathf.Infinity))
            {
                var m = Instantiate(mirror, GameManager.CurrentLevel.transform);
                m.transform.forward = hit.normal;
                GameManager.CurrentLevel.AddMirror(m.GetComponent<Mirror>());
                m.transform.position = hit.point + hit.normal * hitOffset;
                m.transform.rotation = Quaternion.LookRotation(hit.normal);
                GameManager.CurrentLevel.ResetMirrors();
            }
        }
    }
}