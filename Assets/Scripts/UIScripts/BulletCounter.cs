using UnityEngine;

public class BulletCounter : MonoBehaviour
{
    void OnGUI()
{
    GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

    // Definir el estilo del texto
    GUIStyle style = new GUIStyle();
    style.fontSize = 16;
    style.normal.textColor = Color.white;

    // Mostrar el número de balas en pantalla
    Rect rect = new Rect(10, 10, 200, 40); // Posición y tamaño del contador
    GUI.Label(rect, "Balas en pantalla: " + bullets.Length, style);
}
   
}
