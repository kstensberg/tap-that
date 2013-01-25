using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {
	
	public GameObject messageBox;
	public UILabel messageLabel;
	public UILabel tapCountLabel;
	public UILabel previousRankLabel;
	public UILabel nextRankLabel;
	
	private float messageDelay = 5.0f;
	private int tapCount = 0;
	private int previousRank = 0;
	private int nextRank = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		//Update labels with correct values
		tapCountLabel.text = tapCount.ToString();
		previousRankLabel.text = previousRank.ToString();
		nextRankLabel.text = nextRank.ToString();
		
		if (Input.GetKeyUp("m")){
			PushMessage("Hey Patrick! This is a message, dude.", 5);	
		}
	
	}
	
	public void PushMessage(string text, float delay){
		messageLabel.text = text;
		messageDelay = delay;
		Vector3 p = messageBox.transform.position;
		iTween.MoveTo(messageBox, iTween.Hash("position", new Vector3(p.x, -210, p.z), "time", 1, "isLocal", true));
		StartCoroutine("HideMessage");
	}
	
	public void IncrementTapCount(int number){
		tapCount += number;
	}
	
	public void SetPreviousRank (int number){
		previousRank = number;	
	}
	
	public void SetNextRank (int number){
		nextRank = number;	
	}
	
	
	private IEnumerator HideMessage(){
		yield return new WaitForSeconds(messageDelay);	
		Vector3 p = messageBox.transform.position;
		iTween.MoveTo(messageBox, iTween.Hash("position", new Vector3(p.x, -280, p.z), "time", 1, "isLocal", true));
	}
	
	
	
}
