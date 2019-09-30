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

    public void Tick(float dt)
    {
        if (_timer <= 0)
        {
            onCountDown();
            if (_loop)
            {
                var diff = _timer;
                _timer = Length + diff;
            }
            else
            {
                _timer = Length;
                _countingDown = false;
            }
        }
        else if (_countingDown)
        {
            _timer -= dt;
        }
    }

    public void StartTimer()
    {
        _countingDown = true;
        _timer = Length;
    }
}