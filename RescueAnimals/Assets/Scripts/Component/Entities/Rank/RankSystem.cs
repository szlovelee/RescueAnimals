using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = "Ranking System")]
public class RankSystem :ScriptableObject
{
    private const int MaxRankings = 5;
    public List<Rank> rankings = new List<Rank>();

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
}
