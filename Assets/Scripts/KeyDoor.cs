using UnityEngine;

public class KeyDoor : MonoBehaviour
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
    public GameObject lockedIndicator;  // Text/UI das "Verschlossen" zeigt
    public Light doorLight;             // Optional: Licht das grün wird wenn offen

    [Header("Interaction Settings")]
    public float interactionDistance = 2f;
    public KeyCode useKeyKey = KeyCode.E;
    public GameObject useKeyText;  // Optional: "Drücke E um Schlüssel zu benutzen"

    [Header("Colliders")]
    public Collider doorCollider;  // Wird deaktiviert, wenn die Tür offen ist
    public GameObject doorObject;  // Visuelles Tür-Objekt, wird beim Öffnen ausgeblendet

    private bool hasTriedToOpen = false;

    private Camera playerCamera;
    private bool playerInRange = false;

    void Start()
    {
        playerCamera = Camera.main;
        
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Falls kein Collider gesetzt ist, automatisch eigenen nehmen
        if (doorCollider == null)
        {
            doorCollider = GetComponent<Collider>();
        }

        // Falls kein Tür-Objekt gesetzt ist, dieses GameObject verwenden
        if (doorObject == null)
        {
            doorObject = gameObject;
        }

        if (useKeyText != null)
        {
            useKeyText.SetActive(false);
        }
    }

    void Update()
    {
        CheckPlayerDistance();

        // Spieler drückt E um Schlüssel zu benutzen
        if (playerInRange && Input.GetKeyDown(useKeyKey))
        {
            TryOpen();
        }
    }

    void CheckPlayerDistance()
    {
        if (playerCamera == null || isOpen) 
        {
            playerInRange = false;
            ShowUseKeyText(false);
            return;
        }

        float distance = Vector3.Distance(transform.position, playerCamera.transform.position);
        playerInRange = distance <= interactionDistance;

        // Sicherstellen, dass ein GameManager existiert
        if (playerInRange && GameManager.Instance != null && GameManager.Instance.HasKey())
        {
            ShowUseKeyText(true);
        }
        else
        {
            ShowUseKeyText(false);
        }
    }

    public void TryOpen()
    {
        if (GameManager.Instance.HasKey())
        {
            OpenDoor();
        }
        else
        {
            // Spieler hat keinen Schlüssel
            if (!hasTriedToOpen)
            {
                Debug.Log("Die Tür ist verschlossen. Du brauchst einen Schlüssel.");
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

    void ShowUseKeyText(bool show)
    {
        if (useKeyText != null)
        {
            useKeyText.SetActive(show);
        }
    }

    private void OpenDoor()
    {
        if (!isOpen)
        {
            isOpen = true;
            Debug.Log("Tür öffnet sich!");
            PlaySound(openSound);
            
            if (lockedIndicator != null)
            {
                lockedIndicator.SetActive(false);
            }

            // Collider deaktivieren, damit der Spieler durchlaufen kann
            if (doorCollider != null)
            {
                doorCollider.enabled = false;
            }

            // Visuelles Tür-Objekt ausblenden, damit nichts mehr im Weg steht
            if (doorObject != null)
            {
                doorObject.SetActive(false);
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

