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
        if(mixVolumeAnimator == null && mixVolumeObject != null)
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
        bool playSeq = playCurrentSequence();
        if(!playSeq){}
        else { setFlag(currentStep, true); }
        check();
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
        yield return new WaitForSeconds(4f);
        whiskObject.GetComponent<Animator>().SetBool("whisk", false);
        whiskObject.GetComponent<Animator>().SetBool("reset", true);
        yield return null;
    }

    private IEnumerator playSequence4()
    {
        Debug.Log("Playing sequence for step 4: " + breadRecipe.stepsList[4].stepName);
        
        yield return null;
    }


    private void setFlag(int stepIndex, bool value) => breadSequenceFlags[stepIndex] = value; 
    private void updateRecipeText() => recipeInstructionText.text = breadRecipe.stepsList[currentStep].instructions;

    private void check()
    {
        if(breadSequenceFlags[currentStep] == true && currentStep < breadRecipe.stepsList.Length-1)
        {
            currentStep++;
            updateRecipeText();
            // playCurrentSequence();
            // Debug.Log($"Moved to next step: {currentStep}");
        }
        else if(currentStep == breadRecipe.stepsList.Length-1 && breadSequenceFlags[currentStep] == true)
        {
            // Debug.Log("Recipe completed!");
        }
    }
}
