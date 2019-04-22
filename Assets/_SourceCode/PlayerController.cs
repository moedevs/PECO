using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum Form { Test, Human, Bear };

    public static PlayerController pc;

    // PlayerController/movement functionality
    public GameObject controlledPawn;
    private CharacterController pawnController;
    [SerializeField] private Camera pawnCamera;
    private Vector3 moveNorm;
    //private RaycastHit hitObj;
    public LayerMask mask;
    private bool jumpFlag = false;
    [HideInInspector] public bool canAct;

    // Costume/form functionality
    [HideInInspector] public Form currentForm;
    public FormDataBase formData;
    private FormManager formManager;

    // Attacking functionality
    public float attackHoldTimer = 0f;

    // Input variables 
    private const float baseSpeed = 12.0f;
    public float speed = baseSpeed;
    public float turnSpeed = 2f;
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
        currentForm = Form.Test;
        canAct = true;

        // set controlledPawn
        if(controlledPawn == null) {
            try {
                controlledPawn = GameObject.FindGameObjectsWithTag("PlayerControllable")[0];
            } catch {
                Debug.LogError("Unable to find object with tag \"PlayarControllable\", unable to set controlled Player.");
                return;
            }
        }
        pawnController = controlledPawn.GetComponent<CharacterController>();

        // Sets the camera to be a child of the controlled pawn
        if(pawnCamera == null)
            pawnCamera = Camera.main;
        pawnCamera.transform.SetParent(controlledPawn.transform);

        // Force set the cameras position 
        pawnCamera.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
        pawnCamera.transform.localPosition = CameraPosition;

        // Find additional components
        anim = controlledPawn.GetComponent<Animator>();
        formManager = GetComponent<FormManager>();
        formManager.humanPawn = controlledPawn;
    }

    private void Update() {
        if(controlledPawn == null)
            return;

        // Movement
        if(canAct && !jumpFlag && Input.GetButtonDown("Jump") && IsGrounded())
            jumpFlag = true;

        // Attacking
        if(canAct) {
            if(currentForm == Form.Human || currentForm == Form.Test) {
                if(Input.GetButtonDown("AttackStandard") || Input.GetButtonDown("AttackScissor"))
                    anim.SetTrigger("Scissor");
                attackHoldTimer = 0;
            } else {
                if(Input.GetButtonDown("AttackStandard")) {
                    if(!IsGrounded())
                        anim.SetTrigger("AirAttack");
                    else {
                        anim.SetTrigger("Attack");
                    }
                    attackHoldTimer = 0;
                } else if(Input.GetButton("AttackStandard")) {
                    attackHoldTimer += Time.deltaTime;
                } else if(Input.GetButtonUp("AttackStandard")) {
                    
                    attackHoldTimer = 0;
                } else if(Input.GetButtonDown("AttackScissor")) {
                    //anim.SetTrigger("Scissor");
                }
            }
        }
    }

    private void LateUpdate() {
        if(currentForm != Form.Human && currentForm != Form.Test) {
            if(IsGrounded())
                anim.SetBool("Grounded", true);
            else
                anim.SetBool("Grounded", false);
        }
    }

    private void FixedUpdate() {
        if (controlledPawn == null)
            return;
        // Apply movement
        if(canAct)
            Movement();
        //RayTrace();
    }

    private void Movement() {
        // Retrieve inputs
        moveNorm = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) * formData.walkSpeed;
        moveDirection.x = moveNorm.x;
        moveDirection.z = moveNorm.z;
        moveRotation = new Vector3(0.0f, Input.GetAxis("CameraX"), 0.0f);
        moveDirection = controlledPawn.transform.TransformVector(moveDirection);
        
        // Apply gravity and jump
        if(jumpFlag) {
            moveDirection.y = formData.jumpStrength;
            jumpFlag = false;
        } else if(!IsGrounded()) {
            if(moveDirection.y > 0.05f) {
                if(Input.GetButton("Jump")) {
                    moveDirection.y -= formData.gravityBase;
                } else {
                    moveDirection.y -= formData.gravityShortHop;
                }
            } else
                moveDirection.y -= formData.gravityFalling;
            if(moveDirection.y < 0)
                moveDirection.y -= formData.gravityFalling;
            else if(moveDirection.y > 0.05f && !Input.GetButton("Jump"))
                moveDirection.y -= formData.gravityShortHop;
            else
                moveDirection.y -= formData.gravityBase;
        } else {
            moveDirection.y = 0;
        }
        moveDirection.y = Mathf.Clamp(moveDirection.y, Mathf.Abs(formData.maxFallSpeed) * -1f, 50f);

        // Apply movement
        pawnController.Move(moveDirection * Time.fixedDeltaTime);
        pawnController.transform.Rotate((moveRotation * turnSpeed), Space.Self);
    }

    /// <summary>
    /// Checks if the player is grounded or not. Returns false if the player is attemting to jump.
    /// </summary>
    private bool IsGrounded() {
        //Debug.DrawRay(controlledPawn.transform.position - new Vector3(0, formData.formHeight / 2), Vector3.down, Color.black, 1f, false);
        return jumpFlag ? false : Physics.BoxCast(controlledPawn.transform.position - new Vector3(0, formData.formHeight / 2), formData.groundedSkin, Vector3.down, Quaternion.identity, 0.01f, mask);
    }

    private void ChangeAlpha(float alpha)
    {
        var mat = controlledPawn.GetComponent<Renderer>().material;
        var oldColor = mat.color;
        var newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        mat.SetColor("_Color", newColor);
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

    public void ChangeControlledPawn(Form newForm) {
        if(newForm == currentForm) {
            Debug.Log("Attempting to switch to current form " + newForm + ".");
            return;
        }
        currentForm = newForm;
        switch(newForm) {
            case Form.Human:
                SetNewPawn(formManager.humanPawn);
                return;
            case Form.Bear:
                SetNewPawn(formManager.GetNewPawn(Form.Bear));
                return;
            case Form.Test:
                SetNewPawn(formManager.GetNewPawn(Form.Test));
                return;
            default:
                Debug.Log("Attempting to switch to form that is not set up in PlayerController.");
                return;
        }
    }

    // Sets controlled pawn to new pawn, resets camera on new pawn
    private void SetNewPawn(GameObject newPawn) {
        // set pawn
        GameObject oldPawn = controlledPawn;
        controlledPawn = newPawn;
        pawnController = controlledPawn.GetComponent<CharacterController>();

        // grab new form data
        formManager.GetNewData(currentForm);

        // retain current position and rotation
        pawnController.enabled = false;
        controlledPawn.transform.position = new Vector3(oldPawn.transform.position.x, formData.spawnHeight, oldPawn.transform.position.z);
        pawnController.enabled = true;
        controlledPawn.transform.rotation = oldPawn.transform.rotation;
        controlledPawn.SetActive(true);
        oldPawn.SetActive(false);

        // set camera transform
        pawnCamera.transform.SetParent(controlledPawn.transform);
        pawnCamera.transform.localPosition = CameraPosition;
        pawnCamera.transform.rotation = controlledPawn.transform.rotation * Quaternion.Euler(15.0f, 0f, 0f);

        // grab additional components
        anim = controlledPawn.GetComponent<Animator>();
    }
}