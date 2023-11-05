using UnityEngine;

public class NodesManager : MonoBehaviour
{
    private static NodesManager instance;
    public static NodesManager Instance => instance;

    [SerializeField] private GameObject prefab;
    [SerializeField] private float distance = 1.5f;
    private int currentLayer = 0;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnObjectsInNextLayer();
    }

    public void SpawnObjectsInNextLayer()
    {
        currentLayer++;
        Vector2 initialPosTop = transform.position + new Vector3(-1.5f * currentLayer, 1.5f * currentLayer);
        Vector2 initialPosBottom = transform.position + new Vector3(-1.5f * currentLayer, -1.5f * currentLayer);

        for (int i = 0; i < 1 + currentLayer * 2; i++)
        {
            Vector2 nodePos = initialPosTop + new Vector2(i * 1.5f, 0);
            Instantiate(prefab, nodePos, Quaternion.identity);
        }


        for (int i = 1; i < currentLayer * 2; i++)
        {
            float constantX = 1.5f * currentLayer * 2;
            Vector2 nodePos = initialPosTop + new Vector2(constantX, i * -1.5f);
            Instantiate(prefab, nodePos, Quaternion.identity);
        }

        for (int i = 0; i < 1 + currentLayer * 2; i++)
        {
            Vector2 nodePos = initialPosBottom + new Vector2(i * 1.5f, 0);
            Instantiate(prefab, nodePos, Quaternion.identity);
        }

        for (int i = 1; i < currentLayer * 2; i++)
        {
            Vector2 nodePos = initialPosTop + new Vector2(0, i * -1.5f);
            Instantiate(prefab, nodePos, Quaternion.identity);
        }
    }
}
