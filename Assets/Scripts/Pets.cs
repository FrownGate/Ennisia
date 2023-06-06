using UnityEngine;

public class Pets : MonoBehaviour
{
    public GameObject OriginPets;

    public void AddPet()
    {
        GameObject clonePets = Instantiate(OriginPets, transform.position, transform.rotation);
    }
}