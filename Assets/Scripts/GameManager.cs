using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

namespace com.levokerem
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab; // Assign the player prefab in the Inspector
        private static GameObject localPlayerInstance; // Static variable to track the local player instance
        private PlayerControls playerControls;

        private void Awake()
        {
            localPlayerInstance = null;
            playerControls = new PlayerControls();
        }

        private void Start()
        {
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Launcher");
                return;
            }
            if (playerPrefab == null)
            {
                Debug.LogError("Player is not assigned in GameManager.");
                return;
            }
            if (PhotonNetwork.InRoom)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                if (localPlayerInstance == null)
                {
                    SpawnPlayer();
                }
            }
            else
            {
                Debug.LogError("Not connected to Photon or not in a room.");
            }
        }
        private void SpawnPlayer()
        {
            if (localPlayerInstance == null)
            {
                // Determine the spawn position based on the number of players in the room
                Vector3 spawnPosition;
                Quaternion spawnRotation;
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
                {
                    spawnPosition = new Vector3(-0.51f, -0.67f, 2.68f); // First player spawn position
                    spawnRotation = Quaternion.Euler(0, 0, 0); // First player spawn rotation
                }
                else
                {
                    spawnPosition = new Vector3(1.2f, -0.67f, 16.05f); // Second player spawn position
                    spawnRotation = Quaternion.Euler(0, 180, 0); // Second player spawn rotation
                }

                // Spawn the player character
                GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, spawnRotation);
                localPlayerInstance = player; // Set the local player instance
                                              //SetupCameraForLocalPlayer(player);
            }
        }

        // private void SetupCameraForLocalPlayer(GameObject localPlayer)
        // {
        //     // Find the PlayerCameraRoot sub-object
        //     Transform playerCameraRoot = localPlayer.transform.Find("PlayerCameraRoot");
        //     if (playerCameraRoot == null)
        //     {
        //         Debug.LogError("PlayerCameraRoot not found on the player prefab.");
        //         return;
        //     }

        //     // Instantiate the Cinemachine Virtual Camera
        //     GameObject vCam = Instantiate(cinemachineCameraPrefab);
        //     CinemachineVirtualCamera virtualCamera = vCam.GetComponent<CinemachineVirtualCamera>();

        //     if (virtualCamera != null)
        //     {
        //         // Set the Virtual Camera to follow the PlayerCameraRoot
        //         virtualCamera.Follow = playerCameraRoot;
        //     }
        //     else
        //     {
        //         Debug.LogError("CinemachineVirtualCamera component not found on the prefab.");
        //     }
        // }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        // Callbacks for Photon
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.Log("OnPlayerEnteredRoom() " + newPlayer.NickName); // not seen if you're the player connecting
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("OnPlayerLeftRoom() " + otherPlayer.NickName); // seen when other disconnects
        }
        public override void OnJoinedRoom()
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            if (localPlayerInstance == null)
            {
                SpawnPlayer();
            }
        }
        public override void OnLeftRoom()
        {
            Debug.Log("Left the room, returning to Main Menu.");
            localPlayerInstance = null; // Reset the local player instance
            PhotonNetwork.LoadLevel("Launcher"); // Load a Main Menu scene when leaving the room
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogError($"Disconnected from Photon: {cause}");
            localPlayerInstance = null; // Reset the local player instance
            PhotonNetwork.LoadLevel("Launcher"); // Load a Main Menu scene when disconnected
        }
    }
}