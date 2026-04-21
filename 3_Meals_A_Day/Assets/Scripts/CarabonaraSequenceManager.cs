using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System.Reflection;
using UnityEngine.UI;
using Unity.VisualScripting;
using System;

/*
 Recipe step reference:

1) separate the yolks
2) grate cheese
3) add black pepper
4) mix
5) cut meat
6) place cut meat into a pan and cook it
7) pour water into a pot and wait for it to boil
8) place the noodles into the pot and wait mix them occasionally until cooked
9) collect some of the water to be used later
10) collect the noodles
11) place noodles into the pan with the cooked meat
12) pour the noodle water into the pan
13) mix
14) pour the cheese mix that was made earlier
15) mix
16) place on plate
 
 */

public class CarabonaraSequenceManager : MonoBehaviour
{
    public static CarabonaraSequenceManager Instance {  get; private set; }
    public Recipe carbonaraRecipe;

    public GameObject endLevelPanel;

    [SerializeField] private Dictionary<int, bool> carbonaraSequenceFlags = new Dictionary<int, bool>();
    public int currentStep = 0;

    [Header("UI Elements")]
    public TextMeshProUGUI recipeInstuctionOverview;
    public TextMeshProUGUI recipeInstructionText;
    public TextMeshProUGUI feedbackText;
    public Button nextStepButton;

    [Header("Sequence Objects")]
    public GameObject knifeObject;
    public GameObject whiskObject;
    public GameObject spoonObject;
    public GameObject mixVolumeObject;

    [Header("Step 1 Objects")]
    public GameObject eggs;

    [Header("Step 2 Objects")]
    public GameObject cheese;
    public GameObject cheeseGrater;
    public GameObject gratedCheese;

    [Header("Step 3 Object")]
    public GameObject blackPepper;

    [Header("Step 5 Objects")]
    public GameObject meat;
    public GameObject choppedMeat;

    [Header("Step 6 Object")]
    public GameObject cookedMeat;

    [Header("Step 7 Objects")]
    public GameObject water;
    public GameObject pot;

    [Header("Step 8 Objects")]
    public GameObject pasta;
    public GameObject cookedPasta;

    [Header("Step 9 Obijects")]
    public GameObject cookedPastaWater;

    [Header("Step 10 Objects")]
    public GameObject cookedPastaOutOfWater;

    [Header("Step 11 Objects")]
    public GameObject cookedPastaOutOfWater2;
    public GameObject cookedMeat2;

    [Header("Step 12 Objects")]
    public GameObject cookedPastaWater2;

    [Header("Step 13 Objects")]
    public GameObject mixedPastaAndMeat;

    [Header("Step 14 Objects")]
    public GameObject cheeseMix;
    public GameObject almostMadeCarbonara;

    [Header("Step 16 Object")]
    public GameObject pan;
    public GameObject carbonara;

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

    // Update is called once per frame
    private void Update()
    {
        if (currentStep == carbonaraRecipe.stepsList.Length - 1 && carbonaraSequenceFlags[currentStep] == true)
        {
            Debug.Log("Meal Completed");
            nextStepButton.gameObject.SetActive(true);
            feedbackText.gameObject.SetActive(true);
            feedbackText.text = "Recipe Completed";
        }
    }
    private void InitializeFlags()
    {
        for (int i = 0; i < carbonaraRecipe.stepsList.Length; i++)
        {
            carbonaraSequenceFlags[i] = false;
            updateRecipeText();
            updateRecipeOverview();
        }
    }
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
        if(method != null)
        {
            IEnumerator coroutine = (IEnumerator)method.Invoke(this, null);
            StartCoroutine(coroutine);
        }
        else
        {
            Debug.LogWarning($"No method found for playSequence{currentStep}");
        }
    }

    public bool CheckStep()
    {
        PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();
        if (player.currentStation == carbonaraRecipe.stepsList[currentStep].station)
        {
            if (carbonaraRecipe.stepsList[currentStep].ingredientNeeded == null ||
                player.ingredients.Contains(carbonaraRecipe.stepsList[currentStep].ingredientNeeded))
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
            StartCoroutine(ShowMessageCoroutine("Not In the Correct Station"));
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

    private void setFlag(int stepIndex, bool value) => carbonaraSequenceFlags[stepIndex] = value;
    private void updateRecipeText() => recipeInstructionText.text = carbonaraRecipe.stepsList[currentStep].instructions; //update the text steps when pressing the button
    private void updateRecipeOverview() => recipeInstuctionOverview.text = carbonaraRecipe.stepsList[currentStep].instructions; //overview text
    private void check()
    {
        if (carbonaraSequenceFlags[currentStep] == true && currentStep < carbonaraRecipe.stepsList.Length - 1)
        {
            currentStep++;
            updateRecipeText();
            updateRecipeOverview();
        }
        else if (currentStep == carbonaraRecipe.stepsList.Length - 1 && carbonaraSequenceFlags[currentStep] == true)
        {

        }
    }

    private IEnumerator playSequence0()
    {
        //Crack two eggs and gather the yolks
        Debug.Log("Playing sequence for step 1: " + carbonaraRecipe.stepsList[0].stepName);
        eggs.SetActive(true);

        yield return new WaitForSeconds(3f);
        eggs.SetActive(false);
        yield return null;
    }
    private IEnumerator playSequence1()
    {
        //Grate cheese
        //TO animations to grate cheese
        Debug.Log("Playing sequence for step 2: " + carbonaraRecipe.stepsList[1].stepName);
        cheese.SetActive(true);
        gratedCheese.GetComponent<Animator>().SetBool("pickup", true);
        yield return new WaitForSeconds(3f);

        gratedCheese.GetComponent<Animator>().SetBool("pickup", false);
        gratedCheese.GetComponent<Animator>().SetBool("idle", true);
        yield return new WaitForSeconds(3f);

        gratedCheese.GetComponent<Animator>().SetBool("cheese", true);
        yield return new WaitForSeconds(3f);

        gratedCheese.GetComponent<Animator>().SetBool("cheese", false);
        gratedCheese.GetComponent<Animator>().SetBool("shredding", true);
        yield return new WaitForSeconds(3f);

        gratedCheese.GetComponent<Animator>().SetBool("shredding", false);
        cheese.SetActive(false);
        gratedCheese.GetComponent<Animator>().SetBool("idle", false);
        gratedCheese.GetComponent<Animator>().SetBool("back", true);
        yield return new WaitForSeconds(3f);

        gratedCheese.GetComponent<Animator>().SetBool("back", false);
        gratedCheese.SetActive(true);

        yield return null;
    }
    private IEnumerator playSequence2()
    {
        //pour black pepper 
        //TO DO make the bowl have yolks, cheese and pepper on it
        Debug.Log("Playing sequence for step 3: " + carbonaraRecipe.stepsList[2].stepName);
        blackPepper.SetActive(true);
        yield return new WaitForSeconds(1f);
        blackPepper.SetActive(false);
        yield return null;
    }
    private IEnumerator playSequence3()
    {
        //mix
        //TO DO 
        Debug.Log("Playing sequence for step 3: " + carbonaraRecipe.stepsList[3].stepName);
        whiskObject.GetComponent<Animator>().SetBool("whisk", true);
        whiskObject.GetComponent<Animator>().SetBool("reset", true);
        yield return new WaitForSeconds(8f);

        mixVolumeObject.GetComponent<Renderer>().material.SetColor("_Color", Color.softYellow);
        yield return new WaitForSeconds(2f);

        whiskObject.GetComponent<Animator>().SetBool("whisk", false);
        whiskObject.GetComponent<Animator>().SetBool("reset", false);
        yield return null;
    }
    private IEnumerator playSequence4()
    {
        //Cut meat
        Debug.Log("Playing sequence for step 5: " + carbonaraRecipe.stepsList[4].stepName);
        Animator anim = knifeObject.GetComponent<Animator>();
        anim.SetTrigger("Cut");
        yield return null;
    }
    private IEnumerator playSequence5()
    {
        //Move the meat to a pan and cook it
        Debug.Log("Playing sequence for step 5: " + carbonaraRecipe.stepsList[5].stepName);
        choppedMeat.SetActive(true);
        choppedMeat.SetActive(false);
        cookedMeat.SetActive(true);
        yield return null;
    }
    private IEnumerator playSequence6()
    {
        //pour water into a pot and wait for it to boil
        Debug.Log("Playing sequence for step 6: " + carbonaraRecipe.stepsList[6].stepName);

        yield return null;
    }
    private IEnumerator playSequence7()
    {
        Debug.Log("Playing sequence for step 7: " + carbonaraRecipe.stepsList[7].stepName);
        //place noodles into water and stir them until they are cooked
        spoonObject.GetComponent<Animator>().SetBool("potMix", true);
        spoonObject.GetComponent<Animator>().SetBool("reset", true);
        yield return new WaitForSeconds(8f);

        spoonObject.GetComponent<Animator>().SetBool("potMix", false);
        spoonObject.GetComponent<Animator>().SetBool("reset", false);
        yield return null;
    }
    private IEnumerator playSequence8()
    {
        //collect pasta water
        Debug.Log("Playing sequence for step 8: " + carbonaraRecipe.stepsList[8].stepName);
        yield return null;
    }
    private IEnumerator playSequence9()
    {
        Debug.Log("Playing sequence for step 9: " + carbonaraRecipe.stepsList[9].stepName);
        //Collect noodles
        spoonObject.GetComponent<Animator>().SetBool("scoop", true);
        spoonObject.GetComponent<Animator>().SetBool("reset", true);
        yield return new WaitForSeconds(8f);
        spoonObject.GetComponent<Animator>().SetBool("scoop", false);
        spoonObject.GetComponent<Animator>().SetBool("reset", false);
        cookedPasta.SetActive(true);
        yield return null;
    }
    private IEnumerator playSequence10()
    {
        //place noodles in the pan with the meat 
        Debug.Log("Playing sequence for step 10: " + carbonaraRecipe.stepsList[10].stepName);
        yield return null;
    }
    private IEnumerator playSequence11()
    {
        //pour the pasta water
        Debug.Log("Playing sequence for step 11: " + carbonaraRecipe.stepsList[11].stepName);
        yield return null;
    }
    private IEnumerator playSequence12()
    {
        //mix
        Debug.Log("Playing sequence for step 12: " + carbonaraRecipe.stepsList[12].stepName);
        spoonObject.GetComponent<Animator>().SetBool("panMix", true);
        spoonObject.GetComponent<Animator>().SetBool("reset", true);
        yield return new WaitForSeconds(8f);
        spoonObject.GetComponent<Animator>().SetBool("panMix", false);
        spoonObject.GetComponent<Animator>().SetBool("reset", false);
        yield return null;
    }
    private IEnumerator playSequence13()
    {
        //pour cheese mix made in step 4
        Debug.Log("Playing sequence for step 13: " + carbonaraRecipe.stepsList[13].stepName);
        yield return null;
    }
    private IEnumerator playSequence14()
    {
        //mix
        Debug.Log("Playing sequence for step 14: " + carbonaraRecipe.stepsList[14].stepName);
        spoonObject.GetComponent<Animator>().SetBool("panMix", true);
        spoonObject.GetComponent<Animator>().SetBool("reset", true);
        yield return new WaitForSeconds(8f);
        yield return null;
    }
    private IEnumerator playSequence15()
    {
        //Serve on plate
        Debug.Log("Playing sequence for step 16: " + carbonaraRecipe.stepsList[15].stepName);
        carbonara.SetActive(true);
        endLevelPanel.SetActive(true);
        yield return null;
    }
}
