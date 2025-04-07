using UnityEngine;

namespace Cutscene
{
    [System.Serializable]
    public struct CutsceneNPCInfo
    {
        public Material bodyMaterial;
        public Material headMaterial;
        public GameObject hat;
        public Identity.Occupations occupation;
        public string myName;

        public CutsceneNPCInfo(Material bodyMat, Material headMat, GameObject extraClothing, Identity.Occupations theOccupation, string theName)
        {
            bodyMaterial = bodyMat;
            headMaterial = headMat;
            hat = extraClothing;
            occupation = theOccupation;
            myName = theName;
        }

        public override string ToString() => $"(Body: {bodyMaterial.name}, head: {headMaterial.name}, hat: {hat.name}, occupation: {occupation}, name: {myName})";
    }
}
