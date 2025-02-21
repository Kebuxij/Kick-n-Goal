using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public float maxPullDistance = 2f; // Максимальная длина натягивания
    public float forceMultiplier = 10f; // Множитель силы выстрела
    public float minPullDistance = 0.5f; // Минимальная длина натягивания

    private Vector2 startPosition; // Исходная позиция фишки
    private bool isDragging = false; // Проверка, перетягивает ли игрок фишку
    private LineRenderer lineRenderer; // Для отображения индикатора натягивания
    private bool canInteract = true; // Флаг для проверки взаимодействия

    void Start()
    {
        // Сохранить начальную позицию фишки
        startPosition = transform.position;

        // Добавить LineRenderer для индикатора
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;

        lineRenderer.sortingOrder = 10; // Значение выше других объектов
    }

    void Update()
    {
        if (isDragging)
        {
            Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)transform.position;

            // Ограничить длину натягивания
            if (direction.magnitude > maxPullDistance)
            {
                direction = direction.normalized * maxPullDistance;
            }

            // Позиция линии от текущей позиции фишки до положения мыши
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, transform.position); // Текущая позиция фишки
                lineRenderer.SetPosition(1, transform.position + (Vector3)direction);
            }
        }
    }

    void OnMouseDown()
    {
        if (!enabled || !canInteract) return; // Если скрипт выключен или взаимодействие запрещено, ничего не делаем

        if (!IsPointerOverUIObject()) // Проверяем, что игрок не нажимает на UI-элементы
        {
            isDragging = true;
            if (lineRenderer != null) lineRenderer.enabled = true;

            // Обновляем стартовую позицию на текущую позицию фишки
            startPosition = transform.position;
        }
    }

    void OnMouseUp()
    {
        if (!enabled || !canInteract) return;

        if (isDragging)
        {
            isDragging = false;
            if (lineRenderer != null) lineRenderer.enabled = false;

            Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = mousePosition - (Vector2)transform.position;

            if (direction.magnitude > maxPullDistance)
            {
                direction = direction.normalized * maxPullDistance;
            }

            // Проверка минимальной длины натягивания
            if (direction.magnitude < minPullDistance)
            {
                Debug.Log("Слишком малое натягивание, объект не двинется.");
                return;
            }

            // Применить силу к Rigidbody
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(-direction * forceMultiplier, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogError("На объекте отсутствует компонент Rigidbody2D!");
            }
        }
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

    // Включение/отключение возможности взаимодействия
    public void SetInteractable(bool value)
    {
        canInteract = value;
    }
}
