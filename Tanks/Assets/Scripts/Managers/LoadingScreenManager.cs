using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
	public GameManager gameManager;
	[SerializeField]
	private GameObject m_CanvasGameObject;

	[SerializeField]
	private TextMeshProUGUI missionText;
	[SerializeField]
	private TextMeshProUGUI tankCount;
	[SerializeField] 
	private TextMeshProUGUI lifeCount;
	
	private int loadingScreenIndex = 0;

	private void Awake()
	{
		gameManager = gameObject.GetComponent<GameManager>();
	}
	
	private void LoadScreen()
	{
		SetText();
	}

	public void EnableUi()
	{
		m_CanvasGameObject.SetActive(true);
		LoadScreen();
	}

	public void DisableUi()
	{
		m_CanvasGameObject.SetActive(false);
	}

	private void SetText()
	{
		missionText.text = "Mission " + GameManager.missionNumber;
		tankCount.text = "Enemy Tanks: " + gameManager.GetAmountOfTanks();
		lifeCount.text = "Lives x" + gameManager.numberOfLives;
	}

}
