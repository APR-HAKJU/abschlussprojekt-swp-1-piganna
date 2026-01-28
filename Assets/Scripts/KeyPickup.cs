using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [Header("Pickup Einstellungen")]
    public KeyCode pickupKey = KeyCode.E;       // Taste zum Aufheben
    public float pickupDistance = 2f;           // Abstand, in dem man den Schlüssel einsammeln kann
    public Transform player;                    // Referenz auf den Spieler

    [Header("Optional: Feedback")]
    public GameObject pickupText;               // z.B. "Drücke E um Schlüssel aufzunehmen"
    public float rotationSpeed = 50f;
    public AudioSource audioSource;
    public AudioClip pickupSound;

    private bool isPickedUp = false;

    void Start()
    {
        // Falls kein Player gesetzt ist, automatisch suchen (über Tag "Player")
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        if (pickupText != null)
            pickupText.SetActive(false);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPickedUp) return;
        if (player == null) return;

        // Optional: Schlüssel dreht sich
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Abstand zum Spieler prüfen
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= pickupDistance)
        {
            if (pickupText != null)
                pickupText.SetActive(true);

            if (Input.GetKeyDown(pickupKey))
            {
                PickupKey();
            }
        }
        else
        {
            if (pickupText != null)
                pickupText.SetActive(false);
        }
    }

    void PickupKey()
    {
        isPickedUp = true;

        if (pickupText != null)
            pickupText.SetActive(false);

        Debug.Log("Schlüssel aufgenommen!");

        if (audioSource != null && pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        // Schlüssel im GameManager registrieren
        if (GameManager.Instance != null)
        {
            GameManager.Instance.GiveKey();
        }

        // Schlüssel verschwinden lassen
        gameObject.SetActive(false);
    }
}

