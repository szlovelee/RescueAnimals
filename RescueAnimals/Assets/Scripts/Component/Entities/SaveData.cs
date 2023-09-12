using System.Collections.Generic;
using EnumTypes;

namespace Component.Entities
{
    public class SaveData
    {
        public int Level;
        public int Exp;
        public int Gold;
        public int MaxScore;
        public int Atk;
        public Dictionary<AnimalType, int> Reinforce;

        public SaveData(int level, int exp, int gold, int maxScore, int atk, Dictionary<AnimalType, int> reinforce)
        {
            Level = level;
            Exp = exp;
            Gold = gold;
            MaxScore = maxScore;
            Atk = atk;
            Reinforce = reinforce;
        }
    }
}