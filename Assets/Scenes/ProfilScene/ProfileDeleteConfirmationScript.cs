using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ProfileDeleteConfirmationScript : MonoBehaviour
{
    public Text confirmationText; // Référence au texte de confirmation
    public Button confirmButton; // Bouton "Confirmer"
    public Button cancelButton; // Bouton "Annuler"

    public UnityEvent onConfirm; // Événement déclenché lorsque l'utilisateur confirme

    private void Start()
    {
        // Attachez des gestionnaires d'événements aux boutons
        confirmButton.onClick.AddListener(ShowConfirmationDialog);
        cancelButton.onClick.AddListener(HideConfirmationDialog);
    }

    public void ShowConfirmationDialog()
    {
        // Affichez le panneau de confirmation
        gameObject.SetActive(true);

        // Définissez le texte de confirmation
        confirmationText.text = "Êtes-vous sûr de vouloir supprimer votre profil ?";

        // Attachez un gestionnaire d'événements au bouton "Confirmer"
        confirmButton.onClick.AddListener(ConfirmDeletion);
    }

    public void HideConfirmationDialog()
    {
        // Masquez le panneau de confirmation
        gameObject.SetActive(false);

        // Détachez le gestionnaire d'événements du bouton "Confirmer"
        confirmButton.onClick.RemoveListener(ConfirmDeletion);
    }

    public void ConfirmDeletion()
    {
        // Déclenchez l'événement de confirmation
        onConfirm.Invoke();

        // Masquez le panneau de confirmation
        HideConfirmationDialog();
    }
}