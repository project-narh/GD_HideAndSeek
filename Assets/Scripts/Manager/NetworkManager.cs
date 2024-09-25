using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Text StatusText; // ������� ���ӻ���
    public Text roomNumber; // ��ѹ�
    public InputField NickNameInput; // ����ڰ� �г��� �Է�
    public GameObject Cube; //�÷��̾� ���� ��ġ
    public string gameVersion = "1.0"; // ���� ������ ���� ����

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // �濡 ������ Ŭ���̾�Ʈ ������ �� �ε�
    }

    void Start()
    {
        PhotonNetwork.GameVersion = this.gameVersion; // ���� ���� ����
        PhotonNetwork.ConnectUsingSettings(); // ���� ������ ������ ��Ʈ��ũ ���� ����Ͽ� ���� �õ�
    }

    void Update()
    {

        StatusText.text = "Stats : " + PhotonNetwork.NetworkClientState.ToString();
    }

    void Connect() => PhotonNetwork.ConnectUsingSettings(); // ���� ������ ����

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom(); // �����濡 ���� �õ� ���� ���� ���ٸ� OnJoinRandomFailed �ݹ��� ȣ��ɱ��
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("_Player", Cube.transform.position, Quaternion.identity);
        // �÷��̾�3�̶�� �������� �濡 �ִ� ��� �÷��̾�� ����ȭ �Ͽ� ����
        print("�� ���� �Ϸ�");
    }

    public void JoinLobby() => PhotonNetwork.JoinLobby();
    public override void OnJoinedLobby() => Debug.Log("�κ� ����"); 

    public void Disconnect() => PhotonNetwork.Disconnect();

    public override void OnDisconnected(DisconnectCause cause) => print("���� ����");

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("���� ���� ����");
        this.CreateRoom(); // �� ���� ���� ������ ȣ���Ͽ� ���ο� �� ����
    }

    void CreateRoom()
    {
        Debug.Log("�� ���� �õ�");
        PhotonNetwork.CreateRoom(roomNumber.text, new RoomOptions { MaxPlayers = 4 }); // �ִ� �÷��̾� 4�� ����  �� �̸� roomNumber
    }

    public void JoinRoom() => PhotonNetwork.JoinRoom(roomNumber.text);

    public void JoinRandomRoom() => PhotonNetwork.JoinRandomRoom();

    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    public override void OnCreatedRoom() => print("�� ����� �Ϸ�");

}
