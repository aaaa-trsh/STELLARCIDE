using TMPro;
using UnityEngine;
using System.Collections;

public class TextPrompt : MonoBehaviour
{
    private TextMeshProUGUI header;
    private TextMeshProUGUI subtext;

    public int returnTime = 10;
    private int counter;

    public bool countdownActive
    {
        get
        {
            return counter < returnTime;
        }

        set
        {
            if (!value)
            {
                counter = returnTime;
            }
        }
    }

    public bool isVisible 
    {
        set
        { 
            if (header != null && subtext != null)
            {
                header.enabled = value; 
                subtext.enabled = value;
            }
        }
        get
        { 
            return header.enabled && subtext.enabled;
        }
    }

    void Awake()
    {
        header = transform.Find("Objective").GetComponent<TextMeshProUGUI>();
        subtext = transform.Find("Counter").GetComponent<TextMeshProUGUI>();

        counter = returnTime;
        header.text = $"you shouldnt see this B==D";
        GameManager.Instance.textPrompt = this;

        EventBus.Instance.OnGamePaused += HideAll;
        HideAll(false);
    }

    void Start()
    { 
        isVisible = true;
    }

    public IEnumerator StartCountDown()
    {
        isVisible = true;
        counter = returnTime;
        while (counter > 0)
        {
            UpdateHeader($"RETURN TO MISSION ZONE IN {counter--}");
            UpdateText("");
            yield return new WaitForSeconds(1);
        }
        GameManager.Instance.Player.GetComponent<HealthOwner>().TakeDamage(
            new Damage(999, Damage.Type.PHYSICAL)
        );
    }

    public void UpdateHeader(string text)
    {
        header.text = text;
    }

    public void UpdateText(string text)
    {
        subtext.text = text;
    }

    public void HideAll(bool hidden)
    {
        isVisible = !hidden;
    }
}