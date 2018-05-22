using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenManager : MonoBehaviour
{

	private int loadingScreenIndex = 0;
	
	// Update is called once per frame
	void Update () 
	{
		LoadScreen();
	}

	private void LoadScreen()
	{
		if (GameManager.wonGame)
		{
			SceneManager.LoadScene(GameManager.gameIndex);
		}
		else
		{
			
		} 
	}
}
