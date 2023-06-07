using UnityEngine;

public class Pets : MonoBehaviour
{
    [SerializeField] public GameObject GameButton;
    [SerializeField] public GameObject GameParent;
    Object[] PetList;
    public void Awake()
    {
        PetList = Resources.LoadAll("SO/Pets" , typeof(PetSO));

        foreach (var file in PetList)
        {  

            Debug.Log(file);
            //Do work on the files here
            GameObject ActualButton = Instantiate(GameButton, transform.position, transform.rotation, transform.GameParent);
        }
    }
}