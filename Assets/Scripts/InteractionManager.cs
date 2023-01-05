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
            instructionLabel.SetText(currentInteraction.Instruction);
            audioSource.clip = currentInteraction.InstrAudio;
            audioSource.Play();
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

                helpCountLabel.SetText("Hilfen: " + helpCount);

                StopHelpAndErrorDisplay();
                StartCoroutine(DisplayForDuration(helpLabel, currentInteraction.HelpMsg, 5));
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

        private void CheckInteractionOrder(GameObject selectedGameObject)
        {
            if (InteractionsCompleted)
                return;

            if (!selectedGameObject)
                return;

            if (selectedGameObject.Equals(currentInteraction.TargetObject))
            {
                StopHelpAndErrorDisplay();
                currentInteraction.OnExecution?.Invoke();
                interactionIndex++;

                if (InteractionsCompleted)
                {
                    // Code that should be executed at the end of the training goes here
                    StartCoroutine(DelayedTrainingCompletionCallback());
                    return;
                }

                currentInteraction = interactions[interactionIndex];
                instructionLabel.SetText(currentInteraction.Instruction);
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
            StopAllCoroutines();
            helpLabel.SetText("");
            errorLabel.SetText("");
        }
    }
}