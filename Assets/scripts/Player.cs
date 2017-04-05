using UnityEngine;

public class Player : MonoBehaviour {

    private CircleCollider2D myCollider;
	public CircleCollider2D MyCollider {
        get {
            return myCollider;
        }
    }

    public Vector2 MyInput {
        get {
            return input.MyInput;
        }
    }

    [SerializeField]
    private Breadcrumb breadcrumbPrefab;

    private PlayerCollision collision;
    private PlayerInput input;

    private Vector2 lastBreadcrumbPosition;
    private float breadcrumbDistance = 2f;

    private void Awake ()
    {
        myCollider = GetComponent<CircleCollider2D> ();
        input = GetComponent<PlayerInput> ();
    }

    private void Start ()
    {
        lastBreadcrumbPosition = transform.position;
    }

    private void Update ()
    {
        if (Vector2.Distance (transform.position, lastBreadcrumbPosition) > breadcrumbDistance)
        {
            DropBreadcrumb ();
            lastBreadcrumbPosition = transform.position;
        }
    }

    private void DropBreadcrumb ()
    {
        Breadcrumb newBreadcrumb = Instantiate (breadcrumbPrefab, transform.position, Quaternion.identity) as Breadcrumb;
        AiDirector.activeBreadcrumbs.Add (newBreadcrumb);
    }

}
