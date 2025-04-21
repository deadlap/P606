using UnityEngine;

namespace Cutscene
{
    [System.Serializable]
    public struct CutsceneNPCInfo
    {
        public Material bodyMaterial;
        public Material headMaterial;
        public GameObject hat;
        public GameObject carriable;
        public Identity.Occupations occupation;
        public string myName;
        public bool isDead;

        public CutsceneNPCInfo(Material bodyMat, Material headMat, GameObject extraClothing, Identity.Occupations theOccupation, string theName, bool isDead = false)
        {
            bodyMaterial = bodyMat;
            headMaterial = headMat;
            if (extraClothing != null) hat = extraClothing;
            else hat = null;
            carriable = null;
            occupation = theOccupation;
            myName = theName;
            this.isDead = isDead;
        }

        public override string ToString() => $"(Body: {bodyMaterial.name}, head: {headMaterial.name}, hat: {(hat != null ? hat.name : "none")}, occupation: {occupation}, name: {myName})";
    }
}
