using UnityEngine;

namespace Code.Difficulty
{
    [System.Serializable]
    public class DifficultyPreset
    {
        public int organsPerBedRequired;
        public int maxBedsOnTheLevel;
        public float bedSpawnDuration;
        public float bedDurationUntilDeath;
        public float pedestalOrganDuration;
        
        public AnimationCurve organsPerBedCurve;
        public AnimationCurve maxBedsOnTheLevelCurve;
        public AnimationCurve bedSpawnDurationCurve;
        public AnimationCurve bedDurationUntilDeathCurve;
    }
}