using UnityEngine;
using System.Collections.Generic;

public class LogbookManager : MonoBehaviour
{
    public List<GameObject> tabPanels;

    void Start()
    {
        ShowTab(0);
    }

    public void ShowTab(int tabIndex)
    {
        for (int i = 0; i < tabPanels.Count; i++)
        {
            tabPanels[i].SetActive(i == tabIndex);
        }
    }
}