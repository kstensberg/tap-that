using UnityEngine;
using System.Collections;
using EightBitIdeas.WebApi;
using EightBitIdeas.WebApi.Json;
using System;
using LitJson;
using System.Collections.Generic;
using EightBitIdeas.Lib8bit.Net.Http.WebApi;
using EightBitIdeas.Lib8bit.Net.Http.WebApi.ApiResponse;


public class MenuController : MonoBehaviour {
	
	public GameObject leaderBoard;
	public GameObject mainMenu;
	public GameObject signupMenu;
	
	public UILabel nameLabel;
	public UILabel usernameLabel;
	public UILabel passwordLabel;
	public UILabel confPasswordLabel;
	public UILabel errorLabel;
	
	private WebApi webApi;
	private List<UILabel> leaderboardLabels = new List<UILabel>();
	

	// Use this for initialization
	void Start () {
		webApi = new WebApi();
		GameObject leader = GameObject.FindWithTag("Leaderboard");
		UILabel[] labels = leader.GetComponentsInChildren<UILabel>(true);
		
		foreach (UILabel label in labels){
			leaderboardLabels.Add(label);	
		}
		
		
		if (PlayerPrefs.HasKey("username")){
			mainMenu.SetActiveRecursively(true);
			leaderBoard.SetActiveRecursively(false);
			signupMenu.SetActiveRecursively(false);	
		}
		else {
			mainMenu.SetActiveRecursively(false);
			leaderBoard.SetActiveRecursively(false);
			signupMenu.SetActiveRecursively(true);		
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Play(){
		Application.LoadLevel("Game");	
	}
	
	void ShowLeaderboard (){
		leaderBoard.SetActiveRecursively(true);
		mainMenu.SetActiveRecursively(false);
		signupMenu.SetActiveRecursively(false);
		
		StartCoroutine("GetLeaderboardTop10");
	}
	
	void ShowMainMenu (){
		mainMenu.SetActiveRecursively(true);
		leaderBoard.SetActiveRecursively(false);
		signupMenu.SetActiveRecursively(false);
	}
	
	void ShowSignup (){
		signupMenu.SetActiveRecursively(true);
		leaderBoard.SetActiveRecursively(false);
		mainMenu.SetActiveRecursively(false);
	}
	
	void StartAccountCreation(){
		errorLabel.color = Color.black;
		errorLabel.text = "Creating New Account...";
		StartCoroutine("CreateAccount");
	}
	
	private void CreateAccount(){
		if (passwordLabel.text != confPasswordLabel.text){
			errorLabel.color = Color.red;
			errorLabel.text = "Passwords do not match.";
			passwordLabel.text = "";
			confPasswordLabel.text = "";
		}
		else {
			IWebApiResponse response = webApi.CreateUser(usernameLabel.text, passwordLabel.text, nameLabel.text);
			
			if (response is ErrorResponse) {
				errorLabel.color = Color.red;
				errorLabel.text = (response as ErrorResponse).displayError;
			}
			else {
				PlayerPrefs.SetString("username", usernameLabel.text);
				PlayerPrefs.SetString("password", passwordLabel.text);
				ShowMainMenu();
			}
		}
			
	}
	
	private void SetLeaderboardLabel (int rank, string name, int taps){
		
		List<UILabel> labels = leaderboardLabels.FindAll(i=>i.transform.parent.name == rank.ToString());
		foreach (UILabel label in labels)
		{
			switch (label.gameObject.name)
			{
				case "Name":
					label.text = rank + ". " + name;
					break;
				case "Taps":
					label.text = taps.ToString();
					break;
			}
		}
		
	}
	
	private void GetLeaderboardTop10(){
		
		IWebApiResponse response = webApi.GetResponseObject<LeaderboardListResponse>("tapthat/leaderboard", "POST", null, new Dictionary<string, string>()
		{
			{ "top", "10" }
		});
		
		if (response is ErrorResponse) {
			//TODO: show user error here
			Debug.Log((response as ErrorResponse).displayError);
		} else {
			foreach (var responseRow in response as LeaderboardListResponse) {
				SetLeaderboardLabel(responseRow.rank, responseRow.name, responseRow.totalTaps);
			}
		}
	}
}
