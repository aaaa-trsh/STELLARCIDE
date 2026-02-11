using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro; // Include this if your prefab uses TextMeshPro

public class LogbookManager : MonoBehaviour
{
    public List<GameObject> tabPanels;
    public List<string> entriesEnemies;
    public Button buttonPrefab;
    public Transform buttonParent;

    void Start()
    {
        GenerateButtons();
        ShowTab(0);
    }

    void GenerateButtons()
    {
        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < entriesEnemies.Count; i++)
        {
            Button newButton = Instantiate(buttonPrefab, buttonParent);
        }
    }

    public void ShowTab(int tabIndex)
    {
        for (int i = 0; i < tabPanels.Count; i++)
        {
            tabPanels[i].SetActive(i == tabIndex);
        }
    }
}