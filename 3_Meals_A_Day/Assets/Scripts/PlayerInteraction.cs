using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject recipePanel;
    public List<GameObject> ingredients;

    //Called when the player clicks the mouse, will move the camera to the position of the station clicked on
    public void Click(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            KitchenStation station = hit.collider.GetComponent<KitchenStation>();
            Ingredient ingredient = hit.collider.GetComponentInParent<Ingredient>();

            if (station != null)
            {
                station.Interact();
            }

            //used to add ingredient to list (inventory) and destroy the ingredient in the scene
            if (ingredient != null && context.performed)
            {
                ingredients.Add(ingredient.prefab);
                Destroy(ingredient.gameObject);
            }
        }
    }

    //calls the back to beg function on the camera when back button clickeds
    public void BackToMain()
    {
        CameraController.Instance.BackToBeg();
    }

    public void OpenRecipePanel()
    {
        recipePanel.SetActive(true);
    }

    public void CloseRecipePanel()
    {
        recipePanel.SetActive(false);
    }   
}
