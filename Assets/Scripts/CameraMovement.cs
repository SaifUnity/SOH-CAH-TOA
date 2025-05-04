using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float xOffset;

    public Transform player; // Assign the player Transform in the Inspector
    private float fixedY;    // The Y position the camera will stay at

    void Start()
    {
        // Store the camera's starting Y position
        fixedY = transform.position.y;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            // Move the camera only in the X direction, lock Y and Z
            transform.position = new Vector3(player.position.x + xOffset, fixedY, transform.position.z);
        }
    }
}
