using UnityEngine;
using System.Collections;

public class FlyCamera : MonoBehaviour 
{
    [SerializeField] float mainSpeed = 100.0f; // Regular speed
    [SerializeField] [Range(0, 1f)] float camSens = 0.22f; // Mouse sensitivity
    float shiftAdd = 250.0f; // Speed multiplier for shift
    float maxShift = 1000.0f; // Maximum shift speed
    private Vector3 lastMouse = new Vector3(255, 255, 255);
    private float totalRun = 1.0f;

    // Define movement boundaries
    private Vector3 minBounds = new Vector3(-0.28f, -0.28f, -0.28f);
    private Vector3 maxBounds = new Vector3(0.28f, 0.28f, 0.28f);

    void Update() 
    {
        // Mouse Look
        lastMouse = Input.mousePosition - lastMouse;
        lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
        lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
        transform.eulerAngles = lastMouse;
        lastMouse = Input.mousePosition;

        // Keyboard movement
        Vector3 p = GetBaseInput();
        if (p.sqrMagnitude > 0) 
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            } 
            else 
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            transform.Translate(p);

            // Clamp the local position to stay within bounds
            Vector3 clampedPosition = transform.localPosition;
            clampedPosition.x = Mathf.Clamp(clampedPosition.x, minBounds.x, maxBounds.x);
            clampedPosition.y = Mathf.Clamp(clampedPosition.y, minBounds.y, maxBounds.y);
            clampedPosition.z = Mathf.Clamp(clampedPosition.z, minBounds.z, maxBounds.z);
            transform.localPosition = clampedPosition;
        }
    }

    private Vector3 GetBaseInput() 
    {
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W)) p_Velocity += new Vector3(0, 0, 1);
        if (Input.GetKey(KeyCode.S)) p_Velocity += new Vector3(0, 0, -1);
        if (Input.GetKey(KeyCode.A)) p_Velocity += new Vector3(-1, 0, 0);
        if (Input.GetKey(KeyCode.D)) p_Velocity += new Vector3(1, 0, 0);
        return p_Velocity;
    }
}
