using Photon.Pun;
using UnityEngine;

public class ParentObjectSync : MonoBehaviourPun, IPunObservable
{
    public Transform[] childObjects; // 하위 오브젝트 배열
    private Vector3[] localPositions; // 상대 위치 저장
    private Quaternion[] localRotations; // 상대 회전 저장

    private Vector3[] targetLocalPositions; // 네트워크로 받은 상대 위치
    private Quaternion[] targetLocalRotations; // 네트워크로 받은 상대 회전

    public float interpolationSpeed = 10f; // 보간 속도

    void Start()
    {
        // 상대 위치/회전 배열 초기화
        localPositions = new Vector3[childObjects.Length];
        localRotations = new Quaternion[childObjects.Length];
        targetLocalPositions = new Vector3[childObjects.Length];
        targetLocalRotations = new Quaternion[childObjects.Length];

        // 초기값 설정
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
            // 상대 위치와 회전 데이터를 전송
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
            // 상대 위치와 회전 데이터를 수신
            for (int i = 0; i < childObjects.Length; i++)
            {
                targetLocalPositions[i] = (Vector3)stream.ReceiveNext();
                targetLocalRotations[i] = (Quaternion)stream.ReceiveNext();
            }
        }
    }

    void Update()
    {
        // 보간하여 부드럽게 이동 및 회전 적용
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
