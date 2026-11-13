using UnityEngine;

public class wallgenerator : MonoBehaviour
{
    [Header("Wall Settings")]
    public GameObject wallPrefab;
    public int wallCount = 10;

    [Header("Map Boundaries")]
    public Vector2 mapMin = new Vector2(-10f, -10f);
    public Vector2 mapMax = new Vector2(10f, 10f);

    [Header("Maximum Spawn Height")]
    public float maxSpawnY = 50f;

    [Header("Random Scale")]
    public float minScale = 2f;
    public float maxScale = 6f;

    void Start()
    {
        GenerateWalls();
    }

    void GenerateWalls()
    {
        if (wallPrefab == null)
        {
            return;
        }

        for (int i = 0; i < wallCount; i++)
        {
            float randomX = Random.Range(mapMin.x, mapMax.x);

            float randomY = Random.Range(
                mapMin.y,
                Mathf.Min(mapMax.y, maxSpawnY)
            );

            Vector2 spawnPosition = new Vector2(randomX, randomY);

            GameObject wall = Instantiate(
                wallPrefab,
                spawnPosition,
                Quaternion.identity
            );

            Vector2 spawnPosition2 = new Vector2(randomX * -1f, randomY);

            GameObject wall2 = Instantiate(
                wallPrefab,
                spawnPosition2,
                Quaternion.identity
            );

            float scaleX = Random.Range(minScale, maxScale);
            float scaleY = Random.Range(minScale, maxScale);

            Vector3 originalScale = wall.transform.localScale;

            wall.transform.localScale = new Vector3(
                originalScale.x * scaleX,
                originalScale.y * scaleY,
                originalScale.z
            );
            
            wall2.transform.localScale = new Vector3(
                originalScale.x * scaleX,
                originalScale.y * scaleY,
                originalScale.z
            );
        }
    }
}

