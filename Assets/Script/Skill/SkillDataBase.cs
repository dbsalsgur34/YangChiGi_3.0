using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDataBase : MonoBehaviour {

    public List<GameObject> SkillPrefab;
    public List<Sprite> SkillIcon;
    public List<int> SkillIndexList;

    void SetRandomNumber(int StartNumber, int EndNumber)
    {
        while (SkillIndexList.Count <= EndNumber - 1)
        {
            int temp = Random.Range(StartNumber, EndNumber + 1);
            bool checknum = false;
            foreach (int i in SkillIndexList)
            {
                if (i == temp)
                {
                    checknum = true;
                }
            }
            if (!checknum)
            {
                SkillIndexList.Add(temp);
            }
        }
    }

    public void ButtonIconInRandomList(Button targetButton, int index)
    {
        targetButton.image.sprite = SkillIcon[SkillIndexList[index]];
    }

    private void Start()
    {
        if (SkillPrefab.Count == SkillIcon.Count)
        {
            SetRandomNumber(1, SkillPrefab.Count-1);
        }
        else
        {
            Debug.Log("Fill SkillPrefab or SkillIcon.");
        }
    }
}
