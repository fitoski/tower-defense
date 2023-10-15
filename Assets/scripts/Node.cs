using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private Color startColor;

    private void Start()
    {
        startColor = sr.color;
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
        BroadcastMessage("OpenBuyMenu", transform.position, SendMessageOptions.RequireReceiver);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Node"))
            {
                Vector3 targetNodePosition = hit.collider.transform.position;
                GameManager.main.SetSelectedNodePosition(targetNodePosition);
            }
        }
    }
}