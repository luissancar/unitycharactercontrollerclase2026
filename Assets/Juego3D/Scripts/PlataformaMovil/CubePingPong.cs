using UnityEngine;

public class CubePingPong : MonoBehaviour
{
    [Header("Puntos entre los que se moverá")] [SerializeField]
    private Transform pointA;

    [SerializeField] private Transform pointB;

    [Header("Velocidad de movimiento")] [SerializeField]
    private float velocidad = 2f;

    private Transform currentTarget;

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("puntos null");
            Destroy(this.gameObject);
            return;
        }

        transform.position = pointA.position;
        currentTarget = pointB;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position,
            currentTarget.position, velocidad * Time.deltaTime);
        if (transform.position== currentTarget.position)
        {
            currentTarget = (currentTarget == pointA) ? pointB : pointA;
        }
    }
}