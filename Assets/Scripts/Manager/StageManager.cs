using UnityEngine;


public class StageManager : MonoBehaviour
{
    [System.Serializable]
    public struct Stage_Data
    {
        public int maxCount; // 최대 인원(더미 몹 포함)
        public float limitTime; // 스테이지 제한 시간
    }
    [SerializeField] int index = 0; // 현재 스테이지
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