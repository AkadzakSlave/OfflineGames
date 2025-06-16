using UnityEngine;
using UnityEngine.UI;

public class FoodCounterUI : MonoBehaviour
{
    public Text counterText;

    void Start()
    {
        FoodSpawner.OnFoodEaten += UpdateCounter;
        UpdateCounter(FoodSpawner.foodEatenCount);
    }

    void UpdateCounter(int count)
    {
        counterText.text = $"Счет: {count}";
    }

    void OnDestroy()
    {
        FoodSpawner.OnFoodEaten -= UpdateCounter;
    }
}