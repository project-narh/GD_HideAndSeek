using Photon.Pun;
using UnityEngine;

public class ParentObjectSync : MonoBehaviourPun, IPunObservable
{
    public Transform[] childObjects;  // 여러 하위 오브젝트를 배열로 관리

    // 데이터를 전송하고 수신하는 메서드
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // 하위 오브젝트들의 위치와 회전 데이터를 배열을 통해 전송
            foreach (Transform child in childObjects)
            {
                stream.SendNext(child.position);
                stream.SendNext(child.rotation);
            }
        }
        else
        {
            // 하위 오브젝트들의 위치와 회전 데이터를 수신하여 적용
            for (int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].position = (Vector3)stream.ReceiveNext();
                childObjects[i].rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
