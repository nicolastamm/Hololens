using Photon.Pun;
using UnityEngine;

/// <summary>
/// This class contains networked functionality specific to the playerObject.
/// At the moment the only thing it does is to set the gameobject name to correspond to the actual player in the playerlist.
/// Some future features that would be place in this class: 
///     - Change the avatars colour/appearance
///     - If block saving is implemented, here is where they would be managed
///     - etc..
/// </summary>
public class PhotonUser : MonoBehaviour
{
    private PhotonView pv;
    private string username;

    private void Start()
    {
        pv = GetComponent<PhotonView>();

        // The client who owns this playerobjects ...
        if (!pv.IsMine) return;

        // sends an rpc, i.e. asks the server the execute PunRPC_SetNickName.
        username = "User" + PhotonNetwork.NickName;
        pv.RPC("PunRPC_SetNickName", RpcTarget.AllBuffered, username);
    }
    /// <summary>
    /// This is a a remote call send to the server so that all players connected to the server see the same name for this player.
    /// </summary>
    /// <param name="nName">The name to be set.</param>
    [PunRPC]
    private void PunRPC_SetNickName(string nName)
    {
        gameObject.name = nName;
    }
}

