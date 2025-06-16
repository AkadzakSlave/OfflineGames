using UnityEngine;
using UnityEngine.UI;

public class TicTacToeGame : MonoBehaviour
{
    [Header("Game References")]
    public Button[] buttons; // Все 9 кнопок поля
    public Image background; // Фон для смены цвета
    public Text statusText;  // Текст статуса игры
    
    [Header("Player Colors")]
    public Color xColor = Color.blue;
    public Color oColor = Color.red;
    public Color tieColor = Color.gray;

    private string currentPlayer = "X";
    private string[] board = new string[9];
    private bool gameOver = false;

    void Start()
    {
        ValidateButtons();
        ResetGame();
    }

    // Проверка что все кнопки назначены
    private void ValidateButtons()
    {
        if (buttons.Length != 9)
        {
            Debug.LogError("Должно быть ровно 9 кнопок!");
            return;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i] == null)
            {
                Debug.LogError($"Кнопка {i} не назначена!");
            }
        }
    }

    public void OnCellClick(int index)
    {
        if (gameOver) return;
        
        // Проверяем допустимость хода
        if (index < 0 || index >= 9)
        {
            Debug.LogError($"Неверный индекс клетки: {index}");
            return;
        }

        if (!string.IsNullOrEmpty(board[index]))
        {
            Debug.Log($"Клетка {index} уже занята");
            return;
        }

        // Совершаем ход
        MakeMove(index);
    }

    private void MakeMove(int index)
    {
        // Обновляем состояние игры
        board[index] = currentPlayer;
        UpdateButtonVisual(index);
        UpdateBackgroundColor();

        // Проверяем условия окончания игры
        if (CheckWin())
        {
            statusText.text = $"Победа {currentPlayer}!";
            gameOver = true;
            return;
        }

        if (IsBoardFull())
        {
            statusText.text = "Ничья!";
            background.color = tieColor;
            gameOver = true;
            return;
        }

        // Передаем ход другому игроку
        SwitchPlayer();
    }

    private void UpdateButtonVisual(int index)
    {
        Text buttonText = buttons[index].GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = currentPlayer;
            buttonText.color = (currentPlayer == "X") ? Color.white : Color.black;
        }
        else
        {
            Debug.LogError($"Не найден Text компонент на кнопке {index}");
        }
    }

    private void UpdateBackgroundColor()
    {
        background.color = (currentPlayer == "X") ? xColor : oColor;
    }

    private void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == "X") ? "O" : "X";
        statusText.text = $"Ход: {currentPlayer}";
    }

    bool CheckWin()
    {
        // Проверка горизонталей
        for (int i = 0; i < 9; i += 3)
        {
            if (!string.IsNullOrEmpty(board[i]) && 
                board[i] == board[i+1] && 
                board[i] == board[i+2])
                return true;
        }

        // Проверка вертикалей
        for (int i = 0; i < 3; i++)
        {
            if (!string.IsNullOrEmpty(board[i]) && 
                board[i] == board[i+3] && 
                board[i] == board[i+6])
                return true;
        }

        // Проверка диагоналей
        if (!string.IsNullOrEmpty(board[0]) && 
            board[0] == board[4] && 
            board[0] == board[8])
            return true;

        if (!string.IsNullOrEmpty(board[2]) && 
            board[2] == board[4] && 
            board[2] == board[6])
            return true;

        return false;
    }

    bool IsBoardFull()
    {
        foreach (string cell in board)
        {
            if (string.IsNullOrEmpty(cell))
                return false;
        }
        return true;
    }

    bool IsGameOver()
    {
        return gameOver;
    }

    public void ResetGame()
    {
        currentPlayer = "X";
        board = new string[9];
        gameOver = false;
        
        background.color = xColor;
        statusText.text = $"Ход: {currentPlayer}";
        
        foreach (Button button in buttons)
        {
            if (button != null)
            {
                Text text = button.GetComponentInChildren<Text>();
                if (text != null)
                {
                    text.text = "";
                }
            }
        }
    }
}