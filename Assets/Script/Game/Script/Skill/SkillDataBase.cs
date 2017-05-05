using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillDataBase : MonoBehaviour {

    public List<GameObject> SkillPrefab;
    public List<Sprite> SkillIcon;
    public List<int> SkillIndexList;

    public void SetRandomNumber(int StartNumber, int EndNumber)
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

    public void SetRandomNumber(int[] numbers)
    {
        while (SkillIndexList.Count < numbers.Length)
        {
            int temp = Random.Range(0, numbers.Length);
            bool checknum = false;
            foreach (int i in SkillIndexList)
            {
                if (i == numbers[temp])
                {
                    checknum = true;
                }
            }
            if (!checknum)
            {
                SkillIndexList.Add(numbers[temp]);
            }
        }
    }

    public Sprite SetSkillIcon(int num, bool ShowPadLock)
    {
        if (!ShowPadLock && num >= 0)
        {
            return SkillIcon[num];
        }
        else
        {
            return Resources.Load<Sprite>("Image/Resource/Button/Black/padlock");
        }
    }

}
