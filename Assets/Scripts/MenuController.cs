using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
	
	public GameObject leaderBoard;
	public GameObject mainMenu;
	public GameObject signupMenu;
	
	public UILabel nameLabel;
	public UILabel usernameLabel;
	public UILabel passwordLabel;
	public UILabel confPasswordLabel;
	
	

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
	
}
