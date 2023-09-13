using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Ranking System")]
public class RankSystem :ScriptableObject
{
    private const int MaxRankings = 5;

    [SerializeField]
    public List<Rank> rankings = new();

    public void AddRank(Rank rank)
    {
        rankings.Add(rank);
        rankings.Sort();

        if (rankings.Count > MaxRankings)
        {
            rankings.RemoveAt(MaxRankings); // 초과하는 항목 제거
        }
    }

    public List<Rank> GetRankings()
    {
        return new List<Rank>(rankings);
    }

    public void ClearRankings()
    {
        rankings.Clear();
    }

    public void CreateRankUI(GameObject rankPrefab, Transform rankPos)
    {
        for (int i = 0; i < rankings.Count; i++)
        {
            GameObject component = Instantiate(rankPrefab, rankPos);
            int score = rankings[i].Score;
            int stage = rankings[i].StageNumber;
            RankUI ui = component.GetComponent<RankUI>();
            ui.UpdateRankUI(i + 1, score, stage);
        }
    }
}
