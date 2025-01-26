using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class HeartBeatRumble : MonoBehaviour
{
    private PlayerController otherPlayer;
    private Gamepad gamepad;
    [SerializeField]
    private float minRumbleDistance;
    [SerializeField]
    private float BPMRest;
    [SerializeField]
    private float BPMPeak;
    [SerializeField]
    private float intensity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gamepad = GetComponent<PlayerInput>().GetDevice<Gamepad>();
        otherPlayer = GameManager.instance.GetOtherPlayer(GetComponent<PlayerController>());
        StartCoroutine(HeartBeatCoroutine());
    }

    private void OnDestroy()
    {
        if (gamepad != null)
            gamepad.SetMotorSpeeds(0, 0);
        StopAllCoroutines();
    }

    public IEnumerator HeartBeatCoroutine()
    {
        while (true)
        {
            float distance = Vector2.Distance(otherPlayer.transform.position, transform.position);


            Debug.Log($"My name {name} other player: {otherPlayer.name}");
            Debug.Log(distance);

            if (distance > minRumbleDistance)
            {
                yield return null;
                continue;
            }

            float relativeDistance = (minRumbleDistance - distance) / minRumbleDistance;
            Debug.Log(relativeDistance);
            gamepad.SetMotorSpeeds(relativeDistance, 0);
            Debug.Log($"BPM: {Mathf.Lerp(BPMRest, BPMPeak, relativeDistance)}");
            yield return new WaitForSeconds(Mathf.Lerp(BPMRest, BPMPeak, relativeDistance) / 120);
            gamepad.SetMotorSpeeds(0, 0);
            yield return new WaitForSeconds(Mathf.Lerp(BPMRest, BPMPeak, relativeDistance) / 120);
            gamepad.SetMotorSpeeds(0, relativeDistance);
            yield return new WaitForSeconds(Mathf.Lerp(BPMRest, BPMPeak, relativeDistance) / 120);
            gamepad.SetMotorSpeeds(0, 0);
        }
    }
}
