using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] int StartBalence = 150;
    [SerializeField] int currentbalence;

    [SerializeField] TextMeshProUGUI currentBalcenceText;

    public int CurrentBalence
    {
        get
        {
            return currentbalence;
        }
    }

    public void Deposit(int amount)
    {
        currentbalence += Mathf.Abs(amount);
        UpdateBalence();
    }

    public void Withdraw(int amount)
    {
        currentbalence -= Mathf.Abs(amount);
        UpdateBalence();
        
        if(currentbalence < 0)
        {
            // TODO game over sequence 
            ReloadScene();
        }
    }

    void Awake()
    {
        currentbalence = StartBalence;
        UpdateBalence();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void UpdateBalence()
    {
        currentBalcenceText.text = currentbalence.ToString();
    }
}
