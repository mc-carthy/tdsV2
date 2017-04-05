using UnityEngine;

public class PlayerInput : MonoBehaviour {

	private Vector2 myInput;
    public Vector2 MyInput {
        get {
            return myInput;
        }
    }

    [SerializeField]
    private bool isUsingRawInput;

    private PlayerMovement movement;

    private Vector2 mousePos;
    private float minCursorDistance;

    private void Awake ()
    {
        movement = GetComponent<PlayerMovement> ();
    }

    private void Update ()
    {
        GetTranslationInput ();
        GetMousePosition ();
    }

    private void FixedUpdate ()
    {
        movement.Move (myInput);
    }

    private void GetTranslationInput ()
    {
        float h, v;
        if (isUsingRawInput)
        {
            h = Input.GetAxisRaw ("Horizontal");
            v = Input.GetAxisRaw ("Vertical");
        }
        else
        {
            h = Input.GetAxis ("Horizontal");
            v = Input.GetAxis ("Vertical");
        }

        myInput = new Vector2 (h, v).normalized;
    }

    private void GetMousePosition ()
    {
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float angle = Utilities.AngleBetweenPoints (transform.position, mouseWorldPos);
        movement.Rotate (angle);
    }

}
