using UnityEngine;

public class TrashCanBehaviour : MonoBehaviour
{
    [SerializeField] private SquareDropZone[] dropZones;

    public void ClearDropZones()
    {
        foreach (SquareDropZone dropZone in dropZones)
        {
            if (dropZone.currentOccupant != null)
                Destroy(dropZone.currentOccupant.gameObject);
            dropZone.ClearOccupant();
        }
    }
}
