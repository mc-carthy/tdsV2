﻿using UnityEngine;

public class CameraController : MonoBehaviour {

	private Player target;
    private Bounds targetBounds;
    private Vector2 focusAreaSize = new Vector2 (4f, 3f);
    private FocusArea focusArea;
    private float lookAheadDistX = 4f;
    private float lookAheadDistY = 3f;
    private float lookSmoothTime = 0.5f;
    private float currentLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirectionX;
    private float currentLookAheadY;
    private float targetLookAheadY;
    private float lookAheadDirectionY;
    private float smoothLookVelocityX;
    private float smoothLookVelocityY;
    private bool isLookAheadStopped;

    private void Start ()
    {
        target = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
        focusArea = new FocusArea (targetBounds, focusAreaSize);
    }

    private void LateUpdate ()
    {
        focusArea.Update (target.MyCollider.bounds);

        Vector2 focusPosition = focusArea.centre;

        if (focusArea.velocity.x != 0)
        {
            lookAheadDirectionX = Mathf.Sign (focusArea.velocity.x);
            if (Mathf.Sign (target.MyInput.x) == Mathf.Sign (focusArea.velocity.x) && target.MyInput.x != 0)
            {
                isLookAheadStopped = false;
                targetLookAheadX = lookAheadDirectionX * lookAheadDistX;
            }
            else
            {
                if (!isLookAheadStopped)
                {
                    isLookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistX - currentLookAheadX) * 0.25f;
                }
            }
        }

        if (focusArea.velocity.y != 0)
        {
            lookAheadDirectionY = Mathf.Sign (focusArea.velocity.y);
            if (Mathf.Sign (target.MyInput.y) == Mathf.Sign (focusArea.velocity.y) && target.MyInput.y != 0)
            {
                isLookAheadStopped = false;
                targetLookAheadY = lookAheadDirectionY * lookAheadDistY;
            }
            else
            {
                if (!isLookAheadStopped)
                {
                    isLookAheadStopped = true;
                    targetLookAheadY = currentLookAheadY + (lookAheadDirectionY * lookAheadDistY - currentLookAheadY) * 0.25f;
                }
            }
        }

        currentLookAheadX = Mathf.SmoothDamp (currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTime);
        currentLookAheadY = Mathf.SmoothDamp (currentLookAheadY, targetLookAheadY, ref smoothLookVelocityY, lookSmoothTime);

        // focusPosition.y = Mathf.SmoothDamp (transform.position.y, focusPosition.y, ref smoothLookVelocityY, lookSmoothTime);
        focusPosition += Vector2.right * currentLookAheadX;
        focusPosition += Vector2.up * currentLookAheadY;

        transform.position = (Vector3) focusPosition + Vector3.forward * -10f;
    }

    private void OnDrawGizmos ()
    {
        Gizmos.color = new Color (1, 0, 0, 0.5f);
        Gizmos.DrawCube (focusArea.centre, focusAreaSize);
    }

    private struct FocusArea {
        public Vector2 centre;
        public Vector2 velocity;
        private float left, right;
        private float top, bottom;

        public FocusArea (Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2f;
            right = targetBounds.center.x + size.x / 2f;
            top = targetBounds.center.y + size.y / 2f;
            bottom = targetBounds.center.y - size.y / 2f;

            centre = new Vector2 ((left + right) / 2f, (top + bottom) / 2f);
            velocity = Vector2.zero;
        }

        public void Update (Bounds targetBounds)
        {
            float shiftX = 0f;
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            left += shiftX;
            right += shiftX;

            float shiftY = 0f;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            top += shiftY;
            bottom += shiftY;

            centre = new Vector2 ((left + right) / 2f, (top + bottom) / 2f);
            velocity = new Vector2 (shiftX, shiftY);
        }
    }

}