using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class PlayFabLogin : MonoBehaviour
{
	private string userEmail;
	private string userpassword;
	private string username;
	public GameObject loginPanel;
	public GameObject addloginPanel;
	public GameObject Buttoncreate;
	public void Start()
	{
		
		//Note: Setting title Id here can be skipped if you have set the value in Editor Extensions already.
		if (string.IsNullOrEmpty(PlayFabSettings.TitleId))
		{
			PlayFabSettings.TitleId = "F656"; // Please change this value to your own titleId from PlayFab Game Manager
		}
		//var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
		//PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
		if (PlayerPrefs.HasKey("EMAIL"))
		{
			userEmail = PlayerPrefs.GetString("EMAIL");
			userpassword = PlayerPrefs.GetString("PASSWORD");
			var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userpassword };
			PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
		}
		else
		{
#if UNITY_ANDROID
			var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount =true };
			PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginAndroidSuccess, OnLoginAndroidFailure);
#endif

		}
	}
	private void OnLoginAndroidSuccess(LoginResult result)
	{
		Debug.Log("Congratulations, you made your first successful API call!");
		loginPanel.SetActive(false);
	}
	private void OnLoginAndroidFailure(PlayFabError error)
	{
		Debug.Log(error.GenerateErrorReport());
	}
	private void OnregisterSuccess(RegisterPlayFabUserResult result)
	{
		Debug.Log("Congratulations, you made your first successful API call!");
		PlayerPrefs.SetString("EMAIL", userEmail);
		PlayerPrefs.SetString("PASSWORD", userpassword);
		loginPanel.SetActive(false);
		Buttoncreate.SetActive(false);
	}
	private void OnLoginSuccess(LoginResult result)
	{
		Debug.Log("Congratulations, you made your first successful API call!");
		PlayerPrefs.SetString("EMAIL", userEmail);
		PlayerPrefs.SetString("PASSWORD", userpassword);
		loginPanel.SetActive(false);
	}

	private void OnLoginFailure(PlayFabError error)
	{
		/*Debug.LogWarning("Something went wrong with your first API call.  :(");
		Debug.LogError("Here's some debug information:");
		Debug.LogError(error.GenerateErrorReport());*/
		var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userpassword, Username = username };
		PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnregisterSuccess, OnRegisterfailure);
	}

	private void OnRegisterfailure(PlayFabError error)
	{
		Debug.LogError(error.GenerateErrorReport());
	}

	public void GetUserEmail(string emailIn)
	{
		userEmail = emailIn;
	}

	public void GetUserPassword(string passwordIn)
	{
		userpassword = passwordIn;
	}

	public void GetUsername(string usernameIn)
	{
		username = usernameIn;
	}

	public void OnClickLogin()
	{
		var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userpassword };
		PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
	}

	public static string ReturnMobileID()
	{
		string deviceID = SystemInfo.deviceUniqueIdentifier;
		return deviceID;
	}

	public void OpenAddLogin()
	{
		addloginPanel.SetActive(true);
	}

	public void OnClickAddLogin()
	{
		var addLoginRequest = new AddUsernamePasswordRequest { Email = userEmail, Password = userpassword, Username = username };
		PlayFabClientAPI.AddUsernamePassword(addLoginRequest, OnAddLoginSuccess, OnRegisterfailure);
	}

	private void OnAddLoginSuccess(AddUsernamePasswordResult result)
	{
		Debug.Log("Congratulations, you made your first successful API call!");
		PlayerPrefs.SetString("EMAIL", userEmail);
		PlayerPrefs.SetString("PASSWORD", userpassword);
		addloginPanel.SetActive(false);
	}
}