using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager Instance;

    public Text StatusText; // ������� ���ӻ���
    public InputField roomName; // ��ѹ�
    public InputField NickNameInput; // ����ڰ� �г��� �Է�
    public GameObject Cube; //�÷��̾� ���� ��ġ
    public string gameVersion = "1.0"; // ���� ������ ���� ����

    void Awake()
    {
        if(Instance == null)
        {
            PhotonNetwork.AutomaticallySyncScene = true; // �濡 ������ Ŭ���̾�Ʈ ������ �� �ε�
            Instance = this;
            DontDestroyOnLoad(gameObject);
            PhotonNetwork.SendRate = 60;        // �ʴ� 60ȸ ��Ŷ ����
            PhotonNetwork.SerializationRate = 30;
            PhotonNetwork.LogLevel = PunLogLevel.Full;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion; // ���� ���� ����
        PhotonNetwork.ConnectUsingSettings(); // ���� ������ ������ ��Ʈ��ũ ���� ����Ͽ� ���� �õ�
    }

    void Update()
    {
        //Debug.Log($"Ping: {PhotonNetwork.GetPing()} ms");
        //Debug.Log($"Traffic Stats: {PhotonNetwork.NetworkStatisticsToString()}");
        switch (PhotonNetwork.NetworkClientState.ToString())
        {
            case "ConnectedToMasterServer":
                StatusText.text = "Stats : ���� ���� �Ϸ�";
                break;

            case "JoinedLobby":
                StatusText.text = "Stats : �κ� ����";
                break;

            default:
                StatusText.text = "Stats : " + PhotonNetwork.NetworkClientState.ToString();
                break;
        }
    }

    void Connect() => PhotonNetwork.ConnectUsingSettings(); // ���� ������ ����

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom(); // �����濡 ���� �õ� ���� ���� ���ٸ� OnJoinRandomFailed �ݹ��� ȣ��ɱ��
        print("���� ���� ����");
        //PhotonNetwork.JoinLobby();
        //PhotonNetwork.LocalPlayer.NickName = this.NickNameInput.text;
    }

    public override void OnJoinedRoom()
    {
        //PhotonNetwork.Instantiate("_Player", Cube.transform.position, Quaternion.identity);
        // �÷��̾�3�̶�� �������� �濡 �ִ� ��� �÷��̾�� ����ȭ �Ͽ� ����
        print("�� ���� �Ϸ�");
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("���� ������ ������ Ŭ���̾�Ʈ�Դϴ�.");
        }
        else
        {
            Debug.Log("�濡 ������ Ŭ���̾�Ʈ�Դϴ�.");
        }
        SceneManager.LoadScene("Gane");
    }

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby()
    {
        Debug.Log("�κ� ����");
    }

    public void connect() => PhotonNetwork.ConnectUsingSettings();

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("���� ����");

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� ���� ����");
        this.CreateRoom(); // �� ���� ���� ������ ȣ���Ͽ� ���ο� �� ����
    }

    public void CreateRoom()
    {
        //PhotonNetwork.CreateRoom(roomName.text, new RoomOptions { MaxPlayers = 4 }); // �ִ� �÷��̾� 4�� ����  �� �̸� roomNumber
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 }); // �ִ� �÷��̾� 4�� ����  �� �̸� roomNumber
    }

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomName.text);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnCreatedRoom()
    {
        print("�� ����� �Ϸ�");
    }

    [ContextMenu("����")]
    void info()
    {

    }
}
