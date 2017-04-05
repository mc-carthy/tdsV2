using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private PlayerCollision collision;

    private float speed = 5f;

    private void Awake ()
    {
        collision = GetComponent<PlayerCollision> ();
    }

    public void Move (Vector2 input)
    {
        collision.Move (input * speed * Time.deltaTime);
    }

    public void Rotate (float angle)
    {
		transform.rotation = Quaternion.Euler (new Vector3 (0, 0, angle + 90));
    }

}
