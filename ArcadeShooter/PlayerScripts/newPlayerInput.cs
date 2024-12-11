using UnityEngine;
using UnityEngine.InputSystem;

public class newPlayerInput : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 moveDirection;
    private Vector3 LookDirection;
    private PlayerInput playerInput;
    public Camera playerCamera;
    private float jumpSpeed = 3;
    //[SerializeField] public List<GameObject> playerWeapons;
    [SerializeField] public GameObject weaponParent;
    [SerializeField] private GameObject startingWeaponPrefab;
    public int weaponWield;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        GameObject startingWeapon = Instantiate(startingWeaponPrefab);
        startingWeapon.name = startingWeaponPrefab.name;
        startingWeapon.transform.position = weaponParent.transform.position;
        startingWeapon.transform.parent = weaponParent.transform;
        //playerWeapons.Add(startingWeapon);
    }
    private void HandleOnMove(InputAction.CallbackContext ctx)
    {
        if (gameObject.scene.IsValid() || !gameObject.scene.IsValid())
        {
            moveDirection = ctx.ReadValue<Vector2>();
        }
    }
    private void HandleOnLook(InputAction.CallbackContext ctx)
    {
        if (gameObject.scene.IsValid())
        {
            LookDirection = ctx.ReadValue<Vector2>();
        }
    }

    private void HandleOnJump(InputAction.CallbackContext ctx)
    {
        Debug.Log("Jump");
        Vector3 jumpForce = new(0, jumpSpeed, 0);
        rb.AddForce(jumpForce, ForceMode.VelocityChange);
    }

    private void HandleOnSwitchWeapon(InputAction.CallbackContext ctx)
    {
        Debug.Log("Switched weapon");
    }
    private void HandleOnFire(InputAction.CallbackContext ctx)
    {
        Debug.Log("Fired weapon");
        PlayerWeapon playerWeapon = GetComponentInChildren<PlayerWeapon>();
        playerWeapon.FireWeapon();
    }
    private void HandleOnReload(InputAction.CallbackContext ctx)
    {
        Debug.Log("reloaded weapon");
        PlayerWeapon playerWeapon = GetComponentInChildren<PlayerWeapon>();
        playerWeapon.ReloadWeapon();
    }
    public void Initialize(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        playerInput.actions["Move"].started += HandleOnMove;
        playerInput.actions["Move"].performed += HandleOnMove;
        playerInput.actions["Move"].canceled += HandleOnMove;

        playerInput.actions["Look"].started += HandleOnLook;
        playerInput.actions["Look"].performed += HandleOnLook;
        playerInput.actions["Look"].canceled += HandleOnLook;

        playerInput.actions["Jump"].performed += HandleOnJump;

        playerInput.actions["SwitchWeapons"].performed += HandleOnSwitchWeapon;

        playerInput.actions["Fire"].started += HandleOnFire;

        playerInput.actions["Reload"].started += HandleOnReload;

        playerInput.deviceLostEvent.AddListener(HandleDeviceLost);
        playerInput.deviceRegainedEvent.AddListener(HandleDeviceRegained);
    }

    private void HandleDeviceLost(PlayerInput playerInput)
    {
        Debug.Log($"Player {playerInput.playerIndex} device got disconnected.");
    }

    private void HandleDeviceRegained(PlayerInput playerInput)
    {
        Debug.Log($"Player {playerInput.playerIndex} device got reconnected.");
    }

    private void Update()
    {
        transform.Translate(moveDirection.x * Time.deltaTime * 5f, 0f, moveDirection.y * Time.deltaTime * 5f);
        transform.eulerAngles += new Vector3(0f, LookDirection.x * Time.deltaTime * 50f, 0f);
        
        playerCamera.transform.eulerAngles += new Vector3(LookDirection.y * Time.deltaTime * -50f, 0f, 0f);
        Mathf.Clamp(LookDirection.y, -5, -175);
    }
}
