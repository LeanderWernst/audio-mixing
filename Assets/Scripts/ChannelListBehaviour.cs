using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChannelListBehaviour : MonoBehaviour
{

    [SerializeField] GameObject channelListUI;
    [SerializeField] AudioClip showList;
    [SerializeField] AudioClip hideList;
    private bool listIsVisible = false;

    private void Awake()
    {
        channelListUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && channelListUI.activeInHierarchy)
        {
            toggleChannelList();
        }
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) { return;  }
        toggleChannelList();
    }

    public void toggleChannelList()
    {
        listIsVisible = !listIsVisible;
        if (listIsVisible)
        {
            channelListUI.SetActive(true);
        }
        else
        {
            channelListUI.SetActive(false);
        }
        playChannelListSound();
    }

    private void playChannelListSound()
    {
        AudioSource tempAudioSource = gameObject.AddComponent<AudioSource>();
        if (listIsVisible)
        {
            tempAudioSource.clip = showList;
            tempAudioSource.Play();
        }
        else
        {
            tempAudioSource.clip = hideList;
            tempAudioSource.Play();
        }
        Destroy(tempAudioSource, tempAudioSource.clip.length);
    }
}
