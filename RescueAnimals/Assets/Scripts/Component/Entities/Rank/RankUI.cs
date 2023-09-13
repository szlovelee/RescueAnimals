using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class RankUI : MonoBehaviour
{
    [SerializeField] private Text rankTxt;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Text stageTxt;

    public void UpdateRankUI(int rank, int score, int stage)
    {
        rankTxt.text = rank.ToString();
        scoreTxt.text = score.ToString();
        stageTxt.text = string.Format($"stage {stage}");
    }

    
}
