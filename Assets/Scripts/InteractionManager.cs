using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractionManager : MonoBehaviour
{
    [Header("User Interface")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private TextMeshProUGUI headlineLabel;
    [SerializeField] private TextMeshProUGUI instructionLabel;
    [SerializeField] private TextMeshProUGUI errorLabel;
    private Image errorLabelPanel;
    [SerializeField] private TextMeshProUGUI helpLabel;
    [SerializeField] private TextMeshProUGUI helpLabelBonus;

    [SerializeField] private TextMeshProUGUI errorCountLabel = null;
    [SerializeField] private TextMeshProUGUI helpCountLabel = null;

    [SerializeField] private TextMeshProUGUI totalErrorCountLabel;
    [SerializeField] private TextMeshProUGUI totalHelpCountLabel;

    [SerializeField] private TextMeshProUGUI skipLabel;
    private Image skipLabelPanel;
    [SerializeField] private AudioClip soundWrong;
    [SerializeField] private AudioClip soundCorrect;

    [Header("No Error GameObjects")]
    [SerializeField] private GameObject speechFader;
    [SerializeField] private GameObject channelList;
    [SerializeField] private int mixingStep;


    [Header("Raycast")]
    [SerializeField] private LayerMask layerMask;
    private Camera cam;

    [Header("Interactions")]
    [SerializeField] private List<Interaction> interactions;

    [SerializeField] private UnityEvent OnCompleted;
    [SerializeField] private float completionCallbackDelay;

    public bool InteractionsCompleted => interactionIndex >= interactions.Count;
    private int interactionIndex = 0; // >>>>>> CHANGE INDEX TO 0 OR DELETE WHEN DEBUGGING COMPLETE!
    private Interaction currentInteraction;

    private int errorCount;
    private int helpCount;

    private Coroutine lastCoroutine; // keeping track of the latest Coroutine

    private void Awake() => cam = Camera.main;

    private void Start()
    {
        Debug.Log("Start InteractionManager");
        // UI init
        skipLabelPanel = skipLabel.GetComponentInParent<Image>();
        errorLabelPanel = errorLabel.GetComponentInParent<Image>();
        //helpLabel.SetText("");
        //errorLabel.SetText("");
        errorCountLabel.SetText(errorCount.ToString());
        helpCountLabel.SetText(helpCount.ToString());

        // warning if no interactions are defined in the interaction manager
        if (interactions.Count == 0)
        {
            Debug.LogWarning("No Interactions in Interaction Manager.");
            return;
        }

        // Set the first interaction from the list as our current interaction and display its instruction in the ui.
        currentInteraction = interactions[interactionIndex];
        headlineLabel.SetText(currentInteraction.Headline);
        instructionLabel.SetText(currentInteraction.Instruction);
        audioSource.clip = currentInteraction.InstrAudio;
        StartCoroutine(WaitThenPlaySound(2f));
    }

    private void Update()
    {
        DebugDrawRay();

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 20.0f, layerMask))
            {
                Debug.Log("Hit InteractionLayer");
                CheckInteractionOrder(hit.transform.gameObject);
            }
        }

        // Users can request help with the H key as long as we still have "open" interactions (the training is not completed).
        /*if (Input.GetKeyDown(KeyCode.H) && !InteractionsCompleted)
        {
            // If your help counter is limited (because you display the help permanently after it was requested)
            // then you can do this ...
            if (!currentInteraction.HelpCounted)
            {
                helpCount++;
                currentInteraction.HelpCounted = true;
            }
            // otherwise just do ...
            // helpCount++;

            //helpCountLabel.SetText("Hilfen: " + helpCount);

            StartCoroutine(DisplayForDuration(helpLabel, currentInteraction.HelpMsg, 5));
        }*/

        // Skip steps, when skippable or confirm action
        if (Input.GetKeyDown(KeyCode.Return) && currentInteraction.IsSkippable)
            MoveToNextInteraction();
        else if (Input.GetKeyDown(KeyCode.Return) // Check TargetRange if TargetRange was given
                && currentInteraction.TargetValueMax != currentInteraction.TargetValueMin)
        {
            CheckTargetRange();
        }
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
            errorLabelPanel.enabled = false;
            errorLabel.text = "";

            // Show 'ENTER' message when hitting an object that needs confirmation
            if (TargetValueHasRange())
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
                

            if (InteractionsCompleted)
            {
                // Code that should be executed at the end of the training goes here
                StartCoroutine(DelayedTrainingCompletionCallback());
                return;
            }
        }
        else if (!selectedGameObject.Equals(speechFader) && !selectedGameObject.Equals(channelList) && interactionIndex != mixingStep)
        {
            PlayFeedbackSound(false);
            DisplayForDuration(errorLabel, currentInteraction.ErrElement, 5, errorLabelPanel);
            errorCount++;
            errorCountLabel.SetText(errorCount.ToString());
        }
    }

    private IEnumerator DelayedTrainingCompletionCallback()
    {
        totalErrorCountLabel.SetText("Fehler: " + errorCount);
        totalHelpCountLabel.SetText("Hilfen: " + helpCount);
        yield return new WaitForSeconds(completionCallbackDelay);
        OnCompleted?.Invoke();
    }

    private Coroutine coroutine;
    private void DisplayForDuration(TextMeshProUGUI label, string msg, float duration, Image panel = null)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        if (msg != "")
        {
            if (panel != null)
            {
                panel.enabled = true;
            }
            label.text = msg;

            coroutine = StartCoroutine(DisableAfterDuration(duration, label, panel));
        }
    }

    private IEnumerator DisableAfterDuration(float duration, TextMeshProUGUI label, Image panel = null)
    {
        yield return new WaitForSeconds(duration);

        if (panel != null)
        {
            panel.enabled = false;
        }
        label.text = "";
    }


    private void CheckTargetRange()
    {
        if (currentInteraction.TargetValueMax != currentInteraction.TargetValueMin)
            { 
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
                    DisplayForDuration(errorLabel, currentInteraction.ErrBelowMin, 5);
                    errorCount++;
                    errorCountLabel.SetText(errorCount.ToString());
                }
                else 
                {
                    DisplayForDuration(errorLabel, currentInteraction.ErrAboveMax, 5);
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

    private bool TargetValueHasRange()
    {
        return currentInteraction.TargetValueMax != currentInteraction.TargetValueMin;
    }

    private void MoveToNextInteraction()
    {
        // Stop Audio
        if (audioSource.isPlaying)
            audioSource.Stop();
        // Invoke Interaction function 'OnExecution' if defines
        currentInteraction.OnExecution?.Invoke();

        interactionIndex++;

        currentInteraction = interactions[interactionIndex];
        StopAllCoroutines();
        if (!headlineLabel.text.Equals(currentInteraction.Headline))
        { 
            SetTextWithFade(1f, headlineLabel, currentInteraction.Headline);
        }
        SetTextWithFade(1f, instructionLabel, currentInteraction.Instruction, setNewAudio: true);
        // errorLabel.SetText(currentInteraction.ErrElement);
        helpLabel.SetText(currentInteraction.HelpMsg);
        helpLabelBonus.SetText(currentInteraction.HelpMsgBonus);

        if (currentInteraction.IsSkippable)
        {
            SetTextWithFade(1f, skipLabel, "Drücke ENTER, um fortzufahren", delay: 3f);
            FadeGraphic(1f, skipLabelPanel, delay: 3f);
        }
        else
        {
            StartCoroutine(FadeGraphicToZeroAlpha(1f, skipLabel));
            StartCoroutine(FadeGraphicToZeroAlpha(1f, skipLabelPanel));
        }
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
                if (audioSource.clip != null)
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
}