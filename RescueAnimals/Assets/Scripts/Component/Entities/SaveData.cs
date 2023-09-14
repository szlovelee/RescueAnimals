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
        public int Atk;

        public List<ReinforceSaveData> ReinforceSaveData;
        public List<Rank> RankSystemData;

        public SaveData(int level, int exp, int gold, int atk, List<ReinforceSaveData> reinforceSaveData, List<Rank> rankSystemData)
        {
            Level = level;
            Exp = exp;
            Gold = gold;
            Atk = atk;
            ReinforceSaveData = reinforceSaveData;
            RankSystemData = rankSystemData;
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