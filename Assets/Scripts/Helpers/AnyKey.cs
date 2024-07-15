using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnyKey : MonoBehaviour
{
    void Update()
        {
            if (Input.anyKey)
            {
                GetComponent<SceneMenusManagement>().Playground();
            }
        }
}
