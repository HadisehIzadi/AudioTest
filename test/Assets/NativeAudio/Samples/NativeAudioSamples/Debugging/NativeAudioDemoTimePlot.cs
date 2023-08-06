using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E7.Native;
using System;
using System.Linq;

public class NativeAudioDemoTimePlot : MonoBehaviour
{

    public float timeLimit = 2f;
    [Space]
    public AudioSource unitySource;
    [Space]
    public LineRenderer actualTime;
    public LineRenderer unityTime;
    public LineRenderer unityDspTime;
    public LineRenderer nativeAudioTime;
    [Space]
    public LineRenderer guideGrid;

    private NativeAudioPointer pointer;
    public void Start()
    {
        // var pl = PlayerLoop.GetDefaultPlayerLoop();
        // foreach(var subsystem in pl.subSystemList)
        // {
        // 	if(subsystem.GetType() == typeof(Initialization))
        // 	{
        //         var npl = new PlayerLoopSystem[1];
        // 		npl.updateDelegate = TimeCaptureUpdate;
        // 		subsystem.subSystemList = subsystem.subSystemList.Concat(npl);
        // 	}
        // }
#if !UNITY_EDITOR
        NativeAudio.Initialize();
		pointer = NativeAudio.Load("NativeAudioDemo1.wav");
#endif
    }

    private void TimeCaptureUpdate()
    {
        Debug.Log("Time capture on initialization! " + Time.realtimeSinceStartup);
    }

    public void StartPlotting()
    {
        StartCoroutine(PlottingRoutine());
    }

    List<float> actualTimeList = new List<float>();
    IEnumerator PlottingRoutine()
    {
        float startTime = 0;
        double startDspTime = 0;
        actualTimeList.Clear();
        unitySource.clip.LoadAudioData();

        actualTime.positionCount = 0;
        unityTime.positionCount = 0;
        unityDspTime.positionCount = 0;
        nativeAudioTime.positionCount = 0;
        guideGrid.positionCount = 0;

        //Try to start the 4 timer together, however they can never be EXACTLY at the same time.
        startTime = Time.realtimeSinceStartup;
        startDspTime = AudioSettings.dspTime;
        unitySource.Play();
#if !UNITY_EDITOR
		NativeSource controller = NativeAudio.GetNativeSourceAuto();
        controller.Play(pointer);
#endif
        //Debug.Log("Started the test at real time : " + startTime + " dsp time : " + startDspTime);

        float timeProgression;
        int vertexCount = 0;
        do
        {
            //Research result :

            //All the API for each of 4 timers is very sensitive. Each one can change its value even in the same frame.

            //For example if you ask the time here VS at the end of do while, there is a chance that the value will change.
            //Also there is a chance that the value will NOT change. Indicating that the update is in a certain step of audio hardware.
            //The value changing means that "step" occur when we are drawing the graph.

            //However, if AudioSettings.dspTime changes unitySource.time also change with it. If the former stay still the latter also stay still.
            //Because those two timer are in the same update system, the audio DSP update.

            //GetPlaybackTime from native side also may or may not change between the beginning of do while and the end
            //But it is in a different cycle than Unity's DSP. (dspTime and audioSource.time change or not change together, but if those two stay still
            //GetPlaybackTime might still change or not change.

            //Finally Time.realtimeSinceStartup ALWAYS change in between lines of code. There is no instance that this value stay the same in any calls.

            //With that in mind, to make the graph plotting as fair as possible I will use the API to pre cache the data all together here.
            var realTime = Time.realtimeSinceStartup;
            var dspTime = AudioSettings.dspTime;
            var unitySourceTime = unitySource.time;
#if !UNITY_EDITOR
			var nativeSourceTime = controller.GetPlaybackTime();
#endif

            //You can uncomment this and its pair at the end of do while to see what I have said
            // #if !UNITY_EDITOR
            // 			Debug.Log("Test time BEFORE " + Time.realtimeSinceStartup + " " + AudioSettings.dspTime  + " " + unitySource.time + " " + controller.GetPlaybackTime());
            // #endif

            vertexCount++;
            timeProgression = (realTime - startTime) / timeLimit;
            PlotNextVertex(actualTime, vertexCount, timeProgression, timeProgression);
            PlotNextVertex(unityDspTime, vertexCount, timeProgression, (float)((dspTime - startDspTime) / timeLimit));
            PlotNextVertex(unityTime, vertexCount, timeProgression, unitySourceTime / timeLimit);

#if !UNITY_EDITOR
            PlotNextVertex(nativeAudioTime, vertexCount, timeProgression, nativeSourceTime / timeLimit);
#endif

            //Draw frame guide grid
            PlotNextVertex(guideGrid, ((3 * (vertexCount - 1)) + 1), timeProgression, 0);
            PlotNextVertex(guideGrid, ((3 * (vertexCount - 1)) + 2), timeProgression, 1);
            PlotNextVertex(guideGrid, ((3 * (vertexCount - 1)) + 3), timeProgression, 0);

            // #if !UNITY_EDITOR
            // 			Debug.Log("Test time AFTER " + Time.realtimeSinceStartup + " " + AudioSettings.dspTime  + " " + unitySource.time + " " + controller.GetPlaybackTime());
            // #endif

            yield return null;
        }
        while (timeProgression < 1);

        //Debug.Log("Ended the test at " + Time.realtimeSinceStartup);
    }

    private Vector3 PlotNextVertex(LineRenderer lr, int vertexCount, float x, float y)
    {
        lr.positionCount = vertexCount;
        var v3 = Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0));
        v3.z = 0;
        lr.SetPosition(vertexCount - 1, v3);
        return v3;
    }
}
