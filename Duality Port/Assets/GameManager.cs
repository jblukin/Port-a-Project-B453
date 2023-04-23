using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private int enemiesRemaining;

    private int enemiesToSpawn;

    [SerializeField, Range(0, 5)]private float roundCountdownDuration;

    [SerializeField] private GameObject[] enemyPrefabs = new GameObject[6];

    [SerializeField] private Transform[] enemySpawnerTransforms = new Transform[2];

    [SerializeField] private GameObject playerReference;

    private Transform[] enemySpawnPoints = new Transform[2];

    public int totalEnemiesKilled { get; set; }

    public int currentRound { get; private set; }

    public float totalDamageDealt { get; set; } //To be referenced by the player alongside each call of the SendMessage("TakeDamage") to the enemies (+= damage dealt - after multipliers)

    // Start is called before the first frame update
    void Start()
    {
        playerReference.SetActive(false);

        currentRound = 0; //Starts at 0, updated to 1 before first round starts

        totalDamageDealt = 0.0f;

        totalEnemiesKilled = 0;

    }

    public void StartGame() //To be called using SendMessage from UIManager
    {

        playerReference.SetActive(true);

        UpdateGame();

    }

    void RunGame() //During Rounds
    {

        InvokeRepeating("SpawnEnemies", 1.0f, Random.Range(1.5f, 2.5f));

        if(!IsInvoking("SpawnEnemies") && enemiesRemaining == 0)
            UpdateGame();
    }

    void SpawnEnemies() 
    {

        Instantiate(enemyPrefabs[(int)Random.Range(0, 5)], enemySpawnerTransforms[(int)Random.Range(0, 1)].position, Quaternion.identity);

        enemiesToSpawn--;

        if(enemiesToSpawn == 0)
            CancelInvoke("SpawnEnemies");
    }

    void UpdateGame() //Change info between rounds
    {

        Debug.Log("Updating Game");

        currentRound++;

        enemiesRemaining = currentRound + 4;

        enemiesToSpawn = enemiesRemaining;

        this.gameObject.SendMessage("RunInBetweenRoundUI", roundCountdownDuration);  //shows between round UI and updates HUD for next rounds (use UI manager)

        this.gameObject.GetComponent<UIManager>().UpdateHUD(currentRound, enemiesRemaining);

    }

    void AdjustEnemyCount() //To be called in a SendMessage when an Enemy dies
    {

        enemiesRemaining--;

        this.gameObject.GetComponent<UIManager>().UpdateHUD(currentRound, enemiesRemaining);

    }

    void EndGame() //To be used in SendMessage when player dies
    {

        this.gameObject.SendMessage("OpenEndGameScreen");

    }
}
