using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    private GameObject applicationSettings;
    private ApplicationData applicationData;

    [Header("User Interface")]
    [SerializeField] private AudioController audioController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI headlineLabel;
    [SerializeField] private TextMeshProUGUI instructionLabel;
    [SerializeField] private TextMeshProUGUI errorLabel;
    [SerializeField] private TextMeshProUGUI helpLabel;
    [SerializeField] private TextMeshProUGUI helpLabelBonus;
    [SerializeField] private GameObject helpBonusButton;
    private GameObject helpPanel;

    [SerializeField] private TextMeshProUGUI errorCountLabel = null;
    [SerializeField] private TextMeshProUGUI helpCountLabel = null;

    [SerializeField] private GameObject finalScreen;
    [SerializeField] private TextMeshProUGUI totalErrorCountLabel;
    [SerializeField] private TextMeshProUGUI totalHelpCountLabel;

    [SerializeField] private TextMeshProUGUI skipLabel;
    private Image skipLabelPanel;
    [SerializeField] private AudioClip soundWrong;
    [SerializeField] private AudioClip soundCorrect;

    [SerializeField] private PictureInPicture pip;

    [Header("No Error GameObjects")]
    [SerializeField] private GameObject speechFader;
    [SerializeField] private GameObject masterFader;
    [SerializeField] private GameObject channelList;
    [SerializeField] private int mixingStep;


    [Header("Raycast")]
    [SerializeField] private LayerMask layerMask;
    private Camera cam;

    [Header("Interactions")]
    [SerializeField] private List<int> bassSoundSteps = new List<int> { 5, 7, 25, 27, 29 };
    [SerializeField] private List<int> snareSoundSteps = new List<int> { 11, 13, 36, 38 };
    [SerializeField] private List<int> hihatSoundSteps = new List<int> { 17, 19, 45, 47 };
    [SerializeField] private List<int> pipSteps = new List<int> { 5, 11, 17 };
    [SerializeField] private int eqStepsStart = 22;
    [SerializeField] private int eqStepsEnd = 49;
    [SerializeField] private List<Interaction> interactions;

    [SerializeField] private UnityEvent OnCompleted;
    [SerializeField] private float completionCallbackDelay;

    public bool InteractionsCompleted => interactionIndex >= interactions.Count;
    private int interactionIndex = 6; // >>>>>> CHANGE INDEX TO 0 OR DELETE WHEN DEBUGGING COMPLETE!
    private Interaction currentInteraction;

    [SerializeField] private HintBehaviour hintBehaviour;
    private int errorCount;
    private int previousErrorCount;
    public int errorCountInStep;
    private bool helpUsedInThisStep = false;
    private bool bassWasPlayed, snareWasPlayed, hihatWasPlayed, drumsWerePlayed = false;

    private Coroutine lastCoroutine; // keeping track of the latest Coroutine

    private void Awake()
    {
        cam = Camera.main;

        // Read settings from gameobject if present.
        // For debugging purposes, if scene was not started from menu, create settings object
        applicationSettings = GameObject.FindGameObjectWithTag("ApplicationSettings");
        if (applicationSettings == null)
        {
            applicationSettings = new GameObject("ApplicationSettings");
            applicationSettings.tag = "ApplicationSettings";
            applicationSettings.AddComponent<ApplicationData>();
        }
        applicationData = applicationSettings.GetComponent<ApplicationData>();
    }


    private void Start()
    {
        Debug.Log("Start InteractionManager");
        // UI init
        skipLabelPanel = skipLabel.GetComponentInParent<Image>();
        helpPanel = helpLabel.transform.parent.parent.gameObject;
        errorCountLabel.SetText(errorCount.ToString());
        helpCountLabel.SetText(hintBehaviour.getHintCount().ToString());

        // warning if no interactions are defined in the interaction manager
        if (interactions.Count == 0)
        {
            Debug.LogWarning("No Interactions in Interaction Manager.");
            return;
        }

        // Set the first interaction from the list as our current interaction and display its instruction in the ui.
        currentInteraction = interactions[interactionIndex];
        headlineLabel.SetText(ReplaceStepNumInHeadline(currentInteraction.Headline));
        instructionLabel.SetText(currentInteraction.Instruction);
        helpLabel.SetText(currentInteraction.HelpMsg);
        helpLabelBonus.SetText(currentInteraction.HelpMsgBonus);
        audioSource.clip = currentInteraction.InstrAudio;
        helpBonusButton.SetActive(currentInteraction.HelpMsgBonus != "");
        // error label does not need to be set, since it is set when an error occurs.

        // Wait 2 seconds until the first instruction sound is being played
        if (applicationData.speakInstructions)
        { 
            StartCoroutine(WaitThenPlaySound(2f));
        }
    }

    private void Update()
    {
        DebugDrawRay();

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20.0f, layerMask))
            {
                Debug.Log("Hit InteractionLayer");
                CheckInteractionOrder(hit.transform.gameObject);
            }
        }

        // Skip steps, when skippable or confirm action
        if (Input.GetKeyDown(KeyCode.Return) && currentInteraction.IsSkippable)
        {
            if (!MandatorySoundWasPlayed())
            {
                PlayFeedbackSound(false);
                DisplayErrForDuration(errorLabel, "Du solltest hören, was du mischst.", 5);
                errorCount++;
                errorCountLabel.SetText(errorCount.ToString());
                return;
            }
            MoveToNextInteraction();
        }
        else if (Input.GetKeyDown(KeyCode.Return) // Check TargetRange if TargetRange was given
                && currentInteraction.TargetValueMax != currentInteraction.TargetValueMin)
        {
            CheckTargetRange();
        }

        UpdateErrorCountInStepWhenErrorChanges();
    }

    private void DebugDrawRay()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 20.0f, layerMask))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green);
        }
        else
        {
            Debug.DrawLine(ray.origin, ray.origin + ray.direction * 20.0f, Color.red);
        }
    }

    public void CheckInteractionOrder(GameObject selectedGameObject)
    {
        if (InteractionsCompleted)
            return;

        if (!selectedGameObject)
            return;

        if (selectedGameObject.Equals(currentInteraction.TargetObject))
        {
            Debug.Log("Hit correct Object");

            // Hide Error Messages
            errorLabel.transform.parent.gameObject.SetActive(false);
            errorLabel.text = "";

            // Show 'ENTER' message when hitting an object that needs confirmation
            if (TargetValueHasRange() && interactionIndex != mixingStep)
            {
                SetTextWithFade(1f, skipLabel, "Drücke ENTER zum Bestätigen");
                FadeGraphic(1f, skipLabelPanel);
            }

            // When min & max value is set, we need to check the value by pressing Return instead of 
            // just checking the target value
            if (ObjectHasTargetValue() && currentInteraction.TargetValueMax == currentInteraction.TargetValueMin)
            {
                PlayFeedbackSound(true);
                MoveToNextInteraction();
            }
        }
        else if (!selectedGameObject.Equals(speechFader) 
            && !selectedGameObject.Equals(masterFader) 
            && !selectedGameObject.Equals(channelList) 
            && !FinalMixingIsActive()
            && !IsInFinalStep())
        {
            PlayFeedbackSound(false);
            DisplayErrForDuration(errorLabel, currentInteraction.ErrElement, 5);
            errorCount++;
            errorCountLabel.SetText(errorCount.ToString());
        }
    }

    private Coroutine coroutine;
    private void DisplayErrForDuration(TextMeshProUGUI label, string msg, float duration)
    {
        helpPanel.SetActive(false); // disable help when creating an error
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        if (msg != "")
        {
            label.transform.parent.gameObject.SetActive(true);
            label.text = msg;

            coroutine = StartCoroutine(DisableAfterDuration(duration, label));
        }
    }

    private IEnumerator DisableAfterDuration(float duration, TextMeshProUGUI label)
    {
        yield return new WaitForSeconds(duration);

        label.transform.parent.gameObject.SetActive(false);
        label.text = "";
    }


    private void CheckTargetRange()
    {
        if (currentInteraction.TargetValueMax != currentInteraction.TargetValueMin)
        {
            if (!MandatorySoundWasPlayed())
            {
                PlayFeedbackSound(false);
                DisplayErrForDuration(errorLabel, "Du solltest hören, was du mischst.", 5);
                errorCount++;
                errorCountLabel.SetText(errorCount.ToString());
                return;
            }
            var setValue = currentInteraction.TargetObject.GetComponent<ValueStorage>().GetValue();
            var minValue = currentInteraction.TargetValueMin;
            var maxValue = currentInteraction.TargetValueMax;
            if (setValue >= minValue && setValue <= maxValue)
            { 
                PlayFeedbackSound(true);
                MoveToNextInteraction();
            }
            else 
            {
                PlayFeedbackSound(false);
                if (setValue < minValue)
                {
                    DisplayErrForDuration(errorLabel, currentInteraction.ErrBelowMin, 5);
                    errorCount++;
                    errorCountLabel.SetText(errorCount.ToString());
                }
                else 
                {
                    DisplayErrForDuration(errorLabel, currentInteraction.ErrAboveMax, 5);
                    errorCount++;
                    errorCountLabel.SetText(errorCount.ToString());
                }
            }
        }
    }

    private bool ObjectHasTargetValue()
    {
        if (currentInteraction.TargetObject != null)
            // Mathf.Approximately allows a certain tolerance of epsilon (approx. 1.192092896e-07f) to avoid floating point
            // precision errors.
            return Mathf.Approximately(currentInteraction.TargetObject.GetComponent<ValueStorage>().GetValue(), currentInteraction.TargetValue);
        else
            return false;
    }

    // Checks if the target value of the current interaction is within a range
    // (if min and max are not equal, there is a range)
    private bool TargetValueHasRange()
    {
        return currentInteraction.TargetValueMax != currentInteraction.TargetValueMin;
    }

    private void MoveToNextInteraction()
    {
        // Stop Audio
        if (audioSource.isPlaying)
            audioSource.Stop();

        // move interaction index up
        interactionIndex++;
        // skip equalizer steps if not selected in settings
        if (!applicationData.equalizerMode && interactionIndex == eqStepsStart)
        {
            interactionIndex = eqStepsEnd + 1;
        }

        // reset all flags indicating if sound was played in step
        ResetSoundWasPlaying();

        // reset errorCountInStep and hintBehaviour animation
        errorCountInStep = 0;
        hintBehaviour.animationPlayed = false;

        // if step requires master-meter show it, else hide it
        if (pipSteps.Contains(interactionIndex)) { pip.ToggleSmoothSlide(); }
        else { if (pip.IsVisible()) { pip.ToggleSmoothSlide(); } }

        // reset helpUsedInThisStep to count help again, hide helpPanel
        helpUsedInThisStep = false;
        helpPanel.SetActive(false);

        // set next interaction
        currentInteraction = interactions[interactionIndex];
        StopAllCoroutines();

        // Get TargetValue: if range, calculate mean of range.
        var targetValue = TargetValueHasRange() ? (currentInteraction.TargetValueMax + currentInteraction.TargetValueMin) / 2 : currentInteraction.TargetValue;
        // Get animationTime: if 0, use 2f as default
        var animationTime = currentInteraction.animationTime == 0 ? 2f : currentInteraction.animationTime;
        // Invoke Demo-Animation 'Animate' if defined
        currentInteraction.Animate?.Invoke(targetValue, currentInteraction.animationTime);

        // only fade in the headline if it is new and replace step numbers if necessary
        if (!headlineLabel.text.Equals(ReplaceStepNumInHeadline(currentInteraction.Headline)))
        { 
            SetTextWithFade(1f, headlineLabel, ReplaceStepNumInHeadline(currentInteraction.Headline));
        }
        // fade in new instruction
        SetTextWithFade(1f, instructionLabel, currentInteraction.Instruction, setNewAudio: true);

        // set new help texts
        helpLabel.SetText(currentInteraction.HelpMsg);
        helpLabelBonus.SetText(currentInteraction.HelpMsgBonus);
        helpBonusButton.SetActive(currentInteraction.HelpMsgBonus != "");
        if (!helpBonusButton.activeInHierarchy)
        {
            helpLabelBonus.gameObject.SetActive(false); // deactivate bonus, when button is not available
        }

        // if step is skippable fade in info panel, else fade out
        if (currentInteraction.IsSkippable)
        {
            if (interactionIndex == mixingStep)
            {
                SetTextWithFade(1f, skipLabel, "Drücke ENTER zum Bestätigen", delay: 3f);
                FadeGraphic(1f, skipLabelPanel, delay: 3f);
            }
            else
            {
                SetTextWithFade(1f, skipLabel, "Drücke ENTER, um fortzufahren", delay: 3f);
                FadeGraphic(1f, skipLabelPanel, delay: 3f);
            }
        }
        else
        {
            StartCoroutine(FadeGraphicToZeroAlpha(1f, skipLabel));
            StartCoroutine(FadeGraphicToZeroAlpha(1f, skipLabelPanel));
        }

        if (IsInFinalStep())
        {
            StartCoroutine(DelayedTrainingCompletionCallback());
        }
    }

    private IEnumerator DelayedTrainingCompletionCallback()
    {
        totalErrorCountLabel.SetText(errorCount.ToString());
        totalHelpCountLabel.SetText(hintBehaviour.getHintCount().ToString());
        yield return new WaitForSeconds(completionCallbackDelay);
        finalScreen.SetActive(true);
        //OnCompleted?.Invoke();
    }

    private void PlayFeedbackSound(bool success)
    {
        audioSource.Stop();
        AudioSource tempAudioSource = gameObject.AddComponent<AudioSource>();
        if (success)
        {
            tempAudioSource.clip = soundCorrect;
            tempAudioSource.Play();
        }
        else 
        {
            tempAudioSource.clip = soundWrong;
            tempAudioSource.Play();
        }
        Destroy(tempAudioSource, tempAudioSource.clip.length);
    }

    private void SetTextWithFade(float seconds, TextMeshProUGUI textMesh, string newText, bool setNewAudio = false, float delay = 0f)
    {
        StartCoroutine(FadeTextToFullAlpha(seconds, textMesh, newText, delay));

        IEnumerator FadeTextToFullAlpha(float timeInSeconds, TextMeshProUGUI tmpUGUI, string newText, float delay)
        {
            yield return FadeTextToZeroAlpha(timeInSeconds, tmpUGUI);
            yield return new WaitForSeconds(delay);
            tmpUGUI.SetText(newText);
            if (setNewAudio)
            {
                audioSource.clip = currentInteraction.InstrAudio;
                if (audioSource.clip != null && applicationData.speakInstructions)
                    audioSource.Play();
            }
            while (tmpUGUI.color.a < 1.0f)
            {
                tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b, tmpUGUI.color.a + (Time.deltaTime / timeInSeconds));
                yield return null;
            }
        }

        IEnumerator FadeTextToZeroAlpha(float timeInSeconds, TextMeshProUGUI tmpUGUI)
        {
            while (tmpUGUI.color.a > 0.0f)
            {
                tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b, tmpUGUI.color.a - (Time.deltaTime / timeInSeconds));
                yield return null;
            }
        }
    }

    private void FadeGraphic(float seconds, Graphic g, float delay = 0f)
    {
        StartCoroutine(FadeGraphicToFullAlpha(seconds, g, delay));

    }
    IEnumerator FadeGraphicToFullAlpha(float timeInSeconds, Graphic g, float delay = 0f)
    {
        yield return FadeGraphicToZeroAlpha(timeInSeconds, g);
        yield return new WaitForSeconds(delay);
            
        while (g.color.a < 1.0f)
        {
            g.color = new Color(g.color.r, g.color.g, g.color.b, g.color.a + (Time.deltaTime / timeInSeconds));
            yield return null;
        }
    }

    IEnumerator FadeGraphicToZeroAlpha(float timeInSeconds, Graphic g, float delay = 0f)
    {
        yield return new WaitForSeconds(delay);
        while (g.color.a > 0.0f)
        {
            g.color = new Color(g.color.r, g.color.g, g.color.b, g.color.a - (Time.deltaTime / timeInSeconds));
            yield return null;
        }
    }

    IEnumerator WaitThenPlaySound(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (audioSource.clip != null)
            audioSource.Play();
    }

    void StopAllCoroutinesByName(string name)
    {
        var coroutines = new List<Coroutine>();
        foreach (var coroutine in coroutines)
        {
            if (coroutine.ToString().Contains(name))
            {
                StopCoroutine(coroutine);
            }
        }
    }

    void StopAllCoroutinesByNameExceptNewest(string name)
    {
        var coroutines = new List<Coroutine>();
        foreach (var coroutine in coroutines)
        {
            if (coroutine.ToString().Contains(name) && coroutine != lastCoroutine)
            {
                StopCoroutine(coroutine);
            }
        }
    }

    public Interaction GetCurrentInteraction()
    {
        return currentInteraction;
    }

    /**
     * Used for Button in HelpPanel: Disables Button GO and enables
     * the 'HelpMsgBonus' Go to show additional text.
     */
    public void showBonusInformation()
    {
        helpBonusButton.SetActive(false);
        helpLabelBonus.gameObject.SetActive(true);
    }

    public void setHelpUsedInThisStep()
    {
        helpUsedInThisStep = true;
    }

    public bool getHelpUsedInThisStep()
    {
        return helpUsedInThisStep;
    }

    public bool currentInteractionIsSkippable()
    {
        return currentInteraction.IsSkippable;
    }

    // Indicates if current interaction is the final mixing step
    public bool FinalMixingIsActive()
    {
        return interactionIndex == mixingStep;
    }

    private bool MandatorySoundWasPlayed()
    {
        int i = interactionIndex;
        if (bassSoundSteps.Contains(i) && !bassWasPlayed && !drumsWerePlayed)
        {
            return false;
        }
        if (snareSoundSteps.Contains(i) && !snareWasPlayed && !drumsWerePlayed)
        {
            return false;
        }
        if (hihatSoundSteps.Contains(i) && !hihatWasPlayed && !drumsWerePlayed)
        {
            return false;
        }
        if (i == mixingStep && !drumsWerePlayed)
        {
            return false;
        }
        return true;
    }

    public void SetSoundWasPlayed(string drum)
    {
        switch (drum)
        {
            case "drum_bass":
                bassWasPlayed = true;
                break;
            case "drum_snare":
                snareWasPlayed = true;
                break;
            case "drum_hihat":
                hihatWasPlayed = true;
                break;
            case "all":
                drumsWerePlayed = true;
                break;
        }
    }

    private void ResetSoundWasPlaying()
    {
        bassWasPlayed = audioController.IsDrumActive("drum_bass");
        snareWasPlayed = audioController.IsDrumActive("drum_snare");
        hihatWasPlayed = audioController.IsDrumActive("drum_hihat");
        drumsWerePlayed = audioController.IsDrumActive("all");
    }

    private string ReplaceStepNumInHeadline(string headline)
    {
        // if equalizer is skipped, step numbers need to be replaced
        if (!applicationData.equalizerMode)
        { 
            //"Schritt (1/6)\nBassdrum pegeln";

            // extract numbers in Schritt '(d/d)'
            var match = Regex.Match(headline, @"\((\d+)\/(\d+)\)");
            if (match.Success)
            {
                int firstInt = int.Parse(match.Groups[1].Value);
                int secondInt = int.Parse(match.Groups[2].Value);

                // adjust according to steps
                if (firstInt > 3)
                { 
                    firstInt = firstInt - 3;
                }
                secondInt = secondInt - 3;

                // replace values and return
                return headline.Replace(match.Groups[0].Value, $"({firstInt}/{secondInt})");
            }
        }
        return headline;
    }

    private bool IsInFinalStep()
    {
        return interactionIndex == interactions.Count - 1;
    }

    // put into update
    private void UpdateErrorCountInStepWhenErrorChanges()
    {
        if (errorCount != previousErrorCount)
        {
            errorCountInStep++;
            previousErrorCount = errorCount;
        }
    }
}