using Photon.Pun;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public struct Stage
{
    public int stageIndex;
    public int spawnIndex;
}

public class GameSystemManager : MonoBehaviour
{
    [Header("스테이지 데이터")]
    SpawnManager spawnManager;
    [SerializeField] int _stage = -1; // 0 : 대기
    //[SerializeField] Stage[] stageData;
    [SerializeField] int[] stageData;
    Timer _timer;
    bool _isGameStart = false;

    [Header("플레이 데이터")]
    [SerializeField]PhotonView[] playerData;
    [SerializeField] bool isTurn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _timer = gameObject.AddComponent<Timer>(); ;
            spawnManager = gameObject.GetComponent<SpawnManager>();
            _isGameStart = false;
        }
    }

    public bool CheckTurn()
    {
        return isTurn;
    }

    public bool CheckStart()
    {
        return _isGameStart;
    }

    public void GameStart()
    {
        _timer.ResetTimer(5, OnTimer, OnTimerComplate);
        playerData = Get_PlayerData();
    }

    public void OnTimer(int time)
    {
        Debug.Log($"  {time}  ");
    }

    public void OnTimerComplate()
    {
        _isGameStart=true;
        spawnManager.GameReset(stageData[_stage++]);
        _timer.ResetTimer(5, OnGameTimer, OnGameTimerComplate);
    }

    public void OnGameTimer(int index)
    {
        Debug.Log($"  {index}  ");
        Debug.Log(isTurn);
    }

    public void OnGameTimerComplate()
    {
        Debug.Log("실행");
        isTurn = isTurn ? false : true;
        _timer.ResetTimer(5, OnGameTimer, OnGameTimerComplate);
    }



    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !_isGameStart)
            {
                Debug.Log("눌림");
                GameStart();
            }

            if (_isGameStart)
            {
                if(playerData.Length <= 0) // 플레이어 없음
                {
                    Debug.Log("게임 종료");
                }
            }
        }
    }

    private PhotonView[] Get_PlayerData()
    {
        PhotonView[] allPhotonViews = FindObjectsOfType<PhotonView>();
        return allPhotonViews;
    }
}
