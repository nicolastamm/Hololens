using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Photon Callbacks

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() isMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadArena();
        }

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);

        if(PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

            LoadArena();
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom() was called. Loading Launcher.");
        SceneManager.LoadScene("Launcher");
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods

    void LoadArena()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("Trying to load a level but we are not the master Client.");
            return;
        }

        Debug.LogFormat("Loading Level");
        PhotonNetwork.LoadLevel("MainRoom");
    }

    private void Start()
    {
        Instance = this;

        if(playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing<a/></Color> playerPrefab Reference. Please set it up in the GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are instantiating LocalPlayer from {0}", SceneManager.GetActiveScene().name);

            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,0f,0f), Quaternion.identity, 0);
            Destroy(GameObject.Find("DummyCamera"));
        }
    }

    #endregion

    #region Public Fields

    [Tooltip("The prefab to use for representing the player.")]
    public GameObject playerPrefab;
    public static GameManager Instance;


    #endregion

}