using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    Vector3 begPosition;
    Quaternion begRotation;

    private void Awake()
    {
        begPosition = transform.position;
        begRotation = transform.rotation;
        Instance = this;
    }

    //Moves the camera to the position clicked on by the player
    public void MoveTo(Transform target)
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    //Moves the camera back to the beg position 
    public void BackToBeg()
    {
        transform.position = begPosition;
        transform.rotation = begRotation;
    }
}
