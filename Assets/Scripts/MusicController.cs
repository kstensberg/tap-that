using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicController : MonoBehaviour {
	
	public AudioClip[] notes;
	private List<AudioClip> noteQueue = new List<AudioClip>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {	
		if (noteQueue.Count < 10) {
			for (int i=0;i<50;i++) {
				noteQueue.Add(notes[Random.Range(0, notes.Length)]);	
			}
		}
	}
	
	public AudioClip GetRandomNote () {
		AudioClip note = noteQueue[0];
		noteQueue.RemoveAt(0);
		return note;
	}
		
}
