using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TextMeshProUGUI endGameText;
    public GameObject player;
    public GameObject boss;

    void Awake()
    {
        // Singleton para que solo haya una instancia de GameManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void EndGame(string result, GameObject loser)
    {
        // Mostrar el resultado en pantalla
        endGameText.text = result;
        endGameText.gameObject.SetActive(true);

        // Destruir al objeto que ha perdido
        Destroy(loser);

        // Pausar el juego
        Time.timeScale = 0f;
    }
}