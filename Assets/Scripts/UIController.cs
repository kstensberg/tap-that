using UnityEngine;
using System.Collections;
using EightBitIdeas.WebApi;
using EightBitIdeas.WebApi.Json;
using System;
using LitJson;
using System.Collections.Generic;
using EightBitIdeas.Lib8bit.Net.Http.WebApi;
using EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse;

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
	void Start () {
		webApi = new WebApi();
		
		IWebApiResponse loginApiResponse = webApi.Login(PlayerPrefs.GetString("email"), PlayerPrefs.GetString("password"));
		
		if (loginApiResponse is ErrorResponse)
			PushMessage((loginApiResponse as ErrorResponse).displayError, 5);
		else
			loginResponse = loginApiResponse as LoginResponse;
		
		if (loginResponse != null)
		{
			IWebApiResponse tapsResponse = webApi.GetResponseObject<TapsResponse>("tapthat/taps", "POST", null, new Dictionary<string, string>()
			{
				{"authToken", loginResponse.authToken}
			});
			
			if (tapsResponse is ErrorResponse)
			{
				PushMessage((tapsResponse as ErrorResponse).displayError, 5);
			}
			else
			{
				TapsResponse leaderboardResponse = tapsResponse as TapsResponse;
				int totalTaps = leaderboardResponse.totalTaps;
				
				string msg = string.Format("Welcome back {0}, {1} bubble taps from before added", loginResponse.name, totalTaps);
				
				PushMessage(msg, 5);
				tapCount = totalTaps;
			}
			StartCoroutine("UpdateTaps");
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
	
	private IEnumerable UpdateTaps() {
		
		if (loginResponse != null)
		{
			Debug.Log("sending delta " + tapDelta);
			
			Dictionary<string, string> args = new Dictionary<string, string>()
			{
				{"authToken", loginResponse.authToken},
				{"delta", tapDelta.ToString()}
			};
			tapDelta = 0;
			
			IWebApiResponse tapsResponse = webApi.GetResponseObject<TapsResponse>("tapthat/taps", "POST", null, args);
			
			
			if (tapsResponse is ErrorResponse)
			{
				PushMessage((tapsResponse as ErrorResponse).displayError, 5);
			}
			else
			{
				TapsResponse leaderboardResponse = tapsResponse as TapsResponse;
				
				LeaderboardResponse nextRank = null;
				LeaderboardResponse previousRank = null;
				if (leaderboardResponse.rank != 0) 
				{
					foreach (LeaderboardResponse nearRank in leaderboardResponse.leaderboard)
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
		}
		
		yield return new WaitForSeconds(leaderboardDelay);
		StartCoroutine("UpdateTaps");
	}
}
