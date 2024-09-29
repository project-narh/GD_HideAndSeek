using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField] Text stat;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NetworkManager.Instance.StatusText = stat;
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        Destroy(this.gameObject);
    }

}
