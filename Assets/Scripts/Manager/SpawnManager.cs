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
        Spawn(20);
    }

    public void Spawn(int index) // Bot ��ȯ
    {

        while (index > 0 )
        {
            Vector3 randomPosition = GetPosition();

            // �ش� ��ġ���� �浹ü�� �ִ��� Ȯ��
            if (!Physics.CheckSphere(randomPosition, 0.5f, mask))
            {
                // �浹ü�� ���ٸ� ������ ��ȯ
                Instantiate(prefab, randomPosition, Quaternion.identity);
                index--;
            }
        }
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
        return randomPosition;
    }
    
}
