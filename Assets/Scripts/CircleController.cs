using UnityEngine;
using System.Collections;

public class CircleController : MonoBehaviour {
	
	public GameObject circlePrefab;
	public int totalCircles;
	
	private int currentCircles = 0;
	private float lastSpawn;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > lastSpawn + 0.1){
			if (currentCircles < totalCircles){
				SpawnCircle();	
				lastSpawn = Time.time;
			}
		}
	}
	
	void SpawnCircle () 
	{
		float randScale = Random.Range(0.4f, 1.0f);
		Vector3 position = new Vector3(Random.Range(-25.0f + (randScale * 8.0f), 25.0f - (randScale * 8.0f)),Random.Range(-19.0f + (randScale * 8.0f), 19.0f - (randScale * 8.0f)), 0.0f);
		
		GameObject circle = (GameObject)Instantiate(circlePrefab, position, Quaternion.Euler(new Vector3(-90f,0f,0f)));
		circle.transform.localScale = Vector3.zero;
		circle.renderer.material.color = new Color(Random.Range(0f,0.9f), Random.Range(0f,0.9f), Random.Range(0f,0.9f), 1);
		
		iTween.ScaleTo(circle, new Vector3(randScale, randScale, randScale), 0.5f);
		
		currentCircles++;

	}
	
	public void CircleTapped() {
		currentCircles--;	
	}
}
