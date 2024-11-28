using Unity.VisualScripting;
using UnityEngine;

public class TamiNPC : MonoBehaviour
{
    
    public float speed = 2f;  // 이동 속도
    public float rotationSpeed = 128.9f;  // 회전 속도
    public Vector2 actionIntervalRange = new Vector2(2f, 5f);  // 행동 간격 최소/최대값

    private Vector3 destination;
    private float timeSinceLastAction = 0f;
    private float actionInterval;
    GameSystemManager manager;

    private void Start()
    {
        manager = GameObject.FindWithTag("Manager").GetComponent<GameSystemManager>();
        SetRandomDestination();
        SetRandomActionInterval();  // 첫 행동 간격 설정
    }

    private void Update()
    {
        if (manager.CheckStart() && !manager.CheckTurn())
        {
            timeSinceLastAction += Time.deltaTime;

            if (timeSinceLastAction >= actionInterval)
            {
                SetRandomDestination();
                SetRandomActionInterval();
            }

            MoveTowardsDestination();
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomDirection = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)).normalized;
        destination = transform.position + randomDirection * Random.Range(5f, 10f);
    }

    void MoveTowardsDestination()
    {
        Vector3 direction = destination - transform.position;
        if (direction.magnitude > 0.1f)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
        }
    }

    void SetRandomActionInterval()
    {
        actionInterval = Random.Range(actionIntervalRange.x, actionIntervalRange.y);
        timeSinceLastAction = 0f;  // 타이머 초기화
    }

}
