using UnityEngine;
using UnityEngine.Serialization;

namespace Component.Entities.Database
{
    [CreateAssetMenu(menuName = "AnimalData/Coefficient")]
    public class Coefficient : ScriptableObject
    {
        public int beagleCountPerLevel = 4;
        public int beagleAtkPerLevel = 3;
        public int satelliteCountPerLevel = 2;
        public int satelliteAtkPerLevel = 3;
        public int dragonBreathAtkPerLevel = 5; 
        public int pandaCountPerLevel = 1;
        public int ballCloneCountPerLevel = 1;
    }
}