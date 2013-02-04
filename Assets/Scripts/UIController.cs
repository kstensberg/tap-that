using UnityEngine;
using System.Collections;
using EightBitIdeas.WebApi;
using EightBitIdeas.WebApi.Json;
using System;
using LitJson;

public class UIController : MonoBehaviour {
	
	public GameObject messageBox;
	public UILabel messageLabel;
	public UILabel tapCountLabel;
	public UILabel previousRankLabel;
	public UILabel nextRankLabel;
	
	private float leaderboardDelay = 10.0f;
	private float messageDelay = 5.0f;
	private int tapCount = 0;
	private int tapDelta = 0;
	
	private WebApi webApi;
	private LoginResponse loginResponse;
	
	// Use this for initialization
	IEnumerator Start () {
		webApi = new WebApi();
		
		WWW www = webApi.GetAuthWWW(PlayerPrefs.GetString("Username"), PlayerPrefs.GetString("Password"));
		yield return www;
		
		ErrorResponse error = webApi.GetError(www);
		
		if (error != null)
			PushMessage(error.displayError, 5);
		else
			loginResponse = webApi.GetResponse(www);
		
		if (loginResponse != null)
		{
			
			
			string url = WebApi.ApiRootUrl + "TapThat/Leaderboard";
		
			WWWForm form = new WWWForm();
			form.AddField("authToken", loginResponse.authToken);
			
			WWW leaderWww = new WWW(url, form);
			
			yield return leaderWww;
			
			error = webApi.GetError(leaderWww);
			
			if (error != null)
			{
				PushMessage(error.displayError, 5);
			}
			else
			{
				LeaderboardResponse leaderboardResponse = new LeaderboardResponse(JsonMapper.ToObject(leaderWww.text));
				int totalTaps = leaderboardResponse.totalTaps;
				
				string msg = string.Format("Welcome back {0}, {1} bubble taps from before added",loginResponse.name, totalTaps);
				
				PushMessage(msg, 5);
				tapCount = totalTaps;
			}
			StartCoroutine("UpdateLeaderboards");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Update labels with correct values
		tapCountLabel.text = tapCount.ToString();
	
	
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
	
	
	private IEnumerator HideMessage(){
		yield return new WaitForSeconds(messageDelay);	
		Vector3 p = messageBox.transform.position;
		iTween.MoveTo(messageBox, iTween.Hash("position", new Vector3(p.x, -280, p.z), "time", 1, "isLocal", true));
	}
	
	private IEnumerator UpdateLeaderboards() {
		string url = WebApi.ApiRootUrl + "TapThat/Leaderboard";
		
		WWWForm form = new WWWForm();
		form.AddField("authToken", loginResponse.authToken);
		Debug.Log("sending delta " + tapDelta);
		form.AddField("delta", tapDelta);
		tapDelta = 0;
		
		WWW www = new WWW(url, form);
		
		yield return www;
		
		ErrorResponse error = webApi.GetError(www);
		
		if (error != null)
		{
			PushMessage(error.displayError, 5);
		}
		else
		{
			Debug.Log(www.text);
			
			LeaderboardResponse leaderboardResponse = new LeaderboardResponse(JsonMapper.ToObject(www.text));
			
			NearRankLeaderboardEntry nextRank = null;
			NearRankLeaderboardEntry previousRank = null;
			foreach (NearRankLeaderboardEntry nearRank in leaderboardResponse.leaderboard)
			{
				if (nextRank != null && previousRank != null)
					break;
				
				if (nearRank.rank == leaderboardResponse.rank-1)
				{
					nextRank = nearRank;
					continue;
				}
				else if (nearRank.rank == leaderboardResponse.rank+1)
				{
					previousRank = nearRank;
					continue;
				}
			}
			
			if (nextRank == null)
			{
				nextRankLabel.enabled = false;
			}
			else
			{
				nextRankLabel.enabled = true;
				nextRankLabel.text = nextRank.name + ": " + nextRank.totalTaps;
			}
			
			if (previousRank == null)
			{
				previousRankLabel.enabled = false;
			}
			else
			{
				previousRankLabel.enabled = true;
				previousRankLabel.text = previousRank.name + ": " + previousRank.totalTaps;
			}
			
		}
		
		yield return new WaitForSeconds(leaderboardDelay);
		StartCoroutine("UpdateLeaderboards");
	}
}
