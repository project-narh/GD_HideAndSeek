using Photon.Pun;
using UnityEngine;

public class ParentObjectSync : MonoBehaviourPun, IPunObservable
{
    public Transform[] childObjects;  // ���� ���� ������Ʈ�� �迭�� ����

    // �����͸� �����ϰ� �����ϴ� �޼���
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ���� ������Ʈ���� ��ġ�� ȸ�� �����͸� �迭�� ���� ����
            foreach (Transform child in childObjects)
            {
                stream.SendNext(child.position);
                stream.SendNext(child.rotation);
            }
        }
        else
        {
            // ���� ������Ʈ���� ��ġ�� ȸ�� �����͸� �����Ͽ� ����
            for (int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i].position = (Vector3)stream.ReceiveNext();
                childObjects[i].rotation = (Quaternion)stream.ReceiveNext();
            }
        }
    }
}
