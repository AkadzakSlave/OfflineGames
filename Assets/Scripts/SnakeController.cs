using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SnakeController : MonoBehaviour
{
    public GameObject headPrefab;
    public GameObject segmentPrefab;
    public float moveSpeed = 0.2f;
    public int initialSize = 3;
    public LayerMask wallLayer; // Слой для стен

    private Vector2 direction = Vector2.right;
    private Vector2 nextDirection = Vector2.right;
    public List<Transform> bodySegments = new List<Transform>();
    private Transform head;
    private float nextMoveTime;
    private float segmentSize = 0.35f;

    [Header("Mobile Controls")]
    public Button upButton;
    public Button downButton;
    public Button leftButton;
    public Button rightButton;

    void Start()
    {
        upButton.onClick.AddListener(() => SetNextDirection(Vector2.up));
        downButton.onClick.AddListener(() => SetNextDirection(Vector2.down));
        leftButton.onClick.AddListener(() => SetNextDirection(Vector2.left));
        rightButton.onClick.AddListener(() => SetNextDirection(Vector2.right));
        ResetGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && direction != Vector2.down)
            nextDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.S) && direction != Vector2.up)
            nextDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.A) && direction != Vector2.right)
            nextDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.D) && direction != Vector2.left)
            nextDirection = Vector2.right;
    }

    void FixedUpdate()
    {
        if (Time.time >= nextMoveTime)
        {
            direction = nextDirection;
            Move();
            nextMoveTime = Time.time + moveSpeed;
        }
    }
    void SetNextDirection(Vector2 newDirection)
    {
        if (newDirection != -direction)
        {
            nextDirection = newDirection;
        }
    }
    void Move()
    {
        Vector3[] previousPositions = new Vector3[bodySegments.Count + 1];
        previousPositions[0] = head.position;
        
        for (int i = 0; i < bodySegments.Count; i++)
        {
            previousPositions[i+1] = bodySegments[i].position;
        }

        // Двигаем голову с проверкой столкновений
        Vector2 newPosition = head.position + (Vector3)(direction * segmentSize);
        if (!Physics2D.OverlapCircle(newPosition, 0.2f, wallLayer))
        {
            head.position = newPosition;
            
            for (int i = 0; i < bodySegments.Count; i++)
            {
                bodySegments[i].position = previousPositions[i];
            }
        }
        else
        {
            ResetGame(); // Столкновение со стеной
        }
    }

    public void AddBodySegment()
    {
        GameObject newSegment = Instantiate(segmentPrefab);
        if (bodySegments.Count > 0)
            newSegment.transform.position = bodySegments[bodySegments.Count - 1].position;
        else
            newSegment.transform.position = head.position - (Vector3)(direction * segmentSize);
    
        bodySegments.Add(newSegment.transform);
    }

    void ResetGame()
    {
        foreach (Transform segment in bodySegments)
        {
            if (segment != null)
                Destroy(segment.gameObject);
        }
        bodySegments.Clear();

        if (head != null) Destroy(head.gameObject);

        head = Instantiate(headPrefab, Vector3.zero, Quaternion.identity).transform;
        head.GetComponent<Rigidbody2D>().isKinematic = true;

        for (int i = 0; i < initialSize; i++)
        {
            AddBodySegment();
        }

        direction = Vector2.right;
        nextDirection = Vector2.right;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Столкновение с: {other.tag}");
    
        if (other.CompareTag("Food"))
        {
            Debug.Log("Еда найдена!");
            Destroy(other.gameObject);
        
            FoodSpawner spawner = FindObjectOfType<FoodSpawner>();
            if (spawner != null)
            {
                spawner.SpawnFood();
            }
            else
            {
                Debug.LogError("FoodSpawner не найден!");
            }
        
            AddBodySegment();
        }
        else if (other.CompareTag("Wall") || other.CompareTag("Snake"))
        {
            Debug.Log("Столкновение с препятствием");
            ResetGame();
        }
    }
}