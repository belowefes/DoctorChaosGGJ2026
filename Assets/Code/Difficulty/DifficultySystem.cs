using System;
using Code.Difficulty;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Difficulty
{
    public class DifficultySystem : MonoBehaviour
    {
        public static DifficultySystem instance;
        public DifficultyPreset difficultyPreset;

        private void Start()
        {
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
    }
}