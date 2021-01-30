using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverText;
    [SerializeField] GameObject gameClearText;

    [SerializeField] Text scoreText;


    //SE
    [SerializeField] AudioClip gameClearSE;
    [SerializeField] AudioClip gameOverSE;
    AudioSource audioSource;

    const int MAX_SCORE = 9999;
    int score = 0;
    private void Start()
    {
        scoreText.text = score.ToString();
        audioSource = GetComponent<AudioSource>(); 
    }
    public void AddScore(int val)
    {
        score += val;
        if (score > MAX_SCORE) 
        {
            score = MAX_SCORE;
        }
        scoreText.text = score.ToString();
    }
    void RestartScene()
    {
        Scene thisScene = SceneManager.GetActiveScene();//現在のシーンを読み込む
        SceneManager.LoadScene(thisScene.name);
    }
    public void GameOver()
    {
        gameOverText.SetActive(true);//非常時にしているGameOverのテキストを表示する
        audioSource.PlayOneShot(gameOverSE);
        Invoke("RestartScene", 1.5f); //Invoke("関数名",秒数) で関数の発火を遅らせる。

    }

    public void GameClear()
    {
        gameClearText.SetActive(true);
        audioSource.PlayOneShot(gameClearSE);
        Invoke("RestartScene", 1.5f);

    }

}
