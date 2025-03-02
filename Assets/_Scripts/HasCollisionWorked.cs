using UnityEngine;

public class HasCollisionWorked : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player1")) // Замените "Enemy" на нужный тег
        {
            SoundManager.Instance.PlayRandomCollisionSound();
        }

        if (collision.gameObject.CompareTag("Player2")) // Замените "Enemy" на нужный тег
        {
            SoundManager.Instance.PlayRandomCollisionSound();
        }

        if (collision.gameObject.CompareTag("Border")) // Замените "Enemy" на нужный тег
        {
            SoundManager.Instance.PlayRandomCollisionSound();
        }
    }
}
