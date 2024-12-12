using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerInputfield : MonoBehaviour
{
    private TMP_InputField _inputField;

    void Start()
    {
        _inputField = this.GetComponent<TMP_InputField>();
        if (_inputField != null)
        {
            _inputField.onValueChanged.AddListener(SetPlayerName);
        }
    }

    public void SetPlayerName(string value)
    {
        // #Important
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }
        PhotonNetwork.NickName = value;

        // Update the TMP input field text
        if (_inputField != null)
        {
            _inputField.text = value;
        }
    }
}