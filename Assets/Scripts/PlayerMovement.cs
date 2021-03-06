﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	public float speed = 6f;            	// The speed that the player will move at.

	Vector3 movement;                   	// The vector to store the direction of the player's movement.
	Animator anim;                      	// Reference to the animator component.
	Rigidbody playerRigidbody;          	// Reference to the player's rigidbody.
	int floorMask;                     		// A layer mask so that a ray can be cast just at gameobjects on the floor layer.
	private int BASE_SPEED;
	private Vector3 initialAttitude;

	GameObject Character; 
	float timer;                			// A timer to determine when to fire.
	public float timeBetweenAttack = 0.5f; 	// The time between each shot.

	public GameObject Magic; 

	private Vector3 magicDirection;


	void Awake ()
	{
		// Create a layer mask for the floor layer.
		floorMask = LayerMask.GetMask ("Floor");

		// Set up references.
		anim = GetComponent <Animator> ();
		playerRigidbody = GetComponent <Rigidbody> ();
		transform.Find ("Pet").gameObject.SetActive(false);

		Character = transform.Find ("Character").gameObject;

		//Debug.Log (Magic + " found. ");

		setInitialAttitude ();

	}


	void FixedUpdate ()
	{

		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;

		// Store the input axes.
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");


		////////////////////////////////////////////////////////////////////////////////////////////////////////////////
		//float h = (Input.acceleration.x - initialAttitude.x)*BASE_SPEED;
		//float v = (Input.acceleration.y - initialAttitude.y)*BASE_SPEED;
		/**

		if (Mathf.Abs (h) < 1.5)
			h = 0.0f;
		if (Mathf.Abs (v) < 1.5)
			v = 0.0f;

		//Debug.Log ("X = " + h + ", Y = " + v);

		**/
		/////////////////////////////////////////////////////////////////////////////////////////////////////////////////


		// Move the player around the scene.
		Move (h, v);

		// Animate the player.
		MoveAnimating (h, v);
		if ( !anim.GetBool ("IsWalking")) {
			transform.Find ("Pet").gameObject.SetActive(false); //no pet when player stand still

			HopOffPet ();	// Character stand on the ground

		}


		//Check Attack
		if(Input.GetButton ("Fire1") && timer >= timeBetweenAttack)
		{

			// Reset the timer.
			timer = 0f;
			anim.SetTrigger ("Attack");

			//Attack ();
			//HopOnPet ();
			//transform.Find ("Pet").gameObject.SetActive(true);
			//anim.SetBool ("IsWalking", true);
			//Move (1f,1f);
			//transform.position += transform.forward * Time.deltaTime * speed;
		}
	}

	void Move (float h, float v)
	{

		// Set the movement vector based on the axis input.
		movement.Set (h, 0f, v); // also move up character a liitle bit to match the animation

		// Normalise the movement vector and make it proportional to the speed per second.
		movement = movement.normalized * speed * Time.deltaTime;
		if (h != 0 || v != 0) {
			Turning (movement);

			HopOnPet();		//Set Character position to match the animation

		}


		playerRigidbody.MovePosition (transform.position + movement);
	}

	void Turning (Vector3 direction)
	{
		transform.rotation = Quaternion.LookRotation (direction);
	}

	void MoveAnimating (float h, float v)
	{
		// Create a boolean that is true if either of the input axes is non-zero.
		bool walking = h != 0f || v != 0f;

		// Tell the animator whether or not the player is walking.
		anim.SetBool ("IsWalking", walking);

		// Pet appear when player start to walk
		transform.Find ("Pet").gameObject.SetActive(true);

	}

	void setInitialAttitude() {
		initialAttitude = Input.acceleration;
	}
		
	void HopOnPet() {
		Character.transform.position = new Vector3(Character.transform.position.x, 0.5f, Character.transform.position.z); 
	}

	void HopOffPet() {
		Character.transform.position = new Vector3(Character.transform.position.x, 0f, Character.transform.position.z); 
	}

	void Attack() {
		magicDirection = new Vector3 (transform.position.x, -1f, transform.position.z);
		Instantiate(Magic, magicDirection+(transform.forward*3), transform.rotation);		//Create Magic object
	}

}