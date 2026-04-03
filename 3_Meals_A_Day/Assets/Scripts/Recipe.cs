using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "Scriptable Objects/Recipe")]
public class Recipe : ScriptableObject
{
    [Tooltip("The name of the recipe")]
    public string recipeName;

    [Tooltip("The amount of steps this recipe has")]
    public int steps;

    public Step[] stepsList;
}
