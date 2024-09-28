using Photon.Pun;
using System.Net.NetworkInformation;
using UnityEditor.PackageManager;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;

public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public struct Stage_Data
    {
        public int maxCount; // �ִ� �ο�(���� �� ����)
        public float limitTime; // �������� ���� �ð�
    }
    [SerializeField] int index = 0; // ���� ��������
    [SerializeField] Stage_Data[] data;
    private int player_count;
    private float stageTimer;
    private bool isStageActive = false;

    private const string StageIndexKey = "StageIndex";
    private const string PlayerCountKey = "PlayerCount";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePlayerCount();
        if (PhotonNetwork.IsMasterClient)
        {

        }
        else
        {
            SyncStageData();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(PhotonNetwork.IsMasterClient && isStageActive)
        {

        }
    }

    public void InitStage(int index)
    {
        this.index = index;
        player_count = PhotonNetwork.PlayerList.Length;
        stageTimer = data[index].limitTime;

        // �������� ������ ����ȭ
        Hashtable stageProps = new Hashtable
        {
            { StageIndexKey, index },
            { PlayerCountKey, player_count } // �÷��̾� ���� ���� ����ȭ
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(stageProps);
    }

    

    // �������� ������ ����ȭ �Լ�
    void SyncStageData()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(StageIndexKey))
        {
            index = (int)PhotonNetwork.CurrentRoom.CustomProperties[StageIndexKey];
        }

        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PlayerCountKey))
        {
            player_count = (int)PhotonNetwork.CurrentRoom.CustomProperties[PlayerCountKey];
        }

/*        // �ش� �������� Ÿ�̸ӵ� ����ȭ�Ͽ� Ŭ���̾�Ʈ���� �ݿ�
        stageTimer = data[index].limitTime;
        isStageActive = true;*/
    }

    // �÷��̾� �� ������Ʈ �Լ� (Master Client������ ó��)
    public void UpdatePlayerCount()
    {
        player_count = PhotonNetwork.CurrentRoom.PlayerCount;

        // �� �Ӽ��� �÷��̾� ���� ������Ʈ
        Hashtable playerProps = new Hashtable
        {
            { PlayerCountKey, player_count }
        };
        PhotonNetwork.CurrentRoom.SetCustomProperties(playerProps);
    }

    // �� �Ӽ� ������ �����ϴ� ����� ���� ����
    public void OnRoomPropertiesUpdate(Hashtable changedProps)
    {
        // �ٸ� Ŭ���̾�Ʈ���� �������� ������ ����Ǿ��� �� ����ȭ
        if (changedProps.ContainsKey(StageIndexKey))
        {
            index = (int)changedProps[StageIndexKey];
            Debug.Log("���������� ����Ǿ����ϴ�. ���� ��������: " + index);
        }

        // �÷��̾� �� ������ ����Ǿ��� �� ����ȭ
        if (changedProps.ContainsKey(PlayerCountKey))
        {
            player_count = (int)changedProps[PlayerCountKey];
            Debug.Log("�÷��̾� ���� ����Ǿ����ϴ�. ���� �÷��̾� ��: " + player_count);
        }
    }

    // Update �޼��忡�� �� �Ӽ� ������Ʈ Ȯ��
    void LateUpdate()
    {
        // �� �Ӽ� ���� ���θ� �ֱ������� Ȯ��
        Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
        OnRoomPropertiesUpdate(props);
    }
}