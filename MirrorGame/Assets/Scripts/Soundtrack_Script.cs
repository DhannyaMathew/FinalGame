using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack_Script : MonoBehaviour
{
	public AudioSource Soundtrack_Source_1;
    public AudioSource AS_Play;
    public AudioSource[] Soundtrack_Sources;
    public AudioClip Clip01;
    public int musicBPM;
    public int timeSigniture;
    public int barsLength;

    private float ClipLength;
    private float loopPointMin;
    private float loopPointSec;
    private double time;


    // Start is called before the first frame update
    void Start()
    {
		Soundtrack_Source_1 = this.GetComponent <AudioSource> ();
        Clip01 = Soundtrack_Source_1.clip;
        loopPointMin = (barsLength * timeSigniture) / musicBPM;
        loopPointSec = loopPointMin * 60;
        time = AudioSettings.dspTime;
        ClipLength = Clip01.length;
        Debug.Log(ClipLength);
        
        //Soundtrack_Source_1.Play () ;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Soundtrack_Source_1.time);
        int Index = Random.Range(0,3);
        if (Soundtrack_Source_1.isPlaying == false)
        {
            //Debug.Log("hello");
            Soundtrack_Source_1.Play();
            AS_Play = Soundtrack_Sources[Index];
            if (AS_Play.isPlaying == false)
            {
                time = time + loopPointSec;
                AS_Play.PlayScheduled(time);
            }
          
        }
    }
}
