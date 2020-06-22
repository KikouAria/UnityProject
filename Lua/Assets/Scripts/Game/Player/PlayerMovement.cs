using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;            // The speed that the player will move at.

    private Vector3 movement;                   // The vector to store the direction of the player's movement.
    private Animator anim;                      // Reference to the animator component.
    private Rigidbody playerRigidbody;          // Reference to the player's rigidbody.
    private int floorMask;                      // A layer mask so that a ray can be cast just at gameobjects on the floor layer.
    private float camRayLength = 100f;          // The length of the ray from the camera into the scene.

    private Vector3 targetPos;
    private bool IsMoveTarget = false;

    void Awake()
    {
        // Create a layer mask for the floor layer.
        floorMask = LayerMask.GetMask("Floor");

        // Set up references.
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Store the input axes.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // Move the player around the scene.
        Move(h, v);

        // Turn the player to face the mouse cursor.
        Turning();

        // Animate the player.
        Animating(h, v);
    }

    private void Update()
    {
        if(IsMoveTarget)
        {
            MoveTarget(this.targetPos);
        }
    }


    void Move(float h, float v)
    {
        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * Time.deltaTime;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);
    }


    void Turning()
    {
        // Create a ray from the mouse cursor on screen in the direction of the camera.
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Create a RaycastHit variable to store information about what was hit by the ray.
        RaycastHit floorHit;

        // Perform the raycast and if it hits something on the floor layer...
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            // Create a vector from the player to the point on the floor the raycast from the mouse hit.
            Vector3 playerToMouse = floorHit.point - transform.position;

            // Ensure the vector is entirely along the floor plane.
            playerToMouse.y = 0f;

            // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
            Quaternion newRotatation = Quaternion.LookRotation(playerToMouse);

            // Set the player's rotation to this new rotation.
            playerRigidbody.MoveRotation(newRotatation);
        }
    }

    void Animating(float h, float v)
    {
        // Create a boolean that is true if either of the input axes is non-zero.
        bool walking = h != 0f || v != 0f;

        // Tell the animator whether or not the player is walking.
        anim.SetBool("IsWalking", walking);
    }

    public void MoveTarget(Vector3 pos)
    {
        this.targetPos = pos;
        IsMoveTarget = true;

        // Store the input axes.
        float h = pos.x > gameObject.transform.position.x ? 1.0f : -1.0f; 
        float v = pos.z > gameObject.transform.position.z ? 1.0f : -1.0f;

        // Set the movement vector based on the axis input.
        movement.Set(h, 0f, v);

        // Normalise the movement vector and make it proportional to the speed per second.
        movement = movement.normalized * speed * 0.1f;

        // Move the player to it's current position plus the movement.
        playerRigidbody.MovePosition(transform.position + movement);

        // Animate the player.
        anim.SetBool("IsWalking", true);

        // Create a vector from the player to the point on the floor the raycast from the mouse hit.
        Vector3 playerTo = pos - gameObject.transform.position;

        // Ensure the vector is entirely along the floor plane.
        playerTo.y = 0f;

        // Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
        Quaternion newRotatation = Quaternion.LookRotation(playerTo);

        // Set the player's rotation to this new rotation.
        playerRigidbody.MoveRotation(newRotatation);
    }
}