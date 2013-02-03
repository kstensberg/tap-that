using UnityEngine;
using System.Collections;
using EightBitIdeas.WebApi;
using EightBitIdeas.WebApi.Json;
using System;
using LitJson;


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
	

	// Use this for initialization
	void Start () {
		webApi = new WebApi();
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
	
	IEnumerator CreateAccount(){
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
	
}
