using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    [SerializeField] private string[] selectableTags = new string[] { "Button", "Fader", "Knob", "ChannelList" };

    private Transform currentSelection;
    private Transform clickedObject;
    private TextMeshProUGUI canvasValueText;
    private Image valueTextBackground;
    private float vtbMaxAlpha = 235f/255f;

    private int UILayer;
    public enum Fade {Out = 0, In = 1};

    private void Awake()
    {
        canvasValueText = GameObject.FindGameObjectWithTag("ValueText").GetComponent<TextMeshProUGUI>();
        valueTextBackground = GameObject.Find("ValueTextBackground").GetComponent<Image>();
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {

        HighlightOnHover();

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        { 
            HighlightOnClick();
        }
    }

    private void OnMouseUp()
    {
        
    }

    void HighlightOnHover() 
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // if mouse is not over UI
        {
            if (!Input.GetMouseButton(0))
            {
                // Turn off emission when not selected
                if (currentSelection != null && currentSelection != clickedObject)
                {
                    var selectionRenderer = currentSelection.GetComponent<Renderer>();
                    selectionRenderer.material.DisableKeyword("_EMISSION");
                    currentSelection = null;
                }

                // Send ray to mouse position
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                // if ray hits something
                if (Physics.Raycast(ray, out hit))
                {
                    Debug.DrawLine(ray.origin, hit.point, Color.green); // DEBUG
                    var selection = hit.transform;
                    // check if selected object has tag contained in array
                    if (TransformWithTagIsMovable(selectableTags, selection))
                    {
                        // get renderer and if not undefined enable emission
                        var selectionRenderer = selection.GetComponent<Renderer>();
                        if (selectionRenderer != null)
                        {
                            selectionRenderer.material.EnableKeyword("_EMISSION");
                            selectionRenderer.material.SetColor("_EmissionColor", Color.white);
                        }
                        // currentSelection is highlighted
                        currentSelection = selectionRenderer == null ? null : selection;
                    }
                }
            }
        }
        else // turn off emission on currentSelection when pointer is over UI
        {
            if (currentSelection != null)
            {
                var selectionRenderer = currentSelection.GetComponent<Renderer>();
                selectionRenderer.material.DisableKeyword("_EMISSION");
                currentSelection = null;
            }
        }
    }

    void HighlightOnClick()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            // if ray hits something
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red); // DEBUG
                var selection = hit.transform;
                // only highlight on click, when in selectableTags array, but ignore ChannelList
                if (TransformWithTagIsMovable(selectableTags, selection) && !selection.CompareTag("ChannelList"))
                {
                    FadeValueText(Fade.In, valueTextBackground, canvasValueText);
                    var clickedBefore = clickedObject;
                    if (clickedBefore != null)
                    {
                        clickedBefore.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                        if (clickedBefore.GetComponent<Fader>() != null)
                            clickedBefore.GetComponent<Fader>().isClicked = false;
                        else if (clickedBefore.GetComponent<Button>() != null)
                            clickedBefore.GetComponent<Button>().isClicked = false;
                        else if (clickedBefore.GetComponent<Knob>() != null)
                            clickedBefore.GetComponent<Knob>().isClicked = false;
                    }

                    clickedObject = selection;
                    var selectionRenderer = clickedObject.GetComponent<Renderer>();
                    selectionRenderer.material.EnableKeyword("_EMISSION");
                    selectionRenderer.material.SetColor("_EmissionColor", Color.white);
                    if (clickedObject.GetComponent<Fader>() != null)
                        clickedObject.GetComponent<Fader>().isClicked = true;
                    else if (clickedObject.GetComponent<Button>() != null)
                        clickedObject.GetComponent<Button>().isClicked = true;
                    else if (clickedObject.GetComponent<Knob>() != null)
                        clickedObject.GetComponent<Knob>().isClicked = true;
                }
                else
                {
                    FadeValueText(Fade.Out, valueTextBackground, canvasValueText);
                    if (clickedObject != null)
                    { 
                        clickedObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                        if (clickedObject.GetComponent<Fader>() != null)
                            clickedObject.GetComponent<Fader>().isClicked = false;
                        else if (clickedObject.GetComponent<Button>() != null)
                            clickedObject.GetComponent<Button>().isClicked = false;
                        else if (clickedObject.GetComponent<Knob>() != null)
                            clickedObject.GetComponent<Knob>().isClicked = false;
                        clickedObject = null;
                    }
                }
            }
        }
    }

    bool TransformWithTagIsMovable(string[] tagArray, Transform transform) 
    {
        return System.Array.IndexOf(tagArray, transform.tag) != -1;
    }

    public void FadeValueText(SelectionManager.Fade direction, Image valueTextBackground, TextMeshProUGUI valueText) 
    {

        IEnumerator FadeTextToFullAlpha(float timeInSeconds, Image valueTextBackground, TextMeshProUGUI tmpUGUI)
        {
            yield return FadeTextToZeroAlpha(0.05f, valueTextBackground, canvasValueText);
            while (tmpUGUI.color.a < 1.0f)
            {
                tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b, tmpUGUI.color.a + (Time.deltaTime / timeInSeconds));
                valueTextBackground.color = new Color(valueTextBackground.color.r, valueTextBackground.color.g, valueTextBackground.color.b, Mathf.Clamp(valueTextBackground.color.a + (Time.deltaTime / timeInSeconds), 0, vtbMaxAlpha));
                yield return null;
            }
            valueTextBackground.color = new Color(valueTextBackground.color.r, valueTextBackground.color.g, valueTextBackground.color.b, vtbMaxAlpha);
        }

        IEnumerator FadeTextToZeroAlpha(float timeInSeconds, Image valueTextBackground, TextMeshProUGUI tmpUGUI)
        {
            while (tmpUGUI.color.a > 0.0f)
            {
                tmpUGUI.color = new Color(tmpUGUI.color.r, tmpUGUI.color.g, tmpUGUI.color.b, tmpUGUI.color.a - (Time.deltaTime / timeInSeconds));
                valueTextBackground.color = new Color(valueTextBackground.color.r, valueTextBackground.color.g, valueTextBackground.color.b, Mathf.Clamp(valueTextBackground.color.a - (Time.deltaTime / timeInSeconds), 0, vtbMaxAlpha));
                yield return null;
            }
            valueTextBackground.color = new Color(valueTextBackground.color.r, valueTextBackground.color.g, valueTextBackground.color.b, 0);
        }

        switch ((Fade)direction) 
        {
            case Fade.Out:
                StopAllCoroutines();
                StartCoroutine(FadeTextToZeroAlpha(0.5f, valueTextBackground, canvasValueText));
                break;
            case Fade.In:
                if (currentSelection != clickedObject) 
                { 
                    StopAllCoroutines();
                    StartCoroutine(FadeTextToFullAlpha(0.5f, valueTextBackground, canvasValueText));
                }
                break;
            default:
                StopAllCoroutines();
                StartCoroutine(FadeTextToFullAlpha(0.5f, valueTextBackground, canvasValueText));
                break;
        }

    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaycastResults)
    {
        for (int index = 0; index < eventSystemRaycastResults.Count; index++)
        {
            RaycastResult curRaycastResult = eventSystemRaycastResults[index];
            if (curRaycastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }

    //Gets all event system raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }


}
