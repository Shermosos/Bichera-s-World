using UnityEngine;
using UnityEngine.InputSystem;

public class movimientoCoche : MonoBehaviour
{
    public float velocidad = 5f;

    void Start()
    {

    }

    void Update()
    {
        float movimientoHorizontal = 0f;
        float movimientoVertical = 0f;

        if (Keyboard.current.leftArrowKey.isPressed)
        {
            movimientoHorizontal = -1f;
        }

        if (Keyboard.current.rightArrowKey.isPressed)
        {
            movimientoHorizontal = 1f;
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            movimientoVertical = 1f;
        }

        if (Keyboard.current.downArrowKey.isPressed)
        {
            movimientoVertical = -1f;
        }

        Vector3 movimiento = new Vector3(
            movimientoHorizontal,
            0f,
            movimientoVertical
        );

        transform.Translate(movimiento * velocidad * Time.deltaTime);
    }
}