using System.Collections.Generic;
using UnityEngine;

public class NPCSpawnPointFinder : MonoBehaviour {

    public static NPCSpawnPointFinder Instance;
    [SerializeField] Identity.Occupations Passengers;
    [SerializeField] Identity.Occupations Workers;
    [SerializeField] List<NPCSpawnPoint> UnusedPassengerSpawnPoints;
    [SerializeField] List<NPCSpawnPoint> UnusedWorkerSpawnPoints;
    void Start() {
        Passengers = Identity.Occupations.European_Painter 
        | Identity.Occupations.Egg_Merchant | Identity.Occupations.Teacher | Identity.Occupations.Business_Person 
        | Identity.Occupations.Mafioso | Identity.Occupations.Magician | Identity.Occupations.Entertainer | Identity.Occupations.Welder;

        Workers = Identity.Occupations.Chef | Identity.Occupations.Janitor 
        | Identity.Occupations.Waiter | Identity.Occupations.Bartender;
    }
}
