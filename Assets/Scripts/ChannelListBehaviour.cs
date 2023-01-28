using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChannelListBehaviour : MonoBehaviour
{

    [SerializeField] GameObject channelListUI;
    [SerializeField] AudioClip showList;
    [SerializeField] AudioClip hideList;
    [SerializeField] InteractionManager interactionManager;
    private bool listIsVisible = false;
    private bool wasClickedAtStep = false;
    private ValueStorage valueStorage;

    private void Awake()
    {
        channelListUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        valueStorage = gameObject.GetComponent<ValueStorage>();
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
        if (EventSystem.current.IsPointerOverGameObject()) { return; }
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
            confirmInteractionStep();
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

    /**
     * If the current interaction step is to look at this channelList,
     * it informs the interaction manager as soon as the list is closed again.
     * To only inform once, it uses a flag 'wasClickedAtStep'.
     */
    private void confirmInteractionStep()
    {
        if (interactionManager.GetCurrentInteraction().TargetObject == gameObject && !wasClickedAtStep)
        {
            wasClickedAtStep = true;
            valueStorage.SetValue(1f, gameObject); // will be checked from interactionManager
            interactionManager.CheckInteractionOrder(gameObject);
        }
    }
}
