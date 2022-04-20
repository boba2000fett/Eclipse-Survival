using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScroll : MonoBehaviour
{
    [Header("Set in Inspector")]
    public Text tbPrefab;
    public AudioClip optionsBGM;
    public float scrollSpeed;
    public float scrollMultiPerSec;

    [Header("Set Dynamically")]
    private List<Text> tBoxes;
    private AudioSource bgm1;
    private float textLoc = 0f;
    private float scrollTimer = 0f;

    string creditsText =
        "'Scurry',\n,\n,\n,\n,'Alpha Dogs Studio',\n,\n,Producer 1: Michael Hoekstra,\n,Producer 2: Cameron Abrams,\n,Producer 3: Damion Shirkey,\n," +
        "Producer 4: Lukas Stanley,\n,Producer 5: Alex Reid,\n,Producer 6: Trent Andrews,\n,Producer 7: Brendan Adams,\n,\n,\n,\n," +
        "Lead Artist: Brendan Adams,\n,Supporting Artist: Damion Shirkey,\n,Fill-In Artist: Trent Andrews,\n,\n,\n,\n," +
        "Lead Sound Designer: Lukas Stanley,\n,\n,\n,\n,Lead Programmer: Michael Hoekstra,\n,Programmer 1: Trent Andrews,\n," +
        "Programmer 2: Cameron Abrams,\n,Programmer 3: Lukas Stanley,\n,\n,\n,\n,Lead Designer: Trent Andrews,\n,Lead Level Designer: Alex Reid,\n,\n,\n,\n," +
        "Lead Story Writer: Trent Andrews,\n,Supporting Story Writer: Cameron Abrams,\n,\n,\n,\n,Concept Creation: Lukas Stanley,\n,\n," +
        "Hall of the Mountain by King Kevin MacLeod,\n,\n,*[LPC] Spider*,Created by William Thompson,Designed by Stephen Challener,\n,\n,\n,\n,\n,\n,\n,\n,\n,\n,\n,\n,\n,\n,\n,\n,\n";

    string[] creditLines;
    int onLineNum = 0;

    bool doneScrolling = false;

    // From bottom left
    Vector2 pos = new Vector2(1210, -70);
    Vector2 size = new Vector2(1220, 125);

    void Start()
    {
        tBoxes = new List<Text>();

        creditLines = creditsText.Split(',');

        bgm1 = gameObject.AddComponent<AudioSource>();
        bgm1.loop = false;
        bgm1.playOnAwake = false;

        bgm1.PlayOneShot(optionsBGM, 0.75f);
    }

    void FixedUpdate()
    {
        if (!doneScrolling) scrollTimer += Time.deltaTime;
        if (scrollTimer >= 1f)
        {
            scrollTimer = 0f;
            scrollSpeed += scrollMultiPerSec;
        }

        if (!doneScrolling && Mathf.FloorToInt(textLoc / 70f) == onLineNum)
        {
            if (onLineNum >= creditLines.Length)
            {
                onLineNum = 0;
                textLoc = 0f;
                scrollMultiPerSec *= 3.5f;
                if (!bgm1.isPlaying)
                {
                    doneScrolling = true;
                }
            }

            if (creditLines[onLineNum] != "\n" && !doneScrolling)
            {
                Text txt = GameObject.Instantiate(tbPrefab);
                txt.transform.SetParent(gameObject.transform);
                txt.rectTransform.sizeDelta = size;
                txt.rectTransform.position = pos;
                    

                if (creditLines[onLineNum].Contains("'"))
                {
                    txt.fontSize = 150;
                    txt.alignment = TextAnchor.MiddleCenter;
                }
                else if (creditLines[onLineNum].Contains("*"))
                {
                    txt.fontSize = 100;
                    txt.alignment = TextAnchor.MiddleCenter;
                }
                else
                {
                    txt.fontSize = 100;
                    txt.alignment = TextAnchor.MiddleLeft;
                }

                txt.text = creditLines[onLineNum].Trim(new char[] { '\'', '*'});

                tBoxes.Add(txt);
            }
            onLineNum++;
        }

        for (int i = 0; i < tBoxes.Count; i++)
        {
            tBoxes[i].rectTransform.position += new Vector3(0f, scrollSpeed, 0f);
            // if off screen destroy
            if (tBoxes[i].rectTransform.position.y >= 1150)
            {
                Text txt = tBoxes[i];
                Destroy(txt.gameObject);
                tBoxes.Remove(txt);
            }
        }

        textLoc += scrollSpeed;
    }
}
