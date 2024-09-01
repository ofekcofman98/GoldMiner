using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerController : Singleton<PlayerController>
{
    // [SerializeField] private Rigidbody2D _clawRigidBody;
    // [SerializeField] private GameObject _claw;
    [SerializeField] private Transform _clawParentTransform; // Drag ClawParent here in the Inspector
    [SerializeField] private float _rotationSpeed = 5f;

    private float minRotationAngle = -55f;
    private float maxRotationAngle = 55f;
    private float rotationAngle = 0f;
    private float moveSpeed = 3f;
    private float cableLength = -1f;

    private bool isMovingDown;
    private bool canRotate;
    private bool isRotatingRight;
    private bool isGrabbing;

    private float initialY;
    private float initialMoveSpeed;

    private RopeRenderer ropeRenderer;
    Item grabbedItem;

        private void Awake()
    {
        if (_clawParentTransform != null)
        {
            ropeRenderer = _clawParentTransform.GetComponent<RopeRenderer>();
        }
        
        if (ropeRenderer == null)
        {
            Debug.LogError("RopeRenderer component not found on ClawParent!");
        }
    }

    private void Start()
    {
        initialY = /*_claw.*/transform.position.y;
        initialMoveSpeed = moveSpeed;
        canRotate = true;
        isRotatingRight = true;
    }
    private void Update()
    {
        Rotate();
        GetInput();
        HandleThrust();
        //MoveLeftOrRight();
    }

    // private void MoveLeftOrRight()
    // {
    //     throw new NotImplementedException();
    // }


    private void GetInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (canRotate)
            {
                canRotate = false;
                isMovingDown = true;
            }
        }

        // if (Input.GetKey(KeyCode.LeftApple) ||
        //     Input.GetKey(KeyCode.RightArrow))
        // {
        //     if (canRotate)
        //     {
        //         canRotate = true;
        //         moveDown = false;
        //     }
        // }
    }

    private void HandleThrust()
    {
        if (canRotate) return;
        
        else
        {
            var tempPosition = /*_claw.*/transform.position;

            if(isMovingDown)
            {
                tempPosition -= transform.up * Time.deltaTime * moveSpeed;
            }
            else
            {
                tempPosition += transform.up * Time.deltaTime * moveSpeed;
            }

            /*_claw.*/transform.position = tempPosition;

            if (tempPosition.y <= cableLength)
            {
                isMovingDown = false;
            }

            if (tempPosition.y >= initialY)
            {
                if (isGrabbing && grabbedItem != null)
                {
                    Destroy(grabbedItem.gameObject);
                    grabbedItem = null;
                    isGrabbing = false;
                    Debug.Log("Item Destroyed!");
                }
                canRotate = true;
                ropeRenderer.RenderLine(tempPosition, false);
                moveSpeed = initialMoveSpeed;
            }

            ropeRenderer.RenderLine(transform.position, true);
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

        /*_claw.*/transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

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

    }
}
