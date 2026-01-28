using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Door Settings")]
    public bool isOpen = false;
    public float openAngle = -90f;
    public float closeAngle = 0f;
    public float rotationSpeed = 2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip lockedSound;

    [Header("Visual Feedback")]
    public GameObject lockedIndicator;
    public Light doorLight;

    [Header("UI")]
    public GameObject codeDisplayUI;    // Optional: UI das den Code zeigt wenn Spieler ihn hat
    public TMPro.TextMeshProUGUI codeText;

    private bool hasTriedToOpen = false;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        UpdateCodeDisplay();
    }

    void Update()
    {
        // Tür öffnet sich automatisch wenn Spieler den Code hat
        if (!isOpen && GameManager.Instance.HasCode())
        {
            OpenDoor();
        }

        // Rotation für Tür-Animation
        if (isOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0, openAngle, 0);
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );

            if (doorLight != null)
            {
                doorLight.enabled = true;
                doorLight.color = Color.green;
            }
        }
        else
        {
            Quaternion targetRotation = Quaternion.Euler(0, closeAngle, 0);
            transform.localRotation = Quaternion.Slerp(
                transform.localRotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }

        UpdateCodeDisplay();
    }

    public void TryOpen()
    {
        if (GameManager.Instance.HasCode())
        {
            OpenDoor();
        }
        else
        {
            if (!hasTriedToOpen)
            {
                Debug.Log("Die Tür ist verschlossen. Du brauchst den Geheimcode.");
                PlaySound(lockedSound);
                hasTriedToOpen = true;
                
                if (lockedIndicator != null)
                {
                    lockedIndicator.SetActive(true);
                    Invoke(nameof(HideLockedIndicator), 3f);
                }
            }
        }
    }

    private void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Ausgangstür öffnet sich! Du hast es geschafft!");
            PlaySound(openSound);
            
            if (lockedIndicator != null)
            {
                lockedIndicator.SetActive(false);
            }
        }
    }

    private void HideLockedIndicator()
    {
        if (lockedIndicator != null)
        {
            lockedIndicator.SetActive(false);
        }
        hasTriedToOpen = false;
    }

    private void UpdateCodeDisplay()
    {
        if (codeDisplayUI != null)
        {
            codeDisplayUI.SetActive(GameManager.Instance.HasCode());
        }

        if (codeText != null && GameManager.Instance.HasCode())
        {
            codeText.text = "Geheimcode: " + GameManager.Instance.GetSecretCode();
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Für Interaktion wenn Spieler nahe ist
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isOpen)
        {
            TryOpen();
        }
    }
}

