using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _menuItems = new GameObject[5];

    [SerializeField] private Sprite[] _menuItemSprites = new Sprite[2];

    [SerializeField] private GameObject[] _ContainerReferences = new GameObject[5];

    private int _currentMenuIndex;

    public bool _playing;

    private bool _onMainMenu;

    // Start is called before the first frame update
    void Start()
    {
        
        _currentMenuIndex = 0;

        _playing = false;

        _onMainMenu = true;

    }

    // Update is called once per frame
    void Update()
    {
        
        NavigateMenu();
        
    }

    void NavigateMenu()
    {

        if(!_playing)
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
                if(_onMainMenu)
                    SelectOption(_currentMenuIndex);
                else
                    OpenMainMenu();
            

    }

    int SwitchMenuItem(bool UpOrDown) //If recieves true, selected menu item-1 [MOVES UP MENU - bottom to top] (if false, menu item+1)
    {
        if(UpOrDown && _currentMenuIndex == 0)
            return 4;
        else if(!UpOrDown && _currentMenuIndex == 4)
            return 0;
        else if(UpOrDown)
            return _currentMenuIndex-1;
        else
            return _currentMenuIndex+1;

    }

    void SelectOption(int index)
    {

        if(index == 0) {
            
            StartGame(); 

            _playing = true;

        }
            
        else if(index == 1) {

            OpenTutorial();

            _onMainMenu = false;

        }
            
        else if(index == 2) {

            OpenStory();

            _onMainMenu = false;

        }
            
        else if(index == 3) {

            OpenCredits();

            _onMainMenu = false;

        }
            
        else if(index == 4)
            Application.Quit();

    }

    void StartGame()
    {
        
        _ContainerReferences[0].SetActive(false);
        this.gameObject.GetComponent<GameManager>().StartGame();

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

        _onMainMenu = true;

    }

}
