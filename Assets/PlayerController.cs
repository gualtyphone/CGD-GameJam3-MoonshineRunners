using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MotionState
{
	jumping,
	grounded,
	falling
}

public class PlayerController : MonoBehaviour {

    [SerializeField]
    Vector2 velocity;
    [SerializeField]
    Vector2 acceleration;
    [SerializeField]
	MotionState motionSate;
	[SerializeField]
	MotionState previousMotionState;

	[SerializeField]
	float jumpForce;
	[SerializeField]
	float moveForce;

	[SerializeField]
	float jumpMaxTime;
	float jumpTime;

	bool doubleJump = false;

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
		isGrounded ();
		acceleration = new Vector2 (Input.GetAxisRaw ("Horizontal"), 0) * moveForce;
        acceleration.y = calculateVerticalAcceleration();

        velocity += acceleration * Time.deltaTime;

		velocity -= velocity * rb.drag;

		if (motionSate == MotionState.grounded) {
			velocity.y = 0.0f;
		}

        rb.velocity = velocity;
	}

	bool isGrounded ()
	{
		LayerMask mask = new LayerMask ();
		mask.value = 1 << LayerMask.NameToLayer ("Platforms");
		filter.SetLayerMask (mask);
		if (rb.GetContacts (filter, cps) > 0 && motionSate == MotionState.falling) {
			motionSate = MotionState.grounded;
			previousMotionState = MotionState.falling;
			return true;
		}
		return false;
	}

    float calculateVerticalAcceleration()
    {
		if (motionSate == MotionState.grounded || previousMotionState == MotionState.grounded) {
			if (Input.GetButton("Jump")) {
				previousMotionState = motionSate;
				motionSate = MotionState.jumping;
				//if (Input.GetButtonDown("Jump")){
					if (motionSate == MotionState.jumping && !doubleJump) {
						doubleJump = true;
						velocity.y = jumpForce * 2.0f;
						jumpTime = 0;
					}
				//}
				return jumpForce;
			}
		}
		if (motionSate == MotionState.jumping) {
			if (Input.GetButton ("Jump") && jumpTime < jumpMaxTime) {
				return jumpForce;
			} else {
				previousMotionState = motionSate;
				motionSate = MotionState.falling;
			}
		} 
		if (motionSate == MotionState.falling) {
			return -rb.gravityScale;
		}
			return 0;
    }
}
