using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDown : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float timeToCompleate = 180f;
    [SerializeField] float LevelLoadDelay = 3f;
    [SerializeField] ParticleSystem winParticles;
    bool isFinished;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToCompleate -= Time.deltaTime;
        timeToCompleate = Mathf.Clamp(timeToCompleate, 0, float.MaxValue);

        timerText.text = (timeToCompleate).ToString("0");

        if (timeToCompleate <= 10)
        {
            timerText.color = Color.red;
        }

        if (timeToCompleate <= 0 && !isFinished)
        {
            isFinished = true;
            Invoke(nameof(LoadNextLevel), LevelLoadDelay);
            winParticles.Play();
        }
    }


    void LoadNextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
            Debug.Log("This is the last level");
        }

        SceneManager.LoadScene(nextSceneIndex);
    }
}
