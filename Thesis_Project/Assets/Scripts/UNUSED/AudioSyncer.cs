using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UNUSED:
//was an experimental beat detector implementation
//dropped in favor of spectrum viewer and then later bouncing cube
public class AudioSyncer : MonoBehaviour
{
    public float bias; //which spectrum value triggers a beat
    public float timeStep; //the minimum interval between each ebat
    public float timeToBeat; //how much time before beat event completes
    public float restSmoothTime; //how fast object goes to rest after a beat

    //used to determines if value went above or below a bias in the current frame
    private float m_previousAudioValue;
    private float m_audioValue;


    private float m_timer; //keeps track of time step interval

    protected bool m_isBeat;//if currently in a beat state



    public virtual void OnBeat()
    {
        Debug.Log("beat");
        m_timer = 0;
        m_isBeat = true;
    }

    public virtual void OnUpdate()
    {
        m_previousAudioValue = m_audioValue;
        m_audioValue = AudioSpectrum.spectrumValue;

        //current value went below bias trigger
        if (m_previousAudioValue > bias &&
            m_audioValue <= bias)
        {
            if (m_timer > timeStep)
                OnBeat();
        }

        //current beat went above bias trigger
        if (m_previousAudioValue <= bias &&
            m_audioValue > bias)
        {
            if (m_timer > timeStep)
                OnBeat();
        }

        m_timer += Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

}
