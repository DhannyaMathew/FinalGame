using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundtrack_Script : MonoBehaviour
{
	public AudioSource Soundtrack_Source ;
	public AudioClip Clip01 ;
    // Start is called before the first frame update
    void Start()
    {
		Soundtrack_Source = this.GetComponent <AudioSource> () ;
		Soundtrack_Source.clip = Clip01 ;
		Soundtrack_Source.Play () ;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
