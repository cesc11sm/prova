using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	bool grounded, interact; //bool for checking if player is grounded so they can jump, and bool for interact, so that player can only interact when in range of thing to interact with
	public Transform jumpCheck, interactCheck; //transform variable for the end points of the linecasts

	RaycastHit2D interacted; //a variable type that stores a collider that was hit during linecast
	Animator anim;

	void Start()
	{
		anim = GetComponent<Animator> ();

	}
	void Update()
	{
		Movement(); //call the function every frame
		RaycastStuff(); //call the function every frame
	}

	void RaycastStuff()
	{
		//Just a debug visual representation of the Linecast, can only see this in scene view! Doesn't actually do anything!
		Debug.DrawLine(transform.position, jumpCheck.position, Color.magenta);
		Debug.DrawLine(transform.position, interactCheck.position, Color.magenta);

		Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
		Vector2 groundPos = new Vector2(jumpCheck.position.x, jumpCheck.position.y);
		
		grounded = Physics2D.Linecast(playerPos, groundPos, 1 << LayerMask.NameToLayer("Ground"));
		//Debug.Log (playerPos + " " + groundPos + " " + grounded + LayerMask.NameToLayer("Treno"));
		//we assign the bool 'ground' with a linecast, that returns true or false when the end of line 'jumpCheck' touches the ground
		//grounded = Physics2D.Linecast(transform.position, jumpCheck.position, 1 << LayerMask.NameToLayer("Ground"));  

		//Using linecast which takes (start point, end point, layermask) so we can make it only detect objects with specified layers
		//its wrapped in an if statement, so that while the tip of the Linecast (interactCheck.position) is touching an object with layer 'Guard', the code inside executes
		if(Physics2D.Linecast(transform.position, interactCheck.position, 1 << LayerMask.NameToLayer("Guard")))
		{
			//we store the collider object the Linecast hit so that we can do something with that specific object, ie. the guard
			//each time the linecast touches a new object with layer "guard", it updates 'interacted' with that specific object instance
			interacted = Physics2D.Linecast(transform.position, interactCheck.position, 1 << LayerMask.NameToLayer("Guard")); 
			interact = true; //since the linecase is touching the guard and we are in range, we can now interact!
		}
		else
		{
			interact = false; //if the linecast is not touching a guard, we cannot interact
		}

		//Physics2D.IgnoreLayerCollision(8, 10); //if we want certain layers to ignore each others collision, we use this! the number is the layer number in the layers list
	}


	void Movement() //function that stores all the movement
	{
		anim.SetFloat ("velocidad", Mathf.Abs (Input.GetAxis ("Horizontal")));
		if(Input.GetKey (KeyCode.D))
		{
			transform.Translate(Vector3.right * 4f * Time.deltaTime); 
			anim.SetBool("derecha",true);
			//transform.eulerAngles = new Vector2(0, 0); //this sets the rotation of the gameobject
		}

		if(Input.GetKey (KeyCode.A))
		{
			transform.Translate(Vector3.left * 4f * Time.deltaTime);
			anim.SetBool("derecha",false);
			//transform.eulerAngles = new Vector2(0, 180); //this sets the rotation of the gameobject

		}
		//Debug.Log (grounded);
		if(Input.GetKeyDown (KeyCode.Space) && grounded) // If the jump button is pressed and the player is grounded then the player jumps 
		{
			rigidbody2D.AddForce(Vector2.up * 300f);

			//Pruebas para cambiar sprite en tiempo real
			//Sprite[] spr = Resources.LoadAll<Sprite>("linkEdi");
			//Debug.Log(gameObject.GetComponent<SpriteRenderer>().sprite);// = Resources.Load<Sprite>("linkEdit_7");
			//gameObject.GetComponent<SpriteRenderer>().sprite = spr[0];

		}

		if(Input.GetKeyDown (KeyCode.E))// && interact) //if you press E and interact is set to true
		{

			//interacted.collider.gameObject.animation.Play (); //access the gameobject of the collider stored in 'interacted' back in the linecast code, and tell its animation component to play the default animation
		}
	}
}
