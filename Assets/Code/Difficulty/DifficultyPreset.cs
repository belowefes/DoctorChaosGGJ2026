using UnityEngine;

namespace Code.Difficulty
{
    [System.Serializable]
    public class DifficultyPreset
    {
        public int organsPerBedRequired;
        public int maxBedsOnTheLevel;
        public float bedSpawnDuration;
        public float bedOrganSwitchDuration;
        public float pedestalOrganDuration;
    }
}