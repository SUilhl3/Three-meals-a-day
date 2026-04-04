using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject recipePanel;
    public List<GameObject> ingredients; //list of ingredient prefabs 
    public Button mixButton;
    public Recipe recipe;
    int currentRecipeStep = 0;

    //Called when the player clicks the mouse, will move the camera to the position of the station clicked on
    public void Click(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            KitchenStation station = hit.collider.GetComponent<KitchenStation>();
            Ingredient ingredient = hit.collider.GetComponent<Ingredient>();

            if (station != null)
            {
                station.Interact();
                if(station.isCookingStation)
                {
                    mixButton.gameObject.SetActive(true);
                    mixButton.GetComponentInChildren<TextMeshProUGUI>().text = station.buttonText;
                }
                else
                {
                    mixButton.gameObject.SetActive(false);
                }
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
        mixButton.gameObject.SetActive(false);
    }

    public void OpenRecipePanel()
    {
        recipePanel.SetActive(true);
    }

    public void CloseRecipePanel()
    {
        recipePanel.SetActive(false);
    }   

    public void MixIngredients()
    {
       currentRecipeStep = BreadSequenceManager.Instance.currentStep; 
       GameObject needed = recipe.stepsList[currentRecipeStep].ingredientNeeded;

        //if no gameobject specified as needed, will do another action instead of mixing 
        if (needed == null)
        {
            Debug.Log("No ingredient needed for this step");
            OtherCookingAction();
            return;
        }

        //check if needed ingredient is in the list of ingredients, if so complete the step and move to the next one, if not display message that ingredient is missing
        foreach (GameObject ingredient in ingredients)
        {
            if(ingredient == needed)
            {
                Debug.Log("Adding Ingredient");
                BreadSequenceManager.Instance.pressedButton();
                return;
            }
        }
        Debug.Log("Missing ingredient");
    }

    public void OtherCookingAction()
    {
        //do something 
        string t = recipe.stepsList[currentRecipeStep].stepName;
        Debug.Log("Completed: " + t);
        BreadSequenceManager.Instance.pressedButton();
    }
}
