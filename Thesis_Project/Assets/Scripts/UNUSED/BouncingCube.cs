using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UNUSED:
//Used as a way to visualize the fequency band of instruments(emphasis on vocals)
//Provided a way to create onset detection of volume hitting a certain threshold
//Deemed to complicated for average users
//[RequireComponent(typeof(AudioSource))]
public class BouncingCube : MonoBehaviour
{
    public GameObject cubeObject;
    public float rmsValue; //average output value of the sound
    public float dbValue; //sound value at exact frame
    public float pitchValue;
    public float avgBandAmplitude;

    public float visualModifier = 150f;
    public float smoothSpeed = 10;

    [SerializeField]
    private AudioSource source;
    private float[] samples;
    private float[] spectrum;
    private float sampleRate;
    public float maxVisualScale = 25f;
    public float keyPercentage = .5f;

    //sampleSize must be a power of 2 (i.e. 1024,  2048) 
    //Defines the frequency band resolution that we are analyzing
    //Each element shows the relative amplitude of the frequency =  N * 24000/Q   Hz ( N = the element index and 24000 is half the audio sampling rate in a typical PC)
    //https://answers.unity.com/questions/175173/audio-analysis-pass-filter.html
    private const int sampleSize = 1024;


    private Transform[] visualList;
    private float[] visualScale;
    private int amtVisual = 1;

    public bool ________________;
    public float kickThreshold = 3.5f;
    public float snareThreshold = 3.5f;
    public float bassThreshold = 3.5f;
    public float vocalThreshold = 3.5f;

    [SerializeField]
    private float fMax; //the maximum allowed frequency
    [SerializeField]
    private float lowCutOff; //the minimum allowed frequency
    [SerializeField]
    private float highCutOff;

    [Header("Cube Settings")]
    [SerializeField]
    private float cubeOffsetX = 0;
    [SerializeField]
    private float cubeOffsetY = 0;

    private int instrumentSelected = 0;
    private int presetSelected = 0;
    private int sensitivityValue = 0;

    [Header("FrequencyBandScaleVariables")]  //scales spectrum view of the band. lower frequencies require smaller values, whereas higher freq bands need higher values
    [SerializeField] private float vocalBandScaler = 200;
    [SerializeField] private float bassBandScaler = 20;
    [SerializeField] private float pianoBandScaler = 10;
    [SerializeField] private float guitarBandScaler = 10;


    // Start is called before the first frame update
    void Start()
    {
        //source = GetComponent<AudioSource>();
        samples = new float[sampleSize];
        spectrum = new float[sampleSize];
        sampleRate = AudioSettings.outputSampleRate;

        fMax = AudioSettings.outputSampleRate / 2; //the highest frequency the clip will output

    }

    // Update is called once per frame
    void Update()
    {
        instrumentSelected = DropDownMgr.GetSelectedInstrument();
        presetSelected = DropDownMgr.GetSelectedPreset();
        AnalyzeSound();
    
    }


    private void AnalyzeSound() //float freqLow, float freqHigh
    {
        source.GetOutputData(samples, 0);


        float sum = 0;
        for (int i = 0; i < sampleSize; i++)
        {
            sum = samples[i] * samples[i];
        }

        rmsValue = Mathf.Sqrt(sum / sampleSize);
        dbValue = 20 * Mathf.Log10(rmsValue / .1f);

        //Get the sound spectrum
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        float[] freqBands = new float[amtVisual];

        int freqBandWidth = spectrum.Length / amtVisual;
        int bandIndex = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (i != 0 && i % freqBandWidth == 0)
            {
                bandIndex++;
            }

            freqBands[bandIndex] += spectrum[i];

        }

    
        //check instrument selected
        switch (instrumentSelected)
        {
            case 0: //voice
                switch (presetSelected)    //TODO: Fill all preset values out
                {
                    
                    case 0: //M Rock
                            // freqBands[0] = spectrum[28] + spectrum[29] + spectrum[352] + spectrum[30];  //male rock

                        //Male freqs (hz): 900, 1498, 1520, 1560, 1835 (additional: 1372, 999)
                        //Bands = Freq/ 19.53125
                        //Spectrum indexes = 46.08, 76.697, 77.824, 93.952 (additional 70 : 52 )
                        //extras for added vocal coverage 76, 79, 81, 93, 95
                        //seems to work better with incrementing operations in here
                        freqBands[0] =  spectrum[47]  + + spectrum[76] + spectrum[77] + spectrum[78] + + spectrum[79] + spectrum[80] + spectrum[81] + spectrum[93]  + spectrum[94] + spectrum[95];
                        break;
                    case 1: //F ROCK
                        //Female Freqs (hz): 1092, 1520, 1835, 1372, 1498
                        //Bands = Freq/ 19.53125
                        //spectrum indexes =  55.91, 76.6976, 77.824, 93.952, 70.246, 
                        //extras for added vocal coverage: 55, 57, 76, 79, 93, 95, 70, 72
                        freqBands[0] =  spectrum[56] + spectrum[57]  +   spectrum[76] +  spectrum[77] + + spectrum[78] +  spectrum[79] + spectrum[80] +spectrum[81] + spectrum[93] +   spectrum[94] + + spectrum[70] + spectrum[71] + spectrum[72];  

                        break;
                    case 2: //M POP
                        break;
                    case 3: //F POP
                        break;
                    case 4: //other
                        break;
                }
                break;
            case 1: //bass
                switch (presetSelected)
                {
                    case 0: //Rock
                        freqBands[0] = spectrum[6] = spectrum[7] + spectrum[9]; //bass guitar
                        break;
                    case 1: //Pop
                        break;
                    case 2: //EDM
                        break;
                    case 3: //Country
                        break;
                    case 4: //Funk
                        break;
                }
                break;
            case 2: //piano
                switch (presetSelected)
                {
                    case 0: //Rock
                        break;
                    case 1: //Pop
                        break;
                    case 2://Classical
                        break;
                    case 3: //EDM
                        break;
                    case 4: //Blues
                        break;
                }
                break;
            case 3: //guitar 
                switch (presetSelected)
                {
                    case 0: //Rock
                        break;
                    case 1: //Country
                        break;
                    case 2: //Blues
                        break;
                    case 3: //Jazz
                        break;
                    case 4: //Metal
                        break; 
                }
                freqBands[0] = spectrum[204] + spectrum[205] + spectrum[206] + spectrum[207]; //guitar 
                break;
        }

        /* freqBands[0] = spectrum[0] + spectrum[2] + spectrum[4];
         freqBands[1] = spectrum[10] + spectrum[11] + spectrum[12];
         freqBands[2] = spectrum[20] + spectrum[21] + spectrum[22];
         freqBands[3] = spectrum[40] + spectrum[41] + spectrum[42] + spectrum[43];
         freqBands[4] = spectrum[80] + spectrum[81] + spectrum[82] + spectrum[83];
         freqBands[5] = spectrum[160] + spectrum[161] + spectrum[162] + spectrum[163];
          freqBands[6] = spectrum[320] + spectrum[321] + spectrum[322] + spectrum[323];*/

        //grab a sampling of wavelengths that typically characterize each instrument in the frequency spectrum
        //preset for rock music
        //freqBands[0] = spectrum[0] + spectrum[2] + spectrum[4] + spectrum[5]; //kick
       /* freqBands[1] = spectrum[6] = spectrum[7] + spectrum[9]; //bass guitar
        freqBands[2] = spectrum[11] + spectrum[19] + spectrum[8]; //snare 12 works well here too stil some crossover between vocals and snare here.....
        freqBands[3] = spectrum[204] + spectrum[205] + spectrum[206] + spectrum[207]; //guitar
        freqBands[4] = spectrum[28] + spectrum[29] + spectrum[352] + spectrum[30]; //male voice
        freqBands[5] = spectrum[400] + spectrum[401] + spectrum[402] + spectrum[403]; //crash
        freqBands[6] = spectrum[232] + spectrum[417] + spectrum[305]; //  spectrum[342] + spectrum[343]; //vocals    6.8k, 6.82k, 6.84k, 6.3k
        */
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("CUBE");
        for (int i = 0; i < cubes.Length; i++)
        {

            /*if(i < 2)
                    cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[i] * 5, 0.5f);
            else if(i < 4)
                cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[i] * 10, 0.5f);
            else if (i < 6)
                cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[i] * 10, 0.5f);
            else if (i < 8)
                cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[i] * 20, 0.5f);
            else if (i < 10)
                cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[i] * 40, 0.5f);
            else if (i < 12)
                cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[i] * 80, 0.5f);
            else
                cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[i] * 160, 0.5f);
            */
            Vector3 newScale;
            calculateThreshold(instrumentSelected);

            //Scales volume based on frequency ranges
            switch (instrumentSelected)
            {
                case 0: //vocals
                    newScale = new Vector3(1, freqBands[0] * vocalBandScaler, 0.5f);
                    cubes[i].gameObject.transform.localScale = newScale; //vocals
                    if (newScale.y > vocalThreshold)
                        cubes[0].GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                    else
                        cubes[0].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    break;

                case 1: //bass guitar
                    newScale = new Vector3(1, freqBands[0] * bassBandScaler, 0.5f);
                    cubes[i].gameObject.transform.localScale = newScale;// bass guitar
                    if (newScale.y > bassThreshold)
                        cubes[0].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                    else
                        cubes[0].GetComponent<Renderer>().material.SetColor("_Color", Color.white);

                    break;
           
                case 2: //piano
                    cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[0] * pianoBandScaler, 0.5f);
                    break;
                case 3: //guitar
                    cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[0] * guitarBandScaler, 0.5f); //*tsk tsk tsk
                    break;
                
            }
        }

        ////////find pitch
        float maxV = 0; //stroes greatest amplitude
        var maxN = 0; //maximumIndex where greatest amplitude is detected
        // This is for coded for analyzing over the entire sound spectrum
        //finds which frequency band has the greatest amplitude
        for (int i = 0; i < sampleSize; i++)
        {
            if (!(spectrum[i] > maxV) || !(spectrum[i] > 0.0f))
                continue;

            maxV = spectrum[i];
            maxN = i;
        }

        float freqN = maxN;
        if (maxN > 0 && maxN < sampleSize - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN]; //gets the frequency band volume to the left of the maximum frequency  band and divides it by the maximum frquency volume
            var dR = spectrum[maxN + 1] / spectrum[maxN]; //gets the frequency band volume to the right of the maximum frequency and didivides it by the max frequency band
            freqN += .5f * (dR * dR - dL * dL);
        }
        pitchValue = freqN * (sampleRate / 2) / sampleSize;

    }


    public void calculateThreshold(int instrument)
    {
        switch (instrument)
        {
            case 0: //vocals
                
                break;
            case 1: //bass guitar

                break;
            case 2: //piano
                break;
            case 3: //guitar
                break;
        }
    }

    private float BandVol(float fLow, float fHigh)
    {

        fLow = Mathf.Clamp(fLow, 20, fMax); // limit low...
        fHigh = Mathf.Clamp(fHigh, fLow, fMax); // and high frequencies
        // get spectrum
        source.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        //calculates which band the low and high frequencies fall in
        int lowFreqBand = (int)(Mathf.Floor(fLow * sampleSize / fMax));
        int highFreqBand = (int)(Mathf.Floor(fHigh * sampleSize / fMax));
        float sum = 0;
        // average the volumes of frequencies fLow to fHigh
        for (int i = lowFreqBand; i <= highFreqBand; i++)
        {
            sum += samples[i];
        }
        return sum / (highFreqBand - lowFreqBand + 1); //returns average volume over the whole frequency band
    }
}
