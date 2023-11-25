using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;
    [SerializeField] private Sprite notEnoughMoneySprite;  
    [SerializeField] private Sprite enoughMoneySprite;     

    private Color startColor;

    private bool hasBuilding = false;
    public bool HasBuilding => hasBuilding;
    private GameObject turret = null;
    public GameObject Turret => turret;

    private void Start()
    {
        startColor = sr.color;
        GameManager.main.OnCurrencyChanged += UpdateSprite;
        NodesManager.Instance.RegisterNode(this);
        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if (!hasBuilding)
        {
            bool canBuyAnyTurret = CanAffordAnyTurret();
            sr.sprite = canBuyAnyTurret ? enoughMoneySprite : notEnoughMoneySprite;
        }
    }

    private bool CanAffordAnyTurret()
    {
        int playerGold = GameManager.main.currency; 
        foreach (var turretPrefab in GameManager.main.AvailableTurrets)
        {
            Turret turret = turretPrefab.GetComponent<Turret>();
            if (turret != null && playerGold >= turret.Cost)
                return true;
        }
        return false;
    }

    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }

    private void OnMouseDown()
    {
        BuyMenu.Instance.OpenBuyMenu(this);
    }

    private void OnDestroy()
    {
        GameManager.main.OnCurrencyChanged -= UpdateSprite;
    }

    public void BuyTurretToThisNode(GameObject turret)
    {
        hasBuilding = true;
        this.turret = turret;
    }

    public void BuyWallToThisNode()
    {
        hasBuilding = true;
    }

    public void SellTurretFromThisNode()
    {
        hasBuilding = false;
        this.turret = null;
    }

    void Update()
    {
        UpdateSprite();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.transform.CompareTag("Node"))
            {
                if (hit.collider.transform == transform)
                {
                    GameManager.main.SetSelectedNode(this);
                }
            }

            else
            {
                BuyMenu.Instance.CloseBuyMenu();
            }
        }
    }
}