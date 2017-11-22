using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    Vector2 velocity;
    [SerializeField]
    Vector2 acceleration;

    [SerializeField]
    bool grounded;

	[SerializeField]
	float jumpForce;

	bool jumping;

	[SerializeField]
	float jumpMaxTime;
	float jumpTime;

    Rigidbody2D rb;
	ContactPoint2D[] cps;
	ContactFilter2D filter;

    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody2D>();
		cps = new ContactPoint2D[20];
		filter = new ContactFilter2D ();
	}
	
	// Update is called once per frame
	void Update () {
		jumpTime += Time.deltaTime;
		grounded = isGrounded ();
		acceleration = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0);
        acceleration.y = calculateVerticalAcceleration();

        velocity += acceleration * Time.deltaTime;

		if (grounded) {
			velocity.y = 0.0f;
		}

        rb.velocity = velocity;
	}

	bool isGrounded ()
	{
		LayerMask mask = new LayerMask ();
		mask.value = 1 << LayerMask.NameToLayer ("Platforms");
		filter.SetLayerMask (mask);
		return (rb.GetContacts (filter, cps) > 0);
	}

    float calculateVerticalAcceleration()
    {
        if (grounded)
        {
			if (Input.GetButtonDown ("Jump")) {
				jumping = true;
				grounded = false;
				jumpTime = 0;
				return jumpForce;
			} else {
				jumping = false;
			}
            return 0;
        }
        else
        {
			if (jumping) {
				if (Input.GetButton ("Jump") && jumpTime < jumpMaxTime) {
					return jumpForce;
				} else {
					jumping = false;
				}
			}
            return -rb.gravityScale;
        }
    }
}
