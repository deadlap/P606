using UnityEngine;

public class SpawnConsumables : MonoBehaviour
{
    public GameObject[] foodPrefabs;
    public GameObject[] drinkPrefabs;

    Vector3 foodSpawnPosition = new(0, 0, 0);
    Vector3 drinkSpawnPosition = new(0, 0, 0);
    public GameObject SpawnFood(GameObject agent, bool asHat)
    {
        var randomFood = Random.Range(1, foodPrefabs.Length);
        if(asHat)
            return Instantiate(foodPrefabs[0], agent.transform.position + foodSpawnPosition, Quaternion.identity, agent.transform);            
        else
            return Instantiate(foodPrefabs[randomFood], agent.transform.position + foodSpawnPosition, Quaternion.identity);
    }

    public GameObject SpawnDrink(GameObject agent)
    {
        var randomDrink = Random.Range(0, drinkPrefabs.Length);
        return Instantiate(drinkPrefabs[0], agent.transform.position + drinkSpawnPosition, Quaternion.identity, agent.transform);
    }

    public void DestroyConsumable(GameObject consumable)
    {
        Destroy(consumable);
    }
}
