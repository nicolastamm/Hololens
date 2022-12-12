using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics.Eventing.Reader;

public class Launcher : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields

    [Tooltip("The maximum amount of players per room. When a room is full, it can't be joined by new players and a new room will be created.")]
    [SerializeField]
    private byte maxPlayersPerRoom = 4;

    #endregion


    #region Public Fields

    [Tooltip("The Ui Panel  to let the user enter name, connect and play.")]
    [SerializeField]
    private GameObject controlPanel;

    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;
    
    #endregion


    #region Private Fields

    // Client version number. Used to separate clients.
    string gameVersion = "1";

    bool isConnecting;

    #endregion

    #region MonoBehaviour CallBacks

    void Awake()
    {
        // Necessary to call LoadLevel() on the master client and all other clients to sync to it.
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        // Now called through OnClick() of the Play button.
        //Connect();

        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion

    #region Public Methods

    // Start the connection process
    public void Connect()
    {

        progressLabel.SetActive(true);
        controlPanel.SetActive(false);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    #endregion

    #region MonoBehaviourCallbacks Callbacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster was called.");
        if(isConnecting)
        {
            isConnecting = false;
            PhotonNetwork.JoinRandomRoom();
        }
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.LogWarningFormat("OnDiconnected was called for reason: {0}.", cause);

        progressLabel.SetActive(false);
        controlPanel.SetActive(true);

        isConnecting = false;
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("OnJoinRandomFailed() was called. No random room was found.\n Calling PhotonNetwork.CreateRoom().");

        PhotonNetwork.CreateRoom(null, new RoomOptions() {MaxPlayers = maxPlayersPerRoom});
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom was called. This client is in a room.");
        Debug.Log("Player Count should be 1. It is : " + PhotonNetwork.CurrentRoom.PlayerCount.ToString());

        if(PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load 'MainRoom'");

            PhotonNetwork.LoadLevel("MainRoom");
        }
    }
    #endregion
}