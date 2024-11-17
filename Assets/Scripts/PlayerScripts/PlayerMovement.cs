using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float normalSpeed = 6f;
    public float slowSpeed = 4f;

    public KeyCode switchKey;

    // Límites de movimiento
    private float horizontalMin = -3.4f;
    private float horizontalMax = 13f;
    private float verticalMin = -4.5f;
    private float verticalMax = 4.5f;

    void Update()
    {
        // Calcular el movimiento
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) 
                           * (Input.GetKey(switchKey) ? slowSpeed : normalSpeed) 
                           * Time.deltaTime;

        // Actualizar la posición
        Vector3 newPosition = transform.position + movement;

        // Aplicar límites
        newPosition.x = Mathf.Clamp(newPosition.x, horizontalMin, horizontalMax);
        newPosition.y = Mathf.Clamp(newPosition.y, verticalMin, verticalMax);

        // Establecer la nueva posición
        transform.position = newPosition;
    }
}
