using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameObject controlledPawn;
    private CharacterController pawnController;
    public Camera pawnCamera;
    private RaycastHit hitObj;


    public float speed = 12.0f;
    public float turnSpeed = 5f;
    public float jumpSpeed = 15.0f;
    public float gravity = 20.0f;
    Vector3 moveDirection;
    Vector3 moveRotation;

    // Start is called before the first frame update
    void Start()
    {
        controlledPawn = GameObject.FindGameObjectsWithTag("PlayerControllable")[0];
        pawnController = controlledPawn.GetComponent<CharacterController>();
        pawnCamera.transform.SetParent(controlledPawn.transform);

    }

    // Update is called once per frame
    void Update()
    {
        if (controlledPawn != null)
        {
            Movement();
            RayTrace();
        }
    }

    private void RayTrace()
    {

        if (Input.GetMouseButton(0))
        {
            if (Physics.Raycast(controlledPawn.transform.position, controlledPawn.transform.forward, out hitObj, 50, 1))
            {
                Debug.DrawRay(controlledPawn.transform.position, controlledPawn.transform.forward * 50f, Color.red);
                print(hitObj.collider.gameObject.name);
            }
        }
    }

    private void Movement()
    {
        moveDirection = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));
        moveDirection = controlledPawn.transform.TransformVector(moveDirection);
        moveDirection = moveDirection * speed;

        moveRotation = new Vector3(0.0f, Input.GetAxis("Horizontal"), 0.0f);

        if (Input.GetButton("Jump"))
        {
            moveDirection.y = jumpSpeed;
        }

        // Apply gravity
        moveDirection.y = moveDirection.y - ((gravity*6) * Time.deltaTime);

        // Move the controller
        pawnController.Move(moveDirection * Time.deltaTime);
        pawnController.transform.Rotate((moveRotation * turnSpeed), Space.Self);
    }
}
