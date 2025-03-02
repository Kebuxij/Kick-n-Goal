using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider2D))] // Убедимся, что на объекте есть 2D-коллайдер
public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset; // Смещение между объектом и позицией мыши
    private Camera mainCamera; // Ссылка на главную камеру
    private bool isDragging = false; // Флаг перетаскивания
    private bool canInteract = true; // Флаг для проверки взаимодействия

    [SerializeField] private LayerMask obstacleLayer; // Слой препятствий (например, стены)

    void Start()
    {
        // Получаем главную камеру
        mainCamera = Camera.main;
        Debug.Log("камера есть" + mainCamera);

        // Проверяем, найдена ли камера
        if (mainCamera == null)
        {
            Debug.LogError("1-Main Camera отсутствует. Текущая mainCamera: " + Camera.main);
            return;
        }
    }

    void Update()
    {
        if (Camera.main == null)
        {
            Debug.LogError("!!! Camera.main стала NULL в Update() !!!");
        }
    }

    void OnMouseDown()
    {
        if (!enabled || !canInteract) return; // Если скрипт выключен или взаимодействие запрещено, ничего не делаем

        if (!IsPointerOverUIObject()) // Проверяем, что игрок не нажимает на UI-элементы
        {
            mainCamera = Camera.main;
            // Проверяем, найдена ли камера
            if (mainCamera == null)
            {
                Debug.LogError("2-Main Camera отсутствует. Текущая mainCamera: " + Camera.main);
                return;
            }

            // Вычисляем смещение между позицией объекта и позиции мыши
            offset = transform.position - GetMouseWorldPosition();
            isDragging = true;
        }
    }

    void OnMouseDrag()
    {
        // Если объект находится в состоянии перетаскивания
        if (isDragging)
        {
            mainCamera = Camera.main;
            // Проверяем, есть ли камера
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera отсутствует. Событие OnMouseDrag не может быть обработано. 2");
                return;
            }

            // Текущая позиция объекта
            Vector3 currentPos = transform.position;

            // Целевая позиция объекта, исходя из позиции мыши
            Vector3 targetPos = GetMouseWorldPosition() + offset;

            // Проверяем путь к целевой позиции и определяем безопасную позицию
            Vector3 safePosition = GetSafePosition(currentPos, targetPos);

            // Перемещаем объект в безопасную позицию
            transform.position = safePosition;
        }
    }

    void OnMouseUp()
    {
        // Завершаем перетаскивание
        isDragging = false;
    }

    private Vector3 GetMouseWorldPosition()
    {
        mainCamera = Camera.main;
        // Проверяем, есть ли камера
        if (mainCamera == null)
        {
            Debug.LogError("Main Camera отсутствует. Невозможно получить мировую позицию мыши.");
            return Vector3.zero;
        }

        // Получаем позицию мыши в пикселях на экране
        Vector3 mousePosition = Input.mousePosition;

        // Задаём Z-координату для преобразования из экрана в мировую позицию
        mousePosition.z = Mathf.Abs(mainCamera.transform.position.z);

        // Преобразуем экранные координаты в мировые
        return mainCamera.ScreenToWorldPoint(mousePosition);
    }

    private Vector3 GetSafePosition(Vector3 currentPos, Vector3 targetPos)
    {
        // Вычисляем направление движения
        Vector2 direction = (targetPos - currentPos).normalized;

        // Вычисляем расстояние до целевой позиции
        float distance = Vector2.Distance(currentPos, targetPos);

        // Запускаем луч для проверки препятствий
        RaycastHit2D hit = Physics2D.Raycast(currentPos, direction, distance, obstacleLayer);

        if (hit.collider != null)
        {
            // Если препятствие найдено, возвращаем позицию чуть перед ним
            return hit.point - (Vector2)direction * 0.01f; // Небольшое смещение, чтобы избежать пересечения
        }

        // Если препятствий нет, возвращаем целевую позицию
        return targetPos;
    }

    // Проверка, находится ли указатель над UI-объектом
    private bool IsPointerOverUIObject()
    {
        if (EventSystem.current == null)
        {
            Debug.LogError("В сцене отсутствует EventSystem!");
            return false;
        }

        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    public void SetInteractable(bool value)
    {
        canInteract = value;
    }
}
