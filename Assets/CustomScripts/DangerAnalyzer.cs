using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DangerAnalyzer : MonoBehaviour
{
    private GameObject ScannedObject;
    [SerializeField] private TextMeshProUGUI TextBox;
    [SerializeField] private float seconds = 3.0f;
    [SerializeField] private string Tag = "DangerZone";
    private bool isActive = false;

    // Update is called once per frame
    void Update()
    {
        if (ScannedObject != null)
        {
            TextBox.text = Vector3.Distance(transform.position, ScannedObject.transform.position).ToString("#.00");
        }
        else
        {
            TextBox.text = "";
        }
    }

    public void ToggleAnalyzer()
    {
        StartCoroutine(isActive ? TurnOn() : TurnOff());
        isActive = isActive ? false : true;
    }

    public void Cancel()
    {
        StopAllCoroutines();
    }

    IEnumerator TurnOn()
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("Turned on. Start scan");
        Scan(Tag);
    }

    IEnumerator TurnOff()
    {
        yield return new WaitForSeconds(seconds);
        Debug.Log("Turned off. Stop scan");
        StopScan();
    }

    private void Scan(string tag)
    {
        ScannedObject = FindNearest(ScanObjects(tag));
    }

    private void StopScan()
    {
        ScannedObject = null;
    }

    private GameObject[] ScanObjects(string tag)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        return objects;
    }

    private GameObject FindNearest(GameObject[] objects)
    {
        int nearest = 0;
        float distance = Vector3.Distance(transform.position, objects[0].transform.position);
        for (int i = 1; i < objects.Length; i++)
        {
            float distance2 = Vector3.Distance(transform.position, objects[i].transform.position);
            if (distance2 < distance)
            {
                distance = distance2;
                nearest = i;
            }
        }
        Debug.Log(objects[nearest]);
        return objects[nearest];
    }
}
