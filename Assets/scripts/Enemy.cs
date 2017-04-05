using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private LayerMask playerMask;

	private Player target;
    private Breadcrumb targetBreadcrumb;
    private SpriteRenderer sprRen;
    private Color origColor;
    private bool canSeePlayer;
    private bool canSeeBreadcrumb;
    private float sightAngle = 120f;
    private float maxSightDistance = 10f;
    private float minSightDistance = 2f;
    private float sightDistance;
    private float speed = 3f;

    private void Awake ()
    {
        target = GameObject.FindObjectOfType<Player> ();
        sprRen = GetComponent<SpriteRenderer> ();
    }

    private void Start ()
    {
        origColor = sprRen.color;
        sightDistance = maxSightDistance;
    }

    private void Update ()
    {
        SeekPlayer ();

        if (canSeePlayer)
        {
            transform.Translate ((target.transform.position - transform.position).normalized * speed * Time.deltaTime, Space.World);
            float playerAngle = Utilities.AngleBetweenPoints (transform.position, target.transform.position);
            transform.rotation = Quaternion.Euler (new Vector3 (0, 0, playerAngle + 90));
        }
        else if (canSeeBreadcrumb)
        {
            transform.Translate ((targetBreadcrumb.transform.position - transform.position).normalized * speed * Time.deltaTime, Space.World);
            float playerAngle = Utilities.AngleBetweenPoints (transform.position, targetBreadcrumb.transform.position);
            transform.rotation = Quaternion.Euler (new Vector3 (0, 0, playerAngle + 90));
        }

        sprRen.color = (canSeePlayer) ? Color.black : origColor;
    }

    private void SetSightPercentage (float percentage)
    {
        percentage = Mathf.Clamp01 (percentage);
        sightDistance = Mathf.Lerp (minSightDistance, maxSightDistance, percentage);
    }

    private void SeekPlayer ()
    {
        // SetSightPercentage (target.MyInput.magnitude);

        Vector2 vectorToPlayer = target.transform.position - transform.position;
        float playerDist = Mathf.Min (vectorToPlayer.magnitude, sightDistance);

        // The minus 1 is to take account of the fact that we're starting a unit away from the enemy's position
        RaycastHit2D hit = Physics2D.Raycast ((Vector2) transform.position + vectorToPlayer.normalized, vectorToPlayer, playerDist - 1f);
        Debug.DrawRay ((Vector2) transform.position + vectorToPlayer.normalized, target.transform.position - transform.position, Color.red);

        if (hit)
        {
            if (hit.collider.tag == "Player" && Vector2.Angle (transform.up, vectorToPlayer) < sightAngle / 2f)
            {
                canSeePlayer = true;
                return;
            }
        }

        canSeePlayer = false;

        SeekCrumbs ();

    }

    private void SeekCrumbs ()
    {
        foreach (Breadcrumb crumb in AiDirector.activeBreadcrumbs)
        {
            Vector2 vectorToBreadcrumb = crumb.transform.position - transform.position;
            float crumbDist = Mathf.Min (vectorToBreadcrumb.magnitude, sightDistance);

            // The minus 1 is to take account of the fact that we're starting a unit away from the enemy's position
            RaycastHit2D hit = Physics2D.Raycast ((Vector2) transform.position + vectorToBreadcrumb.normalized, vectorToBreadcrumb, crumbDist - 1f);
            Debug.DrawRay ((Vector2) transform.position + vectorToBreadcrumb.normalized, crumb.transform.position - transform.position, Color.red);

            if (hit)
            {
                if (hit.collider.tag == "crumb" && Vector2.Angle (transform.up, vectorToBreadcrumb) < sightAngle)
                {
                    canSeeBreadcrumb = true;
                    if (targetBreadcrumb == null)
                    {
                        targetBreadcrumb = crumb;
                    }
                    else
                    {
                        if (crumb.RemainingLifetime > targetBreadcrumb.RemainingLifetime)
                        {
                            targetBreadcrumb = crumb;
                        }
                    }

                    return;
                }
            }
        }

        canSeeBreadcrumb = false;
    }

}
