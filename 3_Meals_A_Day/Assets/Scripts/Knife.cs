using UnityEngine;

public class Knife : MonoBehaviour
{
    public GameObject garlicObject;
    public GameObject cutGarlicObject;

    public void CutGarlic()
    {
        Debug.Log("Cutting Garlic");
        garlicObject.SetActive(true);
    }
    public void SwapGarlic()
    {
        Debug.Log("Swapping Garlic");
        Destroy(garlicObject);
        cutGarlicObject.SetActive(true);
    }
}
