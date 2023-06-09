using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLeaveClick : MonoBehaviour
{

    [SerializeField] GameObject PetInfo;
    public GameObject Canvas;
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        gameObject.transform.parent.transform.parent.GetComponent<Pets>().InfoDisplayed= false;
        Destroy(PetInfo);
    }
}
