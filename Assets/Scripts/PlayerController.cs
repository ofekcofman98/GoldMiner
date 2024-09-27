using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerController : Singleton<PlayerController>
{
    // [SerializeField] private Rigidbody2D _clawRigidBody;
    // [SerializeField] private GameObject _claw;
    [SerializeField] private Transform _clawParentTransform; // Drag ClawParent here in the Inspector
    [SerializeField] private float _rotationSpeed = 5f;
    [SerializeField] private float _movingDownSpeed = 3f;
    [SerializeField] private float _movingLeftOrRightSpeed = 1f;
    [SerializeField] private float cableLength = -1f;



    private float minRotationAngle = -55f;
    private float maxRotationAngle = 55f;
    private float rotationAngle = 0f;


    private bool isMovingDown;
    private bool canRotate;
    private bool isMovingRightOrLeft;
    private bool isMovingRight;
    private bool isRotatingRight;
    private bool isGrabbing;

    private float initialY;
    private float initialMoveSpeed;

    private RopeRenderer ropeRenderer;
    private Vector3 ropeStartPos;  

    Item grabbedItem;
    private AudioSource audioSource;

    private void Awake()
    {
        if (_clawParentTransform != null)
        {
            ropeRenderer = _clawParentTransform.GetComponent<RopeRenderer>();
            ropeRenderer.SetClawTransform(transform);

        }
        
        if (ropeRenderer == null)
        {
            Debug.LogError("RopeRenderer component not found on ClawParent!");
        }

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

    }

    private void Start()
    {
        
        initialY = /*_claw.*/transform.position.y;
        initialMoveSpeed = _movingDownSpeed;
        canRotate = true;
        isRotatingRight = true;
    }
    private void Update()
    {
        if (canRotate)
        {
            Rotate();
        }
        
        GetInput();
        
        if (isMovingRightOrLeft)
        {
            MoveLeftOrRight();
        }

        HandleThrust();
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (canRotate)
            {
                ropeStartPos = transform.position;  

                canRotate = false;
                isMovingDown = true;
                isMovingRightOrLeft = false;

                ropeRenderer.RenderLine(ropeStartPos, transform.position, true);        
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (canRotate)
            {
                canRotate = true;
                isMovingDown = false;
                isMovingRightOrLeft = true;
                isMovingRight = false;
            }
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (canRotate)
            {
                canRotate = true;
                isMovingDown = false;
                isMovingRightOrLeft = true;
                isMovingRight = true;
            }
        }
    }

    private void MoveLeftOrRight()
    {
        if(isMovingDown) return;

        var rightBorder = GameManager.Instance.RightBorder;
        var leftBorder = GameManager.Instance.LeftBorder;

        if (isMovingRightOrLeft)
        {
            var tempPosition = transform.position;

            if(isMovingRight)
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
            isMovingRightOrLeft = false;
        } 
    }

    private void HandleThrust()
    {
        if (canRotate) return;
        
        else
        {
            var rightBorder = GameManager.Instance.RightBorder;
            var leftBorder = GameManager.Instance.LeftBorder;
            var tempPosition = transform.position;

            if(isMovingDown)
            {
                tempPosition -= transform.up * Time.deltaTime * _movingDownSpeed;
            }
            else
            {
                tempPosition += transform.up * Time.deltaTime * _movingDownSpeed;
            }

            transform.position = tempPosition;

            if (tempPosition.y <= cableLength)
            {
                isMovingDown = false;
            }

            if ((tempPosition.x >= rightBorder) ||
                (tempPosition.x <= leftBorder))
            {
                isMovingDown = false;
            }
            // add touching wall

            if (tempPosition.y >= initialY)
            {
                if (isGrabbing && grabbedItem != null)
                {
                    PlayGrabbedItemSound(grabbedItem); 
                    LevelManager.Instance.OnItemDestroyed(grabbedItem);
                    grabbedItem.itemData.Collect();
                    GameManager.Instance.AddScore(grabbedItem);
                    Destroy(grabbedItem.gameObject);
                    grabbedItem = null;
                    isGrabbing = false;
                    Debug.Log("Item Destroyed!");
                }
                canRotate = true;
                ropeRenderer.RenderLine(ropeStartPos, tempPosition, false);  // Disable rope rendering once claw reaches top
                _movingDownSpeed = initialMoveSpeed;
            }

            ropeRenderer.RenderLine(ropeStartPos, transform.position, true);
        }
    }


    private void Rotate()
    {
        if (!canRotate) return;


        float rotationStep = _rotationSpeed * Time.deltaTime;

        if (isRotatingRight) 
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
            isRotatingRight = false;
        }
        else if(rotationAngle <= minRotationAngle)
        {
            isRotatingRight = true;
        }
    }
    

    public void StopClawMovement()
    {
        isMovingDown = false;
        Debug.Log("Stopped moving down");
    }

    public void Grab(Item item)
    {
        isGrabbing = true;
        grabbedItem = item;

        if (grabbedItem.itemData is GrabbableItemData grabbableItem)
        {
            float weightModifier = 1 / Mathf.Max(grabbableItem.weight, 1f);
            _movingDownSpeed *= weightModifier;
            Debug.Log($"Grabbed {grabbableItem.itemName} with weight: {grabbableItem.weight}.");
        }
    }

       private void PlayGrabbedItemSound(Item grabbedItem)
    {
        if (grabbedItem.itemData.Sound != null && audioSource != null)
        {
            audioSource.PlayOneShot(grabbedItem.itemData.Sound);
        }
    }
 
}

