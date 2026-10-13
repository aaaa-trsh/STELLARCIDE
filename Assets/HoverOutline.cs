using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverOutline : Selectable
{
    private Button button;
    [SerializeField] private Outline outline;
    [SerializeField] private GameObject selected1, selected2;

    private void Awake()
    {
        gameObject.GetComponent<Button>();
    }

    private void Update()
    {
        if (IsHighlighted())
        {
            outline.enabled = true;
            selected1.SetActive(true);
            selected2.SetActive(true);
        }
        else
        {
            outline.enabled = false;
            selected1.SetActive(false);
            selected2.SetActive(false);
        }
    }
}
