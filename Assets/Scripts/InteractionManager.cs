using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace haw.unitytutorium.w22
{
    public class InteractionManager : MonoBehaviour
    {
        [Header("User Interface")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private TextMeshProUGUI headlineLabel;
        [SerializeField] private TextMeshProUGUI instructionLabel;
        [SerializeField] private TextMeshProUGUI errorLabel;
        [SerializeField] private TextMeshProUGUI helpLabel;
        [SerializeField] private TextMeshProUGUI helpLabelBonus;

        [SerializeField] private TextMeshProUGUI errorCountLabel = null;
        [SerializeField] private TextMeshProUGUI helpCountLabel = null;

        [SerializeField] private TextMeshProUGUI totalErrorCountLabel;
        [SerializeField] private TextMeshProUGUI totalHelpCountLabel;

        [SerializeField] private TextMeshProUGUI skipLabel;


        [Header("Raycast")]
        [SerializeField] private LayerMask layerMask;
        private Camera cam;

        [Header("Interactions")]
        [SerializeField] private List<Interaction> interactions;

        [SerializeField] private UnityEvent OnCompleted;
        [SerializeField] private float completionCallbackDelay;

        public bool InteractionsCompleted => interactionIndex >= interactions.Count;
        private int interactionIndex;
        private Interaction currentInteraction;

        private int errorCount;
        private int helpCount;

        private void Awake() => cam = Camera.main;

        private void Start()
        {
            Debug.Log("Start InteractionManager");
            // UI init
            //helpLabel.SetText("");
            //errorLabel.SetText("");
            errorCountLabel.SetText(errorCount.ToString());
            helpCountLabel.SetText(helpCount.ToString());

            // We display a little warning if no interactions are defined in the interaction manager ... just to annoy ourselves :D
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
            if (currentInteraction.IsSkippable)
                skipLabel.color = new Color(skipLabel.color.r, skipLabel.color.g, skipLabel.color.b, 255f);
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
                    CheckInteractionOrder(hit.transform.gameObject);
                }
            }

            // Users can request help with the H key as long as we still have "open" interactions (the training is not completed).
            if (Input.GetKeyDown(KeyCode.H) && !InteractionsCompleted)
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

                StopHelpAndErrorDisplay();
                StartCoroutine(DisplayForDuration(helpLabel, currentInteraction.HelpMsg, 5));
            }

            if (Input.GetKeyDown(KeyCode.Return) && currentInteraction.IsSkippable)
                MoveToNextInteraction();
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

        private void CheckInteractionOrder(GameObject selectedGameObject)
        {
            /*Debug.Log(selectedGameObject.GetComponent<Fader>()?.value);
            Debug.Log(selectedGameObject.GetComponent<Button>()?.isOn);
            Debug.Log(selectedGameObject.GetComponent<Knob>()?.value);*/

            if (InteractionsCompleted)
                return;

            if (!selectedGameObject)
                return;

            if (selectedGameObject.Equals(currentInteraction.TargetObject))
            {
                if (currentInteraction.TargetObject.GetComponent<ValueStorage>().GetValue() == currentInteraction.TargetValue)
                    MoveToNextInteraction();

                if (InteractionsCompleted)
                {
                    // Code that should be executed at the end of the training goes here
                    StartCoroutine(DelayedTrainingCompletionCallback());
                    return;
                }

            }
            else
            {
                StopHelpAndErrorDisplay();
                StartCoroutine(DisplayForDuration(errorLabel, currentInteraction.ErrorMsg, 5));
                errorCount++;
                errorCountLabel.SetText("Fehler: " + errorCount);
            }
        }

        private IEnumerator DelayedTrainingCompletionCallback()
        {
            totalErrorCountLabel.SetText("Fehler: " + errorCount);
            totalHelpCountLabel.SetText("Hilfen: " + helpCount);
            yield return new WaitForSeconds(completionCallbackDelay);
            OnCompleted?.Invoke();
        }

        private IEnumerator DisplayForDuration(TextMeshProUGUI label, string msg, float duration)
        {
            label.text = msg;
            yield return new WaitForSeconds(duration);
            label.text = "";
        }

        private void StopHelpAndErrorDisplay()
        {
            //StopAllCoroutines();
            //helpLabel.SetText("");
            //errorLabel.SetText("");
        }

        private void MoveToNextInteraction()
        {
            StopHelpAndErrorDisplay();
            if (audioSource.isPlaying)
                audioSource.Stop();
            currentInteraction.OnExecution?.Invoke();

            interactionIndex++;

            currentInteraction = interactions[interactionIndex];
            StopAllCoroutines();
            if (!headlineLabel.text.Equals(currentInteraction.Headline))
            { 
                SetTextWithFade(1f, headlineLabel, currentInteraction.Headline);
            }
            SetTextWithFade(1f, instructionLabel, currentInteraction.Instruction);
            errorLabel.SetText(currentInteraction.ErrorMsg);
            helpLabel.SetText(currentInteraction.HelpMsg);
            helpLabelBonus.SetText(currentInteraction.HelpMsgBonus);

            if (currentInteraction.IsSkippable)
                skipLabel.color = new Color(skipLabel.color.r, skipLabel.color.g, skipLabel.color.b, 255f);
            else
                skipLabel.color = new Color(skipLabel.color.r, skipLabel.color.g, skipLabel.color.b, 0f);
        }

        private void SetTextWithFade(float seconds, TextMeshProUGUI textMesh, string newText)
        {
            StartCoroutine(FadeTextToFullAlpha(seconds, textMesh, newText));

            IEnumerator FadeTextToFullAlpha(float timeInSeconds, TextMeshProUGUI tmpUGUI, string newText)
            {
                yield return FadeTextToZeroAlpha(timeInSeconds, tmpUGUI);
                tmpUGUI.SetText(newText);
                audioSource.clip = currentInteraction.InstrAudio;
                if (audioSource.clip != null)
                    audioSource.Play();
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

        IEnumerator WaitThenPlaySound(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            if (audioSource.clip != null)
                audioSource.Play();
        }
    }
}