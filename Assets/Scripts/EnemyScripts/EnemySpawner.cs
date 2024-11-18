using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Tiburón Settings")]
    public GameObject sharkPrefab;     // Prefab del tiburón
    public Transform spawnPoint;      // Punto de aparición de enemigos
    public int maxSharks = 3;         // Número máximo de tiburones
    private int sharksDefeated = 0;   // Contador de tiburones derrotados

    [Header("Otro Enemigo Settings")]
    public GameObject BossPrefab;  // Prefab del otro enemigo
    private Boss bossHealth;  // Referencia al script del jefe

    public float respawnDelay = 1f;

    void OnEnable()
    {
        Shark.OnSharkDefeated += HandleSharkDefeat;
    }

    void OnDisable()
    {
        Shark.OnSharkDefeated -= HandleSharkDefeat;
    }

    void Start()
    {
        SpawnShark();
    }

    void HandleSharkDefeat(Shark defeatedShark)
    {
        sharksDefeated++;

        if (sharksDefeated < maxSharks)
        {
            // Si aún no hemos derrotado a todos los tiburones, generar otro
            Invoke(nameof(SpawnShark), respawnDelay);
        }
        else
        {
            // Si se alcanzó el máximo, generar el jefe
            Invoke(nameof(SpawnBoss), respawnDelay);
        }
    }

    void SpawnShark()
    {
        // Usa la rotación del prefab
        Instantiate(sharkPrefab, spawnPoint.position, sharkPrefab.transform.rotation);
    }

    void SpawnBoss()
    {
        // Genera al jefe
        GameObject boss = Instantiate(BossPrefab, spawnPoint.position, BossPrefab.transform.rotation);
        bossHealth = boss.GetComponent<Boss>();  // Obtiene la referencia al script del jefe
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = Color.white;

        if (sharksDefeated < maxSharks)
        {
            // Si no hemos derrotado a todos los tiburones, mostrar los tiburones restantes
            Rect rect = new Rect(10, 90, 200, 40); // Posición y tamaño del contador
            GUI.Label(rect, "Tiburones restantes: " + (maxSharks - sharksDefeated), style);
        }
        else if (bossHealth != null)
        {
            // Si todos los tiburones han sido derrotados y el jefe ha aparecido, mostrar la vida del jefe
            Rect rect = new Rect(10, 90, 200, 40); // Posición y tamaño del contador
            GUI.Label(rect, "Salud del jefe: " + bossHealth.hp + "/" + bossHealth.startHp, style);
        }
    }
}

