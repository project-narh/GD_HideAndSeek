using Photon.Pun;
using UnityEngine;

public class ParentObjectSync : MonoBehaviourPun, IPunObservable
{
    public Transform[] childObjects; // ���� ������Ʈ �迭
    private Vector3[] localPositions; // ��� ��ġ ����
    private Quaternion[] localRotations; // ��� ȸ�� ����

    private Vector3[] targetLocalPositions; // ��Ʈ��ũ�� ���� ��� ��ġ
    private Quaternion[] targetLocalRotations; // ��Ʈ��ũ�� ���� ��� ȸ��

    public float interpolationSpeed = 10f; // ���� �ӵ�

    void Start()
    {
        // ��� ��ġ/ȸ�� �迭 �ʱ�ȭ
        localPositions = new Vector3[childObjects.Length];
        localRotations = new Quaternion[childObjects.Length];
        targetLocalPositions = new Vector3[childObjects.Length];
        targetLocalRotations = new Quaternion[childObjects.Length];

        // �ʱⰪ ����
        for (int i = 0; i < childObjects.Length; i++)
        {
            localPositions[i] = childObjects[i].localPosition;
            localRotations[i] = childObjects[i].localRotation;
            targetLocalPositions[i] = childObjects[i].localPosition;
            targetLocalRotations[i] = childObjects[i].localRotation;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // ��� ��ġ�� ȸ�� �����͸� ����
            for (int i = 0; i < childObjects.Length; i++)
            {
                localPositions[i] = childObjects[i].localPosition;
                localRotations[i] = childObjects[i].localRotation;

                stream.SendNext(localPositions[i]);
                stream.SendNext(localRotations[i]);
            }
        }
        else
        {
            // ��� ��ġ�� ȸ�� �����͸� ����
            for (int i = 0; i < childObjects.Length; i++)
            {
                targetLocalPositions[i] = (Vector3)stream.ReceiveNext();
                targetLocalRotations[i] = (Quaternion)stream.ReceiveNext();
            }
        }
    }

    void Update()
    {
        // �����Ͽ� �ε巴�� �̵� �� ȸ�� ����
        for (int i = 0; i < childObjects.Length; i++)
        {
            childObjects[i].localPosition = Vector3.Lerp(
                childObjects[i].localPosition,
                targetLocalPositions[i],
                Time.deltaTime * interpolationSpeed
            );

            childObjects[i].localRotation = Quaternion.Lerp(
                childObjects[i].localRotation,
                targetLocalRotations[i],
                Time.deltaTime * interpolationSpeed
            );
        }
    }
}
