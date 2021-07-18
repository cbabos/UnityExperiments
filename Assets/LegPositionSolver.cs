using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegPositionSolver : MonoBehaviour
{
    [Range(0.1f, 0.5f)] public float gizmoRadius = 0.1f;
    [Range(0.1f, 0.5f)] public float stepHeight = 0.4f;
    [Range(0.1f, 1f)] public float stepDistance = 0.1f;
    [Range(0.1f, 10f)] public float speed = 0.5f;
    public LayerMask groundLayer;
    public float distanceFromBody = 1f;
    public Transform body;
    public LegPositionSolver otherLeg; 
    
    Vector3 currentPosition = Vector3.zero;
    Vector3 oldPosition = Vector3.zero;
    Vector3 newPosition = Vector3.zero;
    public float lerp = 1;

    private Vector3 vectorFromBody = Vector3.zero;
    


    // Start is called before the first frame update
    void Start()
    {
        oldPosition = currentPosition = newPosition = transform.position;
        vectorFromBody = body.position - transform.position;
        vectorFromBody.y = 2f;
    }

    private bool canMove()
    {
        return lerp >= 1 && otherLeg.lerp >= 1;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = currentPosition;

        Ray floorChecker = new Ray(body.position + vectorFromBody, Vector3.down);
        if (Physics.Raycast(floorChecker, out RaycastHit info, 10f, groundLayer) & canMove())
        {
            if (Vector3.Distance(newPosition, info.point) > stepDistance)
            {
                newPosition = info.point;
                lerp = 0;
            }
        }

        if (lerp < 1)
        {
            Vector3 legPosition = Vector3.Lerp(oldPosition, newPosition, lerp);
            legPosition.y += Mathf.Sin(lerp * Mathf.PI) * stepHeight;

            currentPosition = legPosition;
            lerp += Time.deltaTime * speed;
        }
        else
        {
            oldPosition = newPosition;
        }
    }

    private void OnDrawGizmos()
    {
        Ray floorChecker = new Ray(body.position + vectorFromBody, Vector3.down);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(floorChecker);
        
        Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(body.position + vectorFromBody, gizmoRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(oldPosition, gizmoRadius);
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(newPosition, gizmoRadius);
    }
}
