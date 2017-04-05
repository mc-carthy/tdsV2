using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField]
    private LayerMask playerMask;

	private Player target;
    private SpriteRenderer sprRen;
    private Color origColor;
    private bool canSeePlayer;
    private float sightAngle = 120f;

    private void Awake ()
    {
        target = GameObject.FindObjectOfType<Player> ();
        sprRen = GetComponent<SpriteRenderer> ();
        origColor = sprRen.color;
    }

    private void Update ()
    {
        SeekPlayer ();

        sprRen.color = (canSeePlayer) ? Color.black : origColor;
    }

    private void SeekPlayer ()
    {
        Vector2 vectorToPlayer = target.transform.position - transform.position;
        float playerDist = vectorToPlayer.magnitude;

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

    }

}
