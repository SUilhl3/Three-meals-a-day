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
    public TextMeshProUGUI feedbackText;
    public GameObject endLevelPanel;

    [Header("Sequence Objects")]
    public GameObject mixVolumeObject;
    public Animator mixVolumeAnimator;

    [Header("Step 0 Objects")]
    public GameObject milkObject;

    [Header("Step 1 Objects")]
    public GameObject yeastObject;

    [Header("Step 2 Objects")]
    public GameObject sugarObject;

    [Header("Step 3 Objects")]
    public GameObject whiskObject;

    [Header("Step 4 Objects")]
    public GameObject eggObject;

    [Header("Step 7 Objects")]
    public GameObject saltObject;
    [Header("Step 8 Objects")]
    public GameObject flourObject;
    [Header("Step 9 Objects")]
    public GameObject doughObject;
    public float doughTargetScale = .7f;
    public float doughScale = .1f;
    [Header("Step 10 Objects")]
    public Animator doughAnimator;
    public GameObject handObject;
    [Header("Step 11 Objects")]
    public GameObject butterObject;

    [Header("Step 14 Objects")]
    public GameObject miniDough_1;
    public GameObject miniDough_2;
    public GameObject miniDough_3;
    public GameObject miniDough_4;
    public GameObject miniDough_5;
    [Header("Step 16 Objects")]
    public TextMeshProUGUI ovenText;
    public GameObject ovenObject;
    public GameObject ovenLight;
    public GameObject trayObject;

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
        if (mixVolumeAnimator == null && mixVolumeObject != null)
        {
            mixVolumeAnimator = mixVolumeObject.GetComponent<Animator>();
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
            // playCurrentSequence(); 
            // Debug.Log($"Initialized flag for step {i} to false");
        }
    }

    //function for setting the current flag to true, continuing to the next step
    public void pressedButton()
    {
        if (!CheckStep())
        {
            Debug.Log("Conditions not met for current step");
            return;
        }
        bool playSeq = playCurrentSequence();
        if (!playSeq) { }
        else { setFlag(currentStep, true); }
        check();
    }

    //checks if player has met conditions for step (in right station and with right items)
    private bool CheckStep()
    {
        PlayerInteraction player = FindFirstObjectByType<PlayerInteraction>();

        if (player.currentStation == breadRecipe.stepsList[currentStep].station)
        {

            if (breadRecipe.stepsList[currentStep].ingredientNeeded == null || player.ingredients.Contains(breadRecipe.stepsList[currentStep].ingredientNeeded))
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

    public bool playCurrentSequence()
    {
        MethodInfo method = GetType().GetMethod($"playSequence{currentStep}", BindingFlags.NonPublic | BindingFlags.Instance);
        if (method != null)
        {
            IEnumerator coroutine = (IEnumerator)method.Invoke(this, null);
            StartCoroutine(coroutine);
            return true;
        }
        else
        {
            Debug.LogWarning($"No method found for playSequence{currentStep}");
            return false;
        }
    }


    //sequence functions for each step, this is to keep the logic behind each step separate and organized
    private IEnumerator playSequence0()
    {
        //Add milk
        Debug.Log("Playing sequence for step 0: " + breadRecipe.stepsList[0].stepName);
        milkObject.SetActive(true);
        mixVolumeObject.SetActive(true);
        mixVolumeAnimator.SetBool("mixMilk", true);
        // mixVolumeObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
        yield return new WaitForSeconds(5f);

        milkObject.SetActive(false);

        yield return null;
    }

    private IEnumerator playSequence1()
    {
        //Add yeast
        Debug.Log("Playing sequence for step 1: " + breadRecipe.stepsList[1].stepName);
        yeastObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        mixVolumeObject.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
        yield return new WaitForSeconds(2f);

        yeastObject.SetActive(false);

        yield return null;
    }

    private IEnumerator playSequence2()
    {
        //Add sugar
        Debug.Log("Playing sequence for step 2: " + breadRecipe.stepsList[2].stepName);
        sugarObject.SetActive(true);
        yield return new WaitForSeconds(1f);

        mixVolumeObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(2f);

        sugarObject.SetActive(false);

        yield return null;
    }

    private IEnumerator playSequence3()
    {
        //Mix ingredients
        Debug.Log("Playing sequence for step 3: " + breadRecipe.stepsList[3].stepName);
        whiskObject.GetComponent<Animator>().SetBool("whisk", true);

        whiskObject.GetComponent<Animator>().SetBool("reset", true);

        yield return new WaitForSeconds(8f);

        whiskObject.GetComponent<Animator>().SetBool("whisk", false);
        whiskObject.GetComponent<Animator>().SetBool("reset", false);
        yield return null;
    }

    private IEnumerator playSequence4()
    {
        Debug.Log("Playing sequence for step 4: " + breadRecipe.stepsList[4].stepName);
        eggObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        eggObject.SetActive(false);
        yield return null;
    }

    private IEnumerator playSequence5()
    {
        Debug.Log("Playing sequence for step 5: " + breadRecipe.stepsList[5].stepName);
        whiskObject.GetComponent<Animator>().SetBool("whisk", true);

        whiskObject.GetComponent<Animator>().SetBool("reset", true);

        yield return new WaitForSeconds(8f);

        whiskObject.GetComponent<Animator>().SetBool("whisk", false);
        whiskObject.GetComponent<Animator>().SetBool("reset", false);
        yield return null;
    }

    private IEnumerator playSequence6()
    {
        Debug.Log("Playing sequence for step 6: " + breadRecipe.stepsList[6].stepName);
        Debug.Log("Mixture is setting...");
        yield return null;
    }

    private IEnumerator playSequence7()
    {
        Debug.Log("Playing sequence for step 7: " + breadRecipe.stepsList[7].stepName);
        saltObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        saltObject.SetActive(false);

        yield return null;
    }

    private IEnumerator playSequence8()
    {
        Debug.Log("Playing sequence for step 8: " + breadRecipe.stepsList[8].stepName);
        flourObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        flourObject.SetActive(false);

        yield return null;
    }

    private IEnumerator playSequence9()
    {
        Debug.Log("Playing sequence for step 9: " + breadRecipe.stepsList[9].stepName);

        doughObject.GetComponent<Animator>().enabled = false;

        Animator whiskAnimator = whiskObject.GetComponent<Animator>();
        whiskAnimator.SetBool("whisk", true);
        whiskAnimator.SetBool("reset", true);

        float duration = 6f;
        float elapsed = 0f;

        Vector3 startScale = new Vector3(0.01f, 0.01f, 0.01f);
        Vector3 targetScale = new Vector3(0.7f, 0.7f, 0.7f);
        doughObject.SetActive(true);
        doughObject.transform.localScale = startScale;

        Material mixMat = mixVolumeObject.GetComponent<Renderer>().material;

        float startFill = mixMat.GetFloat("_Fill");
        float targetFill = .524f;

        yield return new WaitForSeconds(1.5f);

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Grow dough
            doughObject.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            // Reduce mix volume
            float fill = Mathf.Lerp(startFill, targetFill, t);
            mixMat.SetFloat("_Fill", fill);

            elapsed += Time.deltaTime;
            yield return null;
        }

        // doughObject.transform.localScale = targetScale;
        mixMat.SetFloat("_Fill", targetFill);

        whiskAnimator.SetBool("whisk", false);
        whiskAnimator.SetBool("reset", false);

        mixVolumeObject.SetActive(false);

        doughObject.GetComponent<Animator>().enabled = true;
    }

    private IEnumerator playSequence10()
    {
        Debug.Log("Playing sequence for step 10: " + breadRecipe.stepsList[10].stepName);

        doughAnimator = doughObject.GetComponent<Animator>();
        doughAnimator.SetBool("tray", true);

        yield return new WaitForSeconds(1f);

        doughAnimator.SetBool("knead", true);
        yield return new WaitForSeconds(1f);
        handObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        handObject.SetActive(false);

        doughAnimator.SetBool("knead", false);
        doughAnimator.SetBool("tray", false);

        yield return null;
    }

    private IEnumerator playSequence11()
    {
        Debug.Log("Playing sequence for step 11: " + breadRecipe.stepsList[11].stepName);
        butterObject.SetActive(true);

        yield return new WaitForSeconds(2f);
        yield return null;
    }

    private IEnumerator playSequence12()
    {
        Debug.Log("Playing sequence for step 12: " + breadRecipe.stepsList[12].stepName);

        doughAnimator = doughObject.GetComponent<Animator>();
        doughAnimator.SetBool("knead", true);

        yield return new WaitForSeconds(1f);

        handObject.SetActive(true);

        // Butter scaling setup
        float duration = 4f;
        float elapsed = 0f;

        Vector3 startScale = new Vector3(0.01561698f, 0.01561698f, 0.01561698f);
        Vector3 targetScale = new Vector3(0.00346978f, 0.00346978f, 0.00346978f);

        butterObject.transform.localScale = startScale;

        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // Shrink butter
            butterObject.transform.localScale = Vector3.Lerp(startScale, targetScale, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        butterObject.transform.localScale = targetScale;

        handObject.SetActive(false);
        butterObject.SetActive(false);

        doughAnimator.SetBool("knead", false);
        doughAnimator.SetBool("tray", false);

        yield return null;
    }

    private IEnumerator playSequence13()
    {
        Debug.Log("Playing sequence for step 13: " + breadRecipe.stepsList[13].stepName);
        Debug.Log("Dough is rising...");
        yield return null;
    }

    private IEnumerator playSequence14()
    {
        Debug.Log("Playing sequence for step 14: " + breadRecipe.stepsList[14].stepName);
        
        doughAnimator = doughObject.GetComponent<Animator>();
        doughAnimator.SetBool("split", true);

        miniDough_1.SetActive(true);
        miniDough_1.GetComponent<Animator>().enabled = true;
        miniDough_1.GetComponent<Animator>().SetInteger("dough", 1);
        yield return new WaitForSeconds(1f);

        miniDough_2.SetActive(true);
        miniDough_2.GetComponent<Animator>().enabled = true;
        miniDough_2.GetComponent<Animator>().SetInteger("dough", 2);
        yield return new WaitForSeconds(1f);

        miniDough_3.SetActive(true);
        miniDough_3.GetComponent<Animator>().enabled = true;
        miniDough_3.GetComponent<Animator>().SetInteger("dough", 3);
        yield return new WaitForSeconds(1f);

        miniDough_4.SetActive(true);
        miniDough_4.GetComponent<Animator>().enabled = true;
        miniDough_4.GetComponent<Animator>().SetInteger("dough", 4);
        yield return new WaitForSeconds(1f);

        miniDough_5.SetActive(true);
        miniDough_5.GetComponent<Animator>().enabled = true;
        miniDough_5.GetComponent<Animator>().SetInteger("dough", 5);
        yield return new WaitForSeconds(1f);

        yield return null;
    }

    private IEnumerator playSequence15()
    {
        Debug.Log("Playing sequence for step 15: " + breadRecipe.stepsList[15].stepName);
        Debug.Log("Dough is rising...");
        yield return null;
    }

    private IEnumerator playSequence16()
    {
        Debug.Log("Playing sequence for step 16: " + breadRecipe.stepsList[16].stepName);
        ovenText.text = "170°C";
        ovenLight.SetActive(true);
        yield return null;
    }

    private IEnumerator playSequence17()
    {
        Debug.Log("Playing sequence for step 17: " + breadRecipe.stepsList[17].stepName);
        ovenObject.GetComponent<Animator>().SetBool("ovenOpen", true);

        trayObject.GetComponent<Animator>().enabled = true;
        trayObject.GetComponent<Animator>().SetBool("moveTray", true);

        doughObject.SetActive(false);

        yield return new WaitForSeconds(3f);
        ovenObject.GetComponent<Animator>().SetBool("ovenOpen", false);

        yield return null;
    }

    private IEnumerator playSequence18()
    {
        Debug.Log("Playing sequence for step 18: " + breadRecipe.stepsList[18].stepName);
        Debug.Log("Bread is baking...");
        yield return null;
    }

    private IEnumerator playSequence19()
    {
        Debug.Log("Playing sequence for step 19: " + breadRecipe.stepsList[19].stepName);
        ovenObject.GetComponent<Animator>().SetBool("ovenOpen", true);

        trayObject.GetComponent<Animator>().SetBool("removeTray", true);

        yield return new WaitForSeconds(3f);
        ovenObject.GetComponent<Animator>().SetBool("ovenOpen", false);

        ovenLight.SetActive(false);
        ovenText.text = "0°C";

        yield return null;
    }

    private IEnumerator playSequence20()
    {
        Debug.Log("Playing sequence for step 20: " + breadRecipe.stepsList[20].stepName);
        
        trayObject.GetComponent<Animator>().SetBool("removeBread", true);

        endLevelPanel.SetActive(true);

        yield return null;
    }


    private void setFlag(int stepIndex, bool value) => breadSequenceFlags[stepIndex] = value;
    private void updateRecipeText() => recipeInstructionText.text = breadRecipe.stepsList[currentStep].instructions;

    private void check()
    {
        if (breadSequenceFlags[currentStep] == true && currentStep < breadRecipe.stepsList.Length - 1)
        {
            currentStep++;
            updateRecipeText();
            // playCurrentSequence();
            // Debug.Log($"Moved to next step: {currentStep}");
        }
        else if (currentStep == breadRecipe.stepsList.Length - 1 && breadSequenceFlags[currentStep] == true)
        {
            // Debug.Log("Recipe completed!");
        }
    }
}
