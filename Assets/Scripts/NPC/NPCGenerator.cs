using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.TextCore.Text;

public class NPCGenerator : MonoBehaviour {


    [SerializeField] int NPCAmount;
    [SerializeField] int LockedRoleAmount;
    [SerializeField] List<NPC> NPCs;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    List<int> usedNames;
    List<int> usedOccupations;
    List<int> usedRelations;
    void Start() {
        usedNames = new List<int>(1){0};
        usedOccupations = new List<int>(5) {0,1,2,3,4};
        usedRelations = new List<int>(1) {0};

        for (int i = 0; i < NPCAmount; i++) {
            //Generate a new identity and npc to be used.
            NPC _npc = this.gameObject.AddComponent(typeof(NPC)) as NPC;
            Identity _identity = this.gameObject.AddComponent(typeof(Identity)) as Identity;

            // The first occupation is none, and the following 4 are locked to be used in every game.
            if (i < LockedRoleAmount) {
                _identity.Occupation = (Identity.Occupations)i+1;
            } else {
                //after the first 4 have been selected, we generate random occupations from the rest of the list
                _identity.Occupation = (Identity.Occupations)SelectOccupation();
            }
            
            _identity.Name = (Identity.Names)SelectName();
            _identity.Trait = (Identity.Traits)SelectTraits();
            
            _npc.SetIdentity(_identity);
            NPCs.Add(_npc);
        }

        //Initialize each characters relation to eachother as none
        for (int i = 0; i < NPCs.Count; i++) {
            Identity _identity = NPCs[i].NPCIdentity;
            for (int j = 0; j < NPCs.Count-1; j++) {
                if (j == i) continue; // Skip self-referencing
                NPC npckey = NPCs[j];

                if (!_identity.Relations.ContainsKey(npckey)) {  // Prevent duplicate additions
                    _identity.Relations.Add(npckey, Identity.RelationTypes.None);
                }
            }
        }
        //Set coworker relations between the first 4 npcs
        for (int i = 0; i < LockedRoleAmount; i++) {
            Identity _identity = NPCs[i].NPCIdentity;
            for (int j = 0; j < LockedRoleAmount-1; j++) {
                if (j == i) continue; // Skip self-referencing
                NPC npckey = NPCs[j];
                if (!_identity.Relations.ContainsKey(npckey)) {  // Prevent duplicate additions
                    _identity.Relations[npckey] = Identity.RelationTypes.CoWorkers;
                }
            }
        }
        NPCs = ShuffleList(NPCs);

        //Here we select relations between all the npcs
        for (int i = 0; i < NPCs.Count; i++) {
            var nextNPCIndex = (i+1)%NPCs.Count;
            var relation = (Identity.RelationTypes)SelectOccupation();
            if (!NPCs[i].NPCIdentity.Relations.ContainsKey(NPCs[nextNPCIndex])){
                NPCs[i].NPCIdentity.Relations.Add(NPCs[nextNPCIndex], relation);
                NPCs[nextNPCIndex].NPCIdentity.Relations.Add(NPCs[i], relation);
            }
        }
    }

    //Selects a random occupation that has not already been used for another character.
    int SelectOccupation(){
        int value = Random.Range(LockedRoleAmount, Identity.Occupations.GetNames(typeof(Identity.Occupations)).Length);
        if (usedOccupations.Contains(value)) {
            value = SelectOccupation();
        } else {
            usedOccupations.Add(value);
        }
        return value;
    }

    //Selects a random name that has not already been used for another character.
    int SelectName(){
        // the variable used for min is 1, since 1 in the enums is always "none"
        int value = Random.Range(1, Identity.Names.GetNames(typeof(Identity.Names)).Length);
        if (usedNames.Contains(value)) {
            value = SelectName();
        } else {
            usedNames.Add(value);
        }
        return value;
    }

    //Selects a random trait.
    int SelectTraits(){
        return Random.Range(1, Identity.Traits.GetNames(typeof(Identity.Traits)).Length);;
    }


    //Tror vi shuffler listen her.
	public List<NPC> ShuffleList(List<NPC> list) {
		var count = list.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = list[i];
			list[i] = list[r];
			list[r] = tmp;
		}
        return list;
	}
    int SelectRelationType(){
        // 2 cuz 0 is none and 1 is coworkers,
        int value = Random.Range(2, Identity.RelationTypes.GetNames(typeof(Identity.RelationTypes)).Length);
        if (usedRelations.Contains(value)) {
            value = SelectRelationType();
        } else {
            usedRelations.Add(value);
        }
        return value;
    }
}