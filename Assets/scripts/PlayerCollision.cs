using UnityEngine;

[RequireComponent (typeof (CircleCollider2D))]
public class PlayerCollision : MonoBehaviour {

    private const float skinWidth = 0.015f;

    [SerializeField]
    private LayerMask collisionMask;

    private int circularRaycasts = 20;

	private CircleCollider2D myCollider;

    private void Awake ()
    {
        myCollider = GetComponent<CircleCollider2D> ();
    }

    public void Move (Vector3 velocity)
    {
        CircularCollisions (ref velocity);

        transform.Translate (velocity, Space.World);
    }

    private void CircularCollisions (ref Vector3 velocity)
    {
        float rayLength = velocity.magnitude + skinWidth;

        for (int i = 0; i < circularRaycasts; i++)
        {
            float theta = ((float) i / circularRaycasts) * 2 * Mathf.PI;
            Vector2 rayVector = new Vector2 (Mathf.Sin (theta), Mathf.Cos (theta)).normalized;
            Vector2 skinDepth = new Vector2 (Mathf.Sin (theta), Mathf.Cos (theta)).normalized * skinWidth;
            Vector2 edgePoint = (Vector2) transform.position + (rayVector * myCollider.radius) - skinDepth;
            RaycastHit2D hit = Physics2D.Raycast (edgePoint, rayVector, rayLength, collisionMask);

            if (hit)
            {
                Debug.Log (hit.distance);
                if (hit.distance <= skinWidth + 0.01f)
                {
                    velocity = -rayVector * skinWidth;
                }
                velocity = rayVector * hit.distance - skinDepth;
                rayLength = hit.distance;
            }

            Debug.DrawRay (edgePoint, rayVector * rayLength, Color.red);

        }
    }
}