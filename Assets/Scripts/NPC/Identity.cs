using System.Collections.Generic;
using UnityEngine;
using System;
public class Identity : MonoBehaviour {
    public Names Name;
    public PrimaryRoles PrimaryRole;
    public Occupations Occupation;
    public Dictionary<NPC, RelationTypes> Relations;
    public Motives Motive;

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
        European_Painter,
        Egg_Merchant,
        Teacher,
        Business_Person,
        Mafioso,
        Magician,
        Entertainer,
        Welder
    }
    
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
}
