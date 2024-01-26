using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class BuyMenu : MonoBehaviour
{
    private static BuyMenu instance;
    public static BuyMenu Instance => instance;
    [SerializeField] private GameObject buyMenuUI;
    [SerializeField] private GameObject buyButton;
    private Vector3 selectedNodePosition;

    private bool isBuyMenuOpen = false;
    [SerializeField] private GameObject normalTurretPrefab;
    public int NormalTurretCost => normalTurretPrefab.GetComponent<Turret>().Cost;
    [SerializeField] private List<GameObject> availableUpgradedTurrets = new List<GameObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        else
        {
            Destroy(this);
        }
    }

    public void OpenBuyMenu(Node targetNode)
    {
        if (isBuyMenuOpen) return;

        float yOffset = 1f;
        selectedNodePosition = new Vector3(targetNode.transform.position.x, targetNode.transform.position.y + yOffset, targetNode.transform.position.z);
        buyMenuUI.SetActive(true);
        buyMenuUI.transform.position = selectedNodePosition;

        foreach (Transform transform in buyMenuUI.transform)
        {
            Destroy(transform.gameObject);
        }

        if (targetNode.turretTier == 0)
        {
            Button turretBuyButton = Instantiate(buyButton, buyMenuUI.transform).GetComponent<Button>();
            turretBuyButton.onClick.AddListener(() => BuyTurret(normalTurretPrefab));
            turretBuyButton.GetComponentInChildren<TMP_Text>().text = $"Buy {normalTurretPrefab.GetComponent<Turret>().turretName}";
        }

        else
        {
            if (targetNode.turretTier == 1)
            {
                foreach (GameObject prefab in availableUpgradedTurrets)
                {
                    Button turretBuyButton = Instantiate(buyButton, buyMenuUI.transform).GetComponent<Button>();
                    turretBuyButton.onClick.AddListener(() => UpgradeTurretToElement(prefab));
                    turretBuyButton.GetComponentInChildren<TMP_Text>().text = $"Upgrade to {prefab.GetComponent<Turret>().turretName}";
                }
            }

            Button sellButton = Instantiate(buyButton, buyMenuUI.transform).GetComponent<Button>();
            sellButton.onClick.AddListener(() => SellTurret());
            sellButton.GetComponentInChildren<TMP_Text>().text = "Sell Turret";
        }
    }

    public List<int> getAvailableTurretCosts()
    {
        List<int> costs = new List<int>();
        foreach (GameObject turret in availableUpgradedTurrets)
        {
            costs.Add(turret.GetComponent<Turret>().Cost);
        }

        return costs;
    }

    public void CloseBuyMenu()
    {
        buyMenuUI.SetActive(false);
    }

    public void BuyTurret(GameObject turretPrefab)
    {
        Debug.Log("dsada");
        Node targetNode = GameManager.main.GetSelectedNode();

        if (GameManager.main.HasEnoughGold(turretPrefab.GetComponent<Turret>().Cost) && targetNode != null)
        {
            GameObject turret = PlacePrefab(turretPrefab, targetNode.transform.position, turretPrefab.GetComponent<Turret>().Cost);
            if (turret != null)
            {
                targetNode.BuyTurretToThisNode(turret);
            }
            
            GameManager.main.ClearSelectedNodePosition();
        }
        else
        {
            Debug.Log("Not enough gold or no node position available.");
        }

        CloseBuyMenu();
    }
    public void UpgradeTurretToElement(GameObject newTurret)
    {
        Node targetNode = GameManager.main.GetSelectedNode();
        if (GameManager.main.HasEnoughGold(newTurret.GetComponent<Turret>().Cost) && targetNode != null)
        {
            Destroy(targetNode.Turret);
            GameObject turret = PlacePrefab(newTurret, targetNode.transform.position, newTurret.GetComponent<Turret>().Cost);
            if (turret != null)
            {
                targetNode.UpgradeTurretToElement(turret);
            }

            GameManager.main.ClearSelectedNodePosition();
        }
        else
        {
            Debug.Log("Not enough gold or no node position available.");
        }

        CloseBuyMenu();
    }
    public void SellTurret()
    {
        Node targetNode = GameManager.main.GetSelectedNode();
        Destroy(targetNode.Turret);
        targetNode.SellTurretFromThisNode();
        CloseBuyMenu();
    }

    private GameObject PlacePrefab(GameObject prefab, Vector3 position, int cost)
    {
        if (GameManager.main.GetSelectedNode() == null)
        {
            Debug.Log("No selected node position available.");
            return null;
        }

        if (GameManager.main.SpendGold(prefab.GetComponent<Turret>().Cost))
        {
            GameObject placedPrefab = Instantiate(prefab, position, Quaternion.identity);
            return placedPrefab;
        }

        return null;
    }
}