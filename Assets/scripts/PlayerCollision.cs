﻿using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class PlayerCollision : MonoBehaviour {

    private const float skinWidth = 0.15f;

    [SerializeField]
    private LayerMask collisionMask;

    private int horizontalRayCount = 4;
    private int verticalRayCount = 4;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;

	private BoxCollider2D myCollider;
    private RaycastOrigins raycastOrigins;

    private void Awake ()
    {
        myCollider = GetComponent<BoxCollider2D> ();
    }

    private void Start ()
    {
        CalculateRaySpacing ();
    }

    public void Move (Vector3 velocity)
    {
        UpdateRaycastOrigins ();

        if (velocity.x != 0)
        {
            HorizontalCollisions (ref velocity);
        }
        if (velocity.y != 0)
        {
            VerticalCollisions (ref velocity);
        }

        transform.Translate (velocity, Space.World);
    }

    private void HorizontalCollisions (ref Vector3 velocity)
    {
        float directionX = Mathf.Sign (velocity.x);
        float rayLength = Mathf.Abs (velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);

            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
            }

            Debug.DrawRay (rayOrigin, Vector2.right * directionX * rayLength, Color.red);
        }
    }

    private void VerticalCollisions (ref Vector3 velocity)
    {
        float directionY = Mathf.Sign (velocity.y);
        float rayLength = Mathf.Abs (velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;
            }

            Debug.DrawRay (rayOrigin, Vector2.up * directionY * rayLength, Color.red);
        }
    }

    private void UpdateRaycastOrigins ()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand (skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
    }

    private void CalculateRaySpacing ()
    {
        Bounds bounds = myCollider.bounds;
        bounds.Expand (skinWidth * -2);

        horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    private struct RaycastOrigins {
        public Vector2 topLeft, topRight, bottomLeft, bottomRight;
    }

}