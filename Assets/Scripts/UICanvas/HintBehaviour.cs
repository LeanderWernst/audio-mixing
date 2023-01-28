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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hideHelpOnEsc();
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
        }
        else
        {
            helpPanel.SetActive(false);
        }
    }
}
