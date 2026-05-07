using UnityEngine;

public class Manager : MonoBehaviour
{
    public static Manager Instance;

    public Canvas canvas;

    public bool isGamePaused = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        canvas.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PauseGame()
    {
        canvas.gameObject.SetActive(true);
        Time.timeScale = 0f;
        isGamePaused = true;
    }
    public void ResumeGame()
    {
        canvas.gameObject.SetActive(false);
        Time.timeScale = 1f;
        isGamePaused = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
