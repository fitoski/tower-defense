using UnityEngine;
using System.Collections.Generic;

public class NodesManager : MonoBehaviour
{
    private static NodesManager instance;
    public static NodesManager Instance => instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private float distance = 1.5f;
    private int currentLayer = 0;

    private List<Node> allNodes = new List<Node>();

    private void Awake()
    {
        instance = this;
        if (GameManager.main != null)
        {
            GameManager.main.OnGoldChanged += UpdateAllNodes;
        }
        else
        {
            Debug.LogError("GameManager.main is null in NodesManager.Awake");
        }
    }

    void Start()
    {
        SpawnObjectsInNextLayer();
    }

    public void SpawnObjectsInNextLayer()
    {
        currentLayer++;
        Vector2 initialPosTop = transform.position + new Vector3(-distance * currentLayer, distance * currentLayer);
        Vector2 initialPosBottom = transform.position + new Vector3(-distance * currentLayer, -distance * currentLayer);

        for (int i = 0; i < 1 + currentLayer * 2; i++)
        {
            Vector2 nodePos = initialPosTop + new Vector2(i * distance, 0);
            Instantiate(prefab, nodePos, Quaternion.identity, transform);
        }


        for (int i = 1; i < currentLayer * 2; i++)
        {
            float constantX = distance * currentLayer * 2;
            Vector2 nodePos = initialPosTop + new Vector2(constantX, i * -distance);
            Instantiate(prefab, nodePos, Quaternion.identity, transform);
        }

        for (int i = 0; i < 1 + currentLayer * 2; i++)
        {
            Vector2 nodePos = initialPosBottom + new Vector2(i * distance, 0);
            Instantiate(prefab, nodePos, Quaternion.identity, transform);
        }

        for (int i = 1; i < currentLayer * 2; i++)
        {
            Vector2 nodePos = initialPosTop + new Vector2(0, i * -distance);
            Instantiate(prefab, nodePos, Quaternion.identity, transform);
        }
    }

    public void RegisterNode(Node node)
    {
        allNodes.Add(node);
    }

    private void UpdateAllNodes()
    {
        foreach (var node in allNodes)
        {
            node.UpdateSprite();
        }
    }
}
