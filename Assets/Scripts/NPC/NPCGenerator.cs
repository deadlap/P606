using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System.Collections;
using UnityEngine.Experimental.AI;
using UnityEngine.TextCore.Text;

public class NPCGenerator : MonoBehaviour {


    [SerializeField] int NPCAmount;
    [SerializeField] int LockedRoleAmount;
    [SerializeField] List<NPC> NPCs;

    [SerializeField] List<int> unusedNames;
    [SerializeField] List<int> unusedOccupations;
    [SerializeField] List<int> unusedRelations;
    [SerializeField] GameObject NPCBasePrefab;
    
    [SerializeField] Identity.RelationTypes MultiUseRelationTypes;

    void Start() {

        MultiUseRelationTypes =  Identity.RelationTypes.None | Identity.RelationTypes.Rivalry 
            | Identity.RelationTypes.Acquaintances | Identity.RelationTypes.Friends 
            | Identity.RelationTypes.Business_Partners | Identity.RelationTypes.Family;
            // | Identity.RelationTypes.Neighbor;

        //Generate lists of indexes, from given lengths and starting points. Will Correspond to values in the identity enums.
        unusedNames = PopulateList(1, Identity.Names.GetNames(typeof(Identity.Names)).Length);
        unusedOccupations = PopulateList(LockedRoleAmount+1, Identity.Occupations.GetNames(typeof(Identity.Occupations)).Length);
        unusedRelations = PopulateList(2, Identity.RelationTypes.GetNames(typeof(Identity.RelationTypes)).Length);
        
        for (int i = 0; i < NPCAmount; i++) {
            //Generate a new identity and npc to be used.
            var npc_gameobject = Instantiate(NPCBasePrefab);
            NPC _npc = npc_gameobject.gameObject.AddComponent(typeof(NPC)) as NPC;
            Identity _identity = npc_gameobject.gameObject.AddComponent(typeof(Identity)) as Identity;
            // The first occupation is none, and the following 4 are locked to be used in every game.
            if (i < LockedRoleAmount) {
                _identity.Occupation = (Identity.Occupations)i+1;
            } else {
                //after the first 4 have been selected, we generate random occupations from the rest of the list
                _identity.Occupation = (Identity.Occupations)SelectOccupation();
            }
            
            _identity.Name = (Identity.Names)SelectName();
            _identity.Trait = (Identity.Traits)SelectTraits();
            _identity.PrimaryRole = Identity.PrimaryRoles.Civilian;

            _npc.SetIdentity(_identity);
            NPCs.Add(_npc);
            npc_gameobject.gameObject.AddComponent(typeof(NPCSpawnPointFinder));
            npc_gameobject.gameObject.AddComponent(typeof(NPCInitialPromptGenerator));
            npc_gameobject.gameObject.name = System.Enum.GetName(typeof(Identity.Names),_identity.Name);
        }

        //Initialize each characters relation to eachother as none
        for (int i = 0; i < NPCs.Count; i++) {
            Identity _identity = NPCs[i].NPCIdentity;
            for (int j = 0; j < NPCs.Count; j++) {
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
            for (int j = 0; j < LockedRoleAmount; j++) {
                if (j == i) continue; // Skip self-referencing
                NPC npckey = NPCs[j];
                if (_identity.Relations.ContainsKey(npckey)) {  // Prevent duplicate additions
                    _identity.Relations[npckey] = Identity.RelationTypes.CoWorkers;
                }
            }
        }
        //Shuffle the order of npc
        NPCs = ShuffleList(NPCs);

        //Here we select relations between all the npcs
        for (int i = 0; i < NPCs.Count; i++) {
            var nextNPCIndex = (i+1)%NPCs.Count;
            NPCs[i].Index = i;
            var relation = (Identity.RelationTypes)SelectRelationType();
            if (NPCs[i].NPCIdentity.Relations.ContainsKey(NPCs[nextNPCIndex]) 
                && NPCs[i].NPCIdentity.Relations[NPCs[nextNPCIndex]] == Identity.RelationTypes.None){
                NPCs[i].NPCIdentity.Relations[NPCs[nextNPCIndex]] = relation;
            }
            if (NPCs[nextNPCIndex].NPCIdentity.Relations.ContainsKey(NPCs[i]) && 
                NPCs[nextNPCIndex].NPCIdentity.Relations[NPCs[i]] == Identity.RelationTypes.None){
                NPCs[nextNPCIndex].NPCIdentity.Relations[NPCs[i]] = relation;
            }
        }

        //We Select the victim and then the murderer
        NPC victim = NPCs[Random.Range(0, NPCs.Count-1)];
        var victimRelations = victim.NPCIdentity.Relations;
        List<NPC> relationsNPCs = new List<NPC>(victimRelations.Keys);
        NPC murderer = relationsNPCs[(Random.Range(0, victimRelations.Count))];

        victim.NPCIdentity.PrimaryRole = Identity.PrimaryRoles.Victim;
        murderer.NPCIdentity.PrimaryRole = Identity.PrimaryRoles.Murderer;
    }

    //Selects a random occupation that has not already been used for another character.
    int SelectOccupation(){
        int value = Random.Range(0, unusedOccupations.Count-1);
        unusedOccupations.RemoveAt(value);
        return unusedOccupations[value];
    }

    //Selects a random name that has not already been used for another character.
    int SelectName(){
        int value = Random.Range(0,unusedNames.Count-1);
        unusedNames.RemoveAt(value);
        return unusedNames[value];
    }

    //Selects a random trait.
    int SelectTraits(){
        return Random.Range(1, Identity.Traits.GetNames(typeof(Identity.Traits)).Length);;
    }
    int SelectRelationType(){

        int index = Random.Range(0, unusedRelations.Count);
        Identity.RelationTypes value = (Identity.RelationTypes)index;
        if(!MultiUseRelationTypes.HasFlag(value)){
            unusedRelations.RemoveAt(index);
        }
        return unusedRelations[index];
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
    List<int> PopulateList(int start, int size){
        List<int> iList = new List<int>();
        for (int i = start; i < size; i++) {
            iList.Add(i);
        }
        return iList;
    }
}