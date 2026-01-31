using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PointsScaleSystem : MonoBehaviour
{
    public static PointsScaleSystem instance;
    
    [SerializeField] private int currentScore = +2;
    [SerializeField] private TextMeshProUGUI textRenderer;

    [SerializeField] private int scorePerDead = -1;
    [SerializeField] private int scorePerSaved = +1;
    
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private List<ScoreMusicRange> musicRanges;
    private int _currentRangeIndex = -1;
    
    [SerializeField] private Image scoreImage;
    [SerializeField] private Gradient scoreGradient;
    [SerializeField] private int minValue = -10;
    [SerializeField] private int maxValue = 4;

    [SerializeField] private List<GameObject> turnOffAfterDeath;
    [SerializeField] private List<GameObject> turnOnAfterDeath;

    [SerializeField] private AudioClip deathMusic;
    private bool _finished = false;
    
    
    
    public void OnFinish()
    {
        _finished = true;
        musicSource.clip = deathMusic;
        musicSource.Play();
        
        foreach (var target in turnOffAfterDeath)
        {
            target.SetActive(false);
        }
        
        foreach (var target in turnOnAfterDeath)
        {
            target.SetActive(true);
        }
    }
    
    public Color GetColorForInt(int value)
    {
        float t = Mathf.InverseLerp(minValue, maxValue, value);
        return scoreGradient.Evaluate(t);
    }
    
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            print("Instance already exists");
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        OnScoreChanged(currentScore);
    }
    
    public void OnDeadBody()
    {
        currentScore += scorePerDead;
        OnScoreChanged(currentScore);
    }
    
    public void OnSaved()
    {
        currentScore += scorePerSaved;
        OnScoreChanged(currentScore);
    }
    
    int GetMusicRangeIndex(int score)
    {
        for (int i = 0; i < musicRanges.Count; i++)
        {
            if (score >= musicRanges[i].minScore && score <= musicRanges[i].maxScore)
                return i;
        }
        return -1;
    }
    
    public void OnScoreChanged(int score)
    {
        if (_finished) return;
        if (score < minValue)
        {
            OnFinish();
            return;
        }
            
            
        if (this.textRenderer != null)
        {
            this.textRenderer.text = currentScore.ToString();
        }

        if (this.scoreImage != null)
        {
            this.scoreImage.color = GetColorForInt(currentScore);
        }
        
        if (musicSource != null && musicRanges.Count > 0)
        {
            int rangeIndex = GetMusicRangeIndex(score);
            if (rangeIndex == _currentRangeIndex) return;

            _currentRangeIndex = rangeIndex;
            if (rangeIndex < 0) return;

            var range = musicRanges[rangeIndex];
            
            musicSource.clip = range.clip;
            musicSource.Play();
        }
    }

    public int GetCurrentScore()
    {
        return currentScore;
    }
    
    public void RestartLevel()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.buildIndex);
    }
    
    
}


[System.Serializable]
public class ScoreMusicRange
{
    public int minScore;
    public int maxScore;
    public AudioClip clip;
}