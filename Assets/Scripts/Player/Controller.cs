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
        Debug.Log($"{gameObject.name} ���� HP: {hp}");

        if (hp <= 0)
        {
            // HP�� 0 ���ϰ� �Ǹ� ������Ʈ ��Ȱ��ȭ
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