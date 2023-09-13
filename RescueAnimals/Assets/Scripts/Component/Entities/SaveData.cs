using System;
using System.Collections.Generic;
using EnumTypes;
using UnityEngine.Serialization;

namespace Component.Entities
{
    public class SaveData
    {
        public int Level;
        public int Exp;
        public int Gold;
        public int MaxScore;
        public int Atk;

        public List<ReinforceSaveData> ReinforceSaveData;

        public SaveData(int level, int exp, int gold, int maxScore, int atk, List<ReinforceSaveData> reinforceSaveData)
        {
            Level = level;
            Exp = exp;
            Gold = gold;
            MaxScore = maxScore;
            Atk = atk;
            ReinforceSaveData = reinforceSaveData;
        }
    }

    [Serializable]
    public class ReinforceSaveData
    {
        public int reinforceLevel;
        public AnimalType animalType;

        public ReinforceSaveData(AnimalType animalType, int reinforceLevel)
        {
            this.animalType = animalType;
            this.reinforceLevel = reinforceLevel;
        }
    }
}