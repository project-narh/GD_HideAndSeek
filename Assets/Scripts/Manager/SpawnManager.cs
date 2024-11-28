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

    public void Spawn(int index) // Bot 소환
    {
        Debug.Log(index);
        while (index > 0 )
        {
            Vector3 randomPosition = GetPosition();

            // 해당 위치에서 충돌체가 있는지 확인
            if (!Physics.CheckSphere(randomPosition, 0.5f, mask))
            {
                // 충돌체가 없다면 프리팹 소환
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
                PhotonNetwork.Instantiate("Tami", randomPosition, randomRotation);
                index--;
            }
        }
    }

    public void GameReset(int index)
    {
        Spawn(index);
        PlayerpositionMove();
    }

    public void Spawn() // Player 소한
    {
        PhotonNetwork.Instantiate("Player", player_lobby.position, Quaternion.identity,0);
    }
    
    Vector3 GetPosition()
    {
        // 스피어 콜라이더의 반지름을 기준으로 무작위 위치 계산
        Vector3 areaCenter = area.transform.position + area.center;
        float radius = area.radius * area.transform.localScale.x; // 스케일을 고려하여 반지름 계산

        // 구 내부의 무작위 점을 구함
        Vector3 randomDirection = Random.insideUnitSphere;  // 구 내부에서 무작위 방향
        float randomDistance = Random.Range(0f, radius);    // 반지름 내 무작위 거리

        // 무작위 위치 계산 (구 내부의 점)
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
            
            // PhotonView의 소유자가 있는 경우에만 처리
                Debug.Log($"PhotonView ID: {photonView.ViewID}");
                photonView.RPC("UpdateObjectPosition", RpcTarget.AllBuffered, randomPosition);
        }
    }
}
