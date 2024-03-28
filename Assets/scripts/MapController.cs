using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> terrainChunks;
    public GameObject player;
    public float checkerRadius;
    public Vector3 noTerrainPosition;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    PlayerMovement pm;

    [Header("Optimization")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist; 
    float opDist;
    float optimizerCooldown;
    public float optimizerCooldownDur;

    void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
    }

    void Update()
    {
        ChunkChecker();
        ChunkOptimzer();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }

        Vector2 moveDirection = pm.MovementDirection;
        CheckAndSpawnChunk("Right", moveDirection.x > 0 && moveDirection.y == 0);
        CheckAndSpawnChunk("Left", moveDirection.x < 0 && moveDirection.y == 0);
        CheckAndSpawnChunk("Up", moveDirection.y > 0 && moveDirection.x == 0);
        CheckAndSpawnChunk("Down", moveDirection.y < 0 && moveDirection.x == 0);

        if (moveDirection.x > 0 && moveDirection.y > 0) // Sağ üst çapraz
        {
            CheckAndSpawnChunk("Right Up", true);
            CheckAndSpawnChunk("Right", true); // Ek kontrol
            CheckAndSpawnChunk("Up", true); // Ek kontrol
        }
        else if (moveDirection.x > 0 && moveDirection.y < 0) // Sağ alt çapraz
        {
            CheckAndSpawnChunk("Right Down", true);
            CheckAndSpawnChunk("Right", true); // Ek kontrol
            CheckAndSpawnChunk("Down", true); // Ek kontrol
        }
        else if (moveDirection.x < 0 && moveDirection.y > 0) // Sol üst çapraz
        {
            CheckAndSpawnChunk("Left Up", true);
            CheckAndSpawnChunk("Left", true); // Ek kontrol
            CheckAndSpawnChunk("Up", true); // Ek kontrol
        }
        else if (moveDirection.x < 0 && moveDirection.y < 0) // Sol alt çapraz
        {
            CheckAndSpawnChunk("Left Down", true);
            CheckAndSpawnChunk("Left", true); // Ek kontrol
            CheckAndSpawnChunk("Down", true); // Ek kontrol
        }
    }

    void CheckAndSpawnChunk(string direction, bool condition)
    {
        if (condition)
        {
            Transform checkPoint = currentChunk.transform.Find(direction);
            if (checkPoint && !Physics2D.OverlapCircle(checkPoint.position, checkerRadius, terrainMask))
            {
                noTerrainPosition = checkPoint.position;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        foreach (GameObject chunk in spawnedChunks)
        {
            if (chunk.transform.position == (Vector3)noTerrainPosition)
            {
                return;
            }
        }
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPosition, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }

    void ChunkOptimzer()
    {
        optimizerCooldown -= Time.deltaTime;

        if (optimizerCooldown <= 0f)
        {
            optimizerCooldown = optimizerCooldownDur;
        }
        else
        {
            return;
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
}