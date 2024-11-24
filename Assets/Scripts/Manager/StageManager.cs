//using Photon.Pun;
//using UnityEngine;
//using ExitGames.Client.Photon;
//using Photon.Realtime;
//using System.Collections;

//public class StageManager : MonoBehaviour
//{
//    [System.Serializable]
//    public struct Stage_Data
//    {
//        public int maxCount; // �ִ� �ο�(���� �� ����)
//        public float limitTime; // �������� ���� �ð�
//    }
//    [SerializeField] int index = 0; // ���� ��������
//    [SerializeField] Stage_Data[] data;
//    private SpawnManager spawnManager;
//    private int player_count;
//    private float stageTimer;
//    private bool isStageActive = false;
//    Timer _timer;

//    //private const string StageIndexKey = "StageIndex";
//    //private const string PlayerCountKey = "PlayerCount"; // ��Ƴ��� �÷��̾� ��

//    private void Awake()
//    {
//        spawnManager = GetComponent<SpawnManager>();
//    }

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
//        UpdatePlayerCount();
//        if (PhotonNetwork.IsMasterClient)
//        {
//            _timer = new Timer();
//        }
//        else
//        {
//            SyncStageData();
//        }

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if(PhotonNetwork.IsMasterClient)
//        {
//            if(Input.GetKeyDown(KeyCode.Space) && !isStageActive)
//            {
//                Stage_Start();
//            }

//            if(isStageActive)
//            {
//               // if()
//            }
//        }
//        /*if(PhotonNetwork.IsMasterClient && isStageActive)
//        {

//        }*/
//    }

//    public void Stage_Start()
//    {
//        //StartCoroutine(); ���� ������ 3�� ��Ÿ��
//        //InitStage(index++);
//        _timer.ResetTimer(5, OnTimer, OnTimerComplate);
//    }

//    public void OnTimer(int time)
//    {
//        Debug.Log($"  {time}  ");
//    }

//    public void OnTimerComplate()
//    {
//        InitStage(index);
//    }


//    public void InitStage(int index)
//    {
//        this.index = index;
//        player_count = PhotonNetwork.PlayerList.Length;
//        stageTimer = data[index].limitTime;
//        int mob_count = data[index].maxCount - player_count;

//        // �������� ������ ����ȭ
//        ExitGames.Client.Photon.Hashtable stageProps = new ExitGames.Client.Photon.Hashtable
//        {
//            { StageIndexKey, index },
//            { PlayerCountKey, player_count } // �÷��̾� ���� ���� ����ȭ
//        };
//        isStageActive = true;
//        PhotonNetwork.CurrentRoom.SetCustomProperties(stageProps);
//        spawnManager.GameReset(index);

//    }

    

//    // �������� ������ ����ȭ �Լ�
//    void SyncStageData()
//    {
//        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(StageIndexKey))
//        {
//            index = (int)PhotonNetwork.CurrentRoom.CustomProperties[StageIndexKey];
//        }

//        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(PlayerCountKey))
//        {
//            player_count = (int)PhotonNetwork.CurrentRoom.CustomProperties[PlayerCountKey];
//        }

///*        // �ش� �������� Ÿ�̸ӵ� ����ȭ�Ͽ� Ŭ���̾�Ʈ���� �ݿ�
//        stageTimer = data[index].limitTime;
//        isStageActive = true;*/
//    }

//    // �÷��̾� �� ������Ʈ �Լ� (Master Client������ ó��)
//    public void UpdatePlayerCount()
//    {
//        player_count = PhotonNetwork.CurrentRoom.PlayerCount;

//        // �� �Ӽ��� �÷��̾� ���� ������Ʈ
//        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
//        {
//            { PlayerCountKey, player_count }
//        };
//        PhotonNetwork.CurrentRoom.SetCustomProperties(playerProps);
//    }

//    // �� �Ӽ� ������ �����ϴ� ����� ���� ����
//    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changedProps)
//    {
//        // �ٸ� Ŭ���̾�Ʈ���� �������� ������ ����Ǿ��� �� ����ȭ
//        if (changedProps.ContainsKey(StageIndexKey))
//        {
//            index = (int)changedProps[StageIndexKey];
//            Debug.Log("���������� ����Ǿ����ϴ�. ���� ��������: " + index);
//        }

//        // �÷��̾� �� ������ ����Ǿ��� �� ����ȭ
//        if (changedProps.ContainsKey(PlayerCountKey))
//        {
//            player_count = (int)changedProps[PlayerCountKey];
//            Debug.Log("�÷��̾� ���� ����Ǿ����ϴ�. ���� �÷��̾� ��: " + player_count);
//        }
//    }

//    // Update �޼��忡�� �� �Ӽ� ������Ʈ Ȯ��
//    void LateUpdate()
//    {
//        // �� �Ӽ� ���� ���θ� �ֱ������� Ȯ��
//        //ExitGames.Client.Photon.Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
//        //OnRoomPropertiesUpdate(props);
//    }
//}