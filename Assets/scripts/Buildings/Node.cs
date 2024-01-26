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
    [SerializeField] private LayerMask layerMask;
    private Color startColor;
    public int turretTier = 0;
    private bool hasBuilding = false;
    public bool HasBuilding => hasBuilding;
    private GameObject turret = null;
    public GameObject Turret => turret;

    private void Start()
    {
        startColor = sr.color;
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
        int playerGold = GameManager.main.Gold;


        if (turretTier == 0)
        {
            if (playerGold >= BuyMenu.Instance.NormalTurretCost)
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

    public void BuyTurretToThisNode(GameObject turret)
    {
        hasBuilding = true;
        this.turret = turret;
        turretTier = 1;
    }

    public void UpgradeTurretToElement(GameObject turret)
    {
        this.turret = turret;
        turretTier = 2;
    }

    public void BuyWallToThisNode()
    {
        hasBuilding = true;
    }

    public void SellTurretFromThisNode()
    {
        hasBuilding = false;
        this.turret = null;
        turretTier = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, 999, layerMask);
            if (hit.collider != null)
            {
                if (hit.collider.transform.CompareTag("Node") && hit.collider.transform == transform)
                {
                    GameManager.main.SetSelectedNode(this);
                    BuyMenu.Instance.OpenBuyMenu(this);
                }
            }

            else
            {
                BuyMenu.Instance.CloseBuyMenu();
            }
        }
    }
}