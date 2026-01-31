using System;
using System.Collections.Generic;
using System.ComponentModel;
using Code.Difficulty;
using Code.Timer;
using UnityEngine;
using Random = UnityEngine.Random;

public class BedSpawner : MonoBehaviour
{
    [SerializeField] private int maxBedsOnTheLevel = 5;
    [SerializeField] private List<Transform> slots;
    [SerializeField] private Timer timer;
    [SerializeField] private GameObject bedPrefab;
    [SerializeField] private GameObject parentBedsObject;
    [SerializeField] private float bedDurationUntilDeath;
    [SerializeField] private int organAmountRequired;
    
    
    
    private void Start()
    {
        if (timer == null)
        {
            timer = this.GetComponent<Timer>();
            if (timer == null)
            {
                throw new NullReferenceException("Spawner timer wasn't found on GameObject: "+this.GetInstanceID());
            }
        }
        
        if (slots.Count > 0)
        {
            timer.StartTimer();
        }
        else
        {
            throw new WarningException("No slots found on BedSpawners");
        }
    }
    
    public void OnTimerFinish(float duration)
    {
        timer.StartTimer();
        if (parentBedsObject != null && parentBedsObject.transform.childCount >= maxBedsOnTheLevel)
        {
            return;
        }
        GameObject newBed = Instantiate<GameObject>(
            bedPrefab,
            slots[Random.Range(0, slots.Count)].transform.position,
            Quaternion.identity,
            parentBedsObject?.transform
        );
        MedicalCoach medicalCoach = newBed.GetComponent<MedicalCoach>();
        medicalCoach.SetOrganSwitchDuration(bedDurationUntilDeath);
        medicalCoach.SetOrgansRequired(organAmountRequired);
        
    }
    
    public void SetDifficultyPreset(DifficultyPreset preset)
    {
        this.maxBedsOnTheLevel = preset.maxBedsOnTheLevel;
        this.timer.SetDuration(preset.bedSpawnDuration);
        this.bedDurationUntilDeath = preset.bedDurationUntilDeath;
        this.organAmountRequired = preset.organsPerBedRequired;
    }

}
