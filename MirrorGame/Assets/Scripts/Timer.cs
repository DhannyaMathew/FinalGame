using UnityEngine;

public class Timer
{
    public delegate void CountDownDelegate();

    public float Length { get; set; }
    private float _timer;
    private bool _loop;
    private bool _countingDown;
    public readonly CountDownDelegate onCountDown;

    public Timer(float length, bool loop, CountDownDelegate onCountDown)
    {
        Length = length;
        _loop = loop;
        this.onCountDown = onCountDown;
    }

    public void Tick()
    {
        if (_countingDown)
            _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            onCountDown();
            var diff = _timer;
            if (_loop)
                _timer = Length + diff;
            else
            {
                _countingDown = false;
            }
        }
    }

    public void StartTimer()
    {
        _countingDown = true;
        _timer = Length;
    }
}