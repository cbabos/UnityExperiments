using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Range(0.1f, 1.0f)]
    public float movementSpeed = .2f;

    public LayerMask groundMask;
    private CharacterController controller;
    private bool isLookingRight = true;
    private Vector3 distanceFromFloor = Vector3.zero;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        Ray floorChecker = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(floorChecker, out RaycastHit info, 10f, groundMask))
        {
            distanceFromFloor = transform.position - info.point;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float movementX = Input.GetAxis("Horizontal");
        
        if (movementX < 0 && isLookingRight)
        {
            // Look backward
            transform.LookAt(transform.position + transform.forward * -1f);
            isLookingRight = false;
        }
        
        if (movementX > 0 && !isLookingRight)
        {
            // Look backward
            transform.LookAt(transform.position + transform.forward * -1f);
            isLookingRight = true;
        }
        
        controller.Move(transform.right * (Mathf.Abs(movementX) * movementSpeed));
        Ray floorChecker = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(floorChecker, out RaycastHit info, 10f, groundMask))
        {
            transform.position = info.point + distanceFromFloor;
        }
    }
}
