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

    private PlayerCollision collision;
    private PlayerInput input;

    private void Awake ()
    {
        myCollider = GetComponent<CircleCollider2D> ();
        input = GetComponent<PlayerInput> ();
    }

}
