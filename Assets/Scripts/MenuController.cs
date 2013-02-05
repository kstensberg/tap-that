using UnityEngine;
using System.Collections;
using EightBitIdeas.WebApi;
using EightBitIdeas.WebApi.Json;
using System;
using LitJson;
using System.Collections.Generic;


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
	
	private IEnumerator CreateAccount(){
		if (passwordLabel.text != confPasswordLabel.text){
			errorLabel.color = Color.red;
			errorLabel.text = "Passwords do not match.";
			passwordLabel.text = "";
			confPasswordLabel.text = "";
		}
		else {		
			string url = WebApi.ApiRootUrl + "user/create";
			
			WWWForm form = new WWWForm();
			form.AddField("name", nameLabel.text);
			form.AddField("username", usernameLabel.text);
			form.AddField("password", passwordLabel.text);
			
			WWW createWww = new WWW(url, form);
			
			yield return createWww;
			
			Debug.Log(createWww.text);
			
			ErrorResponse error = webApi.GetError(createWww);
			
			if (error != null)
			{
				errorLabel.color = Color.red;
				errorLabel.text = error.displayError;
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
	
}
