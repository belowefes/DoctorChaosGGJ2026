using System;
using System.Collections.Generic;
using Code.Inventary;
using Code.Timer;
using Code.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Collider2D))]
public class OrganPedestal : MonoBehaviour
{
    [SerializeField] private PickupItem slot;
    [SerializeField] private Timer timer;

    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private float iconHeight;
    
    [SerializeField] private SpriteRenderer timerVisualRenderer;
    Material mat;

    [SerializeField] private List<PickupItem> organsPool;

    private void Start()
    {
        if (timer == null)
        {
            timer = this.GetComponent<Timer>();
            if (timer == null)
            {
                throw new NullReferenceException("Pedestal timer wasn't found on GameObject: "+this.GetEntityId());
            }
        }
        
        if (slot == null)
        {
            timer.StartTimer();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (slot == null || !other.CompareTag("Player"))
            return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (!player.ReceiveOrgan(slot))
        {
            return;
        }

        Reset();
    }

    void Reset()
    {
        iconRenderer.sprite = null;
        slot = null;
        timer.StartTimer();
    }

    void SetIcon(Sprite sprite)
    {
        if (iconRenderer == null || sprite == null) return;
        SpriteUtils.SetPicture(sprite, iconRenderer);
        // Debug.Log($"spriteSize={spriteSize}, maxSide={maxSide}, scale={iconHolder.transform.localScale}");
    }

    void Awake()
    {
        putIn(this.slot);
        mat = timerVisualRenderer.material;
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
        PickupItem currentItem = null;
        
        switch (organsPool.Count)
        {   
            case 0:
                return;
            case 1:
                currentItem = organsPool[0];
                break;
            default:
                currentItem = organsPool[Random.Range(0, organsPool.Count)];
                break;
        }
        mat.SetFloat("_Arc1", 0);
        putIn(currentItem);
    }
    
    public void OnTimerTick(float progress)
    {
        if (mat == null) return;
        mat.SetFloat("_Arc1", 360f-progress * 360f);
    }
}
