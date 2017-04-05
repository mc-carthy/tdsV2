using UnityEngine;

public class PlayerInput : MonoBehaviour {

    [SerializeField]
    private bool isUsingRawInput;

    private PlayerMovement movement;

	private Vector2 input;
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
        movement.Move (input);
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

        input = new Vector2 (h, v);
    }

    private void GetMousePosition ()
    {
		Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		float angle = Utilities.AngleBetweenPoints (transform.position, mouseWorldPos);
        movement.Rotate (angle);
    }

}
