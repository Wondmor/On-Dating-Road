using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    // Timer is a Simple SortedList
    // a better timer could also include the args inside the timer itself
    // pretty like the tickless timer in the OS kernel
    // not implemented here
    public delegate void Callback();
    SortedList<float, Callback> events;

    // Start is called before the first frame update
    void Awake()
    {
        events = new SortedList<float, Callback>();
    }

    private float DoAdd(Callback Method, float ExecuteTime){
        try{
            events.Add(ExecuteTime, Method);
        }catch{
            ExecuteTime += 0.001f;
            return DoAdd(Method, ExecuteTime);
        }

        return ExecuteTime;
    }

    // executeTime is used to delete the event if needed
    public float Add(Callback Method, float inSeconds)
    {
        float ExecuteTime = Time.time + inSeconds;

        return DoAdd(Method, ExecuteTime);
    }

    public void RemoveEvent(float time)
    {
        events.Remove(time);
    }

    public void Clear()
    {
        events.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(events.Count == 0)
        {
            return;
        }
        
        if(events.Keys[0] <= Time.time)
        {
            events.Values[0].Invoke();
            events.RemoveAt(0);
        }
    }
}
