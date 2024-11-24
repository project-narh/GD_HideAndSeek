using System;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float duration;
    private Action onUpdate;
    private Action<int> onUpdate_c;
    private Action onComplete;
    private bool isRunning = false;
    private float elapsedTime = 0f;
    bool isCooldown = false; // 시간 반환 할건지 말건지 true : 반환 false : ㄴ반환
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Timer() { }

    public Timer(float duration, Action onUpdate, Action onComplete)
    {
        ResetTimer(duration,onUpdate, onComplete);
    }

    public Timer(float duration, Action<int> onUpdate, Action onComplete)
    {
        ResetTimer(duration, onUpdate, onComplete);
    }

    public void ResetTimer(float duration, Action onUpdate, Action onComlete)
    {
        if (isRunning) StopTimer();
        this.duration = duration;
        this.onUpdate = onUpdate;
        this.onComplete = onComlete;
        elapsedTime = 0f;
        isRunning = true;
        isCooldown = false;
    }

    public void ResetTimer(float duration, Action<int> onUpdate_c, Action onComlete)
    {
        if (isRunning) StopTimer();
        this.duration = duration;
        this.onUpdate_c = onUpdate_c;
        this.onComplete = onComlete;
        elapsedTime = 0f;
        isRunning = true;
        isCooldown = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRunning) return;

        elapsedTime += Time.deltaTime;
        // 매 프레임 onUpdate 실행
        if(!isCooldown)
            onUpdate?.Invoke();
        else
        {
            int remainingTime = Mathf.CeilToInt(duration - elapsedTime);
            onUpdate_c?.Invoke(remainingTime);
        }

        // 타이머 완료
        if (elapsedTime >= duration)
        {
            isRunning = false;
            onComplete?.Invoke(); // onComplete 콜백 실행
        }
    }

    public void StopTimer()
    {
        isRunning = false;
    }
}
