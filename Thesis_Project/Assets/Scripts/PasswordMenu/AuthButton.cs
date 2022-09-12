using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthButton : MonoBehaviour
{
    public Text authText;
    //public Image authImage;
   // public Sprite stopSprite;
    //public Sprite authSprite;
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
        authText.text = "STOP";
       // authImage.sprite = stopSprite;
    }

    public void keyUp(ModeButton p)
    {
        p.keyUp();
    }

    public void onStop()//changes to record images
    {
        authText.text = "Sign In";
        //recImage.sprite = recSprite;
    }
}
