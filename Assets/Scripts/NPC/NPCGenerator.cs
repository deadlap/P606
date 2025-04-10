using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.EditorTools;

public class NPCGenerator : MonoBehaviour {
    [Tooltip("How many NPCs should be created")]
    [SerializeField] int NPCAmount;
    [Tooltip("Fixed NPC occupations that should always be generated, Chef, Janitor, Waiter, Bartender by standard so 4")]
    [SerializeField] int LockedRoleAmount;
    
    //List of the generated NPCs, used to generate relations between them.
    public List<NPC> NPCs;
    [Tooltip("List of unused names, each time a name is selected it is deleted from this list")]
    [SerializeField] List<int> unusedNames;
    [Tooltip("List of unused occupations, each time an occupation is selected it is deleted from this list")]
    [SerializeField] List<int> unusedOccupations;
    [Tooltip("List of 'unused' relation types, some relationtypes can be used multiple times and are not removed after use")]
    [SerializeField] List<int> unusedRelations;
    [Tooltip("NPC prefab")]
    [SerializeField] GameObject NPCBasePrefab;
    
    [Tooltip("Enum of relationtypes that can be used multiple times.")]
    [SerializeField] Identity.RelationTypes MultiUseRelationTypes;

    void Start() {
        //Generate lists of indexes, from given lengths and starting points. Will Correspond to values in the identity enums.
        unusedNames = PopulateList(1, Identity.Names.GetNames(typeof(Identity.Names)).Length-1);
        unusedOccupations = PopulateList(LockedRoleAmount+1, Identity.Occupations.GetNames(typeof(Identity.Occupations)).Length-1);
        unusedRelations = PopulateListType2(2, Identity.RelationTypes.GetNames(typeof(Identity.RelationTypes)).Length-1);
        NPCs = new List<NPC>();


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
            _identity.PrimaryRole = Identity.PrimaryRoles.Civilian;
            _identity.Openness = Random.Range(0,2)*100;
            _identity.Conscientiousness = Random.Range(0,2)*100;
            _identity.Extraversion = Random.Range(0,2)*100;
            _identity.Agreeableness = Random.Range(0,2)*100;
            _identity.Neuroticism = Random.Range(0,2)*100;
            _npc.SetIdentity(_identity);
            NPCs.Add(_npc);
            npc_gameobject.gameObject.name = System.Enum.GetName(typeof(Identity.Names),_identity.Name);
            npc_gameobject.GetComponentInChildren<TMP_Text>().text = System.Enum.GetName(typeof(Identity.Names),_identity.Name);
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
            Identity.RelationTypes relation = Identity.RelationTypes.None;
            if (NPCs[i].NPCIdentity.Relations.ContainsKey(NPCs[nextNPCIndex]) 
                && NPCs[i].NPCIdentity.Relations[NPCs[nextNPCIndex]] == Identity.RelationTypes.None){
                relation = (Identity.RelationTypes) SelectRelationType();
                NPCs[i].NPCIdentity.Relations[NPCs[nextNPCIndex]] = relation;
            }
            if (NPCs[nextNPCIndex].NPCIdentity.Relations.ContainsKey(NPCs[i]) && 
                NPCs[nextNPCIndex].NPCIdentity.Relations[NPCs[i]] == Identity.RelationTypes.None){
                NPCs[nextNPCIndex].NPCIdentity.Relations[NPCs[i]] = relation;
            }
        }

        //We Select the victim and then the murderer
        NPC victim = NPCs[Random.Range(0, NPCs.Count)];
        var victimRelations = victim.NPCIdentity.Relations;
        List<NPC> relationsNPCs = new List<NPC>(victimRelations.Keys);
        NPC murderer = relationsNPCs[(Random.Range(0, victimRelations.Count))];

        victim.NPCIdentity.PrimaryRole = Identity.PrimaryRoles.Victim;
        murderer.NPCIdentity.PrimaryRole = Identity.PrimaryRoles.Murderer;
        Debug.Log(GameStats.INSTANCE != null);
        GameStats.INSTANCE.Victim = victim;
        GameStats.INSTANCE.Murderer = murderer;

        GameStats.INSTANCE.TimeOfDeath = Random.Range(0,GameStats.INSTANCE.ScheduleLength);

        GenerateNPCSchedules();

        GenerateSchedulePairings();

        for (int i = 0; i < NPCs.Count; i++) {
            transform.GetChild(i).SetParent(NPCs[i].gameObject.transform);
        }
    }

    //Selects a random occupation that has not already been used for another character.
    int SelectOccupation(){
        int index = Random.Range(0, unusedOccupations.Count);
        var temp_occupation = unusedOccupations[index];
        unusedOccupations.RemoveAt(index);
        return temp_occupation;
    }

    //Selects a random name that has not already been used for another character.
    int SelectName(){
        int index = Random.Range(0,unusedNames.Count);
        var temp_name = unusedNames[index];
        unusedNames.RemoveAt(index);
        return temp_name;
    }

    int SelectRelationType(){
        int index = Random.Range(0, unusedRelations.Count);
        Identity.RelationTypes value = (Identity.RelationTypes)unusedRelations[index];
        if(!MultiUseRelationTypes.HasFlag(value)){
            unusedRelations.RemoveAt(index);
        }
        int temp = (int)value;
        return temp;
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
    List<int> PopulateListType2(int start, int size){
        List<int> iList = new List<int>();
        for (int i = start; i < size; i++) {
            iList.Add((int)Mathf.Pow(2,i));
        }
        return iList;
    }
    public NPC GetVictim(){
        foreach (NPC npc in NPCs) {
            if (npc.NPCIdentity.PrimaryRole == Identity.PrimaryRoles.Victim){
                return npc;
            }
        }
        return null;
    }
    public int GetNPCAmount(){
        return NPCs.Count;
    }
    void GenerateNPCSchedules(){
        foreach (NPC npc in NPCs) {
            npc.NPCIdentity.Schedule = new List<Identity.Locations>();
        }
        for (int i = 0; i < GameStats.INSTANCE.ScheduleLength; i++) {
            foreach (NPC npc in NPCs) {
                if ((Identity.PrimaryRoles)npc.NPCIdentity.PrimaryRole == (Identity.PrimaryRoles)Identity.PrimaryRoles.Victim && i >= GameStats.INSTANCE.TimeOfDeath){
                    npc.NPCIdentity.Schedule.Add(Identity.Locations.Cabin);
                    goto endLoop;
                }
                if ((Identity.PrimaryRoles)npc.NPCIdentity.PrimaryRole == (Identity.PrimaryRoles)Identity.PrimaryRoles.Murderer && i == GameStats.INSTANCE.TimeOfDeath){
                    npc.NPCIdentity.Schedule.Add(Identity.Locations.None);
                    goto endLoop;
                }

                if (GameStats.INSTANCE.WorkingHours.Contains(i)){
                    switch((Identity.Occupations)npc.NPCIdentity.Occupation) {
                        case (Identity.Occupations)Identity.Occupations.Chef:
                            npc.NPCIdentity.Schedule.Add(Identity.Locations.Kitchen);
                            goto endLoop;
                        case (Identity.Occupations)Identity.Occupations.Waiter:
                            npc.NPCIdentity.Schedule.Add(PickRandomLocation(1,3));
                            goto endLoop;
                        case (Identity.Occupations)Identity.Occupations.Bartender:
                            npc.NPCIdentity.Schedule.Add(Identity.Locations.Bar);
                            goto endLoop;
                        case (Identity.Occupations)Identity.Occupations.Janitor:
                            npc.NPCIdentity.Schedule.Add(PickRandomLocation(0,0));
                            goto endLoop;
                        default:
                        goto SelectRandomPosition;
                    }
                }
                SelectRandomPosition:
                    npc.NPCIdentity.Schedule.Add(PickRandomLocation(0,0));
                endLoop:
                    continue;
            }
        }
    }

    public Identity.Locations PickRandomLocation(int start, int end) {
        //If both are 0 we just want a completely random position except kitchen and none
        //First 2 arent used as random as they are kitchen or None, none currently being used for the murderer lying about where they are.
        if (end == 0 && start == 0)
            return (Identity.Locations)Random.Range(2, System.Enum.GetValues(typeof(Identity.Locations)).Length); 

        return (Identity.Locations)Random.Range(start,end);
        
    }
    void GenerateSchedulePairings(){
        foreach (NPC npc in NPCs) {
            npc.NPCIdentity.SchedulePairings = new List<List<NPC>>();
            for (int i = 0; i < GameStats.INSTANCE.ScheduleLength; i++) {
                npc.NPCIdentity.SchedulePairings.Add(new List<NPC>());
                foreach (NPC NPCToMeet in NPCs) {
                    if (NPCToMeet.NPCIdentity.name == npc.NPCIdentity.name)
                        continue;
                    if (npc.NPCIdentity.Schedule[i] == NPCToMeet.NPCIdentity.Schedule[i] && npc.NPCIdentity.Schedule[i] != Identity.Locations.Cabin)
                        npc.NPCIdentity.SchedulePairings[i].Add(NPCToMeet);
                }
            }
        }

    //Generates "fake schedule" for murderer
    var murderer = GameStats.INSTANCE.Murderer;
    int time = GameStats.INSTANCE.TimeOfDeath;
    if (murderer.NPCIdentity.Schedule[time] == Identity.Locations.None) {
        murderer.NPCIdentity.Schedule[time] = PickRandomLocation(0,0);
        if (murderer.NPCIdentity.Schedule[time] == Identity.Locations.Cabin){
            return;
        }
        int randomFakeNPCMeets = Random.Range(0,3);
        List<string> usednpcs = new List<string>();
        usednpcs.Add(murderer.NPCIdentity.Name.ToString());
        while (usednpcs.Count-1 <= randomFakeNPCMeets) {
            var randomNPC = NPCs[Random.Range(0,NPCs.Count)];
                if (!usednpcs.Contains(randomNPC.NPCIdentity.Name.ToString())){
                    murderer.NPCIdentity.SchedulePairings[time].Add(randomNPC);
                    usednpcs.Add(randomNPC.NPCIdentity.Name.ToString());
                }
            }
        }
    }
}