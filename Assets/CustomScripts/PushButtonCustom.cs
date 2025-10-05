//========= Copyright 2016-2024, HTC Corporation. All rights reserved. ===========

using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.UIElements;

namespace HTC.UnityPlugin.Vive
{
    public class PushButton : MonoBehaviour
    {
        public UnityEvent pushed;
        public UnityEvent released;

#pragma warning disable 0649

        [SerializeField] private Vector3 m_startingPos;
        [SerializeField] private Vector3 m_buttonAxis;
        [SerializeField] private float m_minHeight;
        [SerializeField] private float m_maxHeight;

        [Range(0.01f, 1.0f)]
        [SerializeField] private float m_triggerThresholdHeight = 0.2f;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_triggeredHeight = 0.0f;

        [SerializeField] private float m_recoverSpeed = 0.05f;

#pragma warning restore 0649

        private Rigidbody m_rigidbody;
        private bool m_isRecovering;
        private bool m_isTriggerd;

        private bool HasTriggeredAlternativeHeight
        {
            get { return m_triggeredHeight > 0.0f; }
        }

        protected virtual void Start()
        {
            m_rigidbody = GetComponent<Rigidbody>();
            Assert.IsNotNull(m_rigidbody, "Rigidbody is required.");
        }

        protected virtual void FixedUpdate()
        {
            // Check if triggered
            float percentage = ClampPercentage();
            if (!m_isRecovering && percentage <= m_triggerThresholdHeight)
            {
                InvokePushedEvent();
                m_isRecovering = true;
                m_isTriggerd = !m_isTriggerd;
            }

            if (m_isRecovering && percentage > m_triggerThresholdHeight)
            {
                InvokeReleasedEvent();
                m_isRecovering = false;
            }

            // Recover
            /*Vector3 position = transform.localPosition;
            position.y = transform.localPosition.y + (m_recoverSpeed * Time.deltaTime);
            transform.localPosition = position;*/
            transform.localPosition = Recover();
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        protected virtual void LateUpdate()
        {
            // Lock position in local space except for clamped y
            float maxHeight = m_isTriggerd && HasTriggeredAlternativeHeight ? m_maxHeight * m_triggeredHeight : m_maxHeight;
            float clampedAxis = ClampAxis(maxHeight);
            transform.localPosition = m_buttonAxis.x > 0 ? new Vector3(clampedAxis, m_startingPos.y, m_startingPos.z) :
                m_buttonAxis.y > 0 ? new Vector3(m_startingPos.x, clampedAxis, m_startingPos.z) :
                m_buttonAxis.z > 0 ? new Vector3(m_startingPos.x, m_startingPos.y, clampedAxis) :
                new Vector3(m_startingPos.x, m_startingPos.y, m_startingPos.z);

            // Lock velocity
            if (m_rigidbody)
            {
#if UNITY_6000_0_OR_NEWER
                m_rigidbody.linearVelocity = Vector3.zero;
#else
                m_rigidbody.velocity = Vector3.zero;
#endif
            }
        }

        protected void InvokePushedEvent()
        {
            if (pushed != null)
            {
                pushed.Invoke();
            }
        }

        protected void InvokeReleasedEvent()
        {
            if (released != null)
            {
                released.Invoke();
            }
        }

        private Vector3 Recover()
        {
            Vector3 position = transform.localPosition;
            position.x = m_buttonAxis.x > 0 ? transform.localPosition.x + (m_recoverSpeed * Time.deltaTime) : position.x;
            position.y = m_buttonAxis.y > 0 ? transform.localPosition.y + (m_recoverSpeed * Time.deltaTime) : position.y;
            position.z = m_buttonAxis.z > 0 ? transform.localPosition.z + (m_recoverSpeed * Time.deltaTime) : position.z;
            return position;
        }

        private float ClampPercentage()
        {
            return m_buttonAxis.x > 0 ? Mathf.Clamp01((transform.localPosition.x - m_minHeight) / (m_maxHeight - m_minHeight)) : 
                m_buttonAxis.y > 0 ? Mathf.Clamp01((transform.localPosition.y - m_minHeight) / (m_maxHeight - m_minHeight)) :
                Mathf.Clamp01((transform.localPosition.z - m_minHeight) / (m_maxHeight - m_minHeight));
        }

        private float ClampAxis(float maxHeight)
        {
            return m_buttonAxis.x > 0 ? Mathf.Clamp(transform.localPosition.x, m_minHeight, maxHeight) :
                m_buttonAxis.y > 0 ? Mathf.Clamp(transform.localPosition.y, m_minHeight, maxHeight) :
                Mathf.Clamp(transform.localPosition.z, m_minHeight, maxHeight);
        }
    }
}