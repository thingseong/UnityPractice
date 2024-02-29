using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    public GameObject prefab;

    GameObject tank;
    // Start is called before the first frame update
    void Start()
    {
        tank = Managers.Resource.Instantiate("Tank");
        Destroy(tank, 3.0f);
    }

}
