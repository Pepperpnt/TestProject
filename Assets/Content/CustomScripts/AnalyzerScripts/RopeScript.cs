using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : MonoBehaviour
{
    [SerializeField] GameObject Analyzer;
    private Transform bone;
    // Start is called before the first frame update
    void Start()
    {
        string rope;
        rope = "Rope/Bone";
        for (int i = 1; i <= 25; i++)
        {
            rope += "/Bone." + i.ToString("000");
        }
        //rope += "/Bone.025_end";
        bone = transform.Find(rope);
    }

    // Update is called once per frame
    void Update()
    {
        if (bone != null)
        {
            bone.position = Analyzer.transform.position;
        }
    }
}
