using System;
using UnityEngine;

public class CoolnessManager : MonoBehaviour
{
    public static CoolnessManager Instance { get; private set; }

    public static int Score => Instance != null ? Instance.score : 0;

    public static event Action<int> ScoreChanged;

    [SerializeField] private int startingScore = 0;

    private int score;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        score = startingScore;
    }

    public static void Add(int amount)
    {
        if (Instance == null)
            return;

        Instance.score += amount;
        ScoreChanged?.Invoke(Instance.score);
    }

    public static void Remove(int amount)
    {
        Add(-amount);
    }

    public static void Set(int amount)
    {
        if (Instance == null)
            return;

        Instance.score = amount;
        ScoreChanged?.Invoke(Instance.score);
    }

    public static void Reset()
    {
        Set(0);
    }
}
