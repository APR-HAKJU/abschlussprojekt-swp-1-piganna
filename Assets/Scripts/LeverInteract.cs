using UnityEngine;

public class LeverInteract : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Camera playerCamera;
    public float interactDistance = 3f;
    public KeyCode interactKey = KeyCode.E;

    [Header("UI Feedback")]
    public TMPro.TextMeshProUGUI interactText;  // Optional: "Drücke E zum Interagieren"
    public GameObject crosshair;

    private Lever currentLever;

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        if (interactText != null)
        {
            interactText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckForLever();
        HandleInteraction();
    }

    void CheckForLever()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Lever lever = hit.collider.GetComponentInParent<Lever>();
            
            if (lever != null)
            {
                currentLever = lever;
                ShowInteractText(true);
            }
            else
            {
                currentLever = null;
                ShowInteractText(false);
            }
        }
        else
        {
            currentLever = null;
            ShowInteractText(false);
        }
    }

    void HandleInteraction()
    {
        if (Input.GetKeyDown(interactKey) && currentLever != null)
        {
            currentLever.Activate();
        }
    }

    void ShowInteractText(bool show)
    {
        if (interactText != null)
        {
            interactText.gameObject.SetActive(show);
            if (show)
            {
                interactText.text = "Drücke [E] zum Hebel ziehen";
            }
        }

        // Optional: Crosshair ändern wenn Hebel in Reichweite
        if (crosshair != null)
        {
            // Du könntest hier die Farbe ändern oder ein Icon zeigen
        }
    }

    // Visualisierung der Interaktionsreichweite im Editor
    void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.yellow;
            Vector3 forward = playerCamera.transform.forward * interactDistance;
            Gizmos.DrawRay(playerCamera.transform.position, forward);
        }
    }
}

