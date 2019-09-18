using UnityEngine;

public class Boat : MonoBehaviour
{
    public float xSpeed;
    public float ySpeed;
    public float rotateY;
    public float rotateX;
    public float height=0.5f;
    
    private float startY;

    private void Start()
    {
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = Quaternion.Euler(rotateX*(0.5f*Mathf.Sin(xSpeed*Time.time)), 0,rotateY/2f*(Mathf.Sin(ySpeed*Time.time)));
        transform.position = new Vector3(transform.position.x, startY+ height*(0.5f*Mathf.Sin(ySpeed*Time.time)+1), transform.position.z);
    }
}
