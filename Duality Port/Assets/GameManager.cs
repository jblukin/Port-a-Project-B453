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

    [SerializeField] private AudioClip[] UIAudioReferences = new AudioClip[5]; //0 = HAJIME, 1 = YAME, 2 = Round Clear, 3 = Menu Select, 4 = Menu Option Switch

    [SerializeField] private GameObject playerReference;

    [SerializeField] private GameObject orbReference;

    private Transform[] enemySpawnPoints = new Transform[2];

    private AudioSource audioSource;

    public int totalEnemiesKilled { get; set; }

    public int currentRound { get; private set; }

    public float totalDamageDealt { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        playerReference.SetActive(false);

        currentRound = 0; //Starts at 0, updated to 1 before first round starts

        totalDamageDealt = 0.0f;

        totalEnemiesKilled = 0;

        audioSource = this.gameObject.GetComponent<AudioSource>();

    }

    public void StartGame() //To be called using SendMessage from UIManager
    {

        playerReference.SetActive(true);

        orbReference.SetActive(true);

        currentRound = 0; //Starts at 0, updated to 1 before first round starts

        totalDamageDealt = 0.0f;

        totalEnemiesKilled = 0;

        UpdateGame();

    }

    void RunGame() //During Rounds
    {

        InvokeRepeating("SpawnEnemies", 1.0f, Random.Range(1.5f, 2.5f));

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

        this.gameObject.SendMessage("RunInBetweenRoundUI", roundCountdownDuration);  //shows between round UI and updates HUD for next rounds (use UI manager)

        currentRound++;

        enemiesRemaining = currentRound + 4;

        enemiesToSpawn = enemiesRemaining;

        this.gameObject.GetComponent<UIManager>().UpdateHUD(currentRound, enemiesRemaining);

    }

    public void AdjustEnemyCount() //To be called in a SendMessage when an Enemy dies
    {

        enemiesRemaining--;

        this.gameObject.GetComponent<UIManager>().UpdateHUD(currentRound, enemiesRemaining);

        if(!IsInvoking("SpawnEnemies") && enemiesRemaining == 0)
            UpdateGame();

    }

    public void EndGame() //To be used in SendMessage when player dies
    {

        this.gameObject.SendMessage("OpenEndGameScreen");

        if(IsInvoking("SpawnEnemies"))
            CancelInvoke("SpawnEnemies");

        orbReference.SetActive(false);

        playerReference.SetActive(false);

        playerReference.GetComponent<PlayerScript>().health = 100f;

    }

    public void PlayMenuSound(int index) { //0 = HAJIME, 1 = YAME, 2 = Round Clear, 3 = Menu Select, 4 = Menu Option Switch

        audioSource.clip = UIAudioReferences[index];

        audioSource.Play();

    }

}
