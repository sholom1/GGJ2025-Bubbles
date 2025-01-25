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
    private float timeRemaining;
    private void Start()
    {
        timeRemaining = duration;
    }
    // Update is called once per frame
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
        timeRemaining = Mathf.Max(0, timeRemaining - 1);
    }
}
