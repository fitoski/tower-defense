using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyMenu : MonoBehaviour
{
    private static BuyMenu instance;
    public static BuyMenu Instance => instance;
    [SerializeField] private GameObject buyMenuUI;
    [SerializeField] private GameObject upgradeMenuUI;
    [SerializeField] private GameObject turretPrefab;
    [SerializeField] private GameObject wallPrefab;
    private Vector3 selectedNodePosition;

    private bool isBuyMenuOpen = false;

    private void Awake()
    {
        instance = this;
    }

    public void OpenBuyMenu(Node targetNode)
    {
        if (isBuyMenuOpen) return;

        if (targetNode.HasBuilding && targetNode.Turret != null)
        {
            float yOffset = 1f;
            selectedNodePosition = new Vector3(targetNode.transform.position.x, targetNode.transform.position.y + yOffset, targetNode.transform.position.z);

            upgradeMenuUI.transform.position = selectedNodePosition;
            buyMenuUI.SetActive(false);
            upgradeMenuUI.SetActive(true);

        }

        else if (!targetNode.HasBuilding)
        {
            float yOffset = 1f;
            selectedNodePosition = new Vector3(targetNode.transform.position.x, targetNode.transform.position.y + yOffset, targetNode.transform.position.z);

            buyMenuUI.transform.position = selectedNodePosition;
            buyMenuUI.SetActive(true);
            upgradeMenuUI.SetActive(false);
        }
    }

    public void CloseBuyMenu()
    {
        buyMenuUI.SetActive(false);
        upgradeMenuUI.SetActive(false);
    }

    public void BuyTurret()
    {
        Node targetNode = GameManager.main.GetSelectedNode();

        if (GameManager.main.HasEnoughCurrency(GameManager.main.GetTurretCost()) && targetNode != null)
        {
            GameObject turret = PlacePrefab(turretPrefab, targetNode.transform.position, GameManager.main.GetTurretCost());
            if (turret != null)
            {
                targetNode.BuyTurretToThisNode(turret);
            }
            
            GameManager.main.ClearSelectedNodePosition();
        }
        else
        {
            Debug.Log("Not enough currency or no node position available.");
        }
    }

    public void SellTurret()
    {
        Node targetNode = GameManager.main.GetSelectedNode();
        Destroy(targetNode.Turret);
        targetNode.SellTurretFromThisNode();
    }

    public void BuyWall()
    {
        Node targetNode = GameManager.main.GetSelectedNode();

        if (GameManager.main.HasEnoughCurrency(GameManager.main.GetWallCost()) && targetNode != null)
        {
            targetNode.BuyWallToThisNode();
            PlacePrefab(wallPrefab, targetNode.transform.position, GameManager.main.GetWallCost());
            GameManager.main.ClearSelectedNodePosition();
        }
        else
        {
            Debug.Log("Not enough currency or no node position available.");
        }
    }

    private GameObject PlacePrefab(GameObject prefab, Vector3 position, int cost)
    {
        if (GameManager.main.GetSelectedNode() == null)
        {
            Debug.Log("No selected node position available.");
            return null;
        }

        if (GameManager.main.SpendCurrency(GameManager.main.GetTurretCost()))
        {
            GameObject placedPrefab = Instantiate(prefab, position, Quaternion.identity);
            return placedPrefab;
        }

        return null;
    }
}