using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int numberOfLives = 5;        
    public float m_StartDelay = 3f;         
    public float m_EndDelay = 3f;             
    public Text m_MessageText;              
    public GameObject playerTankPrefab;
    public GameObject aiTankPrefab;
    public TankManager[] m_Tanks;           


    private int missionNumber;              
    private WaitForSeconds m_StartWait;     
    private WaitForSeconds m_EndWait;

    public static int gameIndex = 0;

    private void Start()
    {
        m_StartWait = new WaitForSeconds(m_StartDelay);
        m_EndWait = new WaitForSeconds(m_EndDelay);
        missionNumber = 0;

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
    }

    private void SpawnAI()
    {
        //After the player has been spawned in the ai are at indexs 1 to the length of the spawn counter
        for (int i = 1; i < m_Tanks.Length; i++)
        { 
            m_Tanks[i].m_Instance = Instantiate(aiTankPrefab, m_Tanks[i].m_SpawnPoint.position,
                                                m_Tanks[i].m_SpawnPoint.rotation);
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
                SpawnPlayer();
                SpawnAI();
                SceneManager.LoadScene(0);
            }
            else
            {
                EndGame();
            } 
        }
        else
        {
            StartCoroutine(GameLoop());
        }
    }


    private IEnumerator RoundStarting()
    {
        // As soon as the round starts reset the tanks and make sure they can't move.
        ResetAllTanks();
        DisableTankControl();

        // Increment the round number and display text showing the players what round it is.
        missionNumber++;
        m_MessageText.text = "Mission " + missionNumber;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_StartWait;
    }


    private IEnumerator RoundPlaying()
    {
        // As soon as the round begins playing let the players control the tanks.
        EnableTankControl();

        // Clear the text from the screen.
        m_MessageText.text = string.Empty;

        // While there is not one tank left...
        while (!PlayerDied())
        {
            // ... return on the next frame.
            yield return null;
        }
    }


    private IEnumerator RoundEnding()
    {
        // Stop tanks from moving.
        DisableTankControl();
        
        // Get a message based on the scores and whether or not there is a game winner and display it.
        string message = EndMessage();
        //Set the message to that
        m_MessageText.text = message;

        // Wait for the specified length of time until yielding control back to the game loop.
        yield return m_EndWait;
    }

    
    
    // Returns a string message to display at the end of each round.
    private string EndMessage()
    {
        // By default when a round ends there are no winners so the default end message is a draw.
        string message = "Mission Cleared!";

        // Add some line breaks after the initial message.
        message += "\n\n\n\n";
        
        return message;
    }

    private bool PlayerDied()
    {
        //If the object, aka the player, is active then they are alive and well
        return !m_Tanks[0].m_Instance.activeSelf;
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
        //End the game my dude
    }
}