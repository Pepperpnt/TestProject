using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPushEcho : MonoBehaviour
{
    [SerializeField] string PushMessage;
    [SerializeField] string ReleaseMessage;
    public void EchoPushMessage() { Debug.Log(PushMessage); }
    public void EchoReleaseMessage() { Debug.Log(ReleaseMessage); }
}
