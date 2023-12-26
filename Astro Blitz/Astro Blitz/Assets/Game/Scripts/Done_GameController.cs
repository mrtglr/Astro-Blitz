using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Done_GameController : MonoBehaviour
{
    public GameObject[] hazards;
    public GameObject shield;
    public Vector3 spawnValues;
    public int hazardCount;
    public float spawnWait;
    public float startWait;
    public float waveWait;
    public float spawnWait_shield;
    public float startWait_shield;
    public float waveWait_shield;
    public int coreHeat;
    public bool startCoolDown = false;
    public bool shieldEnabled = false;

    public GameObject endPanel;

    public Text scoreText;
    public Text finalScoreText;
    public Text gameOverText;
    public Text coreHeatText;
    public Text shieldEnabledText;

    public Image shieldImage;

    private bool gameOver;    
    private int score;
    private float shieldCountdownTimer = 5f;
    private string highScoreKey = "HighScore";

    void Start()
    {
        TogglePanel(false);
        gameOver = false;
        showShiedImage(false);
        shieldEnabledText.text = "";
        finalScoreText.text = "";
        gameOverText.text = "";
        score = 0;
        UpdateScore();
        StartCoroutine(ReduceCoreHeat());
        StartCoroutine(SpawnWaves());
        StartCoroutine(SpawnShields());
    }

    void Update()
    {
        if (shieldEnabled)
        {
            StartCoroutine(EnableShield());
        }
    }

    private IEnumerator ReduceCoreHeat()
    {
        while (true)
        {
            if (coreHeat > 0)
            {
                UpdateCoreHeat(-10);
            }            

            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(startWait);
        while (true)
        {
            for (int i = 0; i < hazardCount; i++)
            {
                GameObject hazard = hazards[Random.Range(0, hazards.Length)];
                Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
                Quaternion spawnRotation = Quaternion.identity;
                Instantiate(hazard, spawnPosition, spawnRotation);
                yield return new WaitForSeconds(spawnWait);
            }
            yield return new WaitForSeconds(waveWait);

            if (gameOver)
            {
                break;
            }
        }
    }

    public void IncreaseSpawnRatesByScore(float score)
    {
        if(score % 100 == 0)
        {
            startWait -= 0.1f;
            spawnWait -= 0.1f;
            waveWait -= 0.1f;
        }
    }

    IEnumerator SpawnShields()
    {
        yield return new WaitForSeconds(startWait_shield);
        while (true)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
            Quaternion spawnRotation = Quaternion.identity;
            Instantiate(shield, spawnPosition, spawnRotation);
            yield return new WaitForSeconds(spawnWait_shield);

            yield return new WaitForSeconds(waveWait_shield);

            if (gameOver)
            {
                break;
            }
        }
    }

    public void AddScore(int newScoreValue)
    {
        score += newScoreValue;
        UpdateScore();
    }

    public void RemoveScore(int newScoreValue)
    {
        if(score > 0)
        {
            score -= newScoreValue;
            UpdateScore();
        }        
    }

    void UpdateScore()
    {
        if(score > 100)
            IncreaseSpawnRatesByScore(score);

        if (scoreText.text.Length > 0)
            scoreText.text = "Score: " + score;
    }

    public void UpdateCoreHeat(int _coreHeat)
    {
        coreHeat = Mathf.Clamp(coreHeat + _coreHeat, 0, 100);

        if (coreHeat == 0)
        {
            coreHeatText.color = new Color(0.37f, 1f, 0.28f); // Green color
        }
        else if (coreHeat > 20)
        {
            coreHeatText.color = new Color(0.95f, 1f, 0.28f); // Yellow color
        }
        else if (coreHeat > 50)
        {
            coreHeatText.color = new Color(1f, 0.82f, 0.28f); // Orange color
        }
        else if (coreHeat > 80)
        {
            coreHeatText.color = new Color(1f, 0.38f, 0.13f); // Red-Orange color
        }

        coreHeatText.text = "Core Heat: " + coreHeat + "%";

        if (coreHeat == 100 && !startCoolDown)
        {
            coreHeatText.color = new Color(0.71f, 0f, 0f); // Dark Red color
            StartCoroutine(CoolDown());
        }
    }

    private IEnumerator CoolDown()
    {
        RectTransform textRectTransform = coreHeatText.GetComponent<RectTransform>();
        textRectTransform.anchoredPosition += new Vector2(-140f, 0f);
        startCoolDown = true;

        float startTime = Time.time;
        float elapsedTime = 0f;

        Color startColor = new Color(0.71f, 0f, 0f); // Dark Red color
        Color endColor = new Color(0.37f, 1f, 0.28f); // Green color

        while (elapsedTime < 5f)
        {
            elapsedTime = Time.time - startTime;

            float cooledValue = Mathf.Lerp(100, 0, elapsedTime / 5f);
            coreHeat = (int)cooledValue;

            // Update text color during cooldown
            coreHeatText.color = Color.Lerp(startColor, endColor, elapsedTime / 5f);

            coreHeatText.text = "Cooldown enabled: " + coreHeat + "%";

            yield return null;
        }

        coreHeat = 0;
        coreHeatText.color = new Color(0.37f, 1f, 0.28f); // Green color
        coreHeatText.text = "Core Heat: " + coreHeat + "%";

        startCoolDown = false;
        textRectTransform.anchoredPosition += new Vector2(110f, 0f);
    }

    public void GameOver()
    {
        TogglePanel(true);
        gameOverText.text = "Game Over!";
        gameOver = true;

        int currentHighScore = PlayerPrefs.GetInt(highScoreKey, 0);
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(highScoreKey, score);
            PlayerPrefs.Save();
        }

        finalScoreText.text = scoreText.text;
        scoreText.text = "";
        coreHeatText.text = "";
    }

    private void TogglePanel(bool show)
    {
        if (endPanel != null)
        {
            endPanel.SetActive(show);
        }
    }

    public IEnumerator EnableShield()
    {
        shieldCountdownTimer = 5f;
        UpdateShieldEnabledText();
        while (shieldCountdownTimer > 0f)
        {
            yield return new WaitForSeconds(1f);
            shieldCountdownTimer -= 1f;
            UpdateShieldEnabledText();
        }
        shieldEnabled = false;
        showShiedImage(false);
        shieldEnabledText.text = "";
        UpdateShieldEnabledText();
    }

    public void showShiedImage(bool show)
    {
        if (shieldImage != null)
        {
            shieldImage.enabled = show;
        }
    }

    public void UpdateShieldEnabledText()
    {
        if (shieldCountdownTimer >= 0)
        {
            shieldEnabledText.text = "Shield is enabled for: " + Mathf.Round(shieldCountdownTimer) + "s";
        }
    }
}