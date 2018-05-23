using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

//TODO:: if u die the screen never unloads
public class GameManager : MonoBehaviour
{
    public int numberOfLives = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;             
    public TextMeshProUGUI m_MessageText;              
    public GameObject playerTankPrefab;
    public GameObject[] aiTankPrefabs;
    public TankManager[] m_Tanks;           


    public static int missionNumber = 1;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;


    private LoadingScreenManager loadingManager;
    private bool wonGame;
    private int gameIndex = 0;
    
    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        loadingManager = gameObject.GetComponent<LoadingScreenManager>();

        SpawnAllTanks();

        StartCoroutine(GameLoop());
    }


    private void SpawnAllTanks()
    {
        SpawnPlayer();
        SpawnAI();
    }

    private void SpawnPlayer()
    {
        //The player tank instance should be the first one in the array of transform positions
        m_Tanks[0].m_Instance = Instantiate(playerTankPrefab, m_Tanks[0].m_SpawnPoint.position, 
                                            m_Tanks[0].m_SpawnPoint.rotation);
        m_Tanks[0].Setup();
    }

    //Spawn Ai the i-1 is for the Ai specifically prefabs because it starts at 0 but the m_tanks
    //starts at 1 
    private void SpawnAI()
    {
        //After the player has been spawned in the ai are at indexs 1 to the length of the spawn counter
        for (int i = 1; i < m_Tanks.Length; i++)
        {
            if (!aiTankPrefabs[i-1].GetComponent<EnemyStats>().isKilled)
            {
                m_Tanks[i].m_Instance = Instantiate(aiTankPrefabs[i-1], m_Tanks[i].m_SpawnPoint.position,
                    m_Tanks[i].m_SpawnPoint.rotation);
                m_Tanks[i].IsAi = true;
                m_Tanks[i].Setup();
            }
                
        }
    }
    
    private IEnumerator GameLoop()
    {
        // Start off by running the 'RoundStarting' coroutine but don't return until it's finished.
        yield return StartCoroutine (RoundStarting());

        // Once the 'RoundStarting' coroutine is finished, run the 'RoundPlaying' coroutine but don't return until it's finished.
        yield return StartCoroutine (RoundPlaying());

        // Once execution has returned here, run the 'RoundEnding' coroutine, again don't return until it's finished.
        yield return StartCoroutine (RoundEnding()); 

        if (PlayerDied())
        {
            numberOfLives--;
            if (numberOfLives > 0)
            {
                ResetAllTanks();
                //Load the temporary waiting screen...
                StartCoroutine(GameLoop());
            }
            else
            {
                EndGame();
            } 
        }
        else
        {
            if (wonGame)
            {
                gameIndex++;
                SceneManager.LoadScene(gameIndex);
            }
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        // As soon as the round starts reset the tanks and make sure they can't move.
        ResetAllTanks();
        DisableTankControl();
        EnableLoadingScreen();

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        DisableLoadingScreen();
        // As soon as the round begins playing let the players control the tanks.
        EnableTankControl();

        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        // While there is not one tank left...
        while (!OneTankLeft() && !PlayerDied())
        {
            // ... return on the next frame.
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {      
        // Stop tanks from moving.
        DisableTankControl();

        if (!PlayerDied())
        {
            wonGame = true;
            // Get a message based on the scores and whether or not there is a game winner and display it.
            string message = EndMessage();
            //Set the message to that
            m_MessageText.text = message;
        }
        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }

    
    
    // Returns a string message to display at the end of each round.
    private string EndMessage()
    {
        // By default when a round ends there are no winners so the default end message is a draw.
        string message = "Mission Cleared!";

        missionNumber++;
        
        return message;
    }

    private bool PlayerDied()
    {
        //If the object, aka the player, is active then they are alive and well
        return !m_Tanks[0].m_Instance.activeSelf;
    }
    
    // This is used to check if there is one or fewer tanks remaining and thus the round should end.
    private bool OneTankLeft()
    {
        // Start the count of tanks left at zero.
        int numTanksLeft = 0;

        // Go through all the tanks...
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            // ... and if they are active, increment the counter.
            if (m_Tanks[i].m_Instance.activeSelf)
                numTanksLeft++;
        }
        // If there are one or fewer tanks remaining return true, otherwise return false.
        return numTanksLeft <= 1;
    }


    private void ResetAllTanks()
    {
        //Reset all the tanks in the spawner
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].Reset();
        }
    }


    private void EnableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].EnableControl();
        }
    }


    private void DisableTankControl()
    {
        for (int i = 0; i < m_Tanks.Length; i++)
        {
            m_Tanks[i].DisableControl();
        }
    }

    public int GetAmountOfTanks()
    {
        //Return the count of the spawners minus one since one of them 
        //is occupied by the player themselves
        return m_Tanks.Length - 1;
    }

    private void EndGame()
    {
        Application.Quit();
    }

    private void EnableLoadingScreen()
    {
        print("Enabling the screen...");
        loadingManager.EnableUi();
    }

    private void DisableLoadingScreen()
    {
        print("Disabling the screen...");
        loadingManager.DisableUi();
    }
    
    
}