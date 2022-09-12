using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//UNUSED
//test implementation for viewing spectrum
public class AudioSpectrum : MonoBehaviour
{
    public static float spectrumValue { get; private set; }

    private float[] m_audioSpectrum;

    [SerializeField]
    private float multiplier = 100f; //arbitrary value used for denormalizing
    

    // Start is called before the first frame update
    void Start()
    {
        m_audioSpectrum = new float[128]; //needs to be power of 2 size
    }

    // Update is called once per frame
    void Update()
    {
        //fillls audio spectrum
        AudioListener.GetSpectrumData(m_audioSpectrum, 0, FFTWindow.Hamming);

        if (m_audioSpectrum != null && m_audioSpectrum.Length > 0)
        {
            spectrumValue = m_audioSpectrum[0] * multiplier; //change this value for different results
        }
    }
}
