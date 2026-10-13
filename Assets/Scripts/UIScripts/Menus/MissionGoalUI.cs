using TMPro;
using UnityEngine;

public class MissionGoalUI : MonoBehaviour
{
    public TextMeshProUGUI objective;
    public TextMeshProUGUI counter;
    public Animator UIAnimator; // This is pretty hacky ain't it lmfao
    private void Awake()
    {
        GameManager.Instance.MissionGoalUI = this;
    }
}
