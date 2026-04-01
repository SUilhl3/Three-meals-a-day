using UnityEngine;

public class KitchenStation : MonoBehaviour
{
    public Transform cameraPoint;

    public void Interact()
    {
        CameraController.Instance.MoveTo(cameraPoint);
    }
}
