using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
/*Frequency List:
 * C4 = 262
 * C#4 = 277.18
 * D4 = 294
 * D#4 = 311
 * E4 = 330
 * F4 = 349
 * F#4 = 370
 * G4 = 392
 * G#4 = 415.3
 * A4 = 440
 * A#4 = 466
 * B4 = 494
 * C5 = 523
 * 
*/





public class NumericalKeys : MonoBehaviour
{
    //musical variables
    public string keyLetter = "a";  //set in inspector
    public float frequency = 262;  //set in inspector

    float downTime = 0;
    float downAuthTime = 0;
    int currentIndex;
    int currentAuthIndex;
    public AudioSource aS;    //NOT USED
    AudioClip pianoKey;      //NOT USED



    //button variables
    Graphic targetGraphic;
    Color normalColor;
    Color selectedColor;
    Button button;

    static int currentMode = 0; //default to freePlay
    int recordingIndex;

    void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = false;
        targetGraphic = GetComponent<Graphic>();

        //get target colors from button component values in inspector
        ColorBlock cb = button.colors;
        cb.disabledColor = cb.normalColor;
        normalColor = cb.normalColor;
        selectedColor = cb.selectedColor;
        button.colors = cb;
    }

    //Resets key pressed status on startup
    void Start()
    {
         //NOT USED
        //Creates a sine wave sample for the specified frequency in the instpector that plays back when you press the key.
        //aS = this.GetComponent<AudioSource>();
        /*
        int sampleFreq = 44000;


        float[] samples = new float[44000];
        for (int i = 0; i < samples.Length; i++)
        {
            samples[i] = Mathf.Sin(Mathf.PI * 2 * i * frequency / sampleFreq);
        }
        pianoKey = AudioClip.Create("Test", samples.Length, 1, sampleFreq, false);
        pianoKey.SetData(samples, 0);
        */
        //aS.clip = pianoKey;
        //aS.loop = true;

        button = GetComponent<Button>();
        button.targetGraphic = null;
        keyUp();

    }

    // Update is called once per frame
    //Checks for key presses
    void Update()
    {

        if (Input.GetKeyDown(keyLetter))
        {
            //aS.Play();
            keyDown();


            if (currentMode == 1) //recording mode
            {

                downTime = Time.time;
                PassMaster.NotePlayed(this);


            }

            else if (currentMode == 2) //practice or sign in mode
            {

                downAuthTime = Time.time;
                PassMaster.NoteAuthPlayed(this);


            }

            //controller.registerInput(keyLetter);

        }
        if (Input.GetKeyUp(keyLetter))
        {
            //aS.Stop();
            keyUp();
            if (currentMode == 1)  //record duration of key press
            {

                PassMaster.NoteReleased(currentIndex, Time.time - downTime);

            }

            if (currentMode == 2)  //check duration of key press against recorded durection
            {

                PassMaster.NoteAuthReleased(currentAuthIndex, Time.time - downAuthTime);

            }


        }

    }


    //set current index in recorded password sequence
    public void setCurrentNoteIndex(int index)
    {
        currentIndex = index;
    }

    //set current index in sequence we are testing against
    public void setCurrentNoteAuthIndex(int index)
    {
        currentAuthIndex = index;
    }

    //update key responses to current selected mode in menu
    public static void changeMode(int mode)
    {
        currentMode = mode;
    }

    //NOT USED: Create a synthesized sine wav sample and play it back
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool Beep(uint dwFreq, uint dwDuration);

    public static void PlayBeep(uint iFrequency, uint iDuration)
    {
        Beep(iFrequency, iDuration);
    }

    //NOT USED
    public void PlayKey(uint iDur)
    {
        PlayBeep((uint)frequency, iDur);
    }





    //color change code
    //revert to normal color on key up
    public void keyUp()
    {
        StartColorTween(button.colors.normalColor, false);
    }
    //change to pressed color on key down
    void keyDown()
    {
        StartColorTween(button.colors.pressedColor, false);
        button.onClick.Invoke();
    }

    //fades collors into each other from normal to pressed for a smooth look
    void StartColorTween(Color targetColor, bool instant)
    {
        if (targetGraphic == null)
            return;

        targetGraphic.CrossFadeColor(targetColor, instant ? 0f : button.colors.fadeDuration, true, true);
    }
}
