using UnityEngine;

public class Knife : MonoBehaviour
{
    public GameObject garlicObject;
    public GameObject cutGarlicObject;

    public void CutGarlic()
    {
        garlicObject.SetActive(true);
    }
    public void SwapGarlic()
    {
        Destroy(garlicObject);
        cutGarlicObject.SetActive(true);
    }
}
