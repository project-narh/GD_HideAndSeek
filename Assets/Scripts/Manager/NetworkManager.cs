using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    public Text StatusText; // 사용자의 접속상태
    public InputField roomName; // 룸넘버
    public InputField NickNameInput; // 사용자가 닉네임 입력
    public GameObject Cube; //플레이어 생성 위치
    public string gameVersion = "1.0"; // 게임 버전을 위한 변수

    void Awake()
    {
        if(Instance == null)
        {
            PhotonNetwork.AutomaticallySyncScene = true; // 방에 참여한 클라이언트 동일한 씬 로드
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion; // 게임 버전 설정
        PhotonNetwork.ConnectUsingSettings(); // 포톤 서버에 설정된 네트워크 정보 사용하여 연결 시도
    }

    void Update()
    {
        switch(PhotonNetwork.NetworkClientState.ToString())
        {
            case "ConnectedToMasterServer":
                StatusText.text = "Stats : 서버 연결 완료";
                break;

            case "JoinedLobby":
                StatusText.text = "Stats : 로비 접속";
                break;

            default:
                StatusText.text = "Stats : " + PhotonNetwork.NetworkClientState.ToString();
                break;
        }
    }

    void Connect() => PhotonNetwork.ConnectUsingSettings(); // 포톤 서버에 연결

    public override void OnConnectedToMaster()
    {
        //PhotonNetwork.JoinRandomRoom(); // 랜덤방에 참여 시도 만일 방이 없다면 OnJoinRandomFailed 콜백이 호출될까봐
        print("서버 접속 성공");
        PhotonNetwork.JoinLobby();
        //PhotonNetwork.LocalPlayer.NickName = this.NickNameInput.text;
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.Instantiate("_Player", Cube.transform.position, Quaternion.identity);
        // 플레이어3이라는 프리팹을 방에 있는 모든 플레이어에게 동기화 하여 생성
        print("방 참가 완료");
        SceneManager.LoadScene("Game");
    }

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() => Debug.Log("로비 접속");

    public void connect() => PhotonNetwork.ConnectUsingSettings();

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("연결 끊김");

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("서버 접속 실패");
        this.CreateRoom(); // 방 참여 실패 했을때 호출하여 새로운 방 생성
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 4 }); // 최대 플레이어 4명 참여  방 이름 roomNumber
    }

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomName.text);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnCreatedRoom()
    {
        print("방 만들기 완료");
    }

    [ContextMenu("정보")]
    void info()
    {

    }
}
