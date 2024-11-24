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
//        public int maxCount; // 최대 인원(더미 몹 포함)
//        public float limitTime; // 스테이지 제한 시간
//    }
//    [SerializeField] int index = 0; // 현재 스테이지
//    [SerializeField] Stage_Data[] data;
//    private SpawnManager spawnManager;
//    private int player_count;
//    private float stageTimer;
//    private bool isStageActive = false;
//    Timer _timer;

//    //private const string StageIndexKey = "StageIndex";
//    //private const string PlayerCountKey = "PlayerCount"; // 살아남은 플레이어 수

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
//        //StartCoroutine(); 게임 시작전 3초 쿨타임
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

//        // 스테이지 데이터 동기화
//        ExitGames.Client.Photon.Hashtable stageProps = new ExitGames.Client.Photon.Hashtable
//        {
//            { StageIndexKey, index },
//            { PlayerCountKey, player_count } // 플레이어 수도 같이 동기화
//        };
//        isStageActive = true;
//        PhotonNetwork.CurrentRoom.SetCustomProperties(stageProps);
//        spawnManager.GameReset(index);

//    }

    

//    // 스테이지 데이터 동기화 함수
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

///*        // 해당 스테이지 타이머도 동기화하여 클라이언트에서 반영
//        stageTimer = data[index].limitTime;
//        isStageActive = true;*/
//    }

//    // 플레이어 수 업데이트 함수 (Master Client에서만 처리)
//    public void UpdatePlayerCount()
//    {
//        player_count = PhotonNetwork.CurrentRoom.PlayerCount;

//        // 방 속성에 플레이어 수를 업데이트
//        ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
//        {
//            { PlayerCountKey, player_count }
//        };
//        PhotonNetwork.CurrentRoom.SetCustomProperties(playerProps);
//    }

//    // 방 속성 변경을 수신하는 방법을 직접 구현
//    public void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changedProps)
//    {
//        // 다른 클라이언트에서 스테이지 정보가 변경되었을 때 동기화
//        if (changedProps.ContainsKey(StageIndexKey))
//        {
//            index = (int)changedProps[StageIndexKey];
//            Debug.Log("스테이지가 변경되었습니다. 현재 스테이지: " + index);
//        }

//        // 플레이어 수 정보가 변경되었을 때 동기화
//        if (changedProps.ContainsKey(PlayerCountKey))
//        {
//            player_count = (int)changedProps[PlayerCountKey];
//            Debug.Log("플레이어 수가 변경되었습니다. 현재 플레이어 수: " + player_count);
//        }
//    }

//    // Update 메서드에서 방 속성 업데이트 확인
//    void LateUpdate()
//    {
//        // 방 속성 변경 여부를 주기적으로 확인
//        //ExitGames.Client.Photon.Hashtable props = PhotonNetwork.CurrentRoom.CustomProperties;
//        //OnRoomPropertiesUpdate(props);
//    }
//}