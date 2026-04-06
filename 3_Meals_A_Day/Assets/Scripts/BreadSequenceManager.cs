using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Reflection;

public class BreadSequenceManager : MonoBehaviour
{
    public static BreadSequenceManager Instance { get; private set; } //to interact with the sequence manager from other scripts
    public Recipe breadRecipe; 

    //flags
    [SerializeField] private Dictionary<int, bool> breadSequenceFlags = new Dictionary<int, bool>();
    public int currentStep = 0;

    public TextMeshProUGUI recipeInstructionText;

    [Header("Sequence Objects")]
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
        InitializeFlags();
    }

    private void Update()
    {
        
    }

    private void InitializeFlags()
    {
        for (int i = 0; i < breadRecipe.stepsList.Length; i++)
        {
            breadSequenceFlags[i] = false; //set all flags to false initially
            updateRecipeText(); // Set the initial recipe instruction text
            playCurrentSequence(); 
            // Debug.Log($"Initialized flag for step {i} to false");
        }
    }

    //function for setting the current flag to true, continuing to the next step
    public void pressedButton()
    {
        setFlag(currentStep, true);
        if(breadSequenceFlags[currentStep] == true && currentStep < breadRecipe.stepsList.Length-1)
        {
            currentStep++;
            updateRecipeText();
            playCurrentSequence();
            // Debug.Log($"Moved to next step: {currentStep}");
        }
        else if(currentStep == breadRecipe.stepsList.Length-1 && breadSequenceFlags[currentStep] == true)
        {
            // Debug.Log("Recipe completed!");
        }
    }

    public void playCurrentSequence()
    {
        MethodInfo method = GetType().GetMethod($"playSequence{currentStep}", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method != null)
        {
            method.Invoke(this, null);
        }
        else
        {
            Debug.LogWarning($"No method found for playSequence{currentStep}");
        }
    }


    //sequence functions for each step, this is to keep the logic behind each step separate and organized
    private void playSequence0()
    {
        //Add milk
        Debug.Log("Playing sequence for step 0: " + breadRecipe.stepsList[0].stepName);
        milkObject.SetActive(true);
        StartCoroutine(waitForNextStep(5f));
    }
    private void playSequence1()
    {
        Debug.Log("Playing sequence for step 1: " + breadRecipe.stepsList[1].stepName);
    }




    private void setFlag(int stepIndex, bool value) => breadSequenceFlags[stepIndex] = value; 
    private void updateRecipeText() => recipeInstructionText.text = breadRecipe.stepsList[currentStep].instructions;

    private IEnumerator waitForNextStep(float delay)
    {
        Debug.Log($"Waiting for {delay} seconds before moving to the next step...");
        yield return new WaitForSeconds(delay);

        if(breadSequenceFlags[currentStep] == true && currentStep < breadRecipe.stepsList.Length-1)
        {
            currentStep++;
            updateRecipeText();
            playCurrentSequence();
            // Debug.Log($"Moved to next step: {currentStep}");
        }
        else if(currentStep == breadRecipe.stepsList.Length-1 && breadSequenceFlags[currentStep] == true)
        {
            // Debug.Log("Recipe completed!");
        }
        milkObject.SetActive(false);
        yield return null;
    }

    private IEnumerator waitForStep(float delay)
    {
        yield return new WaitForSeconds(delay);
        yield return null;
    }
}
