using UnityEngine;
using System.Collections;
using EightBitIdeas.WebApi;
using System;

public class UIController : MonoBehaviour {
	
	public GameObject messageBox;
	public UILabel messageLabel;
	public UILabel tapCountLabel;
	public UILabel previousRankLabel;
	public UILabel nextRankLabel;
	
	private float leaderboardDelay = 30.0f;
	private float messageDelay = 5.0f;
	private int tapCount = 0;
	private int previousRank = 0;
	private int nextRank = 0;
	private int tapDelta = 0;
	
	private WebApi webApi;
	private LoginResponse? loginResponse;
	
	// Use this for initialization
	IEnumerator Start () {
		webApi = new WebApi();
		
		WWW www = webApi.GetAuthWWW("asdf", "asdf");
		yield return www;
		
		ErrorResponse? error = webApi.GetError(www);
		
		if (error.HasValue)
			PushMessage(error.Value.displayError, 5);
		else
			loginResponse = webApi.GetResponse(www);
		
		if (loginResponse.HasValue)
		{
			string msg = string.Format("Welcome back {0}, {1} bubble taps from before added", "asdf", loginResponse.Value.totalTaps);
			PushMessage(msg , 5);
			tapCount = loginResponse.Value.totalTaps;
			
			StartCoroutine("UpdateLeaderboards");
		}
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
		tapDelta += number;
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
	
	private IEnumerator UpdateLeaderboards() {
		string url = WebApi.ApiRootUrl + "TapThat/Leaderboard";
		
		WWWForm form = new WWWForm();
		form.AddField("authToken", loginResponse.Value.authToken);
		form.AddField("delta", tapDelta);
		Debug.Log("sending delta " + tapDelta);
		tapDelta = 0;
		
		WWW www = new WWW(url, form);
		
		yield return www;
		
		ErrorResponse? error = webApi.GetError(www);
		
		if (error.HasValue)
		{
			PushMessage(error.Value.displayError, 5);
		}
		else
		{
			Debug.Log(www.text);
		}
		
		yield return new WaitForSeconds(leaderboardDelay);
		StartCoroutine("UpdateLeaderboards");
	}
}
