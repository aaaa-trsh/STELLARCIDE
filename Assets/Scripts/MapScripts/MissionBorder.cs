using UnityEngine;

public class MissionBorder : MonoBehaviour
{
    private Coroutine coroutine;

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            // GameManager.Instance.textPrompt.isVisible = false;
            GameManager.Instance.textPrompt.countdownActive = false;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            SpawnerManager.Instance.UpdateUI();
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") && gameObject.activeInHierarchy)
        {
            if (GameManager.Instance.textPrompt == null) {return;}
            coroutine = StartCoroutine(GameManager.Instance.textPrompt.StartCountDown());
            SpawnerManager.Instance.UpdateUI();
        }
    }
}
