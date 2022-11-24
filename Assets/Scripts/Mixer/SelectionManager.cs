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
        
        if (Input.GetMouseButtonUp(0))
            HighlightOnClick();
    }

    private void OnMouseUp()
    {
        
    }

    void HighlightOnHover() 
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
                    selectionRenderer.material.SetColor("_EmissionColor", Color.yellow);
                }
                // currentSelection is highlighted
                currentSelection = selectionRenderer == null ? null : selection;
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
                    if (clickedBefore.GetComponent<MoveFader>() != null)
                        clickedBefore.GetComponent<MoveFader>().isClicked = false;
                    else if (clickedBefore.GetComponent<MoveButton>() != null)
                        clickedBefore.GetComponent<MoveButton>().isClicked = false;
                    else if (clickedBefore.GetComponent<MoveKnob>() != null)
                        clickedBefore.GetComponent<MoveKnob>().isClicked = false;
                }

                clickedObject = selection;
                var selectionRenderer = clickedObject.GetComponent<Renderer>();
                selectionRenderer.material.EnableKeyword("_EMISSION");
                selectionRenderer.material.SetColor("_EmissionColor", Color.yellow);
                if (clickedObject.GetComponent<MoveFader>() != null)
                    clickedObject.GetComponent<MoveFader>().isClicked = true;
                else if (clickedObject.GetComponent<MoveButton>() != null)
                    clickedObject.GetComponent<MoveButton>().isClicked = true;
                else if (clickedObject.GetComponent<MoveKnob>() != null)
                    clickedObject.GetComponent<MoveKnob>().isClicked = true;
            }
            else
            {
                if (clickedObject != null)
                { 
                    clickedObject.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    if (clickedObject.GetComponent<MoveFader>() != null)
                        clickedObject.GetComponent<MoveFader>().isClicked = false;
                    else if (clickedObject.GetComponent<MoveButton>() != null)
                        clickedObject.GetComponent<MoveButton>().isClicked = false;
                    else if (clickedObject.GetComponent<MoveKnob>() != null)
                        clickedObject.GetComponent<MoveKnob>().isClicked = false;
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
