using UnityEngine;
using TMPro;

/// <summary>
/// Einfaches UI-Script für die Zahl-Anzeige im Hebelraum
/// Erstelle ein Canvas mit diesem Script und weise es dem Lever zu
/// </summary>
public class CodeDisplayUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI codeText;
    [SerializeField] private GameObject codePanel;

    void Start()
    {
        // Verstecke UI am Anfang
        if (codePanel != null)
        {
            codePanel.SetActive(false);
        }
    }

    public void ShowCode(string code)
    {
        if (codeText != null)
        {
            codeText.text = "Geheimzahl: " + code;
        }

        if (codePanel != null)
        {
            codePanel.SetActive(true);
        }
    }

    public void HideCode()
    {
        if (codePanel != null)
        {
            codePanel.SetActive(false);
        }
    }
}

