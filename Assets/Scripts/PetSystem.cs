using TMPro;
using UnityEngine;

public class Pets : MonoBehaviour
{
    [SerializeField] public GameObject GameButton;
    [SerializeField] public GameObject GameParent;

    public bool InfoDisplayed;

    Object[] PetList;
    public void Awake()
    {
        InfoDisplayed = false;
        PetList = Resources.LoadAll("SO/Pets", typeof(PetSO));

        foreach (PetSO file in PetList)
        {

            Debug.Log(file);
            //Do work on the files here
            GameObject ActualButton = Instantiate(GameButton, transform.position, transform.rotation, GameParent.transform);
            ActualButton.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = file.Name;
            ActualButton.GetComponent<OnClickPet>().Canvas = gameObject;


        }
    }
}