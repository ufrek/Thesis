using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecButton : MonoBehaviour
{
    public Text recText;
    public Image recImage;
    public Sprite stopSprite;
    public Sprite recSprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onRecord()//changes to stop images
    {
        recText.text = "STOP";
        recImage.sprite = stopSprite;
    }

    public void keyUp(ModeButton p)
    {
        p.keyUp();
    }

    public void onStop()//changes to record images
    {
        recText.text = "REC";
        recImage.sprite = recSprite;
    }
}
