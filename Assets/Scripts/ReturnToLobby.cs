using UnityEngine;
using Photon.Pun; // For Photon Networking
using Photon.Realtime; // For Photon Room and Lobby references

public class ReturnToLobby : MonoBehaviourPunCallbacks
{
    public void OnReturnLobbyButtonPressed()
    {
        // Check if the player is connected and in a room
        if (PhotonNetwork.InRoom)
        {
            Debug.Log("Leaving the room and returning to the lobby...");
            PhotonNetwork.LeaveRoom(); // Disconnect from the current room
        }
        else
        {
            Debug.Log("Not in a room, loading lobby directly...");
            LoadLobbyScene();
        }
    }

    // Called automatically by Photon when the player leaves the room
    public override void OnLeftRoom()
    {
        Debug.Log("Left the room successfully, loading lobby scene.");
        LoadLobbyScene();
    }

    private void LoadLobbyScene()
    {
        // Replace "LobbySceneName" with the actual scene name of your lobby
        string lobbySceneName = "Launcher";

        // Load the lobby scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(lobbySceneName);
    }
}
