using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//UNUSED
//Used for displaying multiple explanation windows for threshold and frequency tutorials
//Not user friendly so dropped
public class ExplanationMgr : MonoBehaviour
{
    [SerializeField] private GameObject analysisMenu;
    [SerializeField] private GameObject[] explanations;
    int activeMenu = -1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void displayExplanation(int hintID)
    {
        analysisMenu.SetActive(false);
            switch (hintID)
            {
                case 0:
                    activeMenu = 0;
                    explanations[0].SetActive(true);
                    break;
                case 1:
                    activeMenu = 1;
                    explanations[1].SetActive(true);
                    break;
                case 2:
                    activeMenu = 2;
                    explanations[2].SetActive(true);
                    break;
                case 3:
                    activeMenu = 3;
                    explanations[3].SetActive(true);
                    break;
                default:
                    print("Invalid hintID set");
                    break;
            }
    }

    public void backToMain()
    {
        print("running");
        analysisMenu.SetActive(true);
        explanations[activeMenu].SetActive(false);
    }

    public void Back()
    {
        print("running");
    }

    public void clearExpMenu()
    {
        foreach (GameObject g in explanations)
        {
            g.SetActive(false);
        }
    }
}
