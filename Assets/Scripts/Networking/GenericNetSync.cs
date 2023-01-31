using Photon.Pun;
using UnityEngine;

/// <summary>
/// This is the code that handles the positional data streams. 
/// If the object is owned by this client, it sends the data over the network to the server,
/// If the object is not owned by this client, it receives the data from the server.
/// </summary>

public class GenericNetSync : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private bool isUser = default;

    private Camera mainCamera;

    private Vector3 networkLocalPosition;
    private Quaternion networkLocalRotation;

    private Vector3 startingLocalPosition;
    private Quaternion startingLocalRotation;

    /// <summary>
    /// IPunObservable are a list of objects belonging to a Photon View which are declared to get and receive data over the network.
    /// OnPhotonSerializeView is a method called by PUN several times per second (depending on the refresh rate) which allows this object to write and receive data to the photon viewer.
    /// </summary>
    /// <param name="stream"> This is the info stream to write and to read from.</param>
    /// <param name="info"> This contains information like the player that sends an RPC. Not used in this method. </param>
    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.localPosition);
            stream.SendNext(transform.localRotation);
        }
        else
        {
            networkLocalPosition = (Vector3) stream.ReceiveNext();
            networkLocalRotation = (Quaternion) stream.ReceiveNext();
        }
    }

    private void Start()
    {
        mainCamera = Camera.main;

        if (isUser)
        {

            if (photonView.IsMine) GenericNetworkManager.Instance.localUser = photonView;
        }

        var trans = transform;
        startingLocalPosition = trans.localPosition;
        startingLocalRotation = trans.localRotation;

        networkLocalPosition = startingLocalPosition;
        networkLocalRotation = startingLocalRotation;
    }

    /// <summary>
    ///  This is where the information received over the network is set to the actual transform of the obecjt
    ///  <see cref="IPunObservable.OnPhotonSerializeView(PhotonStream, PhotonMessageInfo)"></see>
    /// </summary>
    private void Update()
    {
        if (!photonView.IsMine)
        {
            var trans = transform;
            trans.localPosition = networkLocalPosition;
            trans.localRotation = networkLocalRotation;
        }

        if (photonView.IsMine && isUser)
        {
            var trans = transform;
            var mainCameraTransform = mainCamera.transform;
            trans.position = mainCameraTransform.position;
            trans.rotation = mainCameraTransform.rotation;
        }
    }
}

