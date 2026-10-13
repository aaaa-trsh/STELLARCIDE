using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int enemiesDefeated = 0;
    public int redDwarfDefeated = 0;
    public int yellowDwarfDefeated = 0;
    public int redGiantDefeated = 0;
    
    public void EnemyDefeated(Entity e)
    {
        // determine type of enemy killed and add it
        enemiesDefeated++;
    }
}
