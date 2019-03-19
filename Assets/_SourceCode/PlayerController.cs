using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //playercontroller functionality 
    private GameObject controlledPawn;
    private CharacterController pawnController;
    public Camera pawnCamera;
    private RaycastHit hitObj;

    //input variables 
    public float speed = 12.0f;
    public float turnSpeed = 2f;
    Vector3 moveDirection;
    Vector3 moveRotation;

    //sets the position of the camera behind each pawn once possessed
    private Vector3 CameraPosition = new Vector3(0.8f, 2.5f, -4.2f);


    void Start()
    {
        controlledPawn = GameObject.FindGameObjectsWithTag("PlayerControllable")[0];
        pawnController = controlledPawn.GetComponent<CharacterController>();

        //Sets the camera to be a child of  the controlled pawn
        pawnCamera.transform.SetParent(controlledPawn.transform);

        //force set the cameras position 
        pawnCamera.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
        pawnCamera.transform.localPosition = CameraPosition;
    }

    void Update()
    {
        if (controlledPawn != null)
        {
            Movement();
            RayTrace();
        }
    }

    private void Movement()
    {
        moveDirection = new Vector3(0.0f, 0.0f, Input.GetAxis("Vertical"));
        moveRotation = new Vector3(0.0f, Input.GetAxis("Horizontal"), 0.0f);

        //converts move direction to world space
        moveDirection = controlledPawn.transform.TransformVector(moveDirection);
        moveDirection = moveDirection * speed;

        pawnController.Move(moveDirection * Time.deltaTime);
        pawnController.transform.Rotate((moveRotation * turnSpeed), Space.Self);
    }

    private void RayTrace()
    {
        var position = controlledPawn.transform.position;
        var forward = controlledPawn.transform.forward;

        Debug.DrawRay(position, forward * 10f, Color.red);

        //on LMB 
        if (!Input.GetMouseButtonDown(0)) return;
        //if hit object is tagged as controllable, possess it 
        if (!Physics.Raycast(position, forward, out hitObj, 100, 1)) return;

        if (hitObj.transform.gameObject.CompareTag("PlayerControllable"))
        {
            ChangeControlledPawn(hitObj.transform.gameObject);
        }
    }

    //sets controlled pawn to new pawn, resets camera on new pawn
    private void ChangeControlledPawn(GameObject newPawn)
    {
        var temp = controlledPawn;
        controlledPawn = newPawn;
        pawnController = controlledPawn.GetComponent<CharacterController>();
        pawnCamera.transform.SetParent(controlledPawn.transform);
        pawnCamera.transform.localPosition = CameraPosition;
        pawnCamera.transform.rotation = controlledPawn.transform.rotation * Quaternion.Euler(15.0f, 0f, 0f);
        pawnController.transform.rotation = temp.transform.rotation;
    }
}