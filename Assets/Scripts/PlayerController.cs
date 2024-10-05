using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Transform _clawParentTransform;
    [SerializeField] private Sprite _clawSprite;
    [SerializeField] private CircleCollider2D clawCollider;

    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _movingDownSpeed = 3f;
    [SerializeField] private float _movingLeftOrRightSpeed = 1f;
    [SerializeField] private float cableLength = -1f;

    private SpriteRenderer clawSpriteRenderer;
    private float originalColliderRadius;
    
    private float minRotationAngle = -55f;
    private float maxRotationAngle = 55f;
    private float rotationAngle = 0f;


    private bool v_IsMovingDown;
    private bool v_CanRotate;
    private bool v_IsMovingRightOrLeft;
    private bool v_IsMovingRight;
    private bool v_IsRotatingRight;
    private bool v_IsGrabbing;
    private bool v_CollectedBoosterThrust;
    private bool v_IsBoostedThrustActive;
    private bool v_IsDrillActive;
    private float _boostedSpeed;

    private float initialX;
    private float initialY;
    private float initialMoveSpeed;

    private RopeRenderer ropeRenderer;
    private Vector3 ropeStartPos;  

    Item grabbedItem;

    private void Awake()
    {
        originalColliderRadius = clawCollider.radius;

        if (_clawParentTransform != null)
        {
            ropeRenderer = _clawParentTransform.GetComponent<RopeRenderer>();
            ropeRenderer.SetClawTransform(transform);
        }
        
        if (ropeRenderer == null)
        {
            Debug.LogError("RopeRenderer component not found on ClawParent!");
        }
    }

    private void Start()
    {
        Transform clawTransform = transform.Find("Claw");
        if (clawTransform != null)
        {
            clawSpriteRenderer = clawTransform.GetComponent<SpriteRenderer>();
            clawSpriteRenderer.sprite = _clawSprite;   
        }
        else
        {
            Debug.LogError("Claw child object not found!");
        }

        initialX = transform.position.x;
        initialY = transform.position.y;
        initialMoveSpeed = _movingDownSpeed;

        v_CanRotate = true;
        v_IsRotatingRight = true;
        v_CollectedBoosterThrust = false;
        v_IsBoostedThrustActive = false;
        v_IsDrillActive = false;

        // BoosterManager.Instance.OnSpeedThrustActivated += ActivateBoostedThrust;
    }
    private void Update()
    {
        if (v_CanRotate)
        {
            Rotate();
        }
        
        GetInput();
        
        if (v_IsMovingRightOrLeft)
        {
            MoveLeftOrRight();
        }

        HandleThrust();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (v_CanRotate)
            {
                ropeStartPos = transform.position;  

                v_CanRotate = false;
                v_IsMovingDown = true;
                v_IsMovingRightOrLeft = false;

                ropeRenderer.RenderLine(ropeStartPos, transform.position, true);        
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (v_CanRotate)
            {
                v_CanRotate = true;
                v_IsMovingDown = false;
                v_IsMovingRightOrLeft = true;
                v_IsMovingRight = false;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (v_CanRotate)
            {
                v_CanRotate = true;
                v_IsMovingDown = false;
                v_IsMovingRightOrLeft = true;
                v_IsMovingRight = true;
            }
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (v_IsGrabbing)
            {
                if (BoosterManager.Instance.IsTNTStored())
                {
                    BoosterManager.Instance.UseStoredTNTBooster();
                    Destroy(grabbedItem.gameObject);
                    StopGrabbing();
                    SetThrustSpeedToInitial();
                }
            }
        }
    }

    
    private void MoveLeftOrRight()
    {
        if(v_IsMovingDown) return;

        var rightBorder = GameManager.Instance.RightBorder;
        var leftBorder = GameManager.Instance.LeftBorder;

        if (v_IsMovingRightOrLeft)
        {
            var tempPosition = transform.position;

            if(v_IsMovingRight)
            {
                if (tempPosition.x <= rightBorder)
                {
                    tempPosition.x += Time.deltaTime * _movingLeftOrRightSpeed;
                }
            }
            else
            {
                if (tempPosition.x >= leftBorder)
                {
                    tempPosition.x -= Time.deltaTime * _movingLeftOrRightSpeed;
                }
            }

            transform.position = tempPosition;
            v_IsMovingRightOrLeft = false;
        } 
    }

    private void HandleThrust()
    {
        if (v_CanRotate) return;
        
        else
        {
            var rightBorder = GameManager.Instance.RightBorder;
            var leftBorder = GameManager.Instance.LeftBorder;
            var tempPosition = transform.position;

            if(v_IsMovingDown)
            {
                tempPosition -= transform.up * Time.deltaTime * _movingDownSpeed;
            }
            else
            {
                tempPosition += transform.up * Time.deltaTime * _movingDownSpeed;
            }

            transform.position = tempPosition;

            if (tempPosition.y <= cableLength) // has reached cable length
            {
                v_IsMovingDown = false;
            }

            if ((tempPosition.x >= rightBorder) ||
                (tempPosition.x <= leftBorder))
            {
                v_IsMovingDown = false;
            }

            if (tempPosition.y >= initialY) // has claw reached initial height 
            {
                if (v_IsGrabbing && grabbedItem != null)
                {
                    LevelManager.Instance.OnItemDestroyed(grabbedItem);
                    grabbedItem.itemData.Collect();
                    GameManager.Instance.AddScore(grabbedItem);
                    Destroy(grabbedItem.gameObject);
                    grabbedItem = null;
                    v_IsGrabbing = false;
                    Debug.Log("Item Destroyed!");
                }

                if (v_IsDrillActive)
                {
                    v_IsDrillActive = false;
                    ChangeClawSprite(_clawSprite);
                    clawCollider.radius = originalColliderRadius;
                }
                v_CanRotate = true;
                ropeRenderer.RenderLine(ropeStartPos, tempPosition, false);  // Disable rope rendering once claw reaches top
                _movingDownSpeed = initialMoveSpeed;
                
                BoosterManager.Instance.ApplyingNextThrustBooster();
            }

            ropeRenderer.RenderLine(ropeStartPos, transform.position, true);
        }
    }

    private void Rotate()
    {
        if (!v_CanRotate) return;


        float rotationStep = _rotationSpeed * Time.deltaTime;

        if (v_IsRotatingRight) 
        {
            rotationAngle += rotationStep;
        }
        else 
        {
            rotationAngle -= rotationStep;
        }

        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        if(rotationAngle >= maxRotationAngle)
        {
            v_IsRotatingRight = false;
        }
        else if(rotationAngle <= minRotationAngle)
        {
            v_IsRotatingRight = true;
        }
    }
    
    public void StopClawMovement()
    {
        v_IsMovingDown = false;
        Debug.Log("Stopped moving down");
    }

    public void ResetClawMovement()
    {
        transform.position = new Vector3(initialX, initialY, transform.position.z);

        rotationAngle = 0f;
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);
        
        _movingDownSpeed = initialMoveSpeed;
        v_IsMovingDown = false;
        v_CanRotate = true;
        v_IsRotatingRight = true;
        v_IsGrabbing = false;

        ropeRenderer.RenderLine(Vector3.zero, Vector3.zero, false);
    }

    public void Grab(Item item)
    {
        v_IsGrabbing = true;
        grabbedItem = item;

        if (grabbedItem.itemData is GrabbableItemData grabbableItem)
        {
            float weightModifier = 1 / Mathf.Max(grabbableItem.weight, 1f);
            _movingDownSpeed *= weightModifier;
            Debug.Log($"Grabbed {grabbableItem.itemName} with weight: {grabbableItem.weight}.");
        }
    }

    public void StopGrabbing()
    {
        v_IsGrabbing = false;
    }

    public void SetThrustSpeedToInitial()
    {
        _movingDownSpeed = initialMoveSpeed;
    }

    public void CollectedBoosterForNextThrust(IBooster booster)
    {
        if (booster is SpeedBoosterItem speedBooster)
        {
            _movingDownSpeed = speedBooster.GetSpeedBoost();
            v_IsBoostedThrustActive = true;
        }
    }
    public void ChangeClawSprite(Sprite sprite)
    {
        if (clawSpriteRenderer != null)
        {
            clawSpriteRenderer.sprite = sprite;
        }
        else
        {
            Debug.LogError("clawSpriteRenderer is null!");
        }
    }

    internal void IncreaseColliderSizeForDrill(float number)
    {
        clawCollider.radius = originalColliderRadius * number; // Example: increase by 50%
    }


    internal void SetDrillActive()
    {
        v_IsDrillActive = true;
    }

    internal bool IsDrillActive()
    {
        return v_IsDrillActive;
    }
}

