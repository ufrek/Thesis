using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//UNUSED
//managed selection of different frequency profile presets for each available instrument
//scope too large and too complex for average users
public class DropDownPresetMgr : MonoBehaviour
{
    [SerializeField] private Dropdown presetDropDown;
    [SerializeField] private Dropdown instrumentDropDown;
    List<Dropdown.OptionData> vocalOptions;
    List<Dropdown.OptionData> bassOptions;
    List<Dropdown.OptionData> pianoOptions;
    List<Dropdown.OptionData> guitarOptions;

    // Start is called before the first frame update
    void Start()
    {
        initializeOptionsValues();
        presetDropDown.ClearOptions();
        presetDropDown.AddOptions(vocalOptions);
    }

    public void loadInstrumentOptions()
    {
        switch (instrumentDropDown.value)
        {
            case 0: //vocals
                presetDropDown.ClearOptions();
                presetDropDown.AddOptions(vocalOptions);
                break;
            case 1: //bass
                presetDropDown.ClearOptions();
                presetDropDown.AddOptions(bassOptions);
                break;
            case 2: //piano
                presetDropDown.ClearOptions();
                presetDropDown.AddOptions(pianoOptions);
                break;
            case 3://guitar
                presetDropDown.ClearOptions();
                presetDropDown.AddOptions(guitarOptions);
                break;
            default: //invalid instrument, makes a blank options list
                presetDropDown.ClearOptions();
                break;
        }
    }


    public void initializeOptionsValues()
    {
        vocalOptions = new List<Dropdown.OptionData>();
        vocalOptions.Add(new Dropdown.OptionData("M Rock"));
        vocalOptions.Add(new Dropdown.OptionData("F Rock"));
        vocalOptions.Add(new Dropdown.OptionData("M Pop"));
        vocalOptions.Add(new Dropdown.OptionData("F Pop"));
        vocalOptions.Add(new Dropdown.OptionData("Other"));

        bassOptions = new List<Dropdown.OptionData>();
        bassOptions.Add(new Dropdown.OptionData("Rock"));
        bassOptions.Add(new Dropdown.OptionData("Pop"));
        bassOptions.Add(new Dropdown.OptionData("EDM"));
        bassOptions.Add(new Dropdown.OptionData("Country"));
        bassOptions.Add(new Dropdown.OptionData("Funk"));

        pianoOptions = new List<Dropdown.OptionData>();
        pianoOptions.Add(new Dropdown.OptionData("Rock"));
        pianoOptions.Add(new Dropdown.OptionData("Pop"));
        pianoOptions.Add(new Dropdown.OptionData("Classical"));
        pianoOptions.Add(new Dropdown.OptionData("EDM"));
        pianoOptions.Add(new Dropdown.OptionData("Blues"));

        guitarOptions = new List<Dropdown.OptionData>();
        guitarOptions.Add(new Dropdown.OptionData("Rock"));
        guitarOptions.Add(new Dropdown.OptionData("Country"));
        guitarOptions.Add(new Dropdown.OptionData("Blues"));
        guitarOptions.Add(new Dropdown.OptionData("Jazz"));
        guitarOptions.Add(new Dropdown.OptionData("Metal"));
    }

}
