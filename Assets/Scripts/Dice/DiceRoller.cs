using UnityEngine;
using SysRandom = System.Random;
using System.Collections;

public class DiceRoller : MonoBehaviour
{
    public float rotationSpeed = 90f;
    protected Check checkS;
    public bool isCompleted;

    public void StartDiceRotation()
    {
        if (GameManager.Instance.renderer.enabled == false)
        {
            GameManager.Instance.renderer.enabled = true;
        }

        checkS = GameObject.FindGameObjectWithTag("RayCastCam").GetComponent<Check>();
        // Запускаем корутину для вращения
        StartCoroutine(RotateDice());
    }

    private IEnumerator RotateDice()
    {
        SysRandom random = new SysRandom();

        for (int i = 0; i < 30; i++)
        {
            int rotationSide = random.Next(0, 2);
            if (rotationSide == 0)
            {
                transform.Rotate(rotationSpeed, 0, 0);
            }
            else if (rotationSide == 1)
            {
                transform.Rotate(0, 0, rotationSpeed);
            }

            // Задержка в 1 секунду
            yield return new WaitForSeconds(0.05f);
        }

        if (checkS != null)
        {
            checkS.CheckDiceSide();
        }

        isCompleted = true;
        Debug.Log(isCompleted);
    }
}
