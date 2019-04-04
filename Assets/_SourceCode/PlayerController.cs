using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Form { Test, Human, Bear };
    private Form currentForm = Form.Test;

    public static PlayerController pc;

    // PlayerController/movement functionality 
    private GameObject controlledPawn;
    private CharacterController pawnController;
    public Camera pawnCamera;
    private RaycastHit hitObj;
    public LayerMask mask;
    private bool jumpFlag = false;

    // Input variables 
    private const float baseSpeed = 12.0f;
    public float speed = baseSpeed;
    public float turnSpeed = 2f;
    public float jumpStrength = 5.2f;
    Vector3 moveDirection;
    Vector3 moveRotation;

    // Animation variables
    private Animator anim;

    /**
     * Speed modifiers for now, this will be
     * generalized later on
     */
    private Dictionary<string, SpeedEffect> _effects = new Dictionary<string, SpeedEffect>();

    //sets the position of the camera behind each pawn once possessed
    private Vector3 CameraPosition = new Vector3(0.8f, 2.5f, -4.2f);

    private void Awake() {
        if(pc == null) {
            pc = this;
            DontDestroyOnLoad(gameObject);
        } else
            Destroy(gameObject);
    }

    private void Start() {
        controlledPawn = GameObject.FindGameObjectsWithTag("PlayerControllable")[0];
        pawnController = controlledPawn.GetComponent<CharacterController>();

        // Sets the camera to be a child of  the controlled pawn
        pawnCamera.transform.SetParent(controlledPawn.transform);

        // Force set the cameras position 
        pawnCamera.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
        pawnCamera.transform.localPosition = CameraPosition;

        // Find additional components
        anim = controlledPawn.GetComponent<Animator>();
    }

    private void Update() {
        if(Input.GetButtonDown("Jump") && IsGrounded())
            jumpFlag = true;
        if(Input.GetButtonDown("AttackScissor") || Input.GetButtonDown("AttackStandard"))
            anim.SetTrigger("Attack");
    }

    private void FixedUpdate() {
        if (controlledPawn == null)
            return;
        // Apply movement
        Movement();

        //RayTrace();
    }

    private void Movement() {
        // Retrieve inputs
        moveDirection.x = Input.GetAxis("Horizontal") * speed;
        moveDirection.z = Input.GetAxis("Vertical") * speed;
        moveRotation = new Vector3(0.0f, Input.GetAxis("CameraX"), 0.0f);
        moveDirection = controlledPawn.transform.TransformVector(moveDirection);
        
        // Apply gravity and jump
        if(jumpFlag) {
            moveDirection.y = jumpStrength;
            jumpFlag = false;
        } else if(!IsGrounded()) {
            moveDirection.y += -1f;
        } else {
            moveDirection.y = 0;
        }
        moveDirection.y = Mathf.Clamp(moveDirection.y, -20f, 10f);

        // Apply movement
        pawnController.Move(moveDirection * Time.fixedDeltaTime);
        pawnController.transform.Rotate((moveRotation * turnSpeed), Space.Self);
    }

    /*private void RBMovement()
    {
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        moveRotation = new Vector3(0.0f, Input.GetAxis("CameraX"), 0.0f);

        //converts move direction to world space
        moveDirection = controlledPawn.transform.TransformVector(moveDirection);
        moveDirection = moveDirection * speed;

        pawnRigidbody.MovePosition(controlledPawn.transform.position + moveDirection * Time.deltaTime);
        pawnRigidbody.MoveRotation(Quaternion.Euler(moveRotation * turnSpeed) * controlledPawn.transform.rotation);

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            pawnRigidbody.AddRelativeForce(controlledPawn.transform.up * jumpStrength, ForceMode.Impulse);
        }

        Utils.WithKeyHold(KeyCode.LeftShift, OnSneakBegin, OnSneakEnd);
        ApplyEffects();
    }*/

    /// <summary>
    /// Checks if the player is grounded or not. Returns false if the player is attemting to jump.
    /// </summary>
    private bool IsGrounded() {
        //Debug.DrawRay(controlledPawn.transform.position - new Vector3(0, 0.8f), Vector3.down, Color.black);
        return jumpFlag ? false : Physics.BoxCast(controlledPawn.transform.position - new Vector3(0, 0.8f), new Vector3(0.25f, 0.02f, 0.25f), Vector3.down, Quaternion.identity, 0.01f, mask);
    }

    private void ChangeAlpha(float alpha)
    {
        var mat = controlledPawn.GetComponent<Renderer>().material;
        var oldColor = mat.color;
        var newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        mat.SetColor("_Color", newColor);
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

    private void OnSneakBegin()
    {
        var sneak = new Sneak();
        _effects.Add("Sneak", sneak);
        ChangeAlpha(0.5f);
    }

    private void OnSneakEnd()
    {
        _effects.Remove("Sneak");
        ChangeAlpha(1f);
    }

    /**
     * Currently only works for speed
     */
    private void ApplyEffects()
    {
        var effects = _effects.Values;
        var newFlat = effects.Aggregate(baseSpeed, (accum, effect) => accum + effect.flat);
        speed = effects.Aggregate(newFlat, (accum, effect) => accum * effect.multiplier);
    }

    // Sets controlled pawn to new pawn, resets camera on new pawn
    public void ChangeControlledPawn(GameObject newPawn) {
        var temp = controlledPawn;
        controlledPawn = newPawn;
        pawnController = controlledPawn.GetComponent<CharacterController>();

        // set camera transform
        pawnCamera.transform.SetParent(controlledPawn.transform);
        pawnCamera.transform.localPosition = CameraPosition;
        pawnCamera.transform.rotation = controlledPawn.transform.rotation * Quaternion.Euler(15.0f, 0f, 0f);

        // retain current rotation
        controlledPawn.transform.rotation = temp.transform.rotation;

        // grab additional components
        anim = controlledPawn.GetComponent<Animator>();
    }
}