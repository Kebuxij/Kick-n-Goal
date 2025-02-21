using UnityEngine;

public class Check : MonoBehaviour
{
    public Camera mainCamera; // Камера, которая "смотрит" на объект
    public LayerMask diceLayer; // Слой, к которому принадлежит объект
    public int SideNumber;

    // Update is called once per frame
    public void CheckDiceSide()
    {
        // Найти точку, соответствующую половине ширины экрана
        Vector3 screenPoint = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        // Создать луч из этой точки
        Ray ray = mainCamera.ScreenPointToRay(screenPoint);

        // Визуализировать луч в окне Scene
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);

        // Проверить пересечения с объектами
        RaycastHit2D hit2D = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, diceLayer);
        if (hit2D.collider != null)
        {
            switch (hit2D.collider.gameObject.name)
            {
                case "1":
                    SideNumber = 1;
                    break;
                case "2":
                    SideNumber = 2;
                    break;
                case "3":
                    SideNumber = 3;
                    break;
                case "4":
                    SideNumber = 4;
                    break;
                case "5":
                    SideNumber = 5;
                    break;
                case "6":
                    SideNumber = 6;
                    break;
                default:
                    Debug.Log("Side has't been found");
                    break;
            }

            Debug.Log("Hit 2D object: " + hit2D.collider.gameObject.name);
        }
        else
        {
            Debug.Log("No hit");
        }
    }
}