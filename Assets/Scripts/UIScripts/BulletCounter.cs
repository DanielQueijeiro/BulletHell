using UnityEngine;

public class BulletCounter : MonoBehaviour
{
    void OnGUI()
{
    // Encontrar todos los objetos con la etiqueta "Bullet"
    GameObject[] bullets = GameObject.FindGameObjectsWithTag("Bullet");

    // Definir el estilo del texto
    GUIStyle style = new GUIStyle();
    style.fontSize = 24;
    style.normal.textColor = Color.white;

    // Mostrar el número de balas en pantalla
    Rect rect = new Rect(10, 10, 200, 50); // Posición y tamaño del contador
    GUI.Label(rect, "Balas en pantalla: " + bullets.Length, style);
}
   
}
