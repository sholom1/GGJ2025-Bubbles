using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private new ParticleSystem particleSystem;
    [SerializeField]
    private float emissionSpeedOnPulse;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    public void IncreaseEmissionSpeed()
    {
        var emission = particleSystem.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(emissionSpeedOnPulse);
    }
}
