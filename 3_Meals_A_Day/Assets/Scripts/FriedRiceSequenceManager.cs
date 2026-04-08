using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Reflection;

public class FriedRiceSequenceManager : MonoBehaviour
{
    public static FriedRiceSequenceManager Instance { get; private set; } //to interact with the sequence manager from other scripts
    public Recipe friedRiceRecipe;

    //flags
    [SerializeField] private Dictionary<int, bool> riceSequenceFlags = new Dictionary<int, bool>();
    public int currentStep = 0;

    public TextMeshProUGUI recipeInstructionText;

    [Header("Sequence Objects")]
    //public GameObject mixVolumeObject;
    //public Animator mixVolumeAnimator;

    [Header("Step 0 Objects")]
    public GameObject milkObject;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //if (mixVolumeAnimator == null && mixVolumeObject != null)
        //{
        //    mixVolumeAnimator = mixVolumeObject.GetComponent<Animator>();
        //}
        InitializeFlags();
    }

    private void InitializeFlags()
    {
        for (int i = 0; i < friedRiceRecipe.stepsList.Length; i++)
        {
            riceSequenceFlags[i] = false; //set all flags to false initially
            updateRecipeText(); // Set the initial recipe instruction text
            // playCurrentSequence(); 
            // Debug.Log($"Initialized flag for step {i} to false");
        }
    }

    //function for setting the current flag to true, continuing to the next step
    public void pressedButton()
    {
        if(!CheckStep())
        {
            Debug.Log("Conditions not met for current step");
            return;
        }
        playCurrentSequence();
        setFlag(currentStep, true);
        check();
    }

    public void playCurrentSequence()
    {
        MethodInfo method = GetType().GetMethod($"playSequence{currentStep}", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method != null)
        {
            IEnumerator coroutine = (IEnumerator)method.Invoke(this, null);
            StartCoroutine(coroutine);
        }
        else
        {
            Debug.LogWarning($"No method found for playSequence{currentStep}");
        }
    }

    //checks if player has met conditions for step (in right station and with right items)
    private bool CheckStep()
    {
        PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();

        if (player.currentStation == friedRiceRecipe.stepsList[currentStep].station)
        {
            Debug.Log("In correct station");

            if (friedRiceRecipe.stepsList[currentStep].ingredientNeeded == null || player.ingredients.Contains(friedRiceRecipe.stepsList[currentStep].ingredientNeeded))
            {
                Debug.Log("Has needed ingredient");
                return true;
            }
            else
            {
                Debug.Log("Does not have needed ingredient");
                return false;
            }
        }
        else
        {
            Debug.Log("Not in correct station");
            return false;
        }
    }


    //sequence functions for each step, this is to keep the logic behind each step separate and organized
    private IEnumerator playSequence0()
    {
        //Cut Garlic
        //Some animation below (TODO)

        Debug.Log("Playing sequence for step 0: " + friedRiceRecipe.stepsList[0].stepName);
        //milkObject.SetActive(true);
        //mixVolumeObject.SetActive(true);
        //mixVolumeAnimator.SetBool("mixMilk", true);
        //// mixVolumeObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        //yield return new WaitForSeconds(5f);

        //milkObject.SetActive(false);

        yield return null;
    }



    private void setFlag(int stepIndex, bool value) => riceSequenceFlags[stepIndex] = value;
    private void updateRecipeText() => recipeInstructionText.text = friedRiceRecipe.stepsList[currentStep].instructions;

    private void check()
    {
        if (riceSequenceFlags[currentStep] == true && currentStep < friedRiceRecipe.stepsList.Length - 1)
        {
            currentStep++;
            updateRecipeText();
            // playCurrentSequence();
            // Debug.Log($"Moved to next step: {currentStep}");
        }
        else if (currentStep == friedRiceRecipe.stepsList.Length - 1 && riceSequenceFlags[currentStep] == true)
        {
            // Debug.Log("Recipe completed!");
        }
    }
}