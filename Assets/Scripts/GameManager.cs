using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadSnakeGame()
    {
        SceneManager.LoadScene("Snake");
    }

    public void LoadTicTacToe()
    {
        SceneManager.LoadScene("TicTacToe");
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}