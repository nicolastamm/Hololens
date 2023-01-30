using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace MRTK.Tutorials.MultiUserCapabilities
{
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


        // private PhotonView pv;
        private Player[] photonPlayers;
        private int playersInRoom;
        private int myNumberInRoom;

        // Used to assign the player to the object manager on player instantiation.

        // private GameObject module;
        // private Vector3 moduleLocation = Vector3.zero;

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
            // Allow prefabs not in a Resources folder.
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

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            photonPlayers = PhotonNetwork.PlayerList;
            playersInRoom = photonPlayers.Length;
            myNumberInRoom = playersInRoom;
            PhotonNetwork.NickName = myNumberInRoom.ToString();

            StartGame();
        }

        private void StartGame()
        {
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            var player = PhotonNetwork.Instantiate(photonUserPrefab.name, Vector3.zero, Quaternion.identity);
        }

        public void CreateObject(string prefabName, Vector3 startPosition, Quaternion startRotation)
        {
            PhotonNetwork.InstantiateRoomObject(prefabName, startPosition, startRotation);
        }
        // private void CreateMainLunarModule()
        // {
        //     module = PhotonNetwork.Instantiate(roverExplorerPrefab.name, Vector3.zero, Quaternion.identity);
        //     pv.RPC("Rpc_SetModuleParent", RpcTarget.AllBuffered);
        // }
        //
        // [PunRPC]
        // private void Rpc_SetModuleParent()
        // {
        //     Debug.Log("Rpc_SetModuleParent- RPC Called");
        //     module.transform.parent = TableAnchor.Instance.transform;
        //     module.transform.localPosition = moduleLocation;
        // }
    }
}
