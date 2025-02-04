using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    PlayerAnim playerAnim;
    Rigidbody rb;
    public CharacterController controller;
    [SerializeField] Camera mainCamera;

    public bool isMoving;

    [SerializeField] public float speed;
    private Transform mouse;

    private Animator animator;

    private void Start()
    {
        playerAnim = GetComponent<PlayerAnim>();
        playerAnim = FindObjectOfType<PlayerAnim>();
        rb = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        LookAtMouse();
        Movement();
    }
    void LookAtMouse()
    {
            Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
            float rayLenght;

        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (groundPlane.Raycast(cameraRay, out rayLenght))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLenght);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.blue);

                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }
    }
    public void Movement()
    {
        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        controller.Move(move * Time.deltaTime * speed);

        if (move != Vector3.zero && Input.GetKey(KeyCode.Mouse1) == false)
        {
            playerAnim.animations = PlayerAnimations.Walking;
            gameObject.transform.forward = move;
            isMoving = true;
        }
        else
        {
            playerAnim.animations = PlayerAnimations.idle;
            isMoving = false;
        }
    }
}