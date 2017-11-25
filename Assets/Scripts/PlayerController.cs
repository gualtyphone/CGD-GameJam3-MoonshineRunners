using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MotionState
{
	jumping,
	grounded,
	falling,
	wallTouching,
	wallSliding
}

enum ContactSides
{
	wallLeft,
	wallRight,
	ceiling,
	ground
}
	
class Inputs
{
	public Vector2 axes;
	public bool jump;
	public bool jumpDown;
}

public class PlayerController : MonoBehaviour {
    [SerializeField]
    public string jumpSoundName;
    [SerializeField]
    public string runSoundName;

	[SerializeField]
	public int playerNumber = 0;

    public bool alive = true;
    public bool isRunning = false;

	[SerializeField]
	[Range(0.0f, 45.0f)]
	public float drunknessLevel;

	List<Inputs> inputs;

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

	[SerializeField]
	bool doubleJump = false;

	[SerializeField]
	List<ContactSides> contacts;

    Rigidbody2D rb;
	ContactPoint2D[] cps;
	ContactFilter2D filter;

    private AudioManager audioManager;

    // Use this for initialization
    void Awake () {
        rb = GetComponent<Rigidbody2D>();
		cps = new ContactPoint2D[20];
		filter = new ContactFilter2D ();
		contacts = new List<ContactSides> ();
		inputs = new List<Inputs> ();

        audioManager = FindObjectOfType<AudioManager>();

		//testing drunkness
		//drunknessLevel = Random.Range (0.0f, 45.0f);

	}

	Inputs getDelayedinput(int framesDelay = 0)
	{
		return (inputs [Mathf.Max(0, (inputs.Count - 1 - framesDelay))]);
	}

	void recordInput()
	{
		Inputs input = new Inputs ();
		input.axes = new Vector2 (Input.GetAxisRaw ("Horizontal" + playerNumber), Input.GetAxisRaw ("Vertical" + playerNumber));	
		input.jump = Input.GetButton ("Jump" + playerNumber);
		input.jumpDown = Input.GetButtonDown ("Jump" + playerNumber);

		inputs.Add(input);
		if (inputs.Count > 200) {
			inputs.RemoveAt (0);
		}
	}

	// Update is called once per frame
	void Update () {
        if (!alive)
            return;
		recordInput ();

		jumpTime += Time.deltaTime;
		checkContacts ();
		isGrounded ();
		isTouchingWall ();


		acceleration = new Vector2 (getDelayedinput((int)(drunknessLevel)).axes.x, 0) * moveForce;
		acceleration.y = calculateVerticalAcceleration ();

		velocity += acceleration * Time.deltaTime;

		velocity -= velocity * rb.drag;

		if (motionSate == MotionState.grounded) {
			velocity.y = 0.0f;
		}

		rb.velocity = velocity;

        isRunning = false;

	}

	void checkContacts()
	{
		contacts = new List<ContactSides> ();
		cps = new ContactPoint2D[20];
		LayerMask mask = new LayerMask ();
		mask.value = 1 << LayerMask.NameToLayer ("Platforms");
		filter.SetLayerMask (mask);

		if (rb.GetContacts (filter, cps) > 0) {
			foreach (var cp in cps) {
				Debug.DrawRay (cp.point, cp.normal);
				if (cp.normal.y > 0.5) {
					if (!contacts.Contains(ContactSides.ground))
					contacts.Add (ContactSides.ground);
				}
				if (cp.normal.y < -0.5) {
					if (!contacts.Contains(ContactSides.ceiling))
						contacts.Add (ContactSides.ceiling);
				}
				if (cp.normal.x > 0.5) {
					if (!contacts.Contains(ContactSides.wallLeft))
						contacts.Add (ContactSides.wallLeft);
				}
				if (cp.normal.x < -0.5) {
					if (!contacts.Contains(ContactSides.wallRight))
						contacts.Add (ContactSides.wallRight);
				}
			}
		}
	}
	void isGrounded ()
	{
		LayerMask mask = new LayerMask ();
		mask.value = 1 << LayerMask.NameToLayer ("Platforms");
		filter.SetLayerMask (mask);
		if (contacts.Contains(ContactSides.ground) && motionSate == MotionState.falling) {
			motionSate = MotionState.grounded;
			previousMotionState = MotionState.falling;
			doubleJump = false;
		}

		if (!contacts.Contains (ContactSides.ground) && motionSate == MotionState.grounded) {
			previousMotionState = motionSate;
			motionSate = MotionState.falling;
		}
	}

	void  isTouchingWall ()
	{
		LayerMask mask = new LayerMask ();
		mask.value = 1 << LayerMask.NameToLayer ("Platforms");
		filter.SetLayerMask (mask);
		if ((contacts.Contains(ContactSides.wallLeft) || contacts.Contains(ContactSides.wallRight)) && motionSate == MotionState.falling) {
			previousMotionState = motionSate;
			motionSate = MotionState.wallTouching;
			doubleJump = false;
		}
	}

    float calculateVerticalAcceleration()
	{
		if (motionSate == MotionState.grounded) {
            if(rb.velocity.magnitude > 0.1)
            {
                //if (isRunning == false)
                //{
                //    audioManager.PlaySound(runSoundName);
                //    isRunning = true;
                //}
            }
			if (getDelayedinput((int)(drunknessLevel)).jumpDown) {
				Jump ();
			}
		} else if (motionSate == MotionState.jumping || motionSate == MotionState.falling) {
			if (getDelayedinput((int)(drunknessLevel)).jumpDown && !doubleJump) {
				doubleJump = true;
				Jump ();
			}
			if (contacts.Contains (ContactSides.ceiling)) {
				motionSate = MotionState.falling;
				velocity.y = 0.0f;
			}
			if (motionSate == MotionState.jumping) {
				if (getDelayedinput((int)(drunknessLevel)).jump && jumpTime < jumpMaxTime) {
					return jumpForce;
				} else {
					previousMotionState = motionSate;
					motionSate = MotionState.falling;
				}
			}
			if (motionSate == MotionState.falling) {
				return -rb.gravityScale;
			}
		} else if (motionSate == MotionState.wallSliding || motionSate == MotionState.wallTouching) {
			if (getDelayedinput((int)(drunknessLevel)).jumpDown) {
				WallJump ();
			}
			if (!contacts.Contains (ContactSides.wallLeft) && !contacts.Contains (ContactSides.wallRight)) {
				previousMotionState = motionSate;
				motionSate = MotionState.falling;
			}
		}
		return 0;
	}

	void Jump()
	{
		previousMotionState = motionSate;
		motionSate = MotionState.jumping;
		velocity.y = jumpForce * 2.0f;
		jumpTime = 0;

        audioManager.PlaySound(jumpSoundName);

	}

	void WallJump()
	{
		previousMotionState = motionSate;
		motionSate = MotionState.jumping;
		velocity.y = jumpForce * 2.0f;
		if (contacts.Contains(ContactSides.wallLeft))
			velocity.x = jumpForce;
		if (contacts.Contains(ContactSides.wallRight))
			velocity.x = -jumpForce;

		velocity.Normalize ();
		velocity *= jumpForce *2;
		jumpTime = 0;
	}
}
