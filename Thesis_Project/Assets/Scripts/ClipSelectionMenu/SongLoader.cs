using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using NAudio;
using NAudio.Wave;
using UnityEngine.Networking;
using TMPro;
using System;
using SimpleFileBrowser;
using MinMax_Slider;


//Code Modified from post by  DanielSRRosky1999 at: https://answers.unity.com/questions/652919/music-player-get-songs-from-directory.html
//Uses Runtime File Browser by yasirkula from Unity Asset Store: https://assetstore.unity.com/packages/tools/gui/runtime-file-browser-113006#description

[RequireComponent(typeof(AudioSource))]
public class SongLoader : MonoBehaviour
{
    public string filePath;

    private bool isPreviewing = false;


 
    private AudioSource audioSource;
    private float clipStartTime;
    private float clipEndTime;

    [SerializeField]
    private TMP_Text t_selectedClip; //set in inspector
    [SerializeField]
    private TMP_Text t_songName;
    [SerializeField]
    private MinMaxSlider slider;

    Color originalSelectionColor;
    bool isColorFound = false;



    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
       
    }

    //Called in OnClick() Event for "Upload Button"
    //Opens a file explorer window and allows users to select a song to load into the program for password analysis
    public void ReadSongs(bool clipEdittable)
    {
        if (isColorFound == false)
        {
            originalSelectionColor = t_selectedClip.color;
            isColorFound = true;
        }
            
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Songs", ".mp3", ".wav"));
       // FileBrowser.SetDefaultFilter(".mp3");
       
       StartCoroutine(ShowLoadDialogCoroutine(clipEdittable));
    }

    //IMPORTANT: Change default path from D://Music to null on final build
    IEnumerator ShowLoadDialogCoroutine(bool isEdittable)
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, false, "D:\\Music", null, "Select Sound", "Select"); //Change D://Music to null on final build

        if (FileBrowser.Success)
        {
            byte[] SoundFile = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            yield return SoundFile;
            audioSource.clip = NAudioPlayer.FromMp3Data(SoundFile);
            t_selectedClip.color = originalSelectionColor;
            t_selectedClip.text = "Selected Song: " + FileBrowserHelpers.GetFilename(FileBrowser.Result[0]);

            if (isEdittable)
            {
                slider.SetLimits(0, audioSource.clip.length);
                slider.SetValues(0, audioSource.clip.length);
                //makes a persistent data file Path if needed
                //string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(FileBrowser.Result[0]));
                //FileBrowserHelpers.CopyFile(FileBrowser.Result[0], destinationPath);
            }



            //audioSource.Play();
        }
    }

    public void initilizeClipSelectionElements(string songName)
    {
        slider.SetLimits(0, audioSource.clip.length);
        slider.SetValues(0, audioSource.clip.length);
        t_songName.text = songName;
        //t_selectedClip = slider.gameObject.GetComponent<TMP_Text>();
        //t_selectedClip.text = "Selected Song: " + audioSource.clip.name;
    }

    public void PreviewClip()
    {
        
        if (audioSource.clip != null)
        {
            if (isPreviewing)
            {
                audioSource.Stop();
                isPreviewing = false;
            }
            else
            {
                clipStartTime = slider.GetMinValue();
                clipEndTime = slider.GetMaxValue();

                audioSource.time = clipStartTime;
                audioSource.Play();
                isPreviewing = true;
               
            }
        }
        else 
        {
            t_selectedClip.text = "Please upload a song first";
        }
      
    }

    public void PlaySong()
    {
        if (audioSource.clip != null)
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();

            }
            else
                audioSource.Play();
        }
           
    }



 



    private void Update()
    {
        if (isPreviewing && audioSource.time >= clipEndTime)
        {
            audioSource.Stop();
            isPreviewing = false;
        }

       /* if (Input.GetKeyDown("a"))
        {
            ReadSongs();
        }*/
    }


}