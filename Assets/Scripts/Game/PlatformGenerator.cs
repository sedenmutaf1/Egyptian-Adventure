using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameObject platformPrefab;
    public GameObject finalPrefab;
    public Transform player;
    public GameObject heartGemPrefab;
    private int gemPlatformIndex = -1;

    public float spawnDistance = 5f;
    public float platformLength = 1f;
    public float baseGap = 0.5f;
    public float maxXOffset = 1f;

    public float movingPlatformChanceNormal = 0.3f;
    public float movingPlatformChanceHard = 0.4f;
    public float disappearPlatformChanceHard = 0.4f;

    public int platformCountLimit = 10;
    public int platformCount = 0;

    private float nextSpawnZ;
    private float playerSpeed;
    private float difficultyIncreaseRate = 0.02f;
    private bool finalPrefabSpawned = false;
    private bool gemSpawned = false;

    void Start()
    {
        playerSpeed = DifficultyData.speed;
        nextSpawnZ = player.position.z + 6f;
        gemPlatformIndex = Random.Range(4,9);
    }

    void Update()
    {
        playerSpeed += difficultyIncreaseRate * Time.deltaTime;

        while (player.position.z + spawnDistance > nextSpawnZ && platformCount < platformCountLimit)
        {
            SpawnPlatform();
        }

        if (platformCount >= platformCountLimit && !finalPrefabSpawned)
        {
            SpawnFinalPrefab();
        }
    }

    void SpawnPlatform()
    {
        float difficultyScale = Mathf.Min(playerSpeed / DifficultyData.speed, 1.5f);
        float xOffset = Random.Range(-maxXOffset, maxXOffset) + 1f;
        float gap = baseGap * Random.Range(0.8f, 1.1f) * difficultyScale;

        Vector3 spawnPos = new Vector3(xOffset, player.position.y, nextSpawnZ);
        GameObject platform = Instantiate(platformPrefab, spawnPos, Quaternion.identity);
        if (platformCount == gemPlatformIndex)
        {
            Vector3 gemPosition = spawnPos + new Vector3(0f, 0.7f, 0f);
            Instantiate(heartGemPrefab, gemPosition, Quaternion.identity);
            gemSpawned = true;
        }

        string difficulty = DifficultyData.difficulty;

        if (difficulty == "Normal" && !gemSpawned)
        {
            if (Random.value < movingPlatformChanceNormal)
            {
                MakePlatformMove(platform);
            }
        }
        else if (difficulty == "Hard" && !gemSpawned)
        {
            float roll = Random.value;

            if (roll < movingPlatformChanceHard)
            {
                MakePlatformMove(platform);
            }
            else if (roll < movingPlatformChanceHard + disappearPlatformChanceHard)
            {
                MakePlatformDisappearOnTouch(platform);
            }
        }

        nextSpawnZ += platformLength + gap;
        platformCount++;
        gemSpawned = false; 
    }

    void SpawnFinalPrefab()
    {
        Vector3 spawnPos = new Vector3(2.2f, player.position.y +5f, nextSpawnZ+9f);
        Instantiate(finalPrefab, spawnPos, Quaternion.Euler(0, 180, 0));
        finalPrefabSpawned = true;
    }

    void MakePlatformMove(GameObject platform)
    {
        SetColor(platform, Color.blue);
        float amplitude = Random.Range(0.5f, 2f);
        float speed = Random.Range(0.7f, 2f);
        Vector3 startPos = platform.transform.position;

        platform.AddComponent<PlatformMovement>().Init(startPos, amplitude, speed);
    }

    void MakePlatformDisappearOnTouch(GameObject platform)
    {
        SetColor(platform, Color.red);
        platform.AddComponent<PlatformDisappearOnTouch>();
    }

    void SetColor(GameObject platform, Color color)
    {
        Renderer rend = platform.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = color;
        }
    }

    // --- Internal movement behavior ---
    private class PlatformMovement : MonoBehaviour
    {
        private Vector3 startPos;
        private float amplitude;
        private float speed;

        public void Init(Vector3 start, float amp, float spd)
        {
            startPos = start;
            amplitude = amp;
            speed = spd;
        }

        void Update()
        {
            transform.position = startPos + new Vector3(Mathf.Sin(Time.time * speed) * amplitude, 0, 0);
        }
    }

    // --- Internal disappear-on-touch behavior ---
    private class PlatformDisappearOnTouch : MonoBehaviour
    {
        private bool triggered = false;
        public float delayBeforeDisappear = 1.5f;

        private void OnCollisionEnter(Collision collision)
        {
            if (!triggered && collision.gameObject.CompareTag("Player"))
            {
                triggered = true;
                Invoke(nameof(DestroyPlatform), delayBeforeDisappear);
            }
        }

        private void DestroyPlatform()
        {
            Destroy(gameObject);
        }
    }
}
