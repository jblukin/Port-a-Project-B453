using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int enemiesRemaining;

    [SerializeField] private GameObject[] enemyPrefabs;

    [SerializeField] private GameObject playerReference;

    private Transform[] enemySpawnPoints;

    private int totalEnemiesKilled;

    private int currentRound;

    // Start is called before the first frame update
    void Start()
    {
        playerReference.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void StartGame()
    {

        RunGame();

        playerReference.SetActive(true);

    }

    void RunGame() //During Rounds
    {

        //spawn enemies periodically until ==enemiesRemaing

        //check enemiesRemaining!=0 (if ==0, UpdateGame(), else continue)


    }

    void UpdateGame() //Change Info between rounds
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
