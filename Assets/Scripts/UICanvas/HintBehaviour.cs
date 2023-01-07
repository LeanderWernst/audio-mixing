using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintBehaviour : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI hintCountTMP;
    private int hintCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addToHintCount() 
    {
        hintCount += 1;
        hintCountTMP.text = hintCount.ToString();
    }

    public int getHintCount()
    {
        return hintCount;
    }
}
