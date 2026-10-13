using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;
using IEnumerator = System.Collections.IEnumerator;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject prefab;
    public int spawnWeight;
}

public enum RoundType
{
    EXTERMINATE,
    SURVIVAL
}

public class SpawnerManager
{
    static SpawnerManager thisInstance;

    public static SpawnerManager Instance
    {
        get { return thisInstance ??= new SpawnerManager(); }
    }

    public RoundType roundType = RoundType.SURVIVAL;

    public int initialRedGiantWeight = 0;
    public int initialYellowDwarfWeight = 0;
    public int initialRedDwarfWeight = 0;

    // EXTERMINATE VARIABLES
    private int totalEnemies = 2; // Will be calclulated based on difficulty
    public int numEnemiesSpawned = 0;
    public int enemiesAlive = 0;
    public int enemiesKilled = 0;

    // SURVIVAL VARIABLES
    public float elapsedTime = 0;
    public int roundTime = 10;

    // General variables that can be modified by difficulty
    public int healthMult = 1;
    public float spawnInterval = 10f;

    private List<EnemySpawnData> enemyTypes; // Stores enemy spawn weights + corresponding prefab


    // Call this when you transfer the variable information from .json, but for now we're doing this in enemyspawner
    public void InitializeSpawner()
    {
        EventBus.Instance.OnEnemyDead += EnemyDead;
        EventBus.Instance.OnRoundEnd += RoundEnd;
        ResetVariables();
        CalculateDifficulty();
        UpdateUI();

    }

    // Since this is a public class and will always persist, we need to manually reset the values
    private void ResetVariables()
    {
        initialRedDwarfWeight = initialRedGiantWeight = initialYellowDwarfWeight = numEnemiesSpawned = enemiesAlive = enemiesKilled = 0;
        elapsedTime = 0f;
        enemiesKilled = 0;
        totalEnemies = 2;
        roundTime = 10;
        spawnInterval = 10f;
        healthMult = 1;
    }

    private void CalculateDifficulty()
    {
        if (GameManager.Instance.difficultySum >= 7)
        {
            initialRedDwarfWeight += (initialRedDwarfWeight == 0) ? 0 : 10;
            initialRedGiantWeight += (initialRedGiantWeight == 0) ? 0 : 10;
            initialYellowDwarfWeight += (initialYellowDwarfWeight == 0) ? 0 : 10;
        }
        if (GameManager.Instance.difficultySum >= 14)
        {
            spawnInterval = 8f;
        }
        if (GameManager.Instance.difficultySum >= 21)
        {
            healthMult = 2;
        }
        if (GameManager.Instance.difficultySum >= 28)
        {
            initialYellowDwarfWeight += 10;
        }
        // Need to add what happens if the number goes over 28
        // Very hardcoded.... ;-;
        GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Enemy Prefabs");

        enemyTypes = new List<EnemySpawnData>();

        enemyTypes.Add(new EnemySpawnData { prefab = prefabs[0], spawnWeight = initialRedDwarfWeight });
        enemyTypes.Add(new EnemySpawnData { prefab = prefabs[1], spawnWeight = initialRedGiantWeight });
        enemyTypes.Add(new EnemySpawnData { prefab = prefabs[2], spawnWeight = initialYellowDwarfWeight });
    }

    public bool ContinueSpawning()
    {
        switch (roundType)
        {
            case RoundType.SURVIVAL:
                return (elapsedTime < roundTime);
            case RoundType.EXTERMINATE:
                return (numEnemiesSpawned < totalEnemies);
        }
        return false;
    }
    

    public GameObject GetNextEnemy()
    {
        return enemyTypes[GetRandomWeightedIndex()].prefab;
    }

    // "Signals"
    private void EnemyDead()
    {
        if (roundType == RoundType.SURVIVAL) { return; }
        Debug.Log("we killed something right?");
        enemiesAlive -= 1;
        enemiesKilled += 1;
        UpdateUI();
        if (enemiesAlive == 0)
        {
            if ((totalEnemies == numEnemiesSpawned))
            {
                Debug.Log("all enemies are dead and the conditions for winning are satisfied");
                EventBus.Instance.RoundEnd();
            }
        }
    }

    private void RoundEnd()
    {
        EventBus.Instance.OnEnemyDead -= EnemyDead;
        EventBus.Instance.OnRoundEnd -= RoundEnd;
        CoroutineManager.Instance.Run(EndGameTransition());
    }

    private IEnumerator EndGameTransition()
    {
        if (GameManager.Instance.MissionGoalUI.UIAnimator != null)
        {
            GameManager.Instance.MissionGoalUI.UIAnimator.Play("UIOut");
        }
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene("Scenes/UpgradeSelectorTesting");
    }

    public void UpdateUI()
    {
        if (GameManager.Instance.MissionGoalUI == null) { return; }
        if (GameManager.Instance.textPrompt.countdownActive) { return; }
        // GameManager.Instance.textPrompt.isVisible = true;

        switch (roundType)
        {
            case RoundType.SURVIVAL:
                GameManager.Instance.textPrompt.UpdateHeader("OBJECTIVE: SURVIVE");
                GameManager.Instance.textPrompt.UpdateText($"TIME LEFT: {Mathf.Max(roundTime - elapsedTime, 0f):F2}");
                break;
            case RoundType.EXTERMINATE:
                GameManager.Instance.textPrompt.UpdateHeader("OBJECTIVE: EXTERMINATE ALL ENEMIES");
                GameManager.Instance.textPrompt.UpdateText($"Enemies left: {totalEnemies - enemiesKilled}");
                break;
        };
    }

    private int GetRandomWeightedIndex()
    {
        int totalWeight = enemyTypes.Sum(enemy => enemy.spawnWeight);

        int randomValue = Random.Range(0, totalWeight);

        for (int i = 0; i < enemyTypes.Count; i++)
        {
            randomValue -= enemyTypes[i].spawnWeight;

            if (randomValue < 0)
            {
                return i;
            }
        }

        return 0;
    }
}

