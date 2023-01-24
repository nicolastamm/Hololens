using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class debugger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // var temp = UnityEngine.Object.FindObjectsOfType<debugger3>()[0];

        //IterateOverChild(temp.transform, 0, 100);
    }

    private void IterateOverChild(Transform original, int currentLevel, int maxLevel)
    {
        if (currentLevel > maxLevel) return;
        for (var i = 0; i < original.childCount; i++)
        {
            Debug.Log($"{original.GetChild(i)}"); //Do with child what you need
            IterateOverChild(original.GetChild(i), currentLevel + 1, maxLevel);
        }
    }
}
