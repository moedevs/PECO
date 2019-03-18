using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private GameObject controlledPawn;
    private CharacterController pawnController;
    public Camera pawnCamera;
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
            MoveForward();
        }
    }

    private void MoveForward()
    {
        if (Input.GetKeyDown("w"))
        {
            pawnController.Move(new Vector3(0, 0, 1));
        }
    }
}
