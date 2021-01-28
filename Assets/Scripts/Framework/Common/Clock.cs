using UnityEngine;
using UnityEngine.Events;
using System.Collections;
namespace UDK
{

    public class Clock : MonoBehaviour
    {
        private static TaskBehavior m_Task;
        static Clock()
        {
            GameObject go = new GameObject("#Clock#");
            GameObject.DontDestroyOnLoad(go);
            m_Task = go.AddComponent<TaskBehavior>();
        }

        static public Coroutine ScheduleOnce(UnityAction callback, float time)
        {
            return m_Task.StartCoroutine(ScheduleOnceImpl(callback, time));
        }

        static IEnumerator ScheduleOnceImpl(UnityAction callback, float time)
        {
            yield return new WaitForSeconds(time);
            if (callback != null)
            {
                callback();
            }
        }

        static public Coroutine Schedule(UnityAction callback, float interval, float? time = null)
        {
            return m_Task.StartCoroutine(ScheduleImpl(callback, interval, time));
        }

        static IEnumerator ScheduleImpl(UnityAction callback, float interval, float? time)
        {
            yield return new CustomWait(callback, interval, time);
        }

        static public void Cancel(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                m_Task.StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        static public void Cancel()
        {
            m_Task.StopAllCoroutines();
        }


        class TaskBehavior : MonoBehaviour
        {

        }

        class CustomWait : CustomYieldInstruction
        {
            private UnityAction m_IntervalCallback;
            private float m_StartTime;
            private float m_LastTime;
            private float m_Interval;
            private float? m_Time;

            public CustomWait(UnityAction callback, float interval, float? time)
            {
                m_StartTime = Time.time;
                m_LastTime = Time.time;
                m_Interval = interval;
                // 记录总时间
                m_Time = time;
                m_IntervalCallback = callback;
            }

            public override bool keepWaiting
            {
                get
                {
                    if (m_Time != null && Time.time - m_StartTime >= m_Time)
                    {
                        return false;
                    }
                    else if (Time.time - m_LastTime >= m_Interval)
                    {
                        m_LastTime = Time.time;
                        m_IntervalCallback();
                    }
                    return true;
                }
            }
        }
    }
}