using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    AudioSource[] audioSources;
    [SerializeField]
    private Texture speakerOn;
    [SerializeField]
    private Texture speakerOff;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] k = GameObject.FindGameObjectsWithTag("key");
        audioSources = new AudioSource[k.Length];
        for (int i = 0; i < k.Length; i++)
        {
            audioSources[i] = k[i].GetComponent<AudioSource>();
        }
    }

    public void onClick()
    {
        foreach (AudioSource a in audioSources)
        {
            a.mute = !a.mute;
        }
        if (audioSources[0].mute)
            this.GetComponentInChildren<RawImage>().texture = speakerOff;
        else
            this.GetComponentInChildren<RawImage>().texture = speakerOn;
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
