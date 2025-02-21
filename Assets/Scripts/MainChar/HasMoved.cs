using UnityEngine;

public class HasMoved : MonoBehaviour
{
    private Vector3 initialPosition;
    private bool isMouseHeld = false;
    protected PlayerSelectionOne _plSelectionOne;
    protected PlayerSelectionTwo _plSelectionTwo;
    protected StopDetection _stopDetection;

    void Start()
    {
        // Запоминаем начальную позицию объекта
        //initialPosition = transform.position;

        _plSelectionOne = GetComponent<PlayerSelectionOne>();
        _plSelectionTwo = GetComponent<PlayerSelectionTwo>();

        //Debug.LogWarning("initialPosition = " + initialPosition);
        //Debug.LogWarning("transform.position = " + transform.position);
    }
    
    void OnEnable()
    {
        //Debug.LogWarning("OnEnable сработал!!!");
        initialPosition = transform.position;
    }

    void Update()
    {
        // Проверяем, изменилось ли положение объекта
        if (!isMouseHeld)
        {
            if (initialPosition != transform.position)
            {
                _stopDetection = GetComponent<StopDetection>();
                Debug.LogWarning("Я есть сработать!");

                if (_stopDetection.enabled == false)
                {
                    Debug.LogWarning("Эхуууууу, я работаююю!!!");
                    _stopDetection.enabled = true;
                }

                if (gameObject.tag == "Player1")
                {
                    GameManager.Instance.ToggleComponentsPlayerOne();
                    _plSelectionOne.Deselect();
                    //transform.position = new Vector3(Mathf.Round(transform.position.x * 100f) / 100f, Mathf.Round(transform.position.y * 100f) / 100f, transform.position.z);
                    Debug.Log("Объект Player1 не выбран");
                }

                if (gameObject.tag == "Player2")
                {
                    GameManager.Instance.ToggleComponentsPlayerTwo();
                    _plSelectionTwo.Deselect();
                    //transform.position = new Vector3(Mathf.Round(transform.position.x * 100f) / 100f, Mathf.Round(transform.position.y * 100f) / 100f, transform.position.z);
                    Debug.Log("Объект Player2 не выбран");
                }

                //
                //
                //
                //+//  Сделать отключение PlayerSelectionOne и PlayerSelectionTwo (см. пример его включения в GameManager)
                //+//  Сделать проверку на остановку объекта
                //  После остановки зациклить это всё, чтобы игра фигачила без остановки
                //  Сделать нормальные ворота
                //  Потом добавить в GameManager в Update() проверку на забивание гола в ворота
                //  Ну и вишенка на тортике сделать кубик на каждой из сторон игрока а так же возможность повторно ходить
                //  Добавить меню запуска игры и настройки
                //  Сделать звуки в игре
                //  Отпалировать это всё красивыми текстурками и фан-сервисом
                //
                //
                //  После выполнения всего вышеперечисленного добавить в игру донат, скины и рекламу, а так же её отключение
                //  Сжать максимально игру хотя бы до 150мб
                //
                //
                //

                initialPosition = transform.position; // Обновляем позицию для дальнейшего отслеживания
            }
        }
    }

    void OnMouseDown()
    {
        if (!enabled) return; // Если скрипт выключен или взаимодействие запрещено, ничего не делаем
        isMouseHeld = true;
    }

    void OnMouseUp()
    {
        if (!enabled) return; // Если скрипт выключен или взаимодействие запрещено, ничего не делаем
        isMouseHeld = false;
    }
}
