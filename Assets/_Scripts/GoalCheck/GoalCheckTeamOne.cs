using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GoalCheckTeamOne : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.LogWarning("LOLOLOLOL");
        if (other.CompareTag("Player2"))
        {
            // Проверяем есть ли соприкоснувшийся игрок в списке игроков
            if (GameManager.Instance.playersTwo.Contains(other.gameObject))
            {
                // Удаляем игрока из списка
                GameManager.Instance.playersTwo.Remove(other.gameObject);
                Debug.Log($"Объект {other.gameObject} удалён из списка");

                // Уничтожаем объект на сцене
                Destroy(other.gameObject);
                Debug.Log($"Объект {other.gameObject.name} уничтожен");

                // Начинаем следующий ход
                GameManager.Instance.EndPlayer2Turn();
            }
            else
            {
                Debug.LogError($"Объект {other.gameObject.name} не был найден в списке игроков");
            }

            // Засчитываем очко забившей команде
            GameManager.Instance.CountGoalTeamTwo();
        }
        else if (other.CompareTag("Player1"))
        {
            return;
        }
        else if (other.CompareTag("Neutral"))
        {
            //GameManager.Instance.CountGoalTeamTwo();      x2
            return;
        }
    }
}
