using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Cinemachine;

namespace com.levokerem
{

    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance; // Singleton instance
        public GameObject playerPrefab; // Assign the player prefab in the Inspector
        public GameObject cinemachineCameraPrefab; // Assign the CinemachineVirtualCamera prefab in the Inspector

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (playerPrefab == null || cinemachineCameraPrefab == null)
            {
                Debug.LogError("Player or Cinemachine camera prefab is not assigned in GameManager.");
                return;
            }

            if (PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
            {
                SpawnPlayer();
            }
            else
            {
                Debug.LogError("Not connected to Photon or not in a room.");
            }
        }

        private void SpawnPlayer()
        {
            // Spawn the player character
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);

            // Check if this player object belongs to the local client
            if (player.GetComponent<PhotonView>().IsMine)
            {
                SetupCameraForLocalPlayer(player);
            }
        }
        private void SetupCameraForLocalPlayer(GameObject localPlayer)
        {
            // Find the PlayerCameraRoot sub-object
            Transform playerCameraRoot = localPlayer.transform.Find("PlayerCameraRoot");
            if (playerCameraRoot == null)
            {
                Debug.LogError("PlayerCameraRoot not found on the player prefab.");
                return;
            }

            // Instantiate the Cinemachine Virtual Camera
            GameObject vCam = Instantiate(cinemachineCameraPrefab);
            CinemachineVirtualCamera virtualCamera = vCam.GetComponent<CinemachineVirtualCamera>();

            if (virtualCamera != null)
            {
                // Set the Virtual Camera to follow the PlayerCameraRoot
                virtualCamera.Follow = playerCameraRoot;
            }
            else
            {
                Debug.LogError("CinemachineVirtualCamera component not found on the prefab.");
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void LoadLevel(string levelName)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.LoadLevel(levelName); // Auto Level Sync ensures other players follow
            }
            else
            {
                Debug.LogWarning("Only the Master Client can load levels.");
            }
        }

        // Callbacks for Photon
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} joined the room.");
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log($"Player {otherPlayer.NickName} left the room.");
        }

        public override void OnLeftRoom()
        {
            Debug.Log("Left the room, returning to Main Menu.");
            PhotonNetwork.LoadLevel("Launcher"); // Load a Main Menu scene when leaving the room
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogError($"Disconnected from Photon: {cause}");
            // Optionally handle disconnection, e.g., return to main menu
        }
    }
}