using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum HoldState
{
	EMPTY,
	KNIFE,
	STAKE,
	WATER
};
public class CharacterController : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	[SerializeField] private float attackDuration = 1f;
	[SerializeField] private GameObject attackBox;
	[SerializeField] private float attackCheck = 1f;
	[SerializeField] private Sprite[] sprites;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	private bool isSwinging = false;
	private float swingTimer = 0f;
	private HoldState heldItem = HoldState.EMPTY;

	private bool hasKnife = false;
	private bool hasWater = false;
	private bool hasStake = false;

	private SpriteRenderer weapon;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
		weapon = attackBox.GetComponent<SpriteRenderer>();

	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}

		if (swingTimer > 0)
		{
			swingTimer = swingTimer - Time.fixedDeltaTime;
			if (swingTimer <= 0)
			{
				// disable game object
				attackBox.SetActive(false);
				isSwinging = false;
			}
		}
	}

	public void Update()
	{
		if (Input.GetKeyDown("space"))
		{
			if(heldItem != HoldState.EMPTY)
			{
				Attack();
			}
		}
		if (Input.GetKeyDown("1") && hasWater)
		{
			setItem("w");
		}
		if (Input.GetKeyDown("2") && hasKnife)
		{
			setItem("k");
		}
		if (Input.GetKeyDown("3") && hasStake)
		{
			setItem("s");
		}
		if (Input.GetKeyDown("4"))
		{
			setItem("e");
		}
	}

	public void Move(float move, bool crouch, bool jump)
	{
		// If crouching, check to see if the character can stand up
		if (!crouch)
		{
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Reduce the speed by the crouchSpeed multiplier
				move *= m_CrouchSpeed;

				// Disable one of the colliders when crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (move > 0 && !m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            // Otherwise if the input is moving the player left and the player is facing right...
            else if (move < 0 && m_FacingRight)
            {
                // ... flip the player.
                Flip();
            }
            if (move == 0)
                GetComponent<Animator>().SetBool("isWalking", false);
            else
                GetComponent<Animator>().SetBool("isWalking", true);
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void Attack()
	{
		// check if weapon is equipped
		// isAttacking, attackDuration
		// broadcast to NPC's that the player is attacking
		// make it so that player can't kill when in sight of NPC
		// night conditional
		// enable then disable collider
		if (!isSwinging && numCloseNPC(attackCheck) <= 1)
		{
			isSwinging = true;
			swingTimer = attackDuration;
			// enable collider gameobject
			attackBox.SetActive(true);
		}
	}

	public int numCloseNPC(float proxRadius)
	{
		GameObject[] gos;
		gos = GameObject.FindGameObjectsWithTag("NPC");
		int count = 0;

		Vector3 position = transform.position;
		foreach (GameObject go in gos)
		{
			if (go.GetComponentInParent<NPCController>() != null && !go.GetComponentInParent<NPCController>().isLiving())
			{
				continue;
			}
			Vector3 diff = go.transform.position - position;
			float curDistance = diff.sqrMagnitude;
			if (curDistance < proxRadius)
			{
				count++;
			}
		}
		Debug.Log("Near NPC: " + count);
		return count;
	}

	public void setItem(string item)
	{
		if(item == "w")
		{
			heldItem = HoldState.WATER;
			weapon.sprite = sprites[0];
			weapon.GetComponent<Weapon>().weapon = 1;
		}
		if (item == "e")
		{
			heldItem = HoldState.EMPTY;
		}
		if (item == "k")
		{
			heldItem = HoldState.KNIFE;
			weapon.sprite = sprites[1];
			weapon.GetComponent<Weapon>().weapon = 2;
		}
		if (item == "s")
		{
			heldItem = HoldState.STAKE;
			weapon.sprite = sprites[2];
			weapon.GetComponent<Weapon>().weapon = 3;
		}
	}

	public void giveItem(string item)
	{
		if (item == "w")
		{
			hasWater = true;
		}
		if (item == "e")
		{
			
		}
		if (item == "k")
		{
			hasKnife = true;
		}
		if (item == "s")
		{
			hasStake = true;
		}
	}
}