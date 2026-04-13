using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Reflection;
using UnityEngine.UI;

public class FriedRiceSequenceManager : MonoBehaviour
{
    public static FriedRiceSequenceManager Instance { get; private set; } //to interact with the sequence manager from other scripts
    public Recipe friedRiceRecipe;

    //flags
    [SerializeField] private Dictionary<int, bool> riceSequenceFlags = new Dictionary<int, bool>();
    public int currentStep = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI recipeInstructionText;
    public TextMeshProUGUI feedbackText;
    public Button nextStepButton;
    public GameObject endLevelPanel;

    [Header("Sequence Objects")]
    public GameObject knifeObject;
    //public Animator mixVolumeAnimator;

    [Header("Step 1 Objects")]
    public GameObject cutGarlic;

    [Header("Step 2 Objects")]
    public GameObject oliveOilBottle;
    public GameObject oliveOilLiquid;

    [Header("Step 3 Objects")]
    public GameObject brownedGarlic;

    [Header("Step 4 Objects")]
    public GameObject soySauceBottle;
    public Color nextOilColor;

    [Header("Step 5 Objects")]
    public GameObject salt;

    [Header("Step 6 Objects")]
    public GameObject riceBowl;
    public GameObject rice;

    [Header("Step 7 Objects")]
    public GameObject butter;

    [Header("Step 8 Objects")]
    public GameObject finishedRice; //child component under pan object so it will be destroyed when you pick up the pan
    public GameObject platedRice;
    public GameObject pan;


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

    private void Update()
    {
        if (currentStep == friedRiceRecipe.stepsList.Length - 1 && riceSequenceFlags[currentStep] == true)
        {
            Debug.Log("Recipe completed!");
            nextStepButton.gameObject.SetActive(false);
            feedbackText.gameObject.SetActive(false);
            endLevelPanel.SetActive(true);
        }
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

            if (friedRiceRecipe.stepsList[currentStep].ingredientNeeded == null || player.ingredients.Contains(friedRiceRecipe.stepsList[currentStep].ingredientNeeded))
            {
                StartCoroutine(ShowMessageCoroutine("Correct! Move to the next step."));
                return true;
            }
            else
            {
                StartCoroutine(ShowMessageCoroutine("Missing Ingredient"));
                return false;
            }
        }
        else
        {
            StartCoroutine(ShowMessageCoroutine("Not In The Correct Station"));
            return false;
        }
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        feedbackText.text = message;
        feedbackText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        feedbackText.gameObject.SetActive(false);
    }


    //sequence functions for each step, this is to keep the logic behind each step separate and organized
    private IEnumerator playSequence0()
    {
        //Cut Garlic
        //Some animation below (TODO)

        Debug.Log("Playing sequence for step 0: " + friedRiceRecipe.stepsList[0].stepName);

        Animator anim = knifeObject.GetComponent<Animator>();
        anim.SetTrigger("Cut");


        yield return null;
    }


    private IEnumerator playSequence1()
    {
        Debug.Log("Playing sequence for step 1: " + friedRiceRecipe.stepsList[1].stepName);
        cutGarlic.SetActive(true);
        yield return null;
    }

    private IEnumerator playSequence2()
    {
        Debug.Log("Playing sequence for step 2: " + friedRiceRecipe.stepsList[2].stepName);

        oliveOilBottle.SetActive(true);
        Animator anim = oliveOilBottle.GetComponent<Animator>();
        anim.SetTrigger("Pouring");

        yield  return new WaitForSeconds(1f);

        oliveOilLiquid.SetActive(true);


        yield return null;
    }

    private IEnumerator playSequence3()
    {
        Debug.Log("Playing sequence for step 3: " + friedRiceRecipe.stepsList[3].stepName);

        //some animation for cooking garlic 
        Destroy(cutGarlic);
        brownedGarlic.SetActive(true);

        yield return null;
    }

    private IEnumerator playSequence4()
    {
        Debug.Log("Playing sequence for step 4: " + friedRiceRecipe.stepsList[4].stepName);
        

        soySauceBottle.gameObject.SetActive(true);
        Animator anim = soySauceBottle.GetComponent<Animator>();
        anim.SetTrigger("Pouring");

        yield return new WaitForSeconds(1f);

        oliveOilLiquid.GetComponent<Renderer>().material.SetColor("_Color", nextOilColor);
        yield return null;
    }

    private IEnumerator playSequence5()
    {
        Debug.Log("Playing sequence for step 5: " + friedRiceRecipe.stepsList[5].stepName);
        salt.SetActive(true);
        yield return new WaitForSeconds(2f);
        Destroy(salt);
        yield return null;
    }

    private IEnumerator playSequence6()
    {
        Debug.Log("Playing sequence for step 6: " + friedRiceRecipe.stepsList[6].stepName);
        //some animation for cooking rice with soy sauce and oil
        riceBowl.SetActive(true);
        Animator anim = riceBowl.GetComponent<Animator>();
        anim.SetTrigger("Pouring");
        yield return null;
    }

    private IEnumerator playSequence7()
    {
        Debug.Log("Playing sequence for step 7: " + friedRiceRecipe.stepsList[7].stepName);
        //some animation for plating the fried rice
        butter.SetActive(true);
        yield return new WaitForSeconds(3f);
        Destroy(oliveOilLiquid);
        Destroy(butter);
        Destroy(rice);
        Destroy(brownedGarlic);
        finishedRice.SetActive(true);
        pan.GetComponent<Ingredient>().enabled = true; // so you can pick up the pan after the rice is done
        yield return null;
    }

    private IEnumerator playSequence8()
    {
        Debug.Log("Playing sequence for step 8: " + friedRiceRecipe.stepsList[8].stepName);
        platedRice.SetActive(true);
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