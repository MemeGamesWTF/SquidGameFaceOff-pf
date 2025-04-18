using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public GameObject[] LeftSprites; // Array of left-side sprites
    public GameObject[] RightSprites; // Array of right-side sprites
    public AudioSource audioSource;
    public AudioClip jumpSFX;
    public AudioClip booSFX;
    public Transform LeftStartPosition;
    public Transform RightStartPosition;

    private GameObject currentLeftSprite;
    private GameObject currentRightSprite;

    public Text ScoreTxt;
    private int score = 0;

    public GameObject StartPanel; // Reference to the start screen panel
    public GameObject GameOverPanel; // Reference to the game over panel

    [DllImport("__Internal")]
    private static extern void SendScore(int score, int game);

    void Start()
    {
        ShowStartPanel();
    }

    void ShowStartPanel()
    {
        StartPanel.SetActive(true);
        GameOverPanel.SetActive(false);
        Time.timeScale = 0; // Pause the game
    }

    public void StartGame()
    {
        StartPanel.SetActive(false);
        Time.timeScale = 1; // Resume the game
        SpawnSprites();
    }

    public void GameOver()
    {
        Debug.Log("Game Over! Final Score: " + score);
        audioSource.PlayOneShot(booSFX);
        //SendScore(score, 20);
        GameOverPanel.SetActive(true);
        SendScore(score, 38);
        Time.timeScale = 0; // Pause the game

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the scene
    }

    void SpawnSprites()
    {
        // Destroy existing sprites if any
        if (currentLeftSprite != null) Destroy(currentLeftSprite);
        if (currentRightSprite != null) Destroy(currentRightSprite);

        // Select random sprites
        int leftIndex = Random.Range(0, LeftSprites.Length);
        currentLeftSprite = Instantiate(LeftSprites[leftIndex], LeftStartPosition.position, Quaternion.identity);

        int rightIndex = (leftIndex == 0) ? 1 : 0; // Ensure opposite pair
        currentRightSprite = Instantiate(RightSprites[rightIndex], RightStartPosition.position, Quaternion.identity);

        // Assign scripts
        currentLeftSprite.GetComponent<SpriteController>().Initialize(Vector3.right, this);
        currentRightSprite.GetComponent<SpriteController>().Initialize(Vector3.left, this);
    }

    public void AddScore()
    {
        score++;
        Debug.Log("Score: " + score);
        ScoreTxt.text = "Score: " + score;
        ResetSprites();
    }

    void ResetSprites()
    {
        currentLeftSprite.transform.position = LeftStartPosition.position;
        currentRightSprite.transform.position = RightStartPosition.position;

        // Swap roles
        GameObject temp = currentLeftSprite;
        currentLeftSprite = currentRightSprite;
        currentRightSprite = temp;

        currentLeftSprite.GetComponent<SpriteController>().SetDirection(Vector3.left);
        currentRightSprite.GetComponent<SpriteController>().SetDirection(Vector3.right);
    }
    public void JumpSFX()
    {
        audioSource.PlayOneShot(jumpSFX);
    }
    
}
