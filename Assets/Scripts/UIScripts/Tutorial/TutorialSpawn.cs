using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialSpawn : MonoBehaviour
{
    // prefab of red dwarf
    public GameObject enemy;
    // temp storage of red dwarf reference to tell if null
    public List<GameObject> enemyList;
    // bool flag
    public bool enemyAlive = false;
    // reference to upgrade ui
    UpgradeManager _upgradeManager;
    
    private void Update()
    {
        if (enemyAlive)
        {
            // enemy cleared
            if (enemyList[0] == null)
            {
                UpgradeCall();
            }
        }
    }

    public void Spawn()
    {
        enemyList.Add(Instantiate(enemy));
        enemyAlive = true;
    }

    // move to upgrade tutorial scene
    public void UpgradeCall()
    {
        SceneManager.LoadScene("UpgradeTutorial");
    }
    
}
