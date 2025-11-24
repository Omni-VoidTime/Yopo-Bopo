using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FloatyBobMovement : MonoBehaviour
{
    [Header("Bob Settings")]
    public float minAmplitude = 0.5f; // Height of the bobbing motion
    public float maxAmplitude = 1.5f;   
    public float minFrequency = 1f; // Speed of the bobbing motion
    public float maxFrequency = 2.5f;
    public bool randomizePhase = true;

    private float amplitude;
    private float frequency;

    private Vector3 startPos;
    private float phaseOffset;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;

        amplitude = Random.Range(minAmplitude, maxAmplitude);
        frequency = Random.Range(minFrequency, maxFrequency);

        phaseOffset = randomizePhase ? Random.Range(0f, 2f * Mathf.PI) : 0f;
    }
    void FixedUpdate()
    {
        float offset = Mathf.Sin(Time.time * frequency + phaseOffset) * amplitude;
        Vector2 nextPosition = new Vector2(startPos.x, startPos.y + offset);
        rb.MovePosition(nextPosition);
    }
}
