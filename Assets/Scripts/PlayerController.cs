using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public string jumpSoundEffect;
    [SerializeField]
    public string runSoundEffect;
    [SerializeField]
    public string deathSoundEffect;

	[SerializeField]
	public int playerNumber = 0;

    public bool alive = true;

	[SerializeField]
	[Range(0.0f, 45.0f)]
	public float drunknessLevel;

	private Canvas alcCanvas;
	public GameObject alcPrefab;

	public float alcPanelOffset;
	private GameObject alcPanel;
	private Slider alcSlider;
	private Text alcText;

    public int beerCount;
    public int cocktailCount;
    public int score;
    public float alcoholLevel;
	private float timer = 0.0f;

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

    private Animator anim;

    private AudioManager audioManager;

    [SerializeField]
    float wallJumpStrenght;

    // Use this for initialization
    void Awake () {
		anim = GetComponent<Animator> ();
        rb = GetComponent<Rigidbody2D>();
		cps = new ContactPoint2D[20];
		filter = new ContactFilter2D ();
		contacts = new List<ContactSides> ();
		inputs = new List<Inputs> ();
        anim = GetComponent<Animator>();

        audioManager = FindObjectOfType<AudioManager>();

		//testing drunkness
		//drunknessLevel = Random.Range (0.0f, 45.0f);
	}

	void Start()
	{
		alcPrefab = Instantiate (alcPrefab) as GameObject;
		alcCanvas = alcPrefab.GetComponentInChildren<Canvas> ();
		alcText = alcCanvas.GetComponentInChildren<Text> ();
		alcCanvas.transform.parent = gameObject.transform;
		alcSlider = alcCanvas.GetComponentInChildren<Slider> ();

		alcText.gameObject.SetActive (false);
		drunknessLevel = 0.0f;
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
		checkDrunkenessLevel();

		acceleration = new Vector2 (getDelayedinput((int)(drunknessLevel)).axes.x, 0) * moveForce;
		acceleration.y = calculateVerticalAcceleration ();

		velocity += acceleration * Time.deltaTime;

		velocity -= velocity * rb.drag;

		if (motionSate == MotionState.grounded) {
			velocity.y = 0.0f;
		}

        if (rb.velocity.x > 3 && motionSate == MotionState.grounded)
        {
            anim.SetBool("walkingState", true);

        }

        else if (rb.velocity.magnitude < 1 || motionSate == MotionState.jumping)
        {
                 anim.SetBool("walkingState", false);
        }

        if (motionSate == MotionState.jumping)
        {
            anim.SetBool("jumpingState", true);  
        }
        else
            anim.SetBool("jumpingState", false);

        rb.velocity = velocity;

        alcSlider.transform.position = new Vector3 (gameObject.transform.position.x, gameObject.transform.position.y + 1.0f, gameObject.transform.position.z);

		alcSlider.value = (drunknessLevel / 45.0f) * 100;

		changeAnimation ();

        if (transform.position.y < -0.5f)
        {
            alive = false;
        }

	}

	void changeAnimation()
	{
		//switch (motionSate) {
		//case MotionState.falling:
		//	anim.SetTrigger ("Falling");
		//	break;
		//case MotionState.grounded:
		//	if (rb.velocity.magnitude > 0.3f) {
		//		anim.SetTrigger ("Running");

		//	} else {

		//		anim.SetTrigger ("Idle");
		//	}
		//	break;
		//case MotionState.jumping:
		//	anim.SetTrigger ("Jumping");

		//	break;
		//case MotionState.wallSliding:
		//case MotionState.wallTouching:
		//	if (contacts.Contains (ContactSides.wallLeft)) {
		//		anim.SetTrigger ("WallLeft");

		//	} else {

		//		anim.SetTrigger ("WallRight");
		//	}

		//	break;
		//}
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
            anim.SetBool("fallState", false);
            previousMotionState = MotionState.falling;
            doubleJump = false;
            motionSate = MotionState.grounded;
        }

		if (!contacts.Contains (ContactSides.ground) && (motionSate == MotionState.grounded) || previousMotionState == MotionState.jumping) {
			previousMotionState = motionSate;
			motionSate = MotionState.falling;
            anim.SetBool("fallState", true);
		}
        if(motionSate == MotionState.grounded && rb.velocity.magnitude > 1)
        {
            audioManager.PlaySound(runSoundEffect);
        }
	}

    void isTouchingWall()
    {
        LayerMask mask = new LayerMask();
        mask.value = 1 << LayerMask.NameToLayer("Platforms");
        filter.SetLayerMask(mask);
        if ((contacts.Contains(ContactSides.wallLeft) || contacts.Contains(ContactSides.wallRight)) && motionSate == MotionState.falling) {
            previousMotionState = motionSate;
            motionSate = MotionState.wallTouching;
            changeAnimation();

            doubleJump = false;
        }


        if (contacts.Contains(ContactSides.wallLeft))
        {
            anim.SetBool("leftWall", true);
        }
        else if (!contacts.Contains(ContactSides.wallLeft))
        { 
        anim.SetBool("leftWall", false);
        }

        if (contacts.Contains(ContactSides.wallRight))
        {
            anim.SetBool("rightWall", true);
        }
        else if (!contacts.Contains(ContactSides.wallLeft))
        {
            anim.SetBool("rightWall", false);
        }

    }

    float calculateVerticalAcceleration()
	{
		if (motionSate == MotionState.grounded) {
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
				changeAnimation ();

				velocity.y = 0.0f;
			}
			if (motionSate == MotionState.jumping) {
				if (getDelayedinput((int)(drunknessLevel)).jump && jumpTime < jumpMaxTime) {
					return jumpForce;
				} else {
					previousMotionState = motionSate;
					motionSate = MotionState.falling;
					changeAnimation ();

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
				changeAnimation ();
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
		changeAnimation ();

        audioManager.PlaySound(jumpSoundEffect);

	}

	void WallJump()
	{
        
        previousMotionState = motionSate;
		motionSate = MotionState.jumping;
		velocity.y = jumpForce * 2.0f;
		if (contacts.Contains(ContactSides.wallLeft))
			velocity.x = jumpForce * wallJumpStrenght;
		if (contacts.Contains(ContactSides.wallRight))
			velocity.x = -jumpForce * wallJumpStrenght;
		changeAnimation ();

		//velocity.Normalize ();
		//velocity *= jumpForce *2;
		jumpTime = 0;
	}

	void checkDrunkenessLevel()
	{
		//timer += Time.deltaTime;

		if (drunknessLevel > 45.0f) 
		{
			drunknessLevel = 45.0f;
		}

		if (drunknessLevel < 0.0f) 
		{
			drunknessLevel = 0.0f;
		}

		if (drunknessLevel >= 45.0f)
		{
			alcText.gameObject.SetActive (true);
			StartCoroutine ("FlashText");
		}
		else 
		{
			StopCoroutine ("FlashText");
			alcText.gameObject.SetActive (false);
		}
	}

	IEnumerator FlashText()
	{
		if (alcText.color == Color.red) 
		{
			alcText.color = Color.yellow;
			yield return new WaitForSeconds(2.5f);
		}
		else if (alcText.color == Color.yellow)
		{
			alcText.color = Color.red;
			yield return new WaitForSeconds(2.5f);
		}
	}
}
