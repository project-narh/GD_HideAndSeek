using NUnit.Framework;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEditor.Experimental;
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
    [SerializeField] bool _isGameStart = false;
    [SerializeField] Transform _light;

    [Header("플레이 데이터")]
    [SerializeField] List<PlayerController> playerData;
    [SerializeField] List<PlayerController> gamePlayerData;
    [SerializeField] bool isTurn = false;
    PhotonView _view;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _view = GetComponent<PhotonView>();
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
        _isGameStart = false;
        _timer.ResetTimer(5, OnTimer, OnTimerComplate);
        playerData = Get_PlayerData();
        foreach(PlayerController con in playerData)
        {
            con.Alive();
        }
        _view.RPC("SysnList", RpcTarget.All, gamePlayerData);
    }

    public void OnTimer(int time)
    {
    }

    public void OnTimerComplate()
    {
        if(_stage >= stageData.Length )
        {
            Debug.Log("더 이상의 스테이지가 없습니다.");
            _isGameStart = false;
            return;
        }
        else
        {
            _isGameStart = true;
            spawnManager.GameReset(stageData[_stage++]);
            MapSetting(_stage);
            UpdateSharedVariable(isTurn, _isGameStart);
            _timer.ResetTimer(5, OnGameTimer, OnGameTimerComplate);
        }
    }

    public void OnGameTimer(int index)
    {

    }

    public void OnGameTimerComplate()
    {
        isTurn = isTurn ? false : true;
        UpdateSharedVariable(isTurn, _isGameStart);
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
                if(gamePlayerData.Count <= 1) // 플레이어 없음
                {
                    Debug.Log("다음 스테이지 시작");
                    GameStart();
                }
                else
                {
                }
            }
        }
    }

    public void UpdateSharedVariable(bool isTurn_, bool gameStart)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            isTurn = isTurn_;
            _isGameStart = gameStart;
            Debug.Log("상태 실행");
            _view.RPC("SyncSharedVariable", RpcTarget.Others, isTurn_, gameStart);
        }
    }

    [PunRPC]
    public void SyncSharedVariable(bool isTurn_, bool gameStart)
    {
        Debug.Log("시리시리");
        isTurn = isTurn_;
        _isGameStart = gameStart;
    }

    public void SyncGamePlayerData()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            // ID 리스트 생성
            List<int> playerIds = gamePlayerData.Select(player => player.ID).ToList();

            // 모든 클라이언트로 RPC 호출
            _view.RPC("SyncGamePlayerDataRPC", RpcTarget.All, playerIds.ToArray());
        }
    }

    [PunRPC]
    public void SyncGamePlayerDataRPC(int[] playerIds)
    {
        gamePlayerData = playerIds
            .Select(id => FindID(id))  // ID로 PlayerController를 복원
            .Where(player => player != null)  // 유효한 플레이어만 유지
            .ToList();
    }

    public PlayerController FindID(int id)
    {
        foreach(PlayerController c in gamePlayerData)
        {
            if (c.ID == id) return c;
        }
        return null;
    }

    public void KillPlayer(int ID)
    {
        PlayerController c = FindID(ID);
        if (c != null)
        {
            if (gamePlayerData.Contains(c))
            {
                gamePlayerData.Remove(c);
                SyncGamePlayerData();
            }
        }
        else
        {
            Debug.Log("그런 플레이어 없음");
        }
    }

    private List<PlayerController> Get_PlayerData()
    {
        PlayerController[] allPhotonViews = FindObjectsOfType<PlayerController>();
        List<PlayerController> list = new List< PlayerController>();

        foreach(PlayerController con in allPhotonViews)
        {
            list.Add(con);
        }

        return list;
    }

    private void MapSetting(int index)
    {
        switch(index)
        {
            case 1:
                StartCoroutine(SetFog(0.007f, 17.8f, 4));
                break;
            case 2:
                StartCoroutine(SetFog(0.02f, 186.94f, 4));
                break;
            case 3:
                StartCoroutine(SetFog(0.08f, 296.125f, 4));
                break;

        }
    }

    IEnumerator SetFog(float fog, float sun, float duration)
    {
        float elapsedTime = 0f;

        float initialFogDensity = UnityEngine.RenderSettings.fogDensity;
        float initialSunX = _light.rotation.x;

        while (elapsedTime < duration)
        {
            // 시간에 따른 비율 계산
            float t = elapsedTime / duration;

            // Fog Density 보간
            UnityEngine.RenderSettings.fogDensity = Mathf.Lerp(initialFogDensity, fog, t);

            // Transform X 보간
            float currentSunRotationY = Mathf.Lerp(initialSunX, sun, t);
            Quaternion newRotation = Quaternion.Euler(new Vector3(currentSunRotationY, _light.rotation.eulerAngles.y, _light.rotation.eulerAngles.z));
            _light.rotation = newRotation;

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            // 한 프레임 대기
            yield return null;
        }

    }
}
