using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public GameObject prefab;
    public GameObject spawnObject;

    //just destroys itself after the animation is done
    private void EndAnimation()
    {
        Destroy(gameObject);
        if (spawnObject)
        {
            spawnObject.SetActive(true);
        }
    }

}
