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
        Debug.Log("플레이어 맞음");
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

                // 회전 값 동기화
                pv.RPC("SyncChildRotation", RpcTarget.Others, targetRotation);
            }
            if (Input.GetMouseButtonDown(0)) // 왼쪽 클릭 감지
            {
                Debug.Log("레이 쏨");
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
        // 오브젝트의 위치와 전방 방향 기준으로 Ray 생성
        Ray ray = new Ray(body.position, body.transform.forward);
        RaycastHit hit;

        // Ray를 발사하고, 해당 거리 내에 충돌이 발생했는지 확인
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // 맞은 오브젝트에서 PhotonView 컴포넌트를 찾음
            PhotonView targetPhotonView = hit.collider.GetComponentInParent<PhotonView>();

            if (targetPhotonView != null)
            {
                // Health 컴포넌트를 찾음
                Controller targetHealth = hit.collider.GetComponentInParent<Controller>();
                PlayerController playerHealth = hit.collider.GetComponentInParent<PlayerController>();

                if (targetHealth != null || playerHealth != null)
                {
                    // MasterClient일 경우 직접 처리
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Debug.Log("마스터 처리");
                        targetPhotonView.RPC("TakeDamageDDD", RpcTarget.All);
                    }
                    else
                    {
                        Debug.Log("아닌 처리");
                        targetPhotonView.RPC("RequestDamage", RpcTarget.MasterClient);
                    }
                }
                else
                {
                    Debug.Log("컨트롤러 못찾음");
                }
            }
            else
            {
                Debug.Log("포톤 못찾음");
            }
        }
        else
        {
            Debug.Log("레이 맞은거 없음"); 
        }
    }

    // MasterClient에게 데미지 처리를 요청하는 RPC
    [PunRPC]
    public void RequestDamage()
    {
        Debug.Log($"{gameObject.name}에게 1 데미지를 요청했습니다.");
        pv.RPC("TakeDamageDDD", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateObjectPosition(Vector3 newPosition)
    {
        // PhotonView의 위치를 변경
        transform.position = newPosition;
        Debug.Log("위치이동");
        // 디버그 로그 (추적용)
        Debug.Log($"오브젝트 {gameObject.name}의 위치가 {newPosition}으로 변경되었습니다.");
    }
}
