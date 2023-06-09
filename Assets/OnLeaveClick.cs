using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnLeaveClick : MonoBehaviour
{

    [SerializeField] GameObject PetInfo;
    // Start is called before the first frame update
    private void OnMouseDown()
    {
        Destroy(PetInfo);
    }
}
