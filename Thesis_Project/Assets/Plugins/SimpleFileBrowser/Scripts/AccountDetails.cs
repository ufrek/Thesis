using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountDetails : MonoBehaviour
{

    //TODO: make password variables
    private string username;
    
    // Start is called before the first frame update
    void Start()
    {
        username = "";

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setUserName(string name)
    {
        username = name;
    }
}
