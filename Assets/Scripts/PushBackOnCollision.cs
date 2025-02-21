using UnityEngine;

public class PushBackOnCollision : MonoBehaviour
{
    // Отталкивание на 1 пиксель (с учетом масштаба мира)
    private float pushDistance = 5f / 100f; // 1 пиксель = 1/100 юнита

    // Тег или слой, с которым должно произойти столкновение
    public string targetTag = "TargetTag";
    public string targetLayer = "TargetLayer";

    private void OnCollisionStay2D(Collision2D collision)
    {
        bool matchesTag = !string.IsNullOrEmpty(targetTag) && collision.collider.CompareTag(targetTag);
        bool matchesLayer = !string.IsNullOrEmpty(targetLayer) && collision.collider.gameObject.layer == LayerMask.NameToLayer(targetLayer);

        // Если не совпадает ни тег, ни слой, прекращаем обработку
        if (!matchesTag || !matchesLayer)
        {
            return;
        }

        // Получаем направление столкновения
        Vector2 collisionDirection = collision.contacts[0].point - (Vector2)transform.position;
        collisionDirection = collisionDirection.normalized;

        // Вычисляем новое положение
        Vector3 pushBackPosition = transform.position - (Vector3)collisionDirection * pushDistance;

        // Устанавливаем объект в новое положение
        transform.position = pushBackPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool matchesTag = !string.IsNullOrEmpty(targetTag) && collision.collider.CompareTag(targetTag);
        bool matchesLayer = !string.IsNullOrEmpty(targetLayer) && collision.collider.gameObject.layer == LayerMask.NameToLayer(targetLayer);

        // Если не совпадает ни тег, ни слой, прекращаем обработку
        if (!matchesTag || !matchesLayer)
        {
            return;
        }

        // Получаем направление столкновения
        Vector2 collisionDirection = collision.contacts[0].point - (Vector2)transform.position;
        collisionDirection = collisionDirection.normalized;

        // Вычисляем новое положение
        Vector3 pushBackPosition = transform.position - (Vector3)collisionDirection * pushDistance;

        // Устанавливаем объект в новое положение
        transform.position = pushBackPosition;
    }
}
