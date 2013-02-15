using UnityEngine;
using System.Collections;

public class CircleTouch : MonoBehaviour {
	
	private MusicController music;
	private CircleController circle;
	private UIController ui;

	// Use this for initialization
	void Start () {
		music = GameObject.FindGameObjectWithTag("MusicController").GetComponent<MusicController>();
		circle = GameObject.FindGameObjectWithTag("CircleController").GetComponent<CircleController>();
		ui = GameObject.FindGameObjectWithTag("UIController").GetComponent<UIController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void TapCircle() {
		audio.clip = music.GetRandomNote();
		audio.Play();
		circle.CircleTapped();
		renderer.enabled = false;
		collider.enabled = false;
		ui.IncrementTapCount(1);
		StartCoroutine("DestroyObject");
		
	}
	
	IEnumerator DestroyObject() {
		yield return new WaitForSeconds (audio.clip.length + 0.2f);
		Destroy(gameObject);
		
	}
}
