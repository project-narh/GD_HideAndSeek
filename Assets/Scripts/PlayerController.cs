using Photon.Pun;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PhotonView pv;
    [SerializeField] float speed = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            transform.Translate(new Vector3(x,0,y) * speed * Time.deltaTime);
        }
    }
}
