using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

public class PlayerController : Singleton<PlayerController>
{
    // [SerializeField] private Rigidbody2D _clawRigidBody;
    [SerializeField] private float swingSpeed = 5f;

    private float minRotationAngle = -55f;
    private float maxRotationAngle = 55f;
    private float rotationAngle = 0f;
    private float moveSpeed = 3f;
    private float cableLength = -1f;
    private bool moveDown;
    private bool canRotate;
    private bool rotateRight;

    private float initialY;
    private float initialMoveSpeed;

    private void Start()
    {
        initialY = transform.position.y;
        initialMoveSpeed = moveSpeed;
        canRotate = true;
        rotateRight = true;
    }
    private void Update()
    {
        Rotate();
        GetInput();
        HandleThrust();
    }

    private void HandleThrust()
    {
        if (canRotate) return;
        
        else
        {
            Vector3 temp = transform.position;

            if(moveDown)
            {
                temp -= transform.up * Time.deltaTime * moveSpeed;
            }
            else
            {
                temp += transform.up * Time.deltaTime * moveSpeed;
            }

            transform.position = temp;

            if (temp.y <= cableLength)
            {
                moveDown = false;
            }
            if (temp.y >= initialY)
            {
                canRotate = true;
                moveDown = true;
                moveSpeed = initialMoveSpeed;
            }
        }
    }

    private void GetInput()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (canRotate)
            {
                canRotate = false;
                moveDown = true;
            }
        }
    }

    private void Rotate()
    {
        if (!canRotate) return;
        if (rotateRight) 
        {
            rotationAngle += swingSpeed + Time.deltaTime;
        }
        else 
        {
            rotationAngle -= swingSpeed + Time.deltaTime;
        }
        transform.rotation = Quaternion.AngleAxis(rotationAngle, Vector3.forward);

        if(rotationAngle >= maxRotationAngle)
        {
            rotateRight = false;
        }
        else if(rotationAngle <= minRotationAngle)
        {
            rotateRight = true;
        }


    }
}
