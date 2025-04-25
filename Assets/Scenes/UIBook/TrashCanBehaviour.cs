using UnityEngine;

public class TrashCanBehaviour : MonoBehaviour
{
    private SquareDropZone dropZone;

    private void Awake()
    {
        dropZone = GetComponent<SquareDropZone>();
        if (dropZone == null)
        {
            Debug.LogError("TrashCanBehaviour requires a SquareDropZone component!");
        }
    }

    private void Update()
    {
        if (dropZone.currentOccupant != null)
        {
            GameObject toDelete = dropZone.currentOccupant.gameObject;
            dropZone.ClearOccupant();

            // Optional: play sound or animation here

            Destroy(toDelete);
            //Debug.Log("🗑️ Deleted dropped object: " + toDelete.name);
        }
    }
}
