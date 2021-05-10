using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DropdownMenuController : MonoBehaviour
{
    public Dropdown QualityDropdown;
    //Resolution[] resolutions;
    public Dropdown ResolutionDropdown;

    private HashSet<String> resolutions;
    
    
    void Start()
    {
        resolutions = new HashSet<string>();
        foreach (Resolution res in Screen.resolutions)
        {
            resolutions.Add(ResToString(res));

        }
        
        
        ResolutionDropdown.options = new List<Dropdown.OptionData>();
        int i = 0;
        foreach (String res in resolutions)
        {
            ResolutionDropdown.options.Add(new Dropdown.OptionData(res));
            ResolutionDropdown.options[i].text = res;
            ResolutionDropdown.value = i;
            i++;
        }

        ResolutionDropdown.value = PlayerPrefs.GetInt("ResolutionValue");
        QualityDropdown.value = PlayerPrefs.GetInt("QualityValue");
        setResolution(ResolutionDropdown.options[ResolutionDropdown.value].text.Split(' '));
        ChangeQuality();
        QualityDropdown.onValueChanged.AddListener(delegate { ChangeQuality(); });
        
        ResolutionDropdown.onValueChanged.AddListener(delegate
        {
            string[] ResStrings = ResolutionDropdown.options[ResolutionDropdown.value].text.Split(' ');
            setResolution(ResStrings);
            PlayerPrefs.SetInt("ResolutionValue", ResolutionDropdown.value);
            PlayerPrefs.Save();
        });
    }

    private void setResolution(string[] ResStrings)
    {
        Screen.SetResolution(int.Parse(ResStrings[0]), int.Parse(ResStrings[2]),
            PlayerPrefs.GetInt("Fullscreen") == 1);
    }
    
    string ResToString(Resolution res)
    {
        return res.width + " x " + res.height;
    }
    
    public void ChangeQuality()
    {
        Debug.LogError("Changed Quality");
        QualitySettings.SetQualityLevel(QualityDropdown.value);
        PlayerPrefs.SetInt("QualityValue", QualityDropdown.value);
        PlayerPrefs.Save();
    }
}
