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
    [SerializeField]
    private float pulseThreshold;
    public UnityEvent OnEnd;
    public UnityEvent OnPulse;
    private float timeRemaining;

    private void Start()
    {
        timeRemaining = duration;
    }

    void Update()
    {
        if (!animator.GetBool("Pulse"))
            timeRemaining -= Time.deltaTime;

        timer.text = timeRemaining.ToString("0");

        if (timeRemaining <= pulseThreshold)
        {
            animator.SetBool("Pulse", true);
        }

        if (timeRemaining <= 0)
        {
            OnEnd.Invoke();
            enabled = false;
        }
    }

    public void TickTimerAnim(AnimationEvent animationEvent)
    {
        OnPulse.Invoke();
        timeRemaining = Mathf.Max(0, timeRemaining - 1);
    }

    public void ModifyTime(float amount)
    {
        timeRemaining = Mathf.Clamp(timeRemaining + amount, 0f, duration * 2f);
        
        // If we added time and we're above the pulse threshold, stop pulsing
        if (timeRemaining > pulseThreshold)
        {
            animator.SetBool("Pulse", false);
        }
    }
}