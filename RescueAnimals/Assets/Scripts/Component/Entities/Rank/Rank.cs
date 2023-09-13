using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Rank : IComparable<Rank>
{
    public int Score;
    public int StageNumber;

    public Rank(int score, int stageNumber)
    {
        Score = score;
        StageNumber = stageNumber;
    }

    public int CompareTo(Rank other)
    {
        if (Score != other.Score)
        {
            return other.Score.CompareTo(Score); // 점수가 높은 순으로 정렬
        }
        return StageNumber.CompareTo(other.StageNumber); // 점수가 같으면 스테이지 번호로 정렬
    }
}
