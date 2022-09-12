using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//UNUSED:
//Used as a way to manage different selected instruments and frequency profile presets for each instrument
//Too complex for average users
public class DropDownMgr : MonoBehaviour
{
    private static int instrumentIndex = 0;
    private static int presetIndex = 0;
    private static float sensitivity = .5f;
    [SerializeField] private Dropdown instrumentDropdown;
    [SerializeField] private Dropdown presetDropdown;
    [SerializeField] private GameObject sensitivityObj;
    private Slider sensitivitySlider;
    // Start is called before the first frame update
    void Start()
    {
        sensitivitySlider = sensitivityObj.GetComponentInChildren<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
           
    }

    public void UpdateInstrumentSelected()

    {
        instrumentIndex = instrumentDropdown.value;
    }


    public void UpdatePresetSelected()

    {
        presetIndex = presetDropdown.value;
    }

    public void UpdateSensitivity()
    {
        sensitivity = sensitivitySlider.value;
    }

    public static int GetSelectedInstrument()
    {
        return instrumentIndex;
    }

    public static int GetSelectedPreset()
    {
        return presetIndex;
    }

    public static float GetSensitivity()
    {
        return sensitivity;
    }
}
