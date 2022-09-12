using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(SongLoader))]
public class UIDisplayManager : MonoBehaviour
{

    [SerializeField] private GameObject introScreen;

    [Header("Sign In Menu Elements")]
    [SerializeField] private GameObject signInUserNameScreen;
    [SerializeField] private InputField inputUserName;
    [SerializeField] private Text signInPrompt;


    [SerializeField] private GameObject newAccountScreen;


    [Header("New User Menu")]
    [SerializeField] private InputField inputNewUserName;
    [SerializeField] private InputField inputConfirmNewUserName;
    [SerializeField] private Text newUserPrompt;
    [SerializeField] private Text confirmUserPrompt;

    [Header("Clip Upload Menus")]
    [SerializeField] private GameObject songUploadMenu;
    [SerializeField] private TMP_Text selectedClipPrompt; 
    [SerializeField] private GameObject clipSelectionMenu;

    [Header("Password Creation Menu")]
    [SerializeField] private GameObject passCreationMenu;

    /*
    [Header("Analysis Menu")]
    [SerializeField] private GameObject analysisMenu;
    */


    private AccountDetails account;
    private GameObject currentMenu;

    void Start()
    {
        currentMenu = introScreen;
  
    }

    public void changeMenu(GameObject currMenu, GameObject newMenu)
    {
        currMenu.SetActive(false);
        newMenu.SetActive(true);
        currentMenu = newMenu;
    }

    //Navigates to a menu where the username is asked for
    public void SignInMenu()
    {
        changeMenu(currentMenu, signInUserNameScreen);
           
    }

    public void newAccountMenu()
    {
        changeMenu(currentMenu, newAccountScreen);
        //signInUserNameScreen.SetActive(false);
        
    }


    private bool validateUserName(InputField input, Text prompt)
    {
        bool validUserName = false;
        if (input.text.Length >= 6)
        {
            validUserName = true;
            prompt.text = "Please Enter New Username";
            prompt.color = Color.white;
        }
        else
        {
            prompt.color = Color.red;
            prompt.text = "Must be at least 6 characters";
        }

        if (input.text.ToLower().Equals("username"))
        {
            validUserName = false;
            prompt.color = Color.red;
            prompt.text = "Please type in a valid username.";
        }
        

        return validUserName;
    }

    public void EnterUserName()
    {
        if (validateUserName(inputUserName, signInPrompt))
        {
            account.setUserName(inputUserName.text);
            newAccountScreen.SetActive(false);
            //AnalysisMenu.SetActive(true);
        }
                   
        

    }

    public void NewUserToSongUpload()
    {
        if (validateUserName(inputNewUserName, newUserPrompt) && inputNewUserName.text.Equals(inputConfirmNewUserName.text))
        {
            account.setUserName(inputNewUserName.text);
            changeMenu(currentMenu, songUploadMenu);

            //clipSelectionMenu.SetActive(true);
        }
        else if (!inputNewUserName.text.Equals(inputConfirmNewUserName.text))
        {
            confirmUserPrompt.color = Color.red;
            confirmUserPrompt.text = "Usernames do not match.";
        }
        else 
        {
            confirmUserPrompt.color = Color.white;
            confirmUserPrompt.text = "Confirm Username";
        }
       
    }

    public void UploadConfirmToClipSelection()
    {
        AudioSource audioSource = this.GetComponent<AudioSource>();
        if (audioSource.clip != null)
        {
           
            if (audioSource.clip.length < 3)
            {
                print(" not working");
                selectedClipPrompt.color = Color.red;
                selectedClipPrompt.text = "File must be at least 3 seconds long to make a password";
            }
            else 
            {
                if (audioSource.isPlaying)
                    audioSource.Stop();

                changeMenu(currentMenu, clipSelectionMenu);
                GetComponent<SongLoader>().initilizeClipSelectionElements(selectedClipPrompt.text);

            }
        }
    }

    public void clipSelectionToPassCreation()
    {
        changeMenu(currentMenu, passCreationMenu);
    }

   /* Unused
    * 
    * public void clipSelectionToAnalysisMenu()
    {
        changeMenu(currentMenu, analysisMenu);
    }*/

   

}
