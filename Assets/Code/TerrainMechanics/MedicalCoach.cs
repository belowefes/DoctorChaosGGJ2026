using System;
using System.Collections.Generic;
using Code.Inventary;
using Code.Timer;
using Code.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class MedicalCoach : MonoBehaviour
{
    [SerializeField] private int organAmountRequired = 4;
    
    [SerializeField] private PickupItem slot;
    [SerializeField] private Timer timer;
    
    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private float iconHeight;
    
    [SerializeField] private SpriteRenderer timerVisualRenderer;
    Material mat;
    
    [SerializeField] private List<PickupItem> organsPool;
    
    private void Start()
    {
        mat = timerVisualRenderer.material;
        if (timer == null)
        {
            timer = this.GetComponent<Timer>();
            if (timer == null)
            {
                throw new NullReferenceException("MedicalCoach timer wasn't found on GameObject: "+this.GetEntityId());
            }
        }
        
        if (slot == null)
        {
            timer.StartTimer();
        }
    }
    
    void Reset()
    {
        iconRenderer.sprite = null;
        slot = null;
        // timer.StartTimer();
    }
    
    void SetIcon(Sprite sprite)
    {
        if (iconRenderer == null || sprite == null) return;
        SpriteUtils.SetPicture(sprite, iconRenderer);
        // Debug.Log($"spriteSize={spriteSize}, maxSide={maxSide}, scale={iconHolder.transform.localScale}");
    }
    
    public void NextLoop()
    {
        if (organAmountRequired == 0)
        {
            onSaved();
            return;
        }
        
        PickupItem currentItem = null;
        switch (organsPool.Count)
        {   
            case 0:
                throw new NullReferenceException("There are no active items on the pool for medical coach");
            case 1:
                currentItem = organsPool[0];
                break;
            default:
                currentItem = organsPool[Random.Range(0, organsPool.Count)];
                break;
        }
        putIn(currentItem);
        organAmountRequired--;
    }
    
    void putIn(PickupItem item)
    {
        this.slot = item;
        if (item == null)
        {
            return;
        }
        // print("setting up icon from " + item.name);
        SetIcon(item.icon);
    }
    
    public void OnTimerFinish(float duration)
    {
        Destroy(this.gameObject);
    }
    
    public void OnTimerTick(float progress)
    {
        if (mat == null) return;
        mat.SetFloat("_Arc1", progress * 360f);
    }

    void onSaved()
    {
        print("SAVED");
        timer.enabled = false;
        timerVisualRenderer.enabled = false;
    }
    
    
}
