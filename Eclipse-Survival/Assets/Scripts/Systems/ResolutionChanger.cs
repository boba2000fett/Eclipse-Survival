using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionChanger : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Text txtRes;
    public Toggle fullscreen;

    [Header("Set Dynamically")]
    Resolution[] resList;
    Resolution currRes;



    void Start()
    {
        resList = Screen.resolutions;
        currRes = Screen.currentResolution;

        txtRes.text = currRes.width + "x" + currRes.height;
        fullscreen.isOn = Screen.fullScreen;
    }

    KeyValuePair<int, int> GetSelectedWidthHeight()
    {
        string[] size = txtRes.text.Split('x');
        int w = int.Parse(size[0]);
        int h = int.Parse(size[1]);
        return new KeyValuePair<int, int>(w, h);
    }

    int GetCurrentIndex()
    {
        KeyValuePair<int, int> size = GetSelectedWidthHeight();
        int width = size.Key;
        int height = size.Value;

        for (int i = 0; i < resList.Length; i++)
        {
            if (width == resList[i].width && height == resList[i].height)
            {
                return i;
            }
        }

        return -1;
    }

    public void NextRes()
    {
        int index = GetCurrentIndex();

        if (index == resList.Length - 1) 
        {
            index = 0;
            txtRes.text = resList[index].width + "x" + resList[index].height;
        }
        else txtRes.text = resList[index + 1].width + "x" + resList[index + 1].height;
    }

    public void PreviousRes()
    {
        int index = GetCurrentIndex() - 1;

        if (index < 0) index = resList.Length - 1;

        txtRes.text = resList[index].width + "x" + resList[index].height;
    }

    public void ApplyResolution()
    {
        Resolution selected = resList[GetCurrentIndex()];
        if (selected.width == currRes.width && selected.height == currRes.height)
        {
            return;
        }

        Screen.SetResolution(selected.width, selected.height, fullscreen.isOn);
    }
}
