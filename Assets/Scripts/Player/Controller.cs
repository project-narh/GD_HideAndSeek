using Photon.Pun;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public PhotonView pv;
    public bool isPlayer = false;
    public int hp = 3;

    private void Start()
    {
        /*        if (GetComponentInParent<PlayerController>() != null)
                {
                    Debug.Log("플레이어 맞음");
                    isPlayer = true;
                    pv = GetComponentInParent<PhotonView>();
                    Debug.Log(pv + "플레이어");
                }
                else
                {
                    pv = gameObject.AddComponent<PhotonView>();
                    gameObject.AddComponent<PhotonTransformView>();
                }*/
        Init();
    }
    
    public virtual void Init()
    {
        isPlayer = false;
        pv = gameObject.AddComponent<PhotonView>();
        gameObject.AddComponent<PhotonTransformView>();
    }


    public void CallTakeDamage(PhotonView view)
    {
        view.RPC("TakeDamageDDD", RpcTarget.All);
    }

    [PunRPC]
    public void TakeDamageDDD()
    {
        hp--;
        Debug.Log($"현재 HP: {hp}");

        if (hp <= 0)
        {
            // HP가 0 이하가 되면 오브젝트 비활성화
            pv.RPC("DisableObject", RpcTarget.All);
        }
    }

    [PunRPC]
    public void DisableObject()
    {
        if (!isPlayer)
            gameObject.SetActive(false);
        else
            Dead();

    }

    protected virtual void Dead(){}
}