using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//TODO: 

//check authentication somehow
public class PassMaster : MonoBehaviour
{
    public static float timingMargin = .05f;//play with this for sensitivity in authentication

    static int currentMode = 0; //default mode is FreePLay
    static PassMaster master;

    //password variables
    static float[] durations;
    static string[] keyNumbers;
    static int currentIndex = 0;
    static int currentAuthIndex = 0;
    static PianoKeys[] keys;

    static float[] durAuth;
    static string[] keyLetterAuth;
    static bool isFailAuth = true;
    
    //text variables that change with mode
    private static TMP_Text prompt;
    private static Button recButton;
    private static Button freePlayButton;
    private static Button signInButton;
    //serialization helpers
    //DO NOT USE FOR ANYTHING ELSE
    [SerializeField]
    private TMP_Text sPrompt;

    [SerializeField]
    private Button sFreePlayButton;
    [SerializeField]
    private Button sRecButton;
    [SerializeField]
    private Button sPracticeButton;

    private static ModeButton[] buttons;

    private void Awake()
    {
        master = this;
    }

    void Start()
    {
        prompt = sPrompt;
        recButton = sRecButton;
        freePlayButton = sFreePlayButton;
        signInButton = sPracticeButton;

        buttons = new ModeButton[3];
        buttons[0] = recButton.GetComponent<ModeButton>();
        buttons[1] = freePlayButton.GetComponent<ModeButton>();
        buttons[2] = signInButton.GetComponent<ModeButton>();

        durations = new float[50];
        keyNumbers = new string[50];

        durAuth = new float[50];
        keyLetterAuth = new string[50];

        clearPassword();
        clearAuth();

        GameObject[] k = GameObject.FindGameObjectsWithTag("key");
        keys = new PianoKeys[k.Length];
        for(int i = 0; i < k.Length; i++)
        {
            keys[i] = k[i].GetComponent<PianoKeys>();
        }
    }

    public static void selectMode(int modeID)
    {
        currentMode = modeID;
        master.StartCoroutine(changeButtons());
        switch (modeID)
        {
            case 0: //free play
                prompt.color = new Color(255, 255, 255);
                prompt.text = "Free Play: Play Anything";
                break;
            case 1: //record mode
                prompt.color = new Color(25, 255, 70);
                prompt.text = "Press Any Key To Begin";
                break;
            case 2: //practice mode
                prompt.color = new Color(251, 207, 208);
                prompt.text = "Press Any Key To Start";
                break;
        }

        changeModeCheck(); //checs if password was made
        PianoKeys.changeMode(modeID);
        ModeButton.changeMode(modeID);

     
    }

    
    static IEnumerator changeButtons()
    {
     //   yield return new WaitForSeconds(.3f);
        foreach (ModeButton b in buttons)
        {
            if (currentMode == b.buttonID)
            {
                b.keyDown();
            }
            else
            {
                b.keyUp();
            }
            yield return new WaitForSeconds(.01f);
        }

    }

