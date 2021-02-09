using Photon.Pun;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.Instantiate("CustomCharacter",gameObject.transform.position,gameObject.transform.rotation);
    }
}
