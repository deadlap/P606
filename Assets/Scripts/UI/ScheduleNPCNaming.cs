using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
public class ScheduleNPCNaming : MonoBehaviour {
    public static List<NPC> NPCS;
    [SerializeField] TextMeshProUGUI nameDisplay;
    [SerializeField] RawImage image; 
    void Start() {
        NPCS = null;
        if (NPCS == null){
            NPCS = new List<NPC>(NPCGenerator.INSTANCE.NPCs);
        }
        var npc = NPCS[0];
        NPCS.RemoveAt(0);
        image.texture = npc.GetComponent<TakeNPCPicture>().outPutTexture;
        nameDisplay.text = npc.NPCIdentity.name;
    }
}
