using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private TextMeshProUGUI timer;
    [SerializeField]
    private Animator animator;
    public UnityEvent OnEnd;
    private float timeRemaining;
    private void Start()
    {
        timeRemaining = duration;
    }
    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;
        timer.text = timeRemaining.ToString("0");
        if (timeRemaining <= 0)
        {
            OnEnd.Invoke();
            enabled = false;
        }
    }
}
