using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//UNUSED:
//Used to visualize different parts of the spectrum as a proof of concept
//[RequireComponent(typeof(AudioSource))]
public class SpectrumTest : MonoBehaviour
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
    private int amtVisual = 8;

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

    // Start is called before the first frame update
    void Start()
    {
        //source = GetComponent<AudioSource>();
        samples = new float[sampleSize];
        spectrum = new float[sampleSize];
        sampleRate = AudioSettings.outputSampleRate;

        fMax = AudioSettings.outputSampleRate / 2; //the highest frequency the clip will output

        SpawnLine();
    }

    // Update is called once per frame
    void Update()
    {
        AnalyzeSound();
        //UpdateVisual();
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


        /* freqBands[0] = spectrum[0] + spectrum[2] + spectrum[4];
         freqBands[1] = spectrum[10] + spectrum[11] + spectrum[12];
         freqBands[2] = spectrum[20] + spectrum[21] + spectrum[22];
         freqBands[3] = spectrum[40] + spectrum[41] + spectrum[42] + spectrum[43];
         freqBands[4] = spectrum[80] + spectrum[81] + spectrum[82] + spectrum[83];
         freqBands[5] = spectrum[160] + spectrum[161] + spectrum[162] + spectrum[163];
          freqBands[6] = spectrum[320] + spectrum[321] + spectrum[322] + spectrum[323];*/

        //grab a sampling of wavelengths that typically characterize each instrument in the frequency spectrum
        //preset for rock music
        freqBands[0] = spectrum[0] + spectrum[2] + spectrum[4] + spectrum[5]; //kick
        freqBands[1] = spectrum[6] = spectrum[7] + spectrum[9]; //bass guitar
        freqBands[2] = spectrum[11] + spectrum[19] + spectrum[8]; //snare 12 works well here too stil some crossover between vocals and snare here.....
        freqBands[3] = spectrum[204] + spectrum[205] + spectrum[206] + spectrum[207]; //guitar
        freqBands[4] = spectrum[28] + spectrum[29] + spectrum[352] + spectrum[30]; //male voice
        freqBands[5] = spectrum[400] + spectrum[401] + spectrum[402] + spectrum[403]; //crash
        freqBands[6] = spectrum[232] + spectrum[417] + spectrum[305]; //  spectrum[342] + spectrum[343]; //vocals    6.8k, 6.82k, 6.84k, 6.3k
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("CUBE");
        for (int i = 1; i < cubes.Length; i++)
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
            switch (i)
            {
                case 1:
                    newScale = new Vector3(1, freqBands[0] * 10, 0.5f);
                    cubes[i].gameObject.transform.localScale = newScale; // base drum
                    if (newScale.y > kickThreshold)
                        cubes[i].GetComponent<Renderer>().material.SetColor("_Color", Color.red);
                    else
                        cubes[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    break;
                case 2:
                    newScale = new Vector3(1, freqBands[1] * 20, 0.5f);
                    cubes[i].gameObject.transform.localScale = newScale;// bass guitar
                    if (newScale.y > bassThreshold)
                        cubes[i].GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
                    else
                        cubes[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);

                    break;
                case 3:
                    newScale = new Vector3(1, freqBands[2] * 40, 0.5f);
                    cubes[i].gameObject.transform.localScale = newScale; //snare drum
                    if (newScale.y > snareThreshold)
                        cubes[i].GetComponent<Renderer>().material.SetColor("_Color", Color.green);
                    else
                        cubes[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    break;
                case 4:
                     
                    cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[3] * 80, 0.5f); 
                    break;
                case 5:
                    newScale = new Vector3(1, freqBands[4] * 200, 0.5f);
                    cubes[i].gameObject.transform.localScale = newScale; //vocals
                    if (newScale.y > vocalThreshold)
                        cubes[i].GetComponent<Renderer>().material.SetColor("_Color", Color.cyan);
                    else
                        cubes[i].GetComponent<Renderer>().material.SetColor("_Color", Color.white);
                    break;
                case 6:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[5] * 400, 0.5f);
                    break;
                case 7:
                    cubes[i].gameObject.transform.localScale = new Vector3(1, freqBands[6] * 600, 0.5f); //*tsk tsk tsk
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

    private void SpawnLine()
    {
        visualScale = new float[amtVisual];
        visualList = new Transform[amtVisual];

        for (int i = 0; i < amtVisual; i++)
        {
            GameObject go = GameObject.Instantiate(cubeObject) as GameObject;
            visualList[i] = go.transform;
            visualList[i].position = Vector3.right * i;

        }
    }

    private void UpdateVisual()
    {
        int visualIndex = 0;
        int spectrumIndex = 0;
        int averageSize = (int)(sampleSize * keyPercentage) / amtVisual;

        while (visualIndex < amtVisual)
        {
            int j = 0;
            float sum = 0;
            while (j < averageSize)
            {
                sum += spectrum[spectrumIndex];
                spectrumIndex++;
                j++;
            }

            float scaleY = sum / averageSize * visualModifier;
            visualScale[visualIndex] = Time.deltaTime * smoothSpeed;
            if (visualScale[visualIndex] < scaleY)
                visualScale[visualIndex] = scaleY;

            if (visualScale[visualIndex] > maxVisualScale)
                visualScale[visualIndex] = maxVisualScale;

            visualList[visualIndex].localScale = Vector3.one + Vector3.up * visualScale[visualIndex];
            visualIndex++;
            
        }
    }

    private float BandVol(float fLow, float fHigh) 
    {
 
        fLow = Mathf.Clamp(fLow, 20, fMax); // limit low...
        fHigh = Mathf.Clamp(fHigh, fLow, fMax); // and high frequencies
        // get spectrum
        source.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);

        //calculates which band the low and high frequencies fall in
        int lowFreqBand = (int)(Mathf.Floor(fLow* sampleSize / fMax)); 
        int  highFreqBand = (int)(Mathf.Floor(fHigh* sampleSize / fMax));
        float sum = 0;
        // average the volumes of frequencies fLow to fHigh
        for (int i=lowFreqBand ; i<=highFreqBand; i++){
            sum += samples[i];
        }
        return sum / (highFreqBand - lowFreqBand + 1); //returns average volume over the whole frequency band
    }
}
