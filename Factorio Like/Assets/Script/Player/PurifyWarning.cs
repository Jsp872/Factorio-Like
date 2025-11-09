using System.Collections;
using TMPro;
using UnityEngine;

public class PurifyWarning : MonoBehaviour
{
    public GridManager gridManager;
    public Transform player;
    public float warningTime = 5f;
    
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private Vector2 spawnPosition;

    private Coroutine warningCoroutine;
    private Cell lastCell;

    private void Start()
    {
        player = gameObject.transform;
        if (warningText != null) warningText.gameObject.SetActive(false);
        StartCoroutine(CheckCellRoutine());
    }

    private IEnumerator CheckCellRoutine()
    {
        while (true)
        {
            if (gridManager == null || player == null)
            {
                yield return null;
                continue;
            }

            Cell currentCell = gridManager.GetCellAtPosition(player.position);

            if (currentCell != lastCell)
            {
                // Le joueur est entré dans une nouvelle cellule
                if (!currentCell.isPurify)
                {
                    if (warningCoroutine == null)
                        warningCoroutine = StartCoroutine(StartWarning());
                }
                else
                {
                    if (warningCoroutine != null)
                    {
                        StopCoroutine(warningCoroutine);
                        warningCoroutine = null;
                        if (warningText != null) warningText.gameObject.SetActive(false);
                    }
                }

                lastCell = currentCell;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator StartWarning()
    {
        float timer = warningTime;
        if (warningText != null) warningText.gameObject.SetActive(true);

        while (timer > 0)
        {
            // Met à jour le texte avec le temps restant
            if (warningText != null)
                warningText.text = $"Zone non purifiée ! Temps restant pour revenir à une zone purifiée : {timer:F1}s";

            timer -= 0.1f;
            yield return new WaitForSeconds(0.1f);

            // Vérifie si le joueur est revenu dans une zone purifiée
            Cell currentCell = gridManager.GetCellAtPosition(player.position);
            if (currentCell != null && currentCell.isPurify)
            {
                warningCoroutine = null;
                if (warningText != null) warningText.gameObject.SetActive(false);
                yield break;
            }
        }

        OnTimeUp();
    }

    private void OnTimeUp()
    {
        if (warningText != null) warningText.gameObject.SetActive(false);
        gameObject.transform.position = spawnPosition;
    }
}
