using System.Diagnostics;
using Unity.Jobs;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Unity.Collections;

// Job adding two floating point values together
public struct MyParallelJob : IJob
{
    public void Execute()
    {
        float value = 0f;
        for (int i = 0; i < 5000000; i++)
        {
            value = Mathf.Sqrt(i);
        }

    }
}

public class NewBehaviourScript : MonoBehaviour
{
    public bool UseParallel = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Stopwatch sw = Stopwatch.StartNew();
        if (UseParallel)
        {
            NativeArray<JobHandle> jobHandleList = new NativeArray<JobHandle>(10, Allocator.TempJob);
            for (int i = 0; i < 10; i++)
            {
                MyParallelJob job = new MyParallelJob();
                JobHandle jobHandle = job.Schedule();
                jobHandleList[i] = jobHandle;
            }
            JobHandle.CompleteAll(jobHandleList);
            jobHandleList.Dispose();
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                Insane();
            }
        }



        Debug.Log(sw.ElapsedMilliseconds);
    }

    public void Insane()
    {
        for (int i = 0; i < 5000000; i++)
        {
            float value = Mathf.Sqrt(i);
        }
    }
}
