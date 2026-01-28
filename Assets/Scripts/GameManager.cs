using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game State")]
    public bool hasKey = false;
    public bool hasCode = false;
    
    [Header("Geheimcode")]
    public string secretCode = "2319";

    [Header("Events")]
    public UnityEngine.Events.UnityEvent OnKeyObtained;
    public UnityEngine.Events.UnityEvent OnCodeObtained;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool HasKey()
    {
        return hasKey;
    }

    public bool HasCode()
    {
        return hasCode;
    }

    public string GetSecretCode()
    {
        return secretCode;
    }

    public void GiveKey()
    {
        if (!hasKey)
        {
            hasKey = true;
            Debug.Log("Spieler hat den Schlüssel erhalten!");
            OnKeyObtained?.Invoke();
        }
    }

    public void GiveCode()
    {
        if (!hasCode)
        {
            hasCode = true;
            Debug.Log("Spieler hat den Geheimcode erhalten: " + secretCode);
            OnCodeObtained?.Invoke();
        }
    }

    // Reset für Testing
    [ContextMenu("Reset Game State")]
    public void ResetGameState()
    {
        hasKey = false;
        hasCode = false;
        Debug.Log("Game State zurückgesetzt");
    }
}

