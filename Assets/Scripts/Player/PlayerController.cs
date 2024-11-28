using Photon.Pun;
using UnityEngine;

public class PlayerController : Controller
{
    public int ID = 0;
    private Camera cam;
    [SerializeField] float speed = 2f;
    [SerializeField] float rotationSpeed = 100f;
    private Transform body;
    public float rayDistance = 10f;
    public LayerMask layerMask;
    private Controller controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*        body = transform.GetChild(0);
                pv = GetComponent<PhotonView>();
                cam = GetComponentInChildren<Camera>();
                controller = GetComponentInChildren<Controller>();*/
        /*
                if (pv.IsMine)
                {
                    cam.gameObject.SetActive(true);
                    pv.RPC("SyncObjectState", RpcTarget.All, transform.position, transform.rotation);
                }
                else
                {
                    cam.gameObject.SetActive(false);
                }
                DontDestroyOnLoad(gameObject);*/
        Init();
    }

    protected override void Dead() 
    { 
        body.gameObject.SetActive(false);
    }

    public void Alive()
    {
        body.gameObject.SetActive(true);
    }

    public override void Init()
    {
        Debug.Log("�÷��̾� ����");
        isPlayer = true;
        body = transform.GetChild(0);
        pv = GetComponent<PhotonView>();
        cam = GetComponentInChildren<Camera>();
        controller = GetComponentInChildren<Controller>();
        ID = pv.ViewID;
        if (pv.IsMine)
        {
            cam.gameObject.SetActive(true);
            pv.RPC("SyncObjectState", RpcTarget.All, transform.position, transform.rotation);
        }
        else
        {
            cam.gameObject.SetActive(false);
        }
        DontDestroyOnLoad(gameObject);
    }

    [PunRPC]
    public void SyncObjectState(Vector3 position, Quaternion rotation)
    {
        transform.position = position;
        transform.rotation = rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector3 v = new Vector3(x, 0, y);
            transform.Translate(v.normalized * speed * Time.deltaTime, Space.World);
            if(x != 0 || y != 0 )
            {
                Quaternion targetRotation = Quaternion.LookRotation(v.normalized);
                body.rotation = targetRotation;

                // ȸ�� �� ����ȭ
                pv.RPC("SyncChildRotation", RpcTarget.Others, targetRotation);
            }
            if (Input.GetMouseButtonDown(0)) // ���� Ŭ�� ����
            {
                Debug.Log("���� ��");
                ObjectBasedRaycast();
            }
        }
    }
    [PunRPC]
    void SyncChildRotation(Quaternion rotation)
    {
        body.rotation = rotation;
    }
    void ObjectBasedRaycast()
    {
        // ������Ʈ�� ��ġ�� ���� ���� �������� Ray ����
        Ray ray = new Ray(body.position, body.transform.forward);
        RaycastHit hit;

        // Ray�� �߻��ϰ�, �ش� �Ÿ� ���� �浹�� �߻��ߴ��� Ȯ��
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // ���� ������Ʈ���� PhotonView ������Ʈ�� ã��
            PhotonView targetPhotonView = hit.collider.GetComponentInParent<PhotonView>();

            if (targetPhotonView != null)
            {
                // Health ������Ʈ�� ã��
                Controller targetHealth = hit.collider.GetComponentInParent<Controller>();
                PlayerController playerHealth = hit.collider.GetComponentInParent<PlayerController>();

                if (targetHealth != null || playerHealth != null)
                {
                    // MasterClient�� ��� ���� ó��
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Debug.Log("������ ó��");
                        targetPhotonView.RPC("TakeDamageDDD", RpcTarget.All);
                    }
                    else
                    {
                        Debug.Log("�ƴ� ó��");
                        targetPhotonView.RPC("RequestDamage", RpcTarget.MasterClient);
                    }
                }
                else
                {
                    Debug.Log("��Ʈ�ѷ� ��ã��");
                }
            }
            else
            {
                Debug.Log("���� ��ã��");
            }
        }
        else
        {
            Debug.Log("���� ������ ����"); 
        }
    }

    // MasterClient���� ������ ó���� ��û�ϴ� RPC
    [PunRPC]
    public void RequestDamage()
    {
        Debug.Log($"{gameObject.name}���� 1 �������� ��û�߽��ϴ�.");
        pv.RPC("TakeDamageDDD", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateObjectPosition(Vector3 newPosition)
    {
        // PhotonView�� ��ġ�� ����
        transform.position = newPosition;
        Debug.Log("��ġ�̵�");
        // ����� �α� (������)
        Debug.Log($"������Ʈ {gameObject.name}�� ��ġ�� {newPosition}���� ����Ǿ����ϴ�.");
    }
}
