using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// This class manages the networked Room. This includes adding and removing players and also spawning networked room objects.
/// </summary>

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    public static PhotonRoom Room;

    // Player Prefab
    [SerializeField] private GameObject photonUserPrefab      = default;

    //Building Blocks Prefabs
    [SerializeField] private GameObject photonCubePrefab      = default;
    [SerializeField] private GameObject photonColumnPrefab    = default;
    [SerializeField] private GameObject photonBrickPrefab     = default;
    [SerializeField] private GameObject photonArchBrickPrefab = default;
    [SerializeField] private GameObject photonConePrefab      = default;
    [SerializeField] private GameObject photonDomePrefab      = default;

    private Player[] photonPlayers;
    private int playersInRoom;
    private int myNumberInRoom;

    /// <summary>
    /// Add the new player to the playerlist and increase the amount of players to assign unique player gameobject identifier.
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom++;
    }

    private void Awake()
    {
        if (Room == null)
        {
            Room = this;
        }
        else
        {
            if (Room != this)
            {
                Destroy(Room.gameObject);
                Room = this;
            }
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void Start()
    {
        // Allow prefabs not in a Resources folder by manually adding them to the Resourcecache.
        if (PhotonNetwork.PrefabPool is DefaultPool pool)
        {
            if (photonUserPrefab      != null) pool.ResourceCache.Add(photonUserPrefab.name     , photonUserPrefab);
            if (photonCubePrefab      != null) pool.ResourceCache.Add(photonCubePrefab.name     , photonCubePrefab);
            if (photonBrickPrefab     != null) pool.ResourceCache.Add(photonBrickPrefab.name    , photonBrickPrefab);
            if (photonArchBrickPrefab != null) pool.ResourceCache.Add(photonArchBrickPrefab.name, photonArchBrickPrefab);
            if (photonColumnPrefab    != null) pool.ResourceCache.Add(photonColumnPrefab.name   , photonColumnPrefab);
            if (photonConePrefab      != null) pool.ResourceCache.Add(photonConePrefab.name     , photonConePrefab);
            if (photonDomePrefab      != null) pool.ResourceCache.Add(photonDomePrefab.name     , photonDomePrefab);


        }
    }
    /// <summary>
    /// Overriding <see cref="IMatchmakingCallbacks.OnJoinedRoom"/>
    /// Checks for the amount of users connected to the room already and assigns a corresponding unique integer to the player GameObject.
    /// </summary>
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        photonPlayers = PhotonNetwork.PlayerList;
        playersInRoom = photonPlayers.Length;
        myNumberInRoom = playersInRoom;
        PhotonNetwork.NickName = myNumberInRoom.ToString();

        StartGame();
    }
    /// <summary>
    /// This method is encapsulates all that needs to happen for the gameloop to start. At the moment, only the player object needs to be spawned.
    /// </summary>
    private void StartGame()
    {
        CreatePlayer();
    }
    /// <summary>
    /// This creates the playerUser GameObject. 
    /// </summary>
    private void CreatePlayer()
    {
        var player = PhotonNetwork.Instantiate(photonUserPrefab.name, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// Instantiates a primitive as a room (neutral) object owened by the MasterClient.
    /// </summary>
    /// <param name="prefabName"> The GameObjects are tagged with the name of their corresponding prefab. This string is that tag. </param>
    /// <param name="startPosition"> Given by the hand position at spawn point. </param>
    /// <param name="startRotation"> Given by the hand rotation at spawn point. </param>
    public void CreateObject(string prefabName, Vector3 startPosition, Quaternion startRotation)
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        PhotonNetwork.InstantiateRoomObject(prefabName, startPosition, startRotation);
    }
}
