using Photon.Pun;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public float rotationSpeed = 2f; // 보간 속도
    private Quaternion targetRotation; // 목표 회전값
    public float target = 0f;
    public float temptarget = -180f;
    [SerializeField] GameSystemManager gameSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameSystem.CheckStart())
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (gameSystem.CheckTurn())
                    target = 0f;
                else
                    target = 180f;
                if (target != temptarget)
                {
                    temptarget = target;
                    targetRotation = Quaternion.Euler(transform.eulerAngles.x, target, transform.eulerAngles.z);
                }

                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}
