using UnityEngine;
using System.Collections;

public class PlayerSelectionOne : MonoBehaviour
{
    public Color normalColor = Color.white;       // Цвет по умолчанию
    public Color highlightColor = Color.yellow;  // Цвет при наведении
    public Color selectedColor = Color.green;    // Цвет при выборе

    private SpriteRenderer spriteRenderer;       // Компонент SpriteRenderer
    public static PlayerSelectionOne selectedObject; // Ссылка на текущий выбранный объект
    private bool isSelected = false;             // Флаг, выбран ли объект
    private Collision2D collision;               // Компонент Collision2D
    private bool hasTouchedEnemyFloor = false;        // Дотронулся ли пола
    private bool hasTouchedTeamFloor = false;

    void Start()
    {
        // Проверяем, находится ли объект в слое "SelectableOne"
        if (gameObject.layer != LayerMask.NameToLayer("SelectableOne"))
        {
            //Debug.LogWarning($"Объект {gameObject.name} не находится в слое 'SelectableOne'. Скрипт отключён.");
            enabled = false;
            return;
        }

        // Пытаемся получить компонент SpriteRenderer
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer не найден на объекте {gameObject.name}. Скрипт отключён.");
            enabled = false;
            return;
        }

        // Устанавливаем начальный цвет
        spriteRenderer.color = normalColor;
    }

    void OnMouseEnter()
    {
        if (!enabled) return; // Блокировка выбора, если скрипт выключен
        if (isSelected || spriteRenderer == null) return;

        // Меняем цвет на цвет выделения при наведении
        spriteRenderer.color = highlightColor;
    }

    void OnMouseExit()
    {
        if (!enabled) return; // Блокировка выбора, если скрипт выключен
        if (isSelected || spriteRenderer == null) return;

        // Возвращаем цвет к нормальному при убирании мышки
        spriteRenderer.color = normalColor;
    }

    void OnMouseDown()
    {
        if (!enabled) return; // Блокировка выбора, если скрипт выключен
        if (spriteRenderer == null) return;

        // Если уже есть выбранный объект, снимаем выбор с него
        if (selectedObject != null && selectedObject != this)
        {
            selectedObject.Deselect();
        }

        // Делаем текущий объект выбранным
        Select();
    }

    public void Select()
    {
        spriteRenderer.color = selectedColor;
        if (hasTouchedTeamFloor && !hasTouchedEnemyFloor)
        {
            PlayerMovement plMovement = GetComponent<PlayerMovement>();
            isSelected = true;
            selectedObject = this;

            // Выключить движение у всех остальных объектов
            foreach (PlayerSelectionOne obj in Object.FindObjectsByType<PlayerSelectionOne>(FindObjectsSortMode.None))
            {
                if (obj != this)
                {
                    obj.Deselect();
                }
            }

            StartCoroutine(EnableMovementDelayed(plMovement));
        }

        if (hasTouchedEnemyFloor && !hasTouchedTeamFloor)
        {
            DragAndDrop _dragNDrop = GetComponent<DragAndDrop>();
            isSelected = true;
            selectedObject = this;

            // Выключить движение у всех остальных объектов
            foreach (PlayerSelectionOne obj in Object.FindObjectsByType<PlayerSelectionOne>(FindObjectsSortMode.None))
            {
                if (obj != this)
                {
                    obj.Deselect();
                }
            }

            StartCoroutine(EnableDragAndDropDelayed(_dragNDrop));
        }

        if (hasTouchedTeamFloor && hasTouchedEnemyFloor)
        {
            PlayerMovement plMovement = GetComponent<PlayerMovement>();
            isSelected = true;
            selectedObject = this;

            // Выключить движение у всех остальных объектов
            foreach (PlayerSelectionOne obj in Object.FindObjectsByType<PlayerSelectionOne>(FindObjectsSortMode.None))
            {
                if (obj != this)
                {
                    obj.Deselect();
                }
            }

            StartCoroutine(EnableMovementDelayed(plMovement));
        }

        if (!hasTouchedTeamFloor && !hasTouchedEnemyFloor)
        {
            Debug.LogError("Игрок ничего не пересекает (PlayerSelectionOne)");
        }

        Debug.Log($"Выбран объект: {gameObject.name}");
    }

    public void Deselect()
    {
        PlayerMovement plMovement = GetComponent<PlayerMovement>();
        DragAndDrop _dragNDrop = GetComponent<DragAndDrop>();
        HasMoved _objectIsMoved = GetComponent<HasMoved>();
        StopDetection _stopDetection = GetComponent<StopDetection>();
        StopDragging _stopDragging = GetComponent<StopDragging>();

        if (plMovement != null && plMovement)
        {
            plMovement.enabled = false;
        }

        if (_dragNDrop != null && _dragNDrop)
        {
            _dragNDrop.enabled = false;
        }

        if (_objectIsMoved != null && _objectIsMoved)
        {
            _objectIsMoved.enabled = false;
        }

        /*
        if (_stopDetection != null && _stopDetection==true)
        {
            _stopDetection.enabled = false;
        }
        */

        if (_stopDragging != null && _stopDragging)
        {
            _stopDragging.enabled = false;
        }

        isSelected = false;
        spriteRenderer.color = normalColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Terra2"))
        {
            hasTouchedEnemyFloor = true;
            //Debug.Log($"Игрок {gameObject.name} пересекает Terra2");
        }

        if (other.CompareTag("Terra1"))
        {
            hasTouchedTeamFloor = true;
            //Debug.Log($"Игрок {gameObject.name} пересекает Terra1");
        }

        if (hasTouchedEnemyFloor && hasTouchedTeamFloor)
        {
            Debug.Log($"Игрок {gameObject.name} пересекает одновременно Terra1 и Terra2!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Terra2"))
        {
            hasTouchedEnemyFloor = false;
            //Debug.Log($"Игрок {gameObject.name} покинул Вражескую территорию! (Terra1)");
        }

        if (other.CompareTag("Terra1"))
        {
            hasTouchedTeamFloor = false;
            //Debug.Log($"Игрок {gameObject.name} покинул свою территорию! (Terra2)");
        }
    }

    IEnumerator EnableMovementDelayed(PlayerMovement movement)
    {
        HasMoved _objectIsMoved = GetComponent<HasMoved>();
        yield return new WaitForSeconds(0.5f);
        if (movement != null)
        {
            _objectIsMoved.enabled = true;
            movement.enabled = true;
        }
    }
    /*
    IEnumerator EnableDragAndDropDelayed(DragAndDrop _dragdrop)
    {
        HasMoved _objectIsMoved = GetComponent<HasMoved>();
        yield return new WaitForSeconds(0.5f);
        if (_dragdrop != null)
        {
            _objectIsMoved.enabled = true;
            _dragdrop.enabled = true;
        }
    }
    */

    IEnumerator EnableDragAndDropDelayed(DragAndDrop _dragdrop)
    {
        StopDragging _objectStoppedDragging = GetComponent<StopDragging>();
        yield return new WaitForSeconds(0.5f);
        if (_dragdrop != null)
        {
            _objectStoppedDragging.enabled = true;
            _objectStoppedDragging._canProcessEvent = true;
            _dragdrop.enabled = true;
        }
    }

    void OnDisable()
    {
        if (selectedObject == this)
        {
            selectedObject = null;
        }
    }
}
