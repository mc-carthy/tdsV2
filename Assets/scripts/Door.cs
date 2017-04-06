using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (BoxCollider2D))]
public class Door : MonoBehaviour {

    private GameObject player;
    private SpriteRenderer sprRen;
    private BoxCollider2D col;
    private Color origColour;
    private Color alternateColor;
	private float playerDistToOpen = 2f;
    private bool isOpen = false;

    private void Awake ()
    {
        player = GameObject.FindGameObjectWithTag ("Player");
        sprRen = GetComponent<SpriteRenderer> ();
        col = GetComponent<BoxCollider2D> ();
    }

    private void Start ()
    {
        origColour = sprRen.color;
        gameObject.layer = 10; // TODO - Get rid of hardcoded values
    }

    private void OnCollisionEnter2D (Collision2D other)
    {
        if (other.gameObject.tag == "enemy")
        {
            Open ();
        }
    }

    private void OnMouseDown ()
    {
        if (Vector2.Distance (transform.position, player.transform.position) < playerDistToOpen)
        {
            Toggle ();
        }
    }

    private void Toggle ()
    {
        isOpen = !isOpen;
        sprRen.color = isOpen ? alternateColor : origColour;
        col.isTrigger = isOpen;
    }

    private void Open ()
    {
        isOpen = true;
        sprRen.color = alternateColor;
        col.isTrigger = true;
    }

}
