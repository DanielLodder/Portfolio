using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour
{
    Buyables buyables;
    Barricade barricade;
    [Header("Player Movement")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform orientation;
    [SerializeField] private Vector3 moveDirection;
    [SerializeField] private Vector3 movement;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpHeight;
    [SerializeField] private bool isGrounded;

    [Header("Player Looking")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float mouseSensitivity;
    private Vector3 looking;
    private Vector3 turning;

    [Header("Player Weapons")]
    [SerializeField] public List<GameObject> playerWeapons;
    [SerializeField] public GameObject weaponParent;
    [SerializeField] private GameObject startingWeaponPrefab;
    public int weaponWield;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject startingWeapon = Instantiate(startingWeaponPrefab);
        startingWeapon.name = startingWeaponPrefab.name;
        startingWeapon.transform.position = weaponParent.transform.position;
        startingWeapon.transform.parent = weaponParent.transform;
        playerWeapons.Add(startingWeapon);
        Cursor.lockState = CursorLockMode.Locked;
    }
    void Update()
    {
        PlayerMove();
        PlayerTurning();
        InteractObjects();
        SwitchWeapons();
    }

    private void PlayerMove()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        moveDirection = orientation.forward * movement.z + orientation.right * movement.x;
        rb.linearVelocity = moveDirection.normalized * movementSpeed;
    }

    private void PlayerTurning()
    {
        looking.x = Input.GetAxisRaw("Mouse Y");
        turning.y = Input.GetAxisRaw("Mouse X");

        playerCamera.transform.eulerAngles += new Vector3(looking.x * -mouseSensitivity * Time.deltaTime, looking.y, looking.z);
        rb.transform.eulerAngles += new Vector3(turning.x, turning.y * mouseSensitivity * Time.deltaTime, turning.z);
        Mathf.Clamp(looking.y, 5, 175);
    }
    private void InteractObjects()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 1f))
        {
            if (hit.collider)
            {
                buyables = hit.collider.GetComponent<Buyables>();
                barricade = hit.collider.GetComponent<Barricade>(); 

                if (Input.GetKeyDown(KeyCode.F))
                {
                    if (hit.collider.GetComponent<Buyables>())
                    {
                        buyables.BuyObjects();
                    }
                    if (hit.collider.GetComponent<Barricade>())
                    {
                        barricade.RebuildBarricade();
                    }
                }
            }
        }
    }
    private void SwitchWeapons()
    {
    
        if (playerWeapons.Count >= 1)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                weaponWield = 0;
                playerWeapons[0].SetActive(true);
                playerWeapons[1].SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                weaponWield = 1;
                playerWeapons[1].SetActive(true);
                playerWeapons[0].SetActive(false);
            }
        }
    }
}