    public static void changeModeCheck() //checks for password existance before signing in
    {

        if (currentMode == 2)
        {
            if (durations[0] == 0)
            {
                prompt.text = "Please Make a New Pasword First";
                ModeButton.setHasPassword(false);
            }


            else if (keyNumbers[0] == "")
            {
                prompt.text = "Please Make a New Pasword First";
                ModeButton.setHasPassword(false);
            }
            else
                ModeButton.setHasPassword(true); //if there is a password stored, 
                
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void NotePlayed(PianoKeys key)
    {
        //durations and keyLetters
        keyNumbers[currentIndex] = key.keyLetter;
        key.setCurrentNoteIndex(currentIndex);
        currentIndex++;
        
        
    }

    public static void NotePlayed(NumericalKeys key)
    {
        //durations and keyLetters
        keyNumbers[currentIndex] = key.keyLetter;
        key.setCurrentNoteIndex(currentIndex);
        currentIndex++;


    }

    public static void NoteReleased(int index, float duration)
    {
        durations[index] = duration;
    }

    public static void NoteAuthPlayed(PianoKeys key)
    {
       
        //durations and keyLetters
        keyLetterAuth[currentAuthIndex] = key.keyLetter;
        key.setCurrentNoteAuthIndex(currentAuthIndex);
        currentAuthIndex++;


    }

    public static void NoteAuthPlayed(NumericalKeys key)
    {

        //durations and keyLetters
        keyLetterAuth[currentAuthIndex] = key.keyLetter;
        key.setCurrentNoteAuthIndex(currentAuthIndex);
        currentAuthIndex++;


    }

    public static void NoteAuthReleased(int index, float duration)
    {
        print(index + " duration: " + duration);
        durAuth[index] = duration;
    }

    public static void clearPassword()
    {
        for (int i = 0; i < 50; i++)
        {
            durations[i] = 0;
            keyNumbers[i] = "";
            
        }
        currentIndex = 0;
        ModeButton.setHasPassword(false);
    }

    public static void clearAuth()
    {
        for (int i = 0; i < 50; i++)
        {
            durAuth[i] = 0;
            keyLetterAuth[i] = "";

        }
        currentAuthIndex = 0;
    }

    public static void printPassword()
    {
        print("password; \n");
        for (int i = 0; i < 50; i++)
        {
            if (durations[i] == 0)
                break;

            print(keyNumbers[i] + " duration: " + durations[i]);
        }
    }

    public static void printAuth()
    {
        print("auth: \n");
        for (int i = 0; i < 50; i++)
        {
            if (durAuth[i] == 0)
                break;

            print(keyLetterAuth[i] + " duration: " + durAuth[i]);
        }
    }

    public static void confirmPassword()
    {
        for (int i = 0; i < 50; i++)
        {
            if (durations[i] != 0f && keyNumbers[i] != "")      //if the note is correctly recorded
                continue;
            else if (keyNumbers[i] != "" && durations[i] == 0f)
                keyNumbers[i] = "";
           


        }

        if (durations[0] == 0f)
            prompt.text = "Pasword is invalid.";

        else
            prompt.text = "Password Recorded!";
    }

    public static void confirmAuthPassword()
    {
        for (int i = 0; i < 50; i++)
        {
            if (durAuth[i] != 0f && keyLetterAuth[i] != "")      //if the note is correctly recorded
                continue;
            else if (keyLetterAuth[i] != "" && durAuth[i] == 0f)
                keyLetterAuth[i] = "";



        }

        Authentication();
    }

     static void Authentication()
    {
        printPassword();
        printAuth();
        int passLength = 0;
        int authLength = 0;
        isFailAuth = false;

        for (int i = 0; i < 50; i++)
        {
            if (i > keyNumbers.Length - 1 || i > keyLetterAuth.Length - 1) //bounds check
            {
                isFailAuth = true;
                break;
            }

            if (keyNumbers[i] == "" && keyLetterAuth[i] == "")
                break;

            if (keyNumbers[i] != "")
                passLength++;

            if (keyLetterAuth[i] != "")
                authLength++;
        }

        if (!isFailAuth && passLength != authLength)
        {
            print("pass and auth lengths are different");
            FailSignIn();
            isFailAuth = true;
        }
        else
            isFailAuth = false;

        if (!isFailAuth)
        {
            int correctLetters = 0;
            int correctDur = 0;
            for (int i = 0; i < passLength + 1; i++)
            {
                if (keyNumbers[i] != keyLetterAuth[i])
                {
                    print("Incorrect Note");
                    isFailAuth = true;
                    break;
                }
                else
                    correctLetters++;

//make sure this timing authentication works here
                if (durations[i] > (durAuth[i] + timingMargin) || durations[i] < (durAuth[i] - timingMargin))
                {
                    isFailAuth = true;
                    print("Incorrect Timing");
                    break;
                }
                else
                    correctDur++;
            }
//////////////////////////////////////////////////////////////////////////////////DO THE PIANO KEY CODE
            //Successful  Sign in
            if (!isFailAuth && authLength == passLength)
            {
                isFailAuth = false;
                prompt.text = "Login Successful";
            }
            else
            {
                isFailAuth = true;
                FailSignIn();
            }

        }
        else
            FailSignIn();



    }


    static void FailSignIn()
    {
        prompt.text = "Login Failed. Try Again.";
    }


    public static void recError()
    {
        prompt.text = "REC error. Try again.";
    }
}
