using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack_Script : MonoBehaviour
{
    [SerializeField] private int musicBpm;
    [SerializeField] private int timeSignature;
    [SerializeField] private int barsLength;

    private AudioSource[] _soundtrackSources;
    private AudioSource _backgroundSource;
    private float _loopPointMin;
    private float _loopPointSec;
    private double _time;


    // Start is called before the first frame update
    void Start()
    {
        _backgroundSource = GetComponent<AudioSource>();
        _soundtrackSources = GetComponentsInChildren<AudioSource>();
        _loopPointMin = barsLength * timeSignature / musicBpm;
        Debug.Log(_loopPointMin * 60);
        _loopPointSec = _loopPointMin * 60;
        _time = AudioSettings.dspTime;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (_backgroundSource.isPlaying == false)
        {
            _backgroundSource.Play();
            var index = Random.Range(0, _soundtrackSources.Length);
            var source = _soundtrackSources[index];
            if (source.isPlaying == false)
            {
                _time = _time + _loopPointSec;
                source.PlayScheduled(_time);
            }
        }
    }
}