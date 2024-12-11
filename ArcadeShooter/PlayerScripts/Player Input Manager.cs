using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    //[SerializeField] private GameObject playerPrefab;
    [SerializeField] private Color[] playerColors;
    private PlayerInputManager playerInputManager;
    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
    }

    private void OnEnable()
    {
        playerInputManager.onPlayerJoined += HandleOnPlayerJoined;
        playerInputManager.onPlayerLeft += HandleOnPlayerLeft;
    }

    private void HandleOnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log($"Player {playerInput.playerIndex} left");
    }

    private void HandleOnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.gameObject.name = $"Player {playerInput.playerIndex} Controller";
        //newPlayerInput player = Instantiate(playerPrefab).GetComponent<newPlayerInput>();
        GameObject player = playerInput.transform.Find("player").gameObject;
        player.GetComponent<newPlayerInput>().Initialize(playerInput);
        playerInput.transform.Find("player").GetComponent<MeshRenderer>().material.color = playerColors[playerInput.playerIndex];
    }
}
