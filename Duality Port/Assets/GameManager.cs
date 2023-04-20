using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int enemiesRemaining;

    [SerializeField] private GameObject[] enemyPrefabs;

    private Transform[] enemySpawnPoints;

    private int totalEnemiesKilled;

    private int currentRound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void StartGame()
    {

        RunGame();

    }

    void RunGame()
    {

        //spawn enemies periodically until ==enemiesRemaing

        //check enemiesRemaining!=0 (if ==0, UpdateGame(), else continue)


    }

    void UpdateGame()
    {

        //reset and increase enemiesRemaining

        //begin countdown timer and show UI for next round (use UI manager)

        //Call RunGame()

    }

    void EndGame() //To be used in SendMessage when player dies
    {

        //shows game over UI (use UI Manager)

    }
}
