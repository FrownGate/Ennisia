using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class XPRewardTest : MonoBehaviour
{
    public void LVLUP(int lvl)
    {
        ExpManager.Instance.Rewards.LVLUPReward(lvl);
    }
}