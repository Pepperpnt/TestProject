using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHint : MonoBehaviour
{
    [SerializeField] public GameObject Canvas;
    private int count = 0;

    private void Start()
    {
        Canvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        count = other.tag == "Player" ? ++count : count;
        Canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        count = other.tag == "Player" ? --count : count;
        Canvas.SetActive(count > 0 ? true : false);
    }

}
