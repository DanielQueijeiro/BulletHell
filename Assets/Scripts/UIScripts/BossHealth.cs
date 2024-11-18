using UnityEngine;

public class BossHealth : MonoBehaviour
{
    public GameObject boss; // Arrastra aquí el objeto jefe desde el inspector
    private Boss bossHealth;


    void Update()
    {
        // Obtener el componente de salud del jefe
        if (boss != null)
        {
            bossHealth = boss.GetComponent<Boss>();
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.white;
        if (bossHealth != null)
        {
            // Mostrar la vida del jefe en pantalla
            Rect rect = new Rect(10, 90, 200, 40); // Posición y tamaño del contador
            GUI.Label(rect, "Salud del jefe: " + bossHealth.hp + "/" + bossHealth.startHp, style);
        }
    }
}