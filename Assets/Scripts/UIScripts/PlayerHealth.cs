using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    public GameObject player; // Arrastra aquí el objeto jugador desde el inspector
    private Player playerHealth;


    void Start()
    {
        // Obtener el componente de salud del jugador
        if (player != null)
        {
            playerHealth = player.GetComponent<Player>();
        }
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.white;
        if (playerHealth != null)
        {
            // Mostrar la vida del jugador en pantalla
            Rect rect = new Rect(10, 50, 200, 40); // Posición y tamaño del contador
            GUI.Label(rect, "Salud del jugador: " + playerHealth.hp + "/" + playerHealth.startHp, style);
        }
    }
}
