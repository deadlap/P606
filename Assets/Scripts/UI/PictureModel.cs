using System.Collections.Generic;
using UnityEngine;

public class PictureModel : MonoBehaviour
{
    [SerializeField] private Renderer bodyRenderer;
    [SerializeField] private Renderer headRenderer;

    public void AssignLooks(Material headMat, Material bodyMat, GameObject hat, GameObject carriable)
    {
        Debug.Log($"I'm gonna be assigning my bodyMat as {bodyMat}, and headMat as {headMat}");
        UpdateMaterial(bodyRenderer, bodyMat);
        UpdateMaterial(headRenderer, headMat);
        if (hat != null) Instantiate(hat, headRenderer.transform);
        if (carriable != null) Instantiate(carriable, bodyRenderer.transform);
    }


    private void UpdateMaterial(Renderer renderer, Material material)
    {
        List<Material> oldAndNewMats = new List<Material>(renderer.materials);
        oldAndNewMats[1] = material;
        renderer.SetMaterials(oldAndNewMats);
    }
}
