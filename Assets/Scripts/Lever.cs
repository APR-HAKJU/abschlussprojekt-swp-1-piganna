using UnityEngine;

public class Lever : MonoBehaviour
{
    [Header("Lever Type")]
    [Tooltip("Wenn true: Dieser Hebel gibt den Schlüssel (für Labyrinth). Wenn false: Normaler Hebel")]
    public bool givesKey = false;
    
    [Tooltip("Wenn true: Dieser Hebel gibt den Geheimcode (für Hebelraum)")]
    public bool givesCode = false;

    [Header("Visual Feedback")]
    public Transform leverHandle;      // Das bewegliche Teil des Hebels
    public float rotateAngle = -45f;    // Wie weit sich der Hebel bewegt
    public float rotateSpeed = 5f;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip leverPullSound;
    public AudioClip correctSound;     // Sound wenn richtiger Hebel
    public AudioClip wrongSound;       // Sound wenn falscher Hebel

    [Header("Visual Effects")]
    public GameObject correctEffect;    // Partikel/Effekt bei richtigem Hebel
    public Light indicatorLight;       // Optional: Licht das leuchtet wenn richtig

    [Header("UI für Zahl-Anzeige")]
    public GameObject codeDisplayUI;   // UI Panel das die Zahl zeigt
    public TMPro.TextMeshProUGUI codeText;  // Text-Element für die Zahl
    public float displayDuration = 5f; // Wie lange die Zahl angezeigt wird

    private bool isActivated = false;
    private Quaternion initialRotation;

    void Start()
    {
        if (leverHandle != null)
        {
            initialRotation = leverHandle.localRotation;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void Activate()
    {
        if (isActivated) return;
        
        isActivated = true;
        PlaySound(leverPullSound);

        if (givesKey)
        {
            // Richtiger Hebel im Labyrinth - gibt Schlüssel
            Debug.Log("Richtiger Hebel gezogen! Du hast den Schlüssel erhalten.");
            GameManager.Instance.GiveKey();
            PlaySound(correctSound);
            ShowCorrectEffect();
        }
        else if (givesCode)
        {
            // Richtiger Hebel im Hebelraum - gibt Geheimcode
            string code = GameManager.Instance.GetSecretCode();
            Debug.Log("Richtiger Hebel gezogen! Geheimcode: " + code);
            GameManager.Instance.GiveCode();
            PlaySound(correctSound);
            ShowCorrectEffect();
            ShowCodeDisplay(code);
        }
        else
        {
            // Falscher Hebel
            Debug.Log("Falscher Hebel.");
            PlaySound(wrongSound);
        }
    }

    void Update()
    {
        // Einfache "Umklapp"-Animation
        if (leverHandle != null && isActivated)
        {
            Quaternion targetRot = initialRotation * Quaternion.Euler(rotateAngle, 0f, 0f);
            leverHandle.localRotation = Quaternion.Lerp(
                leverHandle.localRotation,
                targetRot,
                Time.deltaTime * rotateSpeed
            );

            // Optional: Licht einschalten wenn richtig
            if (indicatorLight != null && (givesKey || givesCode))
            {
                indicatorLight.enabled = true;
                indicatorLight.color = Color.green;
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void ShowCorrectEffect()
    {
        if (correctEffect != null)
        {
            correctEffect.SetActive(true);
        }
    }

    private void ShowCodeDisplay(string code)
    {
        if (codeDisplayUI != null)
        {
            codeDisplayUI.SetActive(true);
        }

        if (codeText != null)
        {
            codeText.text = "Geheimzahl: " + code;
        }

        // Nach displayDuration Sekunden wieder ausblenden
        Invoke(nameof(HideCodeDisplay), displayDuration);
    }

    private void HideCodeDisplay()
    {
        if (codeDisplayUI != null)
        {
            codeDisplayUI.SetActive(false);
        }
    }

    // Für Debugging im Editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = givesKey || givesCode ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}

