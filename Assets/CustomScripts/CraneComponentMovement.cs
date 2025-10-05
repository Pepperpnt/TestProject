using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.UIElements;

public class CraneComponentMovement : MonoBehaviour
{
    [SerializeField] float maxVelocity = 10f;
    [SerializeField] float AccelerationDuration = 3.0f;
    [SerializeField] float DecelerationDuration = 3.0f;
    [SerializeField] Vector3 MovementVector;
    [SerializeField] Vector3 MovementLowerLimit;
    [SerializeField] Vector3 MovementUpperLimit;
    private float velocity = 0;

    private void FixedUpdate()
    {
        MoveComponent();
    }

    public void StartComponentMovement(bool reverse)
    {
        StopAllCoroutines();
        StartCoroutine(StartAccelerating(reverse));
    }

    public void StopComponentMovement(bool reverse)
    {
        StopAllCoroutines();
        StartCoroutine(StartDecelerating(reverse));
    }

    IEnumerator StartAccelerating(bool reverse)
    {
        float timeElapsed = Mathf.Abs(velocity / maxVelocity);
        while (Mathf.Abs(velocity) < maxVelocity)
        {
            velocity = SetVelocity(0, maxVelocity, AccelerationDuration, timeElapsed, reverse);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator StartDecelerating(bool reverse)
    {
        float timeElapsed = Mathf.Abs(velocity / maxVelocity);
        while (Mathf.Abs(velocity) > 0)
        {
            velocity = SetVelocity(maxVelocity, 0, DecelerationDuration, timeElapsed, reverse);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void MoveComponent()
    {
        if (IsOverTheLimit() == 1)
        {
            velocity = 0;
            transform.localPosition = MovementUpperLimit;
            StopAllCoroutines();
        }
        else
        if (IsOverTheLimit() == -1)
            {
            velocity = 0;
            transform.localPosition = MovementLowerLimit;
            StopAllCoroutines();
            }
        else
        {
            transform.localPosition += Time.deltaTime * velocity * MovementVector;
        }
    }

    private float SetVelocity(float LerpA, float LerpB, float Acceleration, float TimeElapsed, bool reverse)
    {
        float v = Mathf.Lerp(LerpA, LerpB, TimeElapsed / Acceleration);
        v = reverse ? v * -1 : v;
        return v;
    }

    private int IsOverTheLimit()
    {
        if (GetVectorAxisValue(transform.localPosition + (Time.deltaTime * velocity * MovementVector), MovementVector) > GetVectorAxisValue(MovementLowerLimit, MovementVector)
            & GetVectorAxisValue(transform.localPosition + (Time.deltaTime * velocity * MovementVector), MovementVector) < GetVectorAxisValue(MovementUpperLimit, MovementVector))
            return 0;
        else
        if (GetVectorAxisValue(transform.localPosition + (Time.deltaTime * velocity * MovementVector), MovementVector) <= GetVectorAxisValue(MovementLowerLimit, MovementVector))
            return -1;
        //else
        //if (GetVectorAxisValue(transform.localPosition + MovementVector * velocity * Time.deltaTime, MovementVector) > GetVectorAxisValue(MovementUpperLimit, MovementVector))
        //    return 1;
        else
            return 1;
    }

    private float GetVectorAxisValue(Vector3 vector, Vector3 axis)
    {
        /*Vector3 v = Vector3.Scale(vector, axis);
        Debug.Log(v.magnitude);
        return v.magnitude;*/
        if (axis.x > 0)
            return vector.x;
        else
        if (axis.y > 0)
            return vector.y;
        else
            return vector.z;
    }
}