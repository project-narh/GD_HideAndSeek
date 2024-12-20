using Photon.Pun;
using UnityEngine;

public class Controller : MonoBehaviourPun
{
    PhotonView pv;
    bool isPlayer = false;
    int hp = 3;

    private void Start()
    {
        if (GetComponentInParent<PlayerController>() != null)
        {
            isPlayer = true;
            pv = GetComponentInParent<PhotonView>();
        }
        else
        {
            pv = gameObject.AddComponent<PhotonView>();
            gameObject.AddComponent<PhotonTransformView>();
        }
    }
    [PunRPC]
    public void TakeDamage()
    {
        if (!pv.IsMine)
            return;

        hp--;
        Debug.Log($"{gameObject.name} 현재 HP: {hp}");

        if (hp <= 0)
        {
            // HP가 0 이하가 되면 오브젝트 비활성화
            if(!isPlayer)
                photonView.RPC("DisableObject", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}