using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OffScreenIndicator : MonoBehaviour
{
    public Transform coreTransform;
    public GameObject indicatorPrefab; 
    public Camera mainCamera;
    public float borderOffset = 50f;

    private GameObject indicatorInstance;
    private RectTransform canvasRectTransform;

    private void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            canvasRectTransform = canvas.GetComponent<RectTransform>();
        }
        else
        {
            Debug.LogError("Canvas not found for the OffScreenIndicator.");
        }
    }
    private void Update()
    {
        Vector3 screenPosition = mainCamera.WorldToScreenPoint(coreTransform.position);
        bool isOffScreen = screenPosition.x <= 0 || screenPosition.x >= Screen.width ||
                           screenPosition.y <= 0 || screenPosition.y >= Screen.height;

        if (isOffScreen)
        {
            if (indicatorInstance == null)
            {
                indicatorInstance = Instantiate(indicatorPrefab, transform.position, Quaternion.identity, canvasRectTransform);
                Debug.Log("Indicator instantiated");
            }

            screenPosition.x = Mathf.Clamp(screenPosition.x, borderOffset, Screen.width - borderOffset);
            screenPosition.y = Mathf.Clamp(screenPosition.y, borderOffset, Screen.height - borderOffset);

            RectTransform indicatorRect = indicatorInstance.GetComponent<RectTransform>();
            indicatorRect.position = screenPosition;

            Vector3 toCoreDirection = (coreTransform.position - mainCamera.transform.position).normalized;
            float angle = Mathf.Atan2(toCoreDirection.y, toCoreDirection.x) * Mathf.Rad2Deg;
            indicatorRect.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        else
        {
            if (indicatorInstance != null)
            {
                Destroy(indicatorInstance);
                Debug.Log("Indicator destroyed");
            }
        }
    }
}
