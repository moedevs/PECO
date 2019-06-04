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
    public float rotationSpeed;
    private Vector3 moveNorm;
    [HideInInspector] public bool canAct;
    private float speedMultiplier = 1f;
    public float slideFriction;

    // Jumping
    private bool jumpFlag = false;
    [HideInInspector] public bool grounded, groundedFromCast;
    private float jumpTimer = 0f, baseJumpY;
    private Vector3 baseAirMomentum;
    private PlayerCollisions playerColls;

    // Costume/form functionality
    [HideInInspector] public Form currentForm;
    public FormDataBase formData;
    private FormManager formManager;

    // Attacking functionality
    public float attackHoldTimer = 0f;

    // Stealth functionality
    [HideInInspector] public bool isSneaking;

    // Input variables 
    private const float baseSpeed = 12.0f;
    public float speed = baseSpeed;
    public float turnSpeed = 2f;
    Vector3 moveDirection;

    // Animation variables
    private Animator anim;

    /**
     * Speed modifiers for now, this will be
     * generalized later on
     */
    //private Dictionary<string, SpeedEffect> _effects = new Dictionary<string, SpeedEffect>();

    //sets the position of the camera behind each pawn once possessed
    //private Vector3 CameraPosition = new Vector3(0.8f, 2.5f, -4.2f);

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
        playerColls = controlledPawn.GetComponent<PlayerCollisions>();

        // Find additional components
        anim = controlledPawn.GetComponent<Animator>();
        formManager = GetComponent<FormManager>();
        formManager.humanPawn = controlledPawn;

        HealthSystem.UpdateHP();
    }

    private void Update() {
        if(controlledPawn == null)
            return;

        // Movement
        if(canAct && grounded && !jumpFlag && Input.GetButtonDown("Jump"))
            jumpFlag = true;

        // Attacking
        if(canAct) {
            if(currentForm == Form.Human || currentForm == Form.Test) {
                if(Input.GetButtonDown("AttackStandard") || Input.GetButtonDown("AttackScissor"))
                    anim.SetTrigger("Scissor");
                attackHoldTimer = 0;
            } else {
                if(Input.GetButtonDown("AttackStandard")) {
                    if(!grounded)
                        anim.SetTrigger("AirAttack");
                    else {
                        anim.SetTrigger("Attack");
                    }
                    attackHoldTimer = 0;
                    anim.SetBool("ChargeRelease", false);
                } else if(Input.GetButton("AttackStandard")) {
                    attackHoldTimer += Time.deltaTime;
                    if(attackHoldTimer >= 0.75f)
                        anim.SetTrigger("ChargeAttack");
                } else if(Input.GetButtonUp("AttackStandard")) {
                    attackHoldTimer = 0;
                    anim.SetBool("ChargeRelease", true);
                } else if(Input.GetButtonDown("AttackScissor")) {
                    //anim.SetTrigger("Scissor");
                }
            }
        }

        // Stealth
        if(Input.GetButtonDown("Sneak")) {
            isSneaking = !isSneaking;
            if(isSneaking) {
                speedMultiplier = 0.5f;
                CanvasManager.cm.stealthGradient.SetActive(true);
            } else {
                speedMultiplier = 1f;
                CanvasManager.cm.stealthGradient.SetActive(false);
            }
        }
    }

    private void LateUpdate() {
        if(currentForm != Form.Human && currentForm != Form.Test) {
            if(grounded)
                anim.SetBool("Grounded", true);
            else
                anim.SetBool("Grounded", false);
        }
    }

    private void FixedUpdate() {
        if (controlledPawn == null)
            return;
        // Check grounded
        groundedFromCast = IsGrounded();
        if(grounded && baseAirMomentum.magnitude > 0) {
            baseAirMomentum = Vector3.zero;
            baseAirMomentum.y = 0;
        }
        else if(!grounded && controlledPawn.transform.position.y < baseJumpY)
            baseAirMomentum = Vector3.zero;
        // Jump functionality
        if(jumpTimer > 0f)
            jumpTimer -= Time.fixedDeltaTime;
        else
            jumpTimer = 0f;
        // Apply movement
        if(canAct)
            Movement();
    }
    
    private void Movement() {
        // Retrieve inputs
        moveNorm = Camera.main.transform.TransformVector(Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"))) * formData.walkSpeed * speedMultiplier);
        moveDirection.x = moveNorm.x;
        moveDirection.z = moveNorm.z;
        
        // Apply gravity and jump
        if(jumpFlag) {
            moveDirection.y = formData.jumpStrength;
            baseJumpY = controlledPawn.transform.position.y;
            baseAirMomentum = moveDirection;
            jumpFlag = false;
            jumpTimer = 0.25f;
        } else if(!grounded) {
            // Apply gravity
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

            // Restrict air momentum
            if(moveDirection.y < 0.1f && SlideOffSurface()) {

            } else {
                RedirectAirMomentum();
            }
        } else if(jumpTimer <= 0f && moveDirection.y <= 0.05f) {
            moveDirection.y = 0;
        }
        moveDirection.y = Mathf.Clamp(moveDirection.y, Mathf.Abs(formData.maxFallSpeed) * -1f, 50f);

        // Apply movement
        pawnController.Move(moveDirection * Time.fixedDeltaTime);
        if(moveNorm.magnitude > 0.05f)
            controlledPawn.transform.rotation = Quaternion.Euler(0, Quaternion.RotateTowards(controlledPawn.transform.rotation, Quaternion.LookRotation(moveNorm, Vector3.up), rotationSpeed).eulerAngles.y, 0);
    }

    /// <summary>
    /// Checks if the player is grounded or not. Returns false if the player is attemting to jump.
    /// </summary>
    private bool IsGrounded() {
        //Debug.DrawRay(controlledPawn.transform.position - new Vector3(0, formData.formHeight / 2), Vector3.down, Color.black, 1f, false);
        return jumpFlag ? false : Physics.BoxCast(controlledPawn.transform.position - new Vector3(0, formData.formHeight / 2), formData.groundedSkin, Vector3.down, controlledPawn.transform.rotation, 0.02f, LayerMask.GetMask("Terrain"));
    }


    private bool SlideOffSurface() {
        if(Vector3.Angle(Vector3.up, playerColls.hitNormal) >= 70f) {
            moveDirection.x = (1 - playerColls.hitNormal.y) * playerColls.hitNormal.x * (1f - slideFriction);
            moveDirection.z = (1 - playerColls.hitNormal.y) * playerColls.hitNormal.z * (1f - slideFriction);
            //Debug.Log("sliding");
            return true;
        }
        return false;
    }

    private void RedirectAirMomentum() {
        //Debug.Log(baseAirMomentum);
        if(baseAirMomentum.magnitude > 0) {
            moveDirection.x = baseAirMomentum.x + (moveDirection.x * 0.4f);
            moveDirection.z = baseAirMomentum.z + (moveDirection.z * 0.4f);
        } else {
            moveDirection.x *= 0.75f;
            moveDirection.z *= 0.75f;
        }
    }

    /*private void ChangeAlpha(float alpha) {
        var mat = controlledPawn.GetComponent<Renderer>().material;
        var oldColor = mat.color;
        var newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
        mat.SetColor("_Color", newColor);
    }

    private void OnSneakBegin() {
        var sneak = new Sneak();
        _effects.Add("Sneak", sneak);
        ChangeAlpha(0.5f);
    }

    private void OnSneakEnd() {
        _effects.Remove("Sneak");
        ChangeAlpha(1f);
    }*/

    /**
     * Currently only works for speed
     */
    /*private void ApplyEffects() {
        var effects = _effects.Values;
        var newFlat = effects.Aggregate(baseSpeed, (accum, effect) => accum + effect.flat);
        speed = effects.Aggregate(newFlat, (accum, effect) => accum * effect.multiplier);
    }*/

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
        float oldHeight = formData.spawnHeight;
        formManager.GetNewData(currentForm);

        // retain current position and rotation
        TeleportPlayer(oldPawn.transform.position + new Vector3(0f, -oldHeight + formData.spawnHeight, 0f));
        controlledPawn.transform.rotation = oldPawn.transform.rotation;
        controlledPawn.SetActive(true);
        oldPawn.SetActive(false);

        // set camera reference
        CameraController.player = controlledPawn;

        // grab additional components
        anim = controlledPawn.GetComponent<Animator>();
        playerColls = controlledPawn.GetComponent<PlayerCollisions>();
    }

    // Teleports player to destination
    public void TeleportPlayer(Vector3 destination) {
        pawnController.enabled = false;
        controlledPawn.transform.position = destination;
        pawnController.enabled = true;
    }
}