using Photon.Pun;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PhotonView pv;
    [SerializeField] float speed = 2f;
    [SerializeField] float rotationSpeed = 100f;
    private Transform body;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = transform.GetChild(0);
        pv = GetComponent<PhotonView>();
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
            else
            {

            }
        }
    }
}
