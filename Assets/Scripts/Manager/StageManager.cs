using UnityEngine;


public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public struct Stage_Data
    {
        public int maxCount; // �ִ� �ο�(���� �� ����)
        public float limitTime; // �������� ���� �ð�
    }
    [SerializeField] int index = 0; // ���� ��������
    [SerializeField] Stage_Data[] data;
    private int player_count;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}