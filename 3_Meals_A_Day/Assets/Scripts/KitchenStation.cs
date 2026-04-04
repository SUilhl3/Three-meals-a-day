using UnityEngine;

public class KitchenStation : MonoBehaviour
{
    public Transform cameraPoint;
    public bool isCookingStation = false;
    public void Interact()
    {
        CameraController.Instance.MoveTo(cameraPoint);
    }
}
