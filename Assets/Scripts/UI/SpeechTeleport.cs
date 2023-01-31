using UnityEngine;
/// <summary>
/// Teleports the toolbox to position when summoned using voice commands 
/// </summary>
public class SpeechTeleport : MonoBehaviour
{
    /// <summary>
    /// Teleport to this position relative to player
    /// </summary>
    public Vector3 TeleportPosition = new Vector3(-0.1f, -0.1f, 0.7f);
    public void Teleport()
    {
        this.transform.Find("BlockBox").gameObject.SetActive(true);
        this.transform.position = TeleportPosition;
    }
}
