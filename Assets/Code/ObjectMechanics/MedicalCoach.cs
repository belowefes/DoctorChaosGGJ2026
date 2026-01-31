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

    [SerializeField] private PickupItem requirementSlot;
    [SerializeField] private Timer timer;

    [SerializeField] private SpriteRenderer iconRenderer;
    [SerializeField] private float iconHeight;

    [SerializeField] private SpriteRenderer timerVisualRenderer;
    Material mat;

    [SerializeField] private List<PickupItem> organsPool;
    private int _organLastUsedIndex = -1;

    [SerializeField] private AudioSource sourceBedSound;

    private Rigidbody2D _rb;

    private void Start()
    {
        mat = timerVisualRenderer.material;

        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            throw new NullReferenceException("MedicalCoach must have rigidbody: " + this.GetEntityId());
        }
        
        if (timer == null)
        {
            timer = this.GetComponent<Timer>();
            if (timer == null)
            {
                throw new NullReferenceException("MedicalCoach timer wasn't found on GameObject: " +
                                                 this.GetEntityId());
            }
        }

        NextLoop();
        timer.StartTimer();
    }

    void SetIcon(Sprite sprite)
    {
        if (iconRenderer == null || sprite == null) return;
        SpriteUtils.SetPicture(sprite, iconRenderer);
        // Debug.Log($"spriteSize={spriteSize}, maxSide={maxSide}, scale={iconHolder.transform.localScale}");
    }

    public void NextLoop()
    {
        iconRenderer.sprite = null;
        requirementSlot = null;
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
                _organLastUsedIndex = 0;
                break;
            default:
                int newOrganIndex = Random.Range(0, organsPool.Count);
                while (_organLastUsedIndex == newOrganIndex)
                {
                    newOrganIndex = Random.Range(0, organsPool.Count);
                }

                currentItem = organsPool[newOrganIndex];
                _organLastUsedIndex = newOrganIndex;
                break;
        }

        putIn(currentItem);
        organAmountRequired--;
    }

    void putIn(PickupItem item)
    {
        this.requirementSlot = item;
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

    public void TryHelp(Collider2D helper)
    {
        if (helper.tag.Equals("Player"))
        {
            PlayerController player = helper.GetComponent<PlayerController>();
            if (player.TryRequirementAgainstHand(this.requirementSlot) && player.TryTakeItemFromHands())
            {
                this.timer.AddDuration(this.requirementSlot.timeResource);
                NextLoop();
            }
        }
    }

    void onSaved()
    {
        print("SAVED");
        timer.Stop();
        timer.enabled = false;
        timerVisualRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        if (_rb.linearVelocity != Vector2.zero && sourceBedSound != null && !sourceBedSound.isPlaying)
        {
            sourceBedSound.Play();
        }
        if (_rb.linearVelocity == Vector2.zero && sourceBedSound != null && sourceBedSound.isPlaying)
        {
            sourceBedSound.Stop();
        }
    }
}