using System.Collections.Generic;
using UnityEngine;
using System;
public class Identity : MonoBehaviour {
    public Names Name;
    public PrimaryRoles PrimaryRole;
    public Occupations Occupation;
    public Dictionary<NPC, RelationTypes> Relations;
    public Motives Motive;
    public List<Locations> Schedule;
    public List<List<NPC>> SchedulePairings;

    public int Openness;
    public int Conscientiousness;
    public int Extraversion;
    public int Agreeableness;
    public int Neuroticism;

    static public int RelationsAmount;
    void Awake() {
        RelationsAmount = 2;
        Relations = new Dictionary<NPC, RelationTypes>();
    }
    // public void MakeInitialScheduleList(){
        
    // }
    public enum PrimaryRoles {
        None,
        Civilian,
        Murderer,
        Victim,
    }

    public enum Occupations {
        None,
        Chef,
        Janitor,
        Waiter,
        Bartender,
        Painter,
        Doctor,
        Lawyer,
        Magician,
        Oil_Tycoon,
        Admiral,
        Priest,
        Saxophonist
    }
    //     [Flags] public enum Occupations {
    //     None = 0,
    //     Chef = 1,
    //     Janitor = 2,
    //     Waiter = 4,
    //     Bartender = 8,
    //     Painter = 16,
    //     Doctor = 32,
    //     Lawyer = 64,
    //     Magician = 128,
    //     Musician = 256,
    //     Oil_Tycoon = 512,
    //     Pilot = 1024,
    //     Admiral = 2048,
    //     Priest = 4096,
    //     Actress = 8192,
    //     Merchant,
    //     Athlete,
    //     Sherif
    // }
    [Flags] public enum RelationTypes {
        None = 0,
        CoWorkers = 1,
        Married = 2,
        Lovers = 4,
        Rivalry = 8,
        Ex = 16,
        Acquaintances = 32,
        Friends = 64,
        Business_Partners = 128,
        Family = 256,
    }

    public enum Motives {
        None,
        Scammed,
        Cheated_On,
        Poorly_Treated,
        Envy,
        Betrayal,
        Serial_Killer
    }
    public enum Names {
        None,
        John,
        Joe,
        Bob,
        Bo,
        Yvonne,
        Jessica,
        Rachel,
        James, Mary, Robert, Patricia, Linda, Michael, Barbara, William, Elizabeth, David, Susan, Richard, Nancy, Charles, Karen, Joseph, Betty, Thomas, Margaret, Christopher, Sandra, Daniel, Shirley, Matthew, Carol, Anthony, Diane, Mark, Brenda, Donald, Kathleen, Steven, Janet, Paul, Ruth, Andrew, Helen, Kenneth, Donna, Edward, Sharon, Brian, Cheryl, George, Judith, Timothy, Joan, Ronald, Virginia, Kevin, Marilyn, Jeffrey, Dorothy,
    }
    public enum Locations {
        None, //Currently used to lie about whereabouts for the murderer
        Kitchen,
        Ballroom,
        Hallway,
        Bar,
        Pool,
        Cabin,
    }
}