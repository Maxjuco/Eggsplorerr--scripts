using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField]
    public TMP_Dropdown resolutionDropdownm;

    [SerializeField]
    public List<Resolution> resolutions; 

    // Start is called before the first frame update
    void Start()
    {
        resolutions = new List<Resolution>();
        resolutionDropdownm.ClearOptions();


        //get every screen résolution :
        List<string> resolutionText = new List<string>();

        for (int i = (Screen.resolutions.Length - 1); i >= 0 ; i--)
        {
            resolutions.Add(Screen.resolutions[i]);
            resolutionText.Add(Screen.resolutions[i].ToString().Split("@")[0]);
        }

        resolutionDropdownm.AddOptions(resolutionText);


        //set dropdown on current resolution : 
        resolutionDropdownm.value = resolutions.IndexOf(Screen.currentResolution);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void setScreenResolution(int indexResolution)
    {
        Resolution resolution = resolutions[indexResolution];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        Debug.Log("current resolution = "+Screen.currentResolution);
    }

    public void setFullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
