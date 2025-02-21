using UnityEngine;
using System.Collections;

public class StopDetection : MonoBehaviour
{
    public Rigidbody2D rb;
    public float stopThreshold = 0.05f; // Порог скорости для остановки
    public float checkTime = 0.5f; // Время ожидания перед проверкой
    private bool isChecking = false;
    private bool isDragging = false;
    //public Vector3 _startPosition;

    void Start()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }


        // Если всё ещё null, выводим предупреждение
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D не найден! Убедись, что он добавлен на объект.");
        }
    }

    void Update()
    {
        // Не проверяем остановку во время перетаскивания
        if (isDragging)
        {
            Debug.LogWarning("Иди гуляй вася");
            return;
        }

        if (!isChecking && rb.linearVelocity.magnitude > stopThreshold)
        {
            StartCoroutine(CheckIfStopped());
        }
    }

    IEnumerator CheckIfStopped()
    {
        isChecking = true;
        yield return new WaitForSeconds(checkTime);

        // Проверяем скорость после ожидания
        if (rb.linearVelocity.magnitude <= stopThreshold)
        {
            GameManager.Instance.ZaWarudo();
            OnStopped();
        }

        isChecking = false;
    }

    void OnStopped()
    {
        if (gameObject.CompareTag("Player1"))
        {
            Debug.Log("Object with tag 'Player1' has stopped moving");
            Debug.Log("Script is now: " + this.enabled);
            GameManager.Instance.ChangeState(GameManager.GameState.DiceRollingPlayerOne);
            //GameManager.Instance.EndPlayer1Turn();
            this.enabled = false;
        }

        if (gameObject.CompareTag("Player2"))
        {
            Debug.Log("Object with tag 'Player2' has stopped moving");
            Debug.Log("Script is now: " + this.enabled);
            GameManager.Instance.ChangeState(GameManager.GameState.DiceRollingPlayerTwo);
            //GameManager.Instance.EndPlayer2Turn();
            this.enabled = false;
        }
    }
    /*
    // Начало перетаскивания
    void OnMouseDown()
    {
        isDragging = true;
        _startPosition = transform.position;
    }

    // Конец перетаскивания
    void OnMouseUp()
    {
        isDragging = false;

        if (_startPosition != transform.position)
        {


        } else
        {
            Debug.Log("Объект не был сдвинут с места");
        }
    }
    */
}