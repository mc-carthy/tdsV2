using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
public class Breadcrumb : MonoBehaviour {

    private float remainingLifetime;
    public float RemainingLifetime {
        get {
            return remainingLifetime;
        }
    }
    
    private SpriteRenderer sprRen;
    private Color startCol;
	private float lifetime = 5f;

    private void Awake ()
    {
        sprRen = GetComponent<SpriteRenderer> ();
        startCol = sprRen.color;
    }

    private void Start ()
    {
        remainingLifetime = lifetime;
    }

    private void Update ()
    {
        remainingLifetime -= Time.deltaTime;

        sprRen.color = new Color (startCol.r, startCol.g, startCol.b, remainingLifetime / lifetime);

        if (remainingLifetime <= 0)
        {
            AiDirector.activeBreadcrumbs.Remove (this);
            Destroy (gameObject);
        }
    }

}
