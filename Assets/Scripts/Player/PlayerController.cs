using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PhotonView pv;
    private Camera cam;
    [SerializeField] float speed = 2f;
    [SerializeField] float rotationSpeed = 100f;
    private Transform body;
    public float rayDistance = 10f;
    public LayerMask layerMask;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = transform.GetChild(0);
        pv = GetComponent<PhotonView>();
        cam = GetComponentInChildren<Camera>();

        if(pv.IsMine)
        {
            cam.gameObject.SetActive(true);
        }
        else
        {
            cam.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            Vector3 v = new Vector3(x, 0, y);

            if(v.magnitude > 0.1f)
            {
                // 캐릭터 이동
                transform.Translate(v.normalized * speed * Time.deltaTime, Space.World);
                Quaternion targetRotation = Quaternion.LookRotation(v.normalized, Vector3.up);
                body.rotation = Quaternion.Slerp(body.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                //body.Rotate(y * rotationSpeed * Time.deltaTime, 0, 0);

            }
            if (Input.GetMouseButtonDown(0)) // 왼쪽 클릭 감지
            {
                Debug.Log("레이 쏨");
                ObjectBasedRaycast();
            }
        }
        else
        {
                
        }
    }
    void ObjectBasedRaycast()
    {
        // 오브젝트의 위치와 전방 방향 기준으로 Ray 생성
        Ray ray = new Ray(body.position, transform.forward);
        RaycastHit hit;

        // Ray를 발사하고, 해당 거리 내에 충돌이 발생했는지 확인
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // 맞은 오브젝트에서 PhotonView 컴포넌트를 찾음
            PhotonView targetPhotonView = hit.collider.GetComponent<PhotonView>();

            if (targetPhotonView != null)
            {
                // Health 컴포넌트를 찾음
                Controller targetHealth = hit.collider.GetComponent<Controller>();

                if (targetHealth != null)
                {
                    // MasterClient일 경우 직접 처리
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Debug.Log("마스터 처리");
                        targetPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered);
                    }
                    else
                    {
                        Debug.Log("아닌 처리");
                        targetPhotonView.RPC("RequestDamage", RpcTarget.MasterClient);
                    }
                }
            }
        }
    }

    // MasterClient에게 데미지 처리를 요청하는 RPC
    [PunRPC]
    public void RequestDamage(int damage, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"{info.Sender.NickName}가 {gameObject.name}에게 {damage} 데미지를 요청했습니다.");
            pv.RPC("TakeDamage", RpcTarget.AllBuffered, damage);
        }
    }
}
