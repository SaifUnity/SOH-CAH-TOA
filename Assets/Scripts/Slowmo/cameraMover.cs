using UnityEngine;

public class cameraMover : MonoBehaviour
{
    public float CamMoveSpeed;

    public Transform desiredsPostition;

    private Vector3 desiredLocation;

    
    void Update()
    {
        desiredLocation = desiredsPostition.transform.position;
        if (transform.position != desiredLocation)
        {
            MoveCamera();
        }
    }

    public void MoveCamera()
    {
        transform.position = Vector3.MoveTowards(transform.position, desiredLocation, (CamMoveSpeed * Time.unscaledDeltaTime));
    }
}
