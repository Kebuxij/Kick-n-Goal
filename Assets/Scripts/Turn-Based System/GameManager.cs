using UnityEngine;
using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{

    //===============

    public static GameManager Instance; // Синглтон для глобального доступа

    //===============

    public Rigidbody2D rb;
    public new MeshRenderer renderer;
    protected new BoxCollider collider;

    private int _goalOne;
    private int _goalTwo;

    private int _inRowOne;
    private int _inRowTwo;

    protected DiceRoller dRoll;
    protected Check checkSide;
    protected ScoreDisplayFade scoreDisplayFade;

    public List<GameObject> playersOne = new List<GameObject>();
    public List<GameObject> playersTwo = new List<GameObject>();

    protected bool isPlayerOneTurn = false;
    protected bool isPlayerTwoTurn = false;

    private Dictionary<GameObject, Vector3> previousPositionsPlayerOne = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Vector3> previousPositionsPlayerTwo = new Dictionary<GameObject, Vector3>();

    //public bool PlayerOneDiceRolling = false;
    //public bool PlayerTwoDiceRolling = false;
    //private bool NeutralDiceRolling = false;

    //===============

    public enum GameState
    {
        Start,
        Player1Turn,
        Player2Turn,
        DiceRolling,
        DiceRollingPlayerOne,
        DiceRollingPlayerTwo,
        Victory,
        Defeat
    }

    //===============

    protected GameState CurrentState;

    //===============

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //===============

    private void Start()
    {
        dRoll = GameObject.FindGameObjectWithTag("Dice").GetComponent<DiceRoller>();
        checkSide = GameObject.FindGameObjectWithTag("RayCastCam").GetComponent<Check>();
        renderer = GameObject.FindGameObjectWithTag("Dice").GetComponent<MeshRenderer>();

        ChangeState(GameState.Start); // Устанавливаем начальное состояние

        foreach (var _playerOne in playersOne)
        {
            previousPositionsPlayerOne[_playerOne] = _playerOne.transform.position;
        }

        foreach (var _playerTwo in playersTwo)
        {
            previousPositionsPlayerTwo[_playerTwo] = _playerTwo.transform.position;
        }
    }

    //===============

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        HandleState();
    }

    //===============

    private void HandleState()
    {
        switch (CurrentState)
        {
            case GameState.Start:
                HandleStart();
                break;
            case GameState.Player1Turn:
                HandlePlayer1Turn();
                break;
            case GameState.Player2Turn:
                HandlePlayer2Turn();
                break;
            case GameState.DiceRolling:
                HandleDiceRolling();
                break;
            case GameState.DiceRollingPlayerOne:
                HandleDiceRollingPlayerOne();
                break;
            case GameState.DiceRollingPlayerTwo:
                HandleDiceRollingPlayerTwo();
                break;
            case GameState.Victory:
                HandleVictory();
                break;
            case GameState.Defeat:
                HandleDefeat();
                break;
        }
    }

    //===============

    void Update()
    {
        if (dRoll.isCompleted == true)
        {
            if (checkSide.SideNumber != 0)
            {
                HandleReceivedValue();

                if (checkSide.SideNumber % 2 == 0)
                {
                    _inRowOne += 1;
                    StartCoroutine(WaitForCubeUno());

                    //ChangeState(GameState.Player1Turn);
                }
                else
                {
                    _inRowTwo += 1;
                    StartCoroutine(WaitForCubeDos());

                    //ChangeState(GameState.Player2Turn);
                }
            }

            if (checkSide.SideNumber == 0)
            {
                Debug.LogError("Error with Side Number");
            }
        }
        /*
        if (isPlayerOneTurn)
        {
            foreach (var _player in playersOne)
            {
                Vector3 currentPosition = _player.transform.position;

                // Если положение объекта изменилось
                if (currentPosition != previousPositionsPlayerOne[_player])
                {
                    Debug.Log($"{_player.name} начал движение!");
                    previousPositionsPlayerOne[_player] = currentPosition;
                    PlayerSelectionOne plSelection = _player.GetComponent<PlayerSelectionOne>();
                    if (plSelection && plSelection != null)
                    {
                        plSelection.enabled = !plSelection.enabled;
                    }
                    ChangeState(GameState.DiceRolling);
                }
            }
        }

        if (isPlayerTwoTurn)
        {
            foreach (var _player in playersTwo)
            {
                Vector3 currentPosition = _player.transform.position;

                // Если положение объекта изменилось
                if (currentPosition != previousPositionsPlayerTwo[_player])
                {
                    Debug.Log($"{_player.name} начал движение!");
                    previousPositionsPlayerTwo[_player] = currentPosition;
                    PlayerSelectionTwo plSelection = _player.GetComponent<PlayerSelectionTwo>();
                    if (plSelection && plSelection != null)
                    {
                        plSelection.enabled = !plSelection.enabled;
                    }
                    ChangeState(GameState.DiceRolling);
                }
            }
        }
        */
    }

    private void HandleStart()
    {
        Debug.Log("Game Started!");
        // Логика старта игры (инициализация уровня, игроков и т.д.)
        ChangeState(GameState.DiceRolling);
    }

    private void HandlePlayer1Turn()
    {
        isPlayerOneTurn = true;
        //System.Threading.Thread.Sleep(2000);
        Debug.Log("Player's Turn!");
        ToggleComponentsPlayerOne();
        // Логика хода игрок
    }

    private void HandlePlayer2Turn()
    {
        isPlayerTwoTurn = true;
        //System.Threading.Thread.Sleep(2000);
        Debug.Log("Enemy's Turn!");
        ToggleComponentsPlayerTwo();
        // Логика хода врагов
    }

    private void HandleVictory()
    {
        Debug.Log("Victory!");
        // Логика победы
    }

    private void HandleDefeat()
    {
        Debug.Log("Defeat!");
        // Логика поражения
    }

    public void HandleDiceRolling()
    {
        //NeutralDiceRolling = true;
        GameObject targetObjectDice = GameObject.FindWithTag("Dice");
        GameObject targetObjectRayCastCamera = GameObject.FindWithTag("RayCastCam");
        //MeshRenderer renderer = targetObjectDice.GetComponent<MeshRenderer>();

        if (targetObjectDice != null && targetObjectRayCastCamera != null)
        {
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
            worldCenter.z = targetObjectDice.transform.position.z;
            
            targetObjectDice.transform.position = worldCenter;
            targetObjectRayCastCamera.transform.position = worldCenter;

            renderer.enabled = true;

            dRoll.StartDiceRotation();
        }
        else
        {
            Debug.LogError("No object found with tag targetObjectDice or targetObjectRayCastCamera in HandleDiceRolling");
        }

        //Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        //Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        //ChangeState(GameState.DiceRolling);
    }

    public void HandleDiceRollingPlayerOne()
    {
        //PlayerOneDiceRolling = true;
        GameObject _targetObjectDice = GameObject.FindWithTag("Dice");
        GameObject _targetObjectRayCastCamera = GameObject.FindWithTag("RayCastCam");

        if (_targetObjectDice != null && _targetObjectRayCastCamera != null)
        {
            Vector3 _playerOneCenter = new Vector3(Screen.width / 4, Screen.height / 2, 0);
            Vector3 _playerOneWorldCenter = Camera.main.ScreenToWorldPoint(_playerOneCenter);
            _playerOneWorldCenter.z = _targetObjectDice.transform.position.z;

            _targetObjectDice.transform.position = _playerOneWorldCenter;
            _targetObjectRayCastCamera.transform.position = _playerOneWorldCenter;

            renderer.enabled = true;

            dRoll.StartDiceRotation();
        }
        else
        {
            Debug.LogError("No object found with tag targetObjectDice or targetObjectRayCastCamera in HandleDiceRollingPlayerOne");
        }
    }

    public void HandleDiceRollingPlayerTwo()
    {
        //PlayerTwoDiceRolling = true;
        GameObject _targetObjectDice = GameObject.FindWithTag("Dice");
        GameObject _targetObjectRayCastCamera = GameObject.FindWithTag("RayCastCam");

        if (_targetObjectDice != null && _targetObjectRayCastCamera != null)
        {
            Vector3 _playerTwoCenter = new Vector3((Screen.width / 4) * 3, Screen.height / 2, 0);
            Vector3 _playerTwoWorldCenter = Camera.main.ScreenToWorldPoint(_playerTwoCenter);
            _playerTwoWorldCenter.z = _targetObjectDice.transform.position.z;

            _targetObjectDice.transform.position = _playerTwoWorldCenter;
            _targetObjectRayCastCamera.transform.position = _playerTwoWorldCenter;

            renderer.enabled = true;

            dRoll.StartDiceRotation();
        }
        else
        {
            Debug.LogError("No object found with tag targetObjectDice or targetObjectRayCastCamera in HandleDiceRollingPlayerTwo");
        }
    }

    // Пример завершения хода игрока
    public void EndPlayer1Turn()
    {
        foreach (GameObject _objOne in playersOne)
        {
            if (_objOne != null)
            {
                PlayerSelectionOne _plSelection = _objOne.GetComponent<PlayerSelectionOne>();

                if (_plSelection != null && _plSelection)
                {
                    _plSelection.enabled = false;
                }
                else
                {
                    Debug.LogWarning($"PlayerSelectionOne  не найден на объекте {_objOne.name}.");
                }
            }
        }
            ChangeState(GameState.Player2Turn);
    }

    // Пример завершения хода врагов
    public void EndPlayer2Turn()
    {
        foreach (GameObject _objTwo in playersTwo)
        {
            if (_objTwo != null)
            {
                PlayerSelectionTwo _plSelection = _objTwo.GetComponent<PlayerSelectionTwo>();

                if (_plSelection != null && _plSelection)
                {
                    _plSelection.enabled = false;
                }
                else
                {
                    Debug.LogWarning($"PlayerSelectionOne  не найден на объекте {_objTwo.name}.");
                }
            }
        }

        ChangeState(GameState.Player1Turn);
    }


    private void HandleReceivedValue()
    {
        Debug.Log("Received value: " + checkSide.SideNumber);
        dRoll.isCompleted = false;
    }


    bool ExtractNumber(string name, out int result)
    {
        // Поиск всех цифр в строке
        string digits = "";
        foreach (char c in name)
        {
            if (char.IsDigit(c))
            {
                digits += c;
            }
        }

        // Преобразование найденного числа в int
        return int.TryParse(digits, out result);
    }


    public void ToggleComponentsPlayerOne()
    {
        Debug.LogWarning("Sveiki");
        foreach (GameObject objOne in playersOne)
        {
            if (objOne != null)
            {
                // Попытка получить компонент
                PlayerSelectionOne plSelection = objOne.GetComponent<PlayerSelectionOne>();
                if (plSelection != null)
                {
                    plSelection.enabled = !plSelection.enabled;
                    Debug.Log($"Компонент PlayerSelectionOne на {objOne.name} теперь {(plSelection.enabled ? "включён" : "выключен")}.");
                }
                else
                {
                    Debug.LogWarning($"PlayerSelectionOne  не найден на объекте {objOne.name}.");
                }
            }
        }
    }


    public void ToggleComponentsPlayerTwo()
    {
        Debug.LogWarning("Ола");
        foreach (GameObject objTwo in playersTwo)
        {
            if (objTwo != null)
            {
                // Попытка получить компонент
                PlayerSelectionTwo plSelection = objTwo.GetComponent<PlayerSelectionTwo>();
                if (plSelection != null)
                {
                    plSelection.enabled = !plSelection.enabled;
                    Debug.Log($"Компонент PlayerSelectionTwo на {objTwo.name} теперь {(plSelection.enabled ? "включён" : "выключен")}.");
                }
                else
                {
                    Debug.LogWarning($"PlayerSelectionTwo не найден на объекте {objTwo.name}.");
                }
            }
        }
    }

    public void ZaWarudo()
    {
        foreach (GameObject objOne in playersOne)
        {
            rb = objOne.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }    


        foreach (GameObject objTwo in playersTwo)
        {
            rb = objTwo.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }

    public void CountGoalTeamOne()
    {
        _goalOne += 1;
        Debug.LogWarning(_goalOne + ":" + _goalTwo);
        //scoreDisplayFade.DisplayScore(_goalOne, _goalTwo);
    }

    public void CountGoalTeamTwo()
    {
        _goalTwo += 1;
        Debug.LogWarning(_goalOne + ":" + _goalTwo);
        //scoreDisplayFade.DisplayScore(_goalOne, _goalTwo);
    }

    IEnumerator WaitForCubeUno()
    {
        yield return new WaitForSeconds(2.0f);

        if (_inRowOne == 3)
        {
            _inRowOne = 0;
            EndPlayer1Turn();
        }
        else
        {
            _inRowOne = 0;
            //TeleportObjectFarAway(targetObjectDice);
            ChangeState(GameState.Player1Turn);
        }

        renderer.enabled = false;
    }

    IEnumerator WaitForCubeDos()
    {
        yield return new WaitForSeconds(2.0f);

        if (_inRowTwo == 3)
        {
            _inRowTwo = 0;
            EndPlayer2Turn();
        }
        else
        {
            _inRowTwo = 0;
            //TeleportObjectFarAway(targetObjectDice);
            ChangeState(GameState.Player2Turn);
        }

        renderer.enabled = false;
    }

    private void TeleportObjectFarAway(GameObject _teleportedObject)
    {
        if (_teleportedObject != null)
        {
            Vector3 _teleportedObjectCoords = new Vector3(Screen.width * 100000f, Screen.height * 100000f, 0);
            Vector3 _teleportedObjectCoordsToWorldCenter = Camera.main.ScreenToWorldPoint(_teleportedObjectCoords);
            _teleportedObjectCoordsToWorldCenter.z = _teleportedObject.transform.position.z - 30f;

            _teleportedObject.transform.position = _teleportedObjectCoordsToWorldCenter;
        }
    }

    //===============

}
