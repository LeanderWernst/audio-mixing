using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject helpPanel;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private TextMeshProUGUI hintCountTMP;
    [SerializeField] private InteractionManager interactionManager;
    private int hintCount = 0;

    private Animator animator;
    public bool animationPlayed;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hideHelpOnEsc();
        HighlightButtonWithErrorCountInStep(2); // highlight button after 2 errors in step
    }

    // add one to hintCount
    public void addToHintCount() 
    {
        // only add to hintCount, if help was not already used in step and interaction is not skippable
        // (skippable = does not require a specific step to continue)
        if (!interactionManager.getHelpUsedInThisStep() && !interactionManager.currentInteractionIsSkippable())
        { 
            hintCount += 1;
            hintCountTMP.text = hintCount.ToString();
            interactionManager.setHelpUsedInThisStep(); // sets variable indicating if help was used in step to 'true'
        }
    }

    // return hintCount
    public int getHintCount()
    {
        return hintCount;
    }

    /**
    * Disable HelpPanel on press of Escape
    */
    private void hideHelpOnEsc()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && helpPanel.activeInHierarchy)
        {
            helpPanel.SetActive(false);
        }
    }

    public void ToggleHelpPanel()
    {
        if (!helpPanel.activeInHierarchy)
        {
            errorPanel.SetActive(false);
            helpPanel.SetActive(true);
            addToHintCount();
            StopAnimation();
        }
        else
        {
            helpPanel.SetActive(false);
        }
    }

    private void HighlightButtonWithErrorCountInStep(int countUntilTrigger)
    {
        if (interactionManager.errorCountInStep >= countUntilTrigger && !animationPlayed)
        {
            TriggerAnimation();
            animationPlayed = true;
        }
    }

    private void TriggerAnimation()
    {
        if (!animator.GetBool("highlightButtonOn"))
        { 
            animator.SetBool("highlightButtonOn", true);
        }
    }

    public void StopAnimation()
    {
        animator.SetBool("highlightButtonOn", false);
    }
}
