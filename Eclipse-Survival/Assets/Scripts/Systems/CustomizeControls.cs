using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeControls : MonoBehaviour
{
    bool takingInput = false;
    static public string[] ControlNames = new string[]
    { "Jump", "Run", "Scratch", "Up", "Left", "Down", "Right"};
    string takingInputName = "";

    [Header("Set In Inpector")]
    public Text txtJump;
    public Text txtRun;
    public Text txtScratch;
    public Text txtUp;
    public Text txtLeft;
    public Text txtDown;
    public Text txtRight;



    void Start()
    {
        for (int i = 0; i < ControlNames.Length; i++)
        {
            if (!PlayerPrefs.HasKey(ControlNames[i])) SetDefaults();
        }
        UpdateButtons();
    }

    void Update()
    {
        if (takingInput)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
            {
                takingInput = false;
                takingInputName = "";
            }
            else if (Input.anyKeyDown)
            {
                KeyCode keyDown = KeyCode.None;
                if (Input.inputString == " ") keyDown = KeyCode.Space;
                else if (Input.inputString == "")
                {
                    bool found = false;
                    for (int i = 0; i < 350; i++)
                    {
                        if (Input.GetKeyDown((KeyCode)i))
                        {
                            keyDown = (KeyCode)i;
                            found = true;
                        }
                    }

                    if (!found) return;
                }
                else if (char.IsDigit(Input.inputString[0]))
                {
                    KeyCode kAlpha = (KeyCode)((int)char.GetNumericValue(Input.inputString[0]) + (int)KeyCode.Alpha0);
                    KeyCode kPad = (KeyCode)((int)char.GetNumericValue(Input.inputString[0]) + (int)KeyCode.Keypad0);
                    if (Input.GetKeyDown(kAlpha)) keyDown = kAlpha;
                    else keyDown = kPad;
                }
                else
                {
                    try
                    {
                        keyDown = (KeyCode)System.Enum.Parse(typeof(KeyCode), Input.inputString.ToUpper());
                    }
                    catch (System.Exception)
                    {
                        return;
                    }
                }
                for (int i = 0; i < ControlNames.Length; i++)
                {
                    if (PlayerPrefs.GetInt(ControlNames[i]) == (int)keyDown) return;
                }
                PlayerPrefs.SetInt(takingInputName, (int)keyDown);
                takingInput = false;
                takingInputName = "";
                UpdateButtons();
            }
        }
    }

    public void UpdateButtons()
    {
        txtJump.fontSize = 70;
        txtRun.fontSize = 70;
        txtScratch.fontSize = 70;
        txtUp.fontSize = 70;
        txtLeft.fontSize = 70;
        txtDown.fontSize = 70;
        txtRight.fontSize = 70;

        KeyCode key = (KeyCode)PlayerPrefs.GetInt(ControlNames[0]);
        if (key == KeyCode.Space) txtJump.text = "␣";
        else if ((int)key >= 273 && (int)key <= 276)
        {
            string arrow = "";
            switch (key)
            {
                case KeyCode.UpArrow:
                    arrow = "↑";
                    break;
                case KeyCode.DownArrow:
                    arrow = "↓";
                    break;
                case KeyCode.LeftArrow:
                    arrow = "←";
                    break;
                case KeyCode.RightArrow:
                    arrow = "→";
                    break;
            }
            txtJump.text = arrow;
        }
        else if (key == KeyCode.CapsLock)
        {
            txtJump.fontSize = 35;
            txtJump.text = "CapLk";
        }
        else if (key.ToString().Contains("Keypad"))
        {
            txtJump.text = key.ToString().Remove(1, 5);
        }
        else if (key.ToString().Contains("Alpha"))
        {
            txtJump.text = key.ToString().Remove(1, 4);
        }
        else if (key.ToString().Contains("Right"))
        {
            txtJump.fontSize = 35;
            txtJump.text = key.ToString().Remove(1, 4).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Left"))
        {
            txtJump.fontSize = 35;
            txtJump.text = key.ToString().Remove(1, 3).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Tab"))
        {
            txtJump.fontSize = 35;
            txtJump.text = key.ToString();
        }
        else txtJump.text = key.ToString().ToUpper();

        key = (KeyCode)PlayerPrefs.GetInt(ControlNames[1]);
        if (key == KeyCode.Space) txtRun.text = "␣";
        else if ((int)key >= 273 && (int)key <= 276)
        {
            string arrow = "";
            switch (key)
            {
                case KeyCode.UpArrow:
                    arrow = "↑";
                    break;
                case KeyCode.DownArrow:
                    arrow = "↓";
                    break;
                case KeyCode.LeftArrow:
                    arrow = "←";
                    break;
                case KeyCode.RightArrow:
                    arrow = "→";
                    break;
            }
            txtRun.text = arrow;
        }
        else if (key == KeyCode.CapsLock)
        {
            txtRun.fontSize = 35;
            txtRun.text = "CapLk";
        }
        else if (key.ToString().Contains("Keypad"))
        {
            txtRun.text = key.ToString().Remove(1, 5);
        }
        else if (key.ToString().Contains("Alpha"))
        {
            txtRun.text = key.ToString().Remove(1, 4);
        }
        else if (key.ToString().Contains("Right"))
        {
            txtRun.fontSize = 35;
            txtRun.text = key.ToString().Remove(1, 4).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Left"))
        {
            txtRun.fontSize = 35;
            txtRun.text = key.ToString().Remove(1, 3).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Tab"))
        {
            txtRun.fontSize = 35;
            txtRun.text = key.ToString();
        }
        else txtRun.text = key.ToString().ToUpper();

        key = (KeyCode)PlayerPrefs.GetInt(ControlNames[2]);
        if (key == KeyCode.Space) txtScratch.text = "␣";
        else if ((int)key >= 273 && (int)key <= 276)
        {
            string arrow = "";
            switch (key)
            {
                case KeyCode.UpArrow:
                    arrow = "↑";
                    break;
                case KeyCode.DownArrow:
                    arrow = "↓";
                    break;
                case KeyCode.LeftArrow:
                    arrow = "←";
                    break;
                case KeyCode.RightArrow:
                    arrow = "→";
                    break;
            }
            txtScratch.text = arrow;
        }
        else if (key == KeyCode.CapsLock)
        {
            txtScratch.fontSize = 35;
            txtScratch.text = "CapLk";
        }
        else if (key.ToString().Contains("Keypad"))
        {
            txtScratch.text = key.ToString().Remove(1, 5);
        }
        else if (key.ToString().Contains("Alpha"))
        {
            txtScratch.text = key.ToString().Remove(1, 4);
        }
        else if (key.ToString().Contains("Right"))
        {
            txtScratch.fontSize = 35;
            txtScratch.text = key.ToString().Remove(1, 4).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Left"))
        {
            txtScratch.fontSize = 35;
            txtScratch.text = key.ToString().Remove(1, 3).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Tab"))
        {
            txtScratch.fontSize = 35;
            txtScratch.text = key.ToString();
        }
        else txtScratch.text = key.ToString().ToUpper();

        key = (KeyCode)PlayerPrefs.GetInt(ControlNames[3]);
        if (key == KeyCode.Space) txtUp.text = "␣";
        else if ((int)key >= 273 && (int)key <= 276)
        {
            string arrow = "";
            switch (key)
            {
                case KeyCode.UpArrow:
                    arrow = "↑";
                    break;
                case KeyCode.DownArrow:
                    arrow = "↓";
                    break;
                case KeyCode.LeftArrow:
                    arrow = "←";
                    break;
                case KeyCode.RightArrow:
                    arrow = "→";
                    break;
            }
            txtUp.text = arrow;
        }
        else if (key == KeyCode.CapsLock)
        {
            txtUp.fontSize = 35;
            txtUp.text = "CapLk";
        }
        else if (key.ToString().Contains("Keypad"))
        {
            txtUp.text = key.ToString().Remove(1, 5);
        }
        else if (key.ToString().Contains("Alpha"))
        {
            txtUp.text = key.ToString().Remove(1, 4);
        }
        else if (key.ToString().Contains("Right"))
        {
            txtUp.fontSize = 35;
            txtUp.text = key.ToString().Remove(1, 4).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Left"))
        {
            txtUp.fontSize = 35;
            txtUp.text = key.ToString().Remove(1, 3).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Tab"))
        {
            txtUp.fontSize = 35;
            txtUp.text = key.ToString();
        }
        else txtUp.text = key.ToString().ToUpper();

        key = (KeyCode)PlayerPrefs.GetInt(ControlNames[4]);
        if (key == KeyCode.Space) txtLeft.text = "␣";
        else if ((int)key >= 273 && (int)key <= 276)
        {
            string arrow = "";
            switch (key)
            {
                case KeyCode.UpArrow:
                    arrow = "↑";
                    break;
                case KeyCode.DownArrow:
                    arrow = "↓";
                    break;
                case KeyCode.LeftArrow:
                    arrow = "←";
                    break;
                case KeyCode.RightArrow:
                    arrow = "→";
                    break;
            }
            txtLeft.text = arrow;
        }
        else if (key == KeyCode.CapsLock)
        {
            txtLeft.fontSize = 35;
            txtLeft.text = "CapLk";
        }
        else if (key.ToString().Contains("Keypad"))
        {
            txtLeft.text = key.ToString().Remove(1, 5);
        }
        else if (key.ToString().Contains("Alpha"))
        {
            txtLeft.text = key.ToString().Remove(1, 4);
        }
        else if (key.ToString().Contains("Right"))
        {
            txtLeft.fontSize = 35;
            txtLeft.text = key.ToString().Remove(1, 4).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Left"))
        {
            txtLeft.fontSize = 35;
            txtLeft.text = key.ToString().Remove(1, 3).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Tab"))
        {
            txtLeft.fontSize = 35;
            txtLeft.text = key.ToString();
        }
        else txtLeft.text = key.ToString().ToUpper();

        key = (KeyCode)PlayerPrefs.GetInt(ControlNames[5]);
        if (key == KeyCode.Space) txtDown.text = "␣";
        else if ((int)key >= 273 && (int)key <= 276)
        {
            string arrow = "";
            switch (key)
            {
                case KeyCode.UpArrow:
                    arrow = "↑";
                    break;
                case KeyCode.DownArrow:
                    arrow = "↓";
                    break;
                case KeyCode.LeftArrow:
                    arrow = "←";
                    break;
                case KeyCode.RightArrow:
                    arrow = "→";
                    break;
            }
            txtDown.text = arrow;
        }
        else if (key == KeyCode.CapsLock)
        {
            txtDown.fontSize = 35;
            txtDown.text = "CapLk";
        }
        else if (key.ToString().Contains("Keypad"))
        {
            txtDown.text = key.ToString().Remove(1, 5);
        }
        else if (key.ToString().Contains("Alpha"))
        {
            txtDown.text = key.ToString().Remove(1, 4);
        }
        else if (key.ToString().Contains("Right"))
        {
            txtDown.fontSize = 35;
            txtDown.text = key.ToString().Remove(1, 4).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Left"))
        {
            txtDown.fontSize = 35;
            txtDown.text = key.ToString().Remove(1, 3).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Tab"))
        {
            txtDown.fontSize = 35;
            txtDown.text = key.ToString();
        }
        else txtDown.text = key.ToString().ToUpper();

        key = (KeyCode)PlayerPrefs.GetInt(ControlNames[6]);
        if (key == KeyCode.Space) txtRight.text = "␣";
        else if ((int)key >= 273 && (int)key <= 276)
        {
            string arrow = "";
            switch (key)
            {
                case KeyCode.UpArrow:
                    arrow = "↑";
                    break;
                case KeyCode.DownArrow:
                    arrow = "↓";
                    break;
                case KeyCode.LeftArrow:
                    arrow = "←";
                    break;
                case KeyCode.RightArrow:
                    arrow = "→";
                    break;
            }
            txtRight.text = arrow;
        }
        else if (key == KeyCode.CapsLock)
        {
            txtRight.fontSize = 35;
            txtRight.text = "CapLk";
        }
        else if (key.ToString().Contains("Keypad"))
        {
            txtRight.text = key.ToString().Remove(1, 5);
        }
        else if (key.ToString().Contains("Alpha"))
        {
            txtRight.text = key.ToString().Remove(1, 4);
        }
        else if (key.ToString().Contains("Right"))
        {
            txtRight.fontSize = 35;
            txtRight.text = key.ToString().Remove(1, 4).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Left"))
        {
            txtRight.fontSize = 35;
            txtRight.text = key.ToString().Remove(1, 3).Replace("Control", "Ctrl");
        }
        else if (key.ToString().Contains("Tab"))
        {
            txtRight.fontSize = 35;
            txtRight.text = key.ToString();
        }
        else txtRight.text = key.ToString().ToUpper();
    }

    static public void SetDefaults()
    {
        PlayerPrefs.SetInt(ControlNames[0], (int)KeyCode.Space);
        PlayerPrefs.SetInt(ControlNames[1], (int)KeyCode.LeftShift);
        PlayerPrefs.SetInt(ControlNames[2], (int)KeyCode.V);
        PlayerPrefs.SetInt(ControlNames[3], (int)KeyCode.W);
        PlayerPrefs.SetInt(ControlNames[4], (int)KeyCode.A);
        PlayerPrefs.SetInt(ControlNames[5], (int)KeyCode.S);
        PlayerPrefs.SetInt(ControlNames[6], (int)KeyCode.D);
    }

    public void ChangeControl(string name)
    {
        takingInputName = name;
        takingInput = true;
    }
}
