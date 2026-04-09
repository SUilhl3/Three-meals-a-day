using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public GameObject prefab;

    //just destroys itself after the animation is done
    private void EndAnimation()
    {
        Destroy(gameObject);
    }
}
