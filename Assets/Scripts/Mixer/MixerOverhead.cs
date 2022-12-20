using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixerOverhead : MonoBehaviour
{

    ChannelScript channel1;
    ChannelScript channel2;
    ChannelScript channel3;
    ChannelScript channel4;
    ChannelScript channel5;
    ChannelScript channel6;
    ChannelScript channel7;
    ChannelScript channel8;
    ChannelScript channel9;
    ChannelScript channel10;
    ChannelScript channel11;
    ChannelScript channel12;
    ChannelScript channel13;
    ChannelScript channel14;
    ChannelScript channel15;
    ChannelScript channel16;
    ChannelScript channel17;
    ChannelScript channel18;
    ChannelScript channel19;
    ChannelScript channel20;
    ChannelScript channel21;
    ChannelScript channel22;
    ChannelScript channel23;
    ChannelScript channel24;

    StereoInputScript stereoLine1;
    StereoInputScript stereoLine2;
    StereoInputScript stereoLine3;
    StereoInputScript stereoLine4;

    MasterScript masterChannel;

    private void Awake()
    {
        channel1 = GameObject.Find("Midas/Channel1").GetComponent<ChannelScript>();
        channel2 = GameObject.Find("Midas/Channel2").GetComponent<ChannelScript>();
        channel3 = GameObject.Find("Midas/Channel3").GetComponent<ChannelScript>();
        channel4 = GameObject.Find("Midas/Channel4").GetComponent<ChannelScript>();
        channel5 = GameObject.Find("Midas/Channel5").GetComponent<ChannelScript>();
        channel6 = GameObject.Find("Midas/Channel6").GetComponent<ChannelScript>();
        channel7 = GameObject.Find("Midas/Channel7").GetComponent<ChannelScript>();
        channel8 = GameObject.Find("Midas/Channel8").GetComponent<ChannelScript>();
        channel9 = GameObject.Find("Midas/Channel9").GetComponent<ChannelScript>();
        channel10 = GameObject.Find("Midas/Channel10").GetComponent<ChannelScript>();
        channel11 = GameObject.Find("Midas/Channel11").GetComponent<ChannelScript>();
        channel12 = GameObject.Find("Midas/Channel12").GetComponent<ChannelScript>();
        channel13 = GameObject.Find("Midas/Channel13").GetComponent<ChannelScript>();
        channel14 = GameObject.Find("Midas/Channel14").GetComponent<ChannelScript>();
        channel15 = GameObject.Find("Midas/Channel15").GetComponent<ChannelScript>();
        channel16 = GameObject.Find("Midas/Channel16").GetComponent<ChannelScript>();
        channel17 = GameObject.Find("Midas/Channel17").GetComponent<ChannelScript>();
        channel18 = GameObject.Find("Midas/Channel18").GetComponent<ChannelScript>();
        channel19 = GameObject.Find("Midas/Channel19").GetComponent<ChannelScript>();
        channel20 = GameObject.Find("Midas/Channel20").GetComponent<ChannelScript>();
        channel21 = GameObject.Find("Midas/Channel21").GetComponent<ChannelScript>();
        channel22 = GameObject.Find("Midas/Channel22").GetComponent<ChannelScript>();
        channel23 = GameObject.Find("Midas/Channel23").GetComponent<ChannelScript>();
        channel24 = GameObject.Find("Midas/Channel24").GetComponent<ChannelScript>();

        stereoLine1 = GameObject.Find("Midas/StereoInput1").GetComponent<StereoInputScript>();
        stereoLine2 = GameObject.Find("Midas/StereoInput2").GetComponent<StereoInputScript>();
        stereoLine3 = GameObject.Find("Midas/StereoInput3").GetComponent<StereoInputScript>();
        stereoLine4 = GameObject.Find("Midas/StereoInput4").GetComponent<StereoInputScript>();

        masterChannel = GameObject.Find("Midas/Master").GetComponent<MasterScript>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
