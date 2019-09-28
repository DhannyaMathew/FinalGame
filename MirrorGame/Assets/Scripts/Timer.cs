using UnityEngine;

public class Timer
{
    public delegate void CountDownDelegate();
    
    public float Length { get; set; }
    private float _timer;
    public readonly CountDownDelegate onCountDown;

    public Timer(float length, CountDownDelegate onCountDown)
    {
        Length = length;
        
        this.onCountDown = onCountDown;
    }

    public void Tick()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            onCountDown();
            var diff = _timer;
            _timer = Length + diff;
        }
    }
}