using UnityEngine;

public class FloatingMotion : MonoBehaviour
{
    [Header("Float Settings")]
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 1f;
    [SerializeField] private bool randomizeStart = true;

    private Vector3 startPosition;
    private float timeOffset;

    private void Start()
    {
        startPosition = transform.position;
        if (randomizeStart)
        {
            timeOffset = Random.Range(0f, 2f * Mathf.PI);
        }
    }

    private void Update()
    {
        float newY = startPosition.y + amplitude * Mathf.Sin((Time.time + timeOffset) * frequency);
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}