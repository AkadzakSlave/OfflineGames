using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;
    public float spawnDelay = 3f;
    public Vector2 bounds = new Vector2(10, 10);
    
    // Добавляем счетчик еды
    public static int foodEatenCount = 0;
    public static System.Action<int> OnFoodEaten; 
    

    void Start()
    {
        SpawnFood();
    }

    public void SpawnFood()
    {
        Vector3 randomPos = new Vector3(
            Mathf.Round(Random.Range(-bounds.x, bounds.x)*0.4f),
            Mathf.Round(Random.Range(-bounds.y, bounds.y)*0.4f),
            0
        );
        
        GameObject newFood = Instantiate(foodPrefab, randomPos, Quaternion.identity);
        
        // Добавляем компонент для обработки столкновений
        FoodCollisionHandler handler = newFood.AddComponent<FoodCollisionHandler>();
        handler.Initialize(this);
    }

    // Метод для обработки поедания еды
    public void FoodEaten(GameObject food)
    {
        Destroy(food);
        foodEatenCount++;
        OnFoodEaten?.Invoke(foodEatenCount); // Уведомляем UI об изменении
        FindObjectOfType<SnakeController>()?.AddBodySegment();
        SpawnFood();
    }
}

// Новый компонент для обработки столкновений с едой
public class FoodCollisionHandler : MonoBehaviour
{
    private FoodSpawner spawner;

    public void Initialize(FoodSpawner spawner)
    {
        this.spawner = spawner;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Проверяем столкновение с головой змейки (должен быть тег "SnakeHead")
        if (other.CompareTag("SnakeHead"))
        {
            spawner.FoodEaten(gameObject);
        }
    }
}