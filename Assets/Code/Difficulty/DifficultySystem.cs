using System;
using Code.Difficulty;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Difficulty
{
    public class DifficultySystem : MonoBehaviour
    {
        [SerializeField] private float maximumDifficultyTimePoint = 300f;
        private float _timeProgress;
        private float _lastInterval = 0f;
        [SerializeField] private float updateInterval = 2f;
        public float CurrentProgress => Mathf.Clamp01(_timeProgress / maximumDifficultyTimePoint);

        public DifficultyPreset difficultyPreset;
        
        float CurrentOrgansPerBed => difficultyPreset.organsPerBedRequired * difficultyPreset.organsPerBedCurve.Evaluate(CurrentProgress);
        float CurrentMaxBedsOnTheLevel => difficultyPreset.maxBedsOnTheLevel * difficultyPreset.maxBedsOnTheLevelCurve.Evaluate(CurrentProgress);
        float CurrentBedSpawnDuration => difficultyPreset.bedSpawnDuration * difficultyPreset.bedSpawnDurationCurve.Evaluate(CurrentProgress);
        float CurrentBedDurationUntilDeath => difficultyPreset.bedDurationUntilDeath * difficultyPreset.bedDurationUntilDeathCurve.Evaluate(CurrentProgress);


        
        void Update()
        {
            _timeProgress += Time.deltaTime;
            if (_timeProgress - _lastInterval > updateInterval)
            {
                _lastInterval = _timeProgress;
                UpdateDifficulty(true);
            }
        }

        private void UpdateDifficulty(bool progression)
        {
            if (progression) {
                difficultyPreset.organsPerBedRequired += (int)CurrentOrgansPerBed;
                difficultyPreset.maxBedsOnTheLevel += (int)CurrentMaxBedsOnTheLevel;
                difficultyPreset.bedSpawnDuration += CurrentBedSpawnDuration;
                difficultyPreset.bedDurationUntilDeath += CurrentBedDurationUntilDeath;
            }
            
            BedSpawner[] spawners = Object.FindObjectsByType<BedSpawner>(FindObjectsSortMode.None);
            foreach (var bedSpawner in spawners)
            {
                bedSpawner.SetDifficultyPreset(difficultyPreset);
            }
            
            OrganPedestal[] pedestals = Object.FindObjectsByType<OrganPedestal>(FindObjectsSortMode.None);
            foreach (var pedestal in pedestals)
            {
                pedestal.SetSpawnDuration(difficultyPreset.pedestalOrganDuration);
            }
        }
        
        private void Start()
        {
            UpdateDifficulty(false);
        }
    }
}