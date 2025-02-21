using UnityEngine;
using System.Collections;

public class StopDragging : MonoBehaviour
{
    protected Vector3 _startPosition;
    public bool _canProcessEvent = false;

    protected PlayerSelectionOne _plSelectionOne;
    protected PlayerSelectionTwo _plSelectionTwo;

    void Start()
    {
        _plSelectionOne = GetComponent<PlayerSelectionOne>();
        _plSelectionTwo = GetComponent<PlayerSelectionTwo>();
    }

    //private bool isDragging = false;

    // Начало перетаскивания
    void OnMouseDown()
    {
        //isDragging = true;
        if (enabled == false || !_canProcessEvent) return;

        _startPosition = transform.position;
    }

    // Конец перетаскивания
    void OnMouseUp()
    {
        //isDragging = false;
        if (enabled == false || !_canProcessEvent) return;

        if (_startPosition != transform.position)
        {
            if (gameObject.CompareTag("Player1"))
            {
                Debug.Log("Object with tag 'Player1' has stopped moving");
                GameManager.Instance.ToggleComponentsPlayerOne();
                _canProcessEvent = false;
                _plSelectionOne.Deselect();
                Debug.Log("Script is now: " + this.enabled);
                GameManager.Instance.ChangeState(GameManager.GameState.DiceRollingPlayerOne);
                this.enabled = false;
            }

            if (gameObject.CompareTag("Player2"))
            {
                Debug.Log("Object with tag 'Player2' has stopped moving");
                GameManager.Instance.ToggleComponentsPlayerTwo();
                _canProcessEvent = false;
                _plSelectionTwo.Deselect();
                Debug.Log("Script is now: " + this.enabled);
                GameManager.Instance.ChangeState(GameManager.GameState.DiceRollingPlayerTwo);
                this.enabled = false;
            }
        }
        else
        {
            Debug.Log("Объект не был сдвинут с места");
        }
    }
}
