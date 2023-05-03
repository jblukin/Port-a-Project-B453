using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _menuItems = new GameObject[5]; //Main Menu Items

    [SerializeField] private Sprite[] _menuItemSprites = new Sprite[2];
    /* 0 = Unhighlighted Sprite, 1 = Highlighted Sprite */

    [SerializeField] private GameObject[] _ContainerReferences = new GameObject[8];
    /* 0 = Entire UI Container, 1 = Main Menu Container, 2 = Tutorial Container, 3 = Story Container, 
        4 = Credits Container, 5 = End Game UI Container, 6 = In Between Rounds UI Container, 7 = HUD Container */

    [SerializeField] private GameObject[] _gameOverMenuItems = new GameObject[3]; //Game Over Menu Items

    [SerializeField] private GameObject[] _resultsTextReferences = new GameObject[3]; //End Game UI text references

    [SerializeField] private GameObject[] _betweenRoundUITextReferences = new GameObject[3]; //In Between Round UI text references

    [SerializeField] private GameObject _betweenRoundUICountdownBarRef; //In Between Round UI Fill Bar reference

    [SerializeField] private GameObject[] _HUDTextReferences = new GameObject[2]; //HUD text references

    private int _currentMenuIndex;

    public bool playing { get; private set; }

    private bool _onMainMenu;

    private bool _onMenuList;

    private float _maxCountdownDuration, _countdownDuration;

    // Start is called before the first frame update
    void Start()
    {
        
        _currentMenuIndex = 0;

        playing = false;

        _onMainMenu = true;

        _onMenuList = true;

    }

    // Update is called once per frame
    void Update()
    {
        
        NavigateMenu();

        NavigateGameOverMenu();
        
    }

    void NavigateMenu()
    {

        if(!playing && _onMainMenu) {

            if(Input.GetKeyDown(KeyCode.W)) {

                _menuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[0];

                _menuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                    new Vector2(_menuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x - 50, _menuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);



                _currentMenuIndex = SwitchMenuItem(true);



                _menuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[1];

                _menuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                    new Vector2(_menuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x + 50, _menuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);



            } else if(Input.GetKeyDown(KeyCode.S)) {



                _menuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[0];

                _menuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                    new Vector2(_menuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x - 50, _menuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);



                _currentMenuIndex = SwitchMenuItem(false);



                _menuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[1];

                _menuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                    new Vector2(_menuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x + 50, _menuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);

                    

            } else if(Input.GetKeyDown(KeyCode.J))
                if(_onMainMenu && _onMenuList)
                    SelectOption(_currentMenuIndex);
                else if(_onMainMenu && !_onMenuList)
                    OpenMainMenu();
            
        }

    }

    void NavigateGameOverMenu()
    {

        if(!playing && !_onMainMenu) {

            if(Input.GetKeyDown(KeyCode.W)) {

                _gameOverMenuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[0];

                _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                    new Vector2(_gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x + 25, _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);



                _currentMenuIndex = SwitchMenuItem(true);



                _gameOverMenuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[1];

                _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                    new Vector2(_gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x - 25, _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);

            } else if(Input.GetKeyDown(KeyCode.S)) {

                _gameOverMenuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[0];

                _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                    new Vector2(_gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x + 25, _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);



                _currentMenuIndex = SwitchMenuItem(false);



                _gameOverMenuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[1];

                _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                    new Vector2(_gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x - 25, _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);

            } else if(Input.GetKeyDown(KeyCode.J)) {

                if(_currentMenuIndex == 0) {

                    this.gameObject.GetComponent<GameManager>().PlayMenuSound(4); //Menu Select sound plays
                    StartGame();

                }

                else if(_currentMenuIndex == 1) {

                    _gameOverMenuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[0];

                    _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                        new Vector2(_gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x + 25, _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);

                    _currentMenuIndex = 0;

                    _gameOverMenuItems[_currentMenuIndex].GetComponent<Image>().sprite = _menuItemSprites[1];

                    _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position = 
                        new Vector2(_gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.x - 25, _gameOverMenuItems[_currentMenuIndex].GetComponent<RectTransform>().position.y);

                    OpenMainMenu();

                }

                else if(_currentMenuIndex == 2)
                    Application.Quit();
            }
        }

    }

    int SwitchMenuItem(bool UpOrDown) //If recieves true, selected menu item-1 [MOVES UP MENU - bottom to top] (if false, menu item+1)
    {

        this.gameObject.GetComponent<GameManager>().PlayMenuSound(3); //Menu Option Switch sound plays

        if(UpOrDown && _currentMenuIndex == 0 && _onMainMenu) //sets menu index to last menu item on main menu
            return 4;
        else if(!UpOrDown && _currentMenuIndex == 4 && _onMainMenu) //sets menu index to first menu item on main menu
            return 0;
        else if(UpOrDown && _currentMenuIndex == 0 && !_onMainMenu) //set menu index to last menu item on game over screen
            return 2;
        else if(!UpOrDown && _currentMenuIndex == 2 && !_onMainMenu) //sets menu index to first menu item on game over screen
            return 0;
        else if(UpOrDown) //moves menu item up by 1
            return _currentMenuIndex-1;
        else //moves menu item down by 1
            return _currentMenuIndex+1;

    }

    void SelectOption(int index)
    {

        if(index == 0) {
            
            StartGame(); 

            playing = true;

            _onMainMenu = false;

            _onMenuList = false;

        }
            
        else if(index == 1) {

            OpenTutorial();

            _onMenuList = false;

        }
            
        else if(index == 2) {

            OpenStory();

            _onMenuList = false;

        }
            
        else if(index == 3) {

            OpenCredits();

            _onMenuList = false;

        }
            
        else if(index == 4)
            Application.Quit();

        
        this.gameObject.GetComponent<GameManager>().PlayMenuSound(4); //Menu Select sound plays

    }

    void StartGame()
    {
        
        this.gameObject.GetComponent<GameManager>().StartGame();
        playing = true;
        OpenHUD();
        
    }

    void OpenTutorial()
    {

        _ContainerReferences[1].SetActive(false);
        _ContainerReferences[2].SetActive(true);
        _ContainerReferences[3].SetActive(false);
        _ContainerReferences[4].SetActive(false);

    }

    void OpenStory()
    {

        _ContainerReferences[1].SetActive(false);
        _ContainerReferences[2].SetActive(false);
        _ContainerReferences[3].SetActive(true);
        _ContainerReferences[4].SetActive(false);

    }

    void OpenCredits()
    {

        _ContainerReferences[1].SetActive(false);
        _ContainerReferences[2].SetActive(false);
        _ContainerReferences[3].SetActive(false);
        _ContainerReferences[4].SetActive(true);

    }

    void OpenMainMenu()
    {

        _ContainerReferences[1].SetActive(true);
        _ContainerReferences[2].SetActive(false);
        _ContainerReferences[3].SetActive(false);
        _ContainerReferences[4].SetActive(false);
        _ContainerReferences[5].SetActive(false);
        _ContainerReferences[6].SetActive(false);
        _ContainerReferences[7].SetActive(false);

        _onMainMenu = true;

        _onMenuList = true;

        this.gameObject.GetComponent<GameManager>().PlayMenuSound(4); //Menu Select sound plays

    }

    void OpenHUD()
    {

        _ContainerReferences[1].SetActive(false);
        _ContainerReferences[2].SetActive(false);
        _ContainerReferences[3].SetActive(false);
        _ContainerReferences[4].SetActive(false);
        _ContainerReferences[5].SetActive(false);
        _ContainerReferences[7].SetActive(true);

    }

    void RunInBetweenRoundUI(float countdownDuration)
    {

        this.gameObject.GetComponent<GameManager>().PlayMenuSound(2); //Round Cleared sound plays

        _maxCountdownDuration = countdownDuration;

        _countdownDuration = _maxCountdownDuration;

        _ContainerReferences[6].SetActive(true);

        if(!IsInvoking("InBetweenRoundInitialTextTimer"))
            StartCoroutine(InBetweenRoundInitialTextTimer(_maxCountdownDuration));

    }

    public void UpdateHUD(int round, int enemyCount)
    {

        _HUDTextReferences[0].GetComponent<TextMeshProUGUI>().text = "Round: " + round;

        _HUDTextReferences[1].GetComponent<TextMeshProUGUI>().text = "Foes: " + enemyCount;

    }

    private IEnumerator InBetweenRoundInitialTextTimer(float duration) //Shows Clear Round Text, then starts Round Countdown (YAME)
    {
        if(GameObject.Find("GameManager").GetComponent<GameManager>().currentRound == 1) {

            _betweenRoundUITextReferences[0].SetActive(false); //Initial Text Inactive

            _betweenRoundUITextReferences[1].SetActive(false); //Secondary Text Inactive

            _betweenRoundUITextReferences[2].SetActive(false); //Tertiary Text Inactive

            _betweenRoundUICountdownBarRef.transform.parent.gameObject.SetActive(false); //Countdown Bar Inactive

        } else {

            _betweenRoundUITextReferences[0].SetActive(true); //Initial Text Active

            _betweenRoundUITextReferences[1].SetActive(false); //Secondary Text Inactive

            _betweenRoundUITextReferences[2].SetActive(false); //Tertiary Text Inactive

            _betweenRoundUICountdownBarRef.transform.parent.gameObject.SetActive(false); //Countdown Bar Inactive

        }

        this.gameObject.GetComponent<GameManager>().PlayMenuSound(1); //YAME sound plays

        yield return new WaitForSeconds(1.0f); //Wait

        _betweenRoundUITextReferences[0].SetActive(false); //Initial Text Inactive

        _betweenRoundUITextReferences[1].SetActive(true);  //Secondary Text Active

        _betweenRoundUICountdownBarRef.transform.parent.gameObject.SetActive(true); //Countdown Bar Active

        //Tertiary Text remains Inactive

        StartCoroutine(RoundStartCountdownTimer(duration)); //Start Round Begin Countdown

    }

    private IEnumerator RoundStartCountdownTimer(float duration) //Counts down to start of round, and displays the countdown in a decreasing progress bar - starts tertiary text display sequence at end
    {

        InvokeRepeating("AdjustCountdownBarFillAmount", 0.0f, 0.5f * duration * Time.deltaTime);

        yield return new WaitForSeconds(duration);

        CancelInvoke("AdjustCountdownBarFillAmount");

        StartCoroutine(InBetweenRoundTertiaryTextTimer());

        this.gameObject.SendMessage("RunGame");

    }

    void AdjustCountdownBarFillAmount()
    {

        _countdownDuration -= Time.deltaTime;

        _betweenRoundUICountdownBarRef.GetComponent<Image>().fillAmount = (_countdownDuration / _maxCountdownDuration);

    }

    private IEnumerator InBetweenRoundTertiaryTextTimer() //Displays round start text briefly, then resets In Between Round UI and closes it (HAJIME)
    {
        _betweenRoundUITextReferences[0].SetActive(false); //Initial Text Inactive

        _betweenRoundUITextReferences[1].SetActive(false); //Secondary Text Inactive

        _betweenRoundUITextReferences[2].SetActive(true); //Tertiary Text Active

        _betweenRoundUICountdownBarRef.transform.parent.gameObject.SetActive(false); //Countdown Bar Inactive

        this.gameObject.GetComponent<GameManager>().PlayMenuSound(0); //HAJIME sound plays

        yield return new WaitForSeconds(1.0f); //Wait

        _betweenRoundUITextReferences[0].SetActive(true); //Initial Text Active

        _betweenRoundUITextReferences[1].SetActive(false);  //Secondary Text Inactive

        _betweenRoundUITextReferences[2].SetActive(false); //Tertiary Text Inactive

        _betweenRoundUICountdownBarRef.transform.parent.gameObject.SetActive(true); //Countdown Bar Active

        _ContainerReferences[6].SetActive(false); //Container Inactive/Closed

    }

    private IEnumerator EndScreenDisplay()
    {

        _ContainerReferences[6].SetActive(true);

        _betweenRoundUITextReferences[0].SetActive(true); //Initial Text Active (SHOW YAME)

        _betweenRoundUITextReferences[1].SetActive(false); //Secondary Text Inactive (HIDE since in Container 6)

        _betweenRoundUITextReferences[2].SetActive(false); //Tertiary Text Inactive (HIDE since in Container 6)

        _betweenRoundUICountdownBarRef.transform.parent.gameObject.SetActive(false); //Countdown Bar Inactive (HIDE since in Container 6)

        yield return new WaitForSeconds(1.0f);

        _resultsTextReferences[0].GetComponent<TextMeshProUGUI>().text = "ROUNDS CLEARED: " + (this.gameObject.GetComponent<GameManager>().currentRound - 1);

        _resultsTextReferences[1].GetComponent<TextMeshProUGUI>().text = "FOES DEFEATED: " + this.gameObject.GetComponent<GameManager>().totalEnemiesKilled;

        _resultsTextReferences[2].GetComponent<TextMeshProUGUI>().text = "DAMAGE DEALT: " + Mathf.Floor(this.gameObject.GetComponent<GameManager>().totalDamageDealt); //Floor functions sets to int below for readability

        _betweenRoundUICountdownBarRef.transform.parent.gameObject.SetActive(false); //Countdown Bar Active (Reset Container 6 State)

        _ContainerReferences[7].SetActive(false);

        _ContainerReferences[6].SetActive(false);

        _ContainerReferences[5].SetActive(true);

    }

    void OpenEndGameScreen() //To be used in SendMessage from GameManager
    {

        _currentMenuIndex = 0;

        playing = false;

        _onMainMenu = false;

        this.gameObject.GetComponent<GameManager>().PlayMenuSound(1); //YAME sound plays

        StartCoroutine("EndScreenDisplay");

    }

}
