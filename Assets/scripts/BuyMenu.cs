using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyMenu : MonoBehaviour
{
    [SerializeField] private GameObject buyMenuUI;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject wallPrefab;

    private Vector3 selectedNodePosition;

    private bool isBuyMenuOpen = false;

    public void OpenBuyMenu(Vector3 nodePosition)
    {
        if (isBuyMenuOpen) return;

        float yOffset = 1f;
        selectedNodePosition = new Vector3(nodePosition.x, nodePosition.y + yOffset, nodePosition.z);

        buyMenuUI.transform.position = selectedNodePosition;
        buyMenuUI.SetActive(!buyMenuUI.activeSelf);
    }

    public void CloseBuyMenu()
    {
        buyMenuUI.SetActive(false);
    }

    public void BuyTurret()
    {
        Vector3 targetNodePosition = GameManager.main.GetSelectedNodePosition();

        if (GameManager.main.HasEnoughCurrency(GameManager.main.GetTurretCost()) && targetNodePosition != null)
        {
            PlacePrefab(turretPrefab, targetNodePosition, GameManager.main.GetTurretCost());
            GameManager.main.ClearSelectedNodePosition();
        }
        else
        {
            Debug.Log("Not enough currency or no node position available.");
        }
    }

    public void BuyWall()
    {
        Vector3 targetNodePosition = GameManager.main.GetSelectedNodePosition();

        if (GameManager.main.HasEnoughCurrency(GameManager.main.GetWallCost()) && targetNodePosition != null)
        {
            PlacePrefab(wallPrefab, targetNodePosition, GameManager.main.GetWallCost());
            GameManager.main.ClearSelectedNodePosition();
        }
        else
        {
            Debug.Log("Not enough currency or no node position available.");
        }
    }

    private void PlacePrefab(GameObject prefab, Vector3 position, int cost)
    {
        if (GameManager.main.GetSelectedNodePosition() == null)
        {
            Debug.Log("No selected node position available.");
            return;
        }

        if (GameManager.main.SpendCurrency(GameManager.main.GetTurretCost()))
        {
            GameObject placedPrefab = Instantiate(prefab, position, Quaternion.identity);
        }
    }
}