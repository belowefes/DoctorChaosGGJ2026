using System;
using Code.Inventary;
using Code.Utils;
using UnityEngine;
using UnityEngine.InputSystem;  


public class PlayerController : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private InputAction moveInput;
    
    [SerializeField] private SpriteRenderer inHandsRenderer;
    [SerializeField] private PickupItem currentPickupInHands;
        
    Rigidbody2D rb;

    private void Awake()
    {
        if (inHandsRenderer == null)
        {
            throw new NullReferenceException("inHandsIcon spriter is missing");
        } 
        rb = GetComponent<Rigidbody2D>();
    }
    
    void OnEnable()
    {
        moveInput.Enable();
    }

    void OnDisable()
    {
        moveInput.Disable();
    }

    void FixedUpdate()
    {
        Vector2 moveDirection = moveInput.ReadValue<Vector2>();
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }

    public bool ReceiveOrgan(PickupItem pickup)
    {
        if (currentPickupInHands != null)
        {
            return false;
        }
        
        if (pickup.pickupType == PickupType.Organ)
        {
            SetInHands(pickup);
            return true;
        }

        return false;
    }

    void SetInHands(PickupItem pickup)
    {
        if (pickup == null)
        {
            inHandsRenderer.gameObject.SetActive(false);
            currentPickupInHands = null;
            return;
        }
        
        inHandsRenderer.gameObject.SetActive(true);
        currentPickupInHands = pickup;
        SpriteUtils.SetPicture(pickup.icon, inHandsRenderer);
    }

    public bool TryRequirementAgainstHand(PickupItem pickup)
    {
        if (this.currentPickupInHands == null)
        {
            return false;
        }
        return this.currentPickupInHands.name.Equals(pickup.name);
    }
    
    public bool TryTakeItemFromHands()
    {
        if (this.currentPickupInHands == null) return false;
        SetInHands(null);
        return true;
    }
}
