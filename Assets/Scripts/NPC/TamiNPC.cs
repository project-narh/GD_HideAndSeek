using Unity.VisualScripting;
using UnityEngine;

public class TamiNPC : MonoBehaviour
{
    
    public float speed = 2f;  // �̵� �ӵ�
    public float rotationSpeed = 128.9f;  // ȸ�� �ӵ�
    public Vector2 actionIntervalRange = new Vector2(2f, 5f);  // �ൿ ���� �ּ�/�ִ밪

    private Vector3 destination;
    private float timeSinceLastAction = 0f;
    private float actionInterval;
    GameSystemManager manager;

    private void Start()
    {
        manager = GameObject.FindWithTag("Manager").GetComponent<GameSystemManager>();
        SetRandomDestination();
        SetRandomActionInterval();  // ù �ൿ ���� ����
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
        timeSinceLastAction = 0f;  // Ÿ�̸� �ʱ�ȭ
    }

}
