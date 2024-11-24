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
                // ĳ���� �̵�
                transform.Translate(v.normalized * speed * Time.deltaTime, Space.World);
                Quaternion targetRotation = Quaternion.LookRotation(v.normalized, Vector3.up);
                body.rotation = Quaternion.Slerp(body.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                //body.Rotate(y * rotationSpeed * Time.deltaTime, 0, 0);

            }
            if (Input.GetMouseButtonDown(0)) // ���� Ŭ�� ����
            {
                Debug.Log("���� ��");
                ObjectBasedRaycast();
            }
        }
        else
        {
                
        }
    }
    void ObjectBasedRaycast()
    {
        // ������Ʈ�� ��ġ�� ���� ���� �������� Ray ����
        Ray ray = new Ray(body.position, transform.forward);
        RaycastHit hit;

        // Ray�� �߻��ϰ�, �ش� �Ÿ� ���� �浹�� �߻��ߴ��� Ȯ��
        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            // ���� ������Ʈ���� PhotonView ������Ʈ�� ã��
            PhotonView targetPhotonView = hit.collider.GetComponent<PhotonView>();

            if (targetPhotonView != null)
            {
                // Health ������Ʈ�� ã��
                Controller targetHealth = hit.collider.GetComponent<Controller>();

                if (targetHealth != null)
                {
                    // MasterClient�� ��� ���� ó��
                    if (PhotonNetwork.IsMasterClient)
                    {
                        Debug.Log("������ ó��");
                        targetPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered);
                    }
                    else
                    {
                        Debug.Log("�ƴ� ó��");
                        targetPhotonView.RPC("RequestDamage", RpcTarget.MasterClient);
                    }
                }
            }
        }
    }

    // MasterClient���� ������ ó���� ��û�ϴ� RPC
    [PunRPC]
    public void RequestDamage(int damage, PhotonMessageInfo info)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log($"{info.Sender.NickName}�� {gameObject.name}���� {damage} �������� ��û�߽��ϴ�.");
            pv.RPC("TakeDamage", RpcTarget.AllBuffered, damage);
        }
    }
}
