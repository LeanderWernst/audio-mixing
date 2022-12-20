using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{

    [SerializeField] private string[] selectableTags = new string[] { "Button", "Fader", "Knob" };

    private Transform currentSelection;
    private Transform clickedObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HighlightOnHover();
        
        if (Input.GetMouseButtonDown(0))
            HighlightOnClick();
    }

    private void OnMouseUp()
    {
        
    }

    void HighlightOnHover() 
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

    void HighlightOnClick()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // if ray hits something
        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red); // DEBUG
            var selection = hit.transform;
            if (TransformWithTagIsMovable(selectableTags, selection))
            {
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

    bool TransformWithTagIsMovable(string[] tagArray, Transform transform) 
    {
        return System.Array.IndexOf(tagArray, transform.tag) != -1;
    }

}
