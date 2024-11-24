using Photon.Pun;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject prefab;
    public SphereCollider area;
    public Transform player_lobby;
    
    public LayerMask mask;

    private void Start()
    {
        Spawn();
    }

    public void Spawn(int index) // Bot ��ȯ
    {
        Debug.Log("��ȯ ��ȯ����");
        Debug.Log(index);
        while (index > 0 )
        {
            Debug.Log("��ȯ ��ȯ");
            Vector3 randomPosition = GetPosition();

            // �ش� ��ġ���� �浹ü�� �ִ��� Ȯ��
            if (!Physics.CheckSphere(randomPosition, 0.5f, mask))
            {
                // �浹ü�� ���ٸ� ������ ��ȯ
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                PhotonNetwork.Instantiate("Tami", randomPosition, randomRotation);
                index--;
            }
        }
    }

    public void GameReset(int index)
    {
        Debug.Log("�����" + index);
        Spawn(index);
        //PlayerpositionMove();
    }

    public void Spawn() // Player ����
    {
        PhotonNetwork.Instantiate("Player", player_lobby.position, Quaternion.identity);
    }
    
    Vector3 GetPosition()
    {
        // ���Ǿ� �ݶ��̴��� �������� �������� ������ ��ġ ���
        Vector3 areaCenter = area.transform.position + area.center;
        float radius = area.radius * area.transform.localScale.x; // �������� ����Ͽ� ������ ���

        // �� ������ ������ ���� ����
        Vector3 randomDirection = Random.insideUnitSphere;  // �� ���ο��� ������ ����
        float randomDistance = Random.Range(0f, radius);    // ������ �� ������ �Ÿ�

        // ������ ��ġ ��� (�� ������ ��)
        Vector3 randomPosition = areaCenter + randomDirection * randomDistance;
        randomPosition.y = 0.9f;
        return randomPosition;
    }

    Vector3 CheckGet()
    {
        Vector3 randomPosition;
        while (true)
        {
            randomPosition = GetPosition();

            if (!Physics.CheckSphere(randomPosition, 0.5f, mask)) break;
        }
        return randomPosition;
    }

    public void MoveAllPlayersToPosition(Vector3 targetPosition)
    {
        GameObject player = PhotonNetwork.LocalPlayer.TagObject as GameObject;
    }

    public void PlayerpositionMove()
    {
        PlayerController[] allPhotonViews = FindObjectsOfType<PlayerController>();
        foreach (PlayerController player in allPhotonViews)
        {
            PhotonView photonView = player.gameObject.GetComponent<PhotonView>();
            Vector3 randomPosition;
            while (true) 
            {
                randomPosition = GetPosition();

                if (!Physics.CheckSphere(randomPosition, 0.5f, mask)) break;
            }
            
            // PhotonView�� �����ڰ� �ִ� ��쿡�� ó��
            if (photonView.IsMine) // �ڽ��� Ŭ���̾�Ʈ�� ������ ������Ʈ�� ó��
            {
                // ��ġ ������ �ٸ� Ŭ���̾�Ʈ�� ����ȭ
                photonView.RPC("UpdateObjectPosition", RpcTarget.All, randomPosition);
            }
        }
    }

    [PunRPC]
    public void UpdateObjectPosition(Vector3 newPosition, PhotonMessageInfo info)
    {
        // PhotonView�� ��ġ�� ����
        transform.position = newPosition;

        // ����� �α� (������)
        Debug.Log($"������Ʈ {gameObject.name}�� ��ġ�� {newPosition}���� ����Ǿ����ϴ�. ȣ����: {info.Sender.NickName}");
    }
}
