using UnityEngine;
using TMPro;

public class SocketTrigger : MonoBehaviour
{
    [Header("Einstellungen")]
    public string movableTag = "Moveable";       // Tag der beweglichen Objekte
    public string correctObjectName = "Dagger";  // Name des Dolches
    public AudioClip successSound;               // Sound, der abgespielt wird
    public TMP_Text numberText;                  // Referenz auf 3D TextMeshPro Text

    private AudioSource audioSource;

    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        //if (audioSource == null)
      // {
           // Debug.LogWarning("Keine AudioSource gefunden! Sound wird nicht abgespielt.");
       // }

       // if (successSound == null)
      //  {
            // Debug.LogWarning("Kein SuccessSound zugewiesen!");
        //}

        // Text zu Beginn leer machen
        if (numberText != null)
        {
            numberText.text = "";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Prüfen: Ist es der richtige Dolch?
        if (other.CompareTag(movableTag) && other.gameObject.name.Contains(correctObjectName))
        {
            // Sound nur abspielen, wenn AudioSource und Clip vorhanden
            if (audioSource != null && successSound != null)
            {
                audioSource.PlayOneShot(successSound);
            }

            // Zahl 1 im Text anzeigen
            if (numberText != null)
            {
                numberText.text = " Your number is 1";
            }

            Debug.Log("Your number is 1");
        }
    }
}
