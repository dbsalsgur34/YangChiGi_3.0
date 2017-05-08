using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour {

    // Use this for initialization
    public float speed;
    public float angle;
    public float score;
    public float distance;
    public bool IsBoost;
    public string explaintext1;
    public string explaintext2;
    public string explaintext3;
    public int restraintlevel;

    public Sprite Locker;
         
    int playerlevel;
    Button B;
    Text ET;
    Text leveltext;
    public Image[] otherImage;

    private void Start()
    {
        B = this.gameObject.GetComponent<Button>();
        leveltext = this.gameObject.GetComponentInChildren<Text>();
        otherImage = this.gameObject.GetComponentsInChildren<Image>();
        playerlevel = PlayManage.Instance.GetPlayerLevel();
        if (playerlevel >= restraintlevel)
        {
            B.onClick.AddListener(ShowExplain);
            ET = GameObject.Find("ExplainText").GetComponent<Text>();
            leveltext.text = null;
        }
        else
        {
            B.image.overrideSprite = Locker;
            leveltext.text = "LV." + restraintlevel;
            for (int i = 1; i < otherImage.GetLength(0); i++)
            {
                otherImage[i].color = new Vector4(0, 0, 0, 0);
            }
        }
    }

    private void ShowExplain()
    {
        ET.text = this.explaintext1 + "\n" + this.explaintext2 + "\n" + explaintext3;
    }

}