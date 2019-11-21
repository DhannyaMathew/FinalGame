using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainMenu
{
	public class SoundController : MonoBehaviour
	{
		public AudioSource[] Audio;
		public AudioSource MusicSource;
		public static SoundController instance = null;
		private float higherPitch = 1.00f;
		private float lowerPitch = 0.99f;

		void Awake ()
		{
			if (instance != null)
			{
				Destroy(instance.gameObject);
				Debug.Log("I got replaced");
			}
			instance = this;
			if (Math.Abs(AudioListener.volume) < 0.001f)
			{
				AudioListener.volume = 1;
			}
		}


		public void Playone (int AudioSourceNum, AudioClip clip)
		{

			Audio[AudioSourceNum].clip = clip;
			Audio[AudioSourceNum].Play ();
		}

		public void PlayBG (AudioClip clip)
		{

			MusicSource.clip = clip;
			MusicSource.Play ();
		}

		public void RandomPitchandsfx(int AudioSourceNum, params AudioClip[] clips)
		{ 		
			int randomIndex = Random.Range (0, clips.Length);			
			float randomPitch = Random.Range (lowerPitch, higherPitch);	
			Audio[AudioSourceNum].pitch = 0.5f;
			Audio[AudioSourceNum].clip = clips [randomIndex];                           

			if (!Audio[AudioSourceNum].isPlaying)
			{
				Audio[AudioSourceNum].Play();
			}


		}


	}
}
