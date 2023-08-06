//#define TESTLAB
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using E7.Native;
using System;
using UnityEngine.UI;
using System.Linq;
using System.Diagnostics;

public class NativeAudioDemoPerformance : MonoBehaviour {

#pragma warning disable 0649
    [SerializeField] private Text fpsText;
    [SerializeField] private Text resultText;
#pragma warning restore 0649
    NativeAudioPointer loaded;
	public AudioSource audioSource;
    public Image background;
    public Color color1;
    public Color color2;

    public IEnumerator Start()
    {
		StartCoroutine(FPSUpdate());
#if !TESTLAB
        yield break;
#else
        StartPerformanceTest();
        yield return new WaitUntil(() => running == false);
        background.color = color1;
        StartPerformanceTest();
        yield return new WaitUntil(() => running == false);
        background.color = color2;
#endif
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            audioSource.Play();
        }
    }

    IEnumerator FPSUpdate()
    {
        while (true)
        {
            fpsText.text = (1 / Time.deltaTime).ToString("0.00");
            yield return new WaitForSeconds(0.1f);
        }
    }

	Stopwatch sw;
	PerformanceResult pr;
	private class PerformanceResult
	{
		public long asLoad;
		public List<long> asTicks = new List<long>();

		public float silenceAnalysis;

		public long naInitialize;
		public long naLoad;
		public List<long> naTicks = new List<long>();

        private float TicksToMs(long ticks) { return ticks / 10000f; }
        private float TicksToMs(double ticks) { return (float)(ticks / 10000); }

        private static float StdDev(IEnumerable<long> values)
        {
            float ret = 0;
            int count = values.Count();
            if (count > 1)
            {
                float avg = (float)values.Average();
                float sum = values.Sum(d => (d - avg) * (d - avg));
                ret = Mathf.Sqrt(sum / count);
            }
            return ret;
        }

        public string AnalysisText
        {
            get
            {
                if (naTicks.Count == 0) { naTicks.Add(0); }
                string[] a = new string[]{
                     "AudioSource Load : [Ticks] " + asLoad + " [Ms] " + TicksToMs(asLoad),
                     "AudioSource Play : [Avg. Ticks] " + asTicks.Average() + " [Avg. Ms] " + TicksToMs(asTicks.Average()) + " [Avg. FPS] " + (1000/TicksToMs(asTicks.Average())),
                     "AudioSource Play : [SD Ticks] " + StdDev(asTicks) + " [SD Ms] " + (TicksToMs(StdDev(asTicks))),
                     "",
                     "Silent analysis : [Avg. FPS] " + silenceAnalysis,
                     "",
                     "NativeAudio Initialize : [Ticks] " + naInitialize + " [Ms] " + TicksToMs(naInitialize),
                     "NativeAudio Load : [Ticks] " + naLoad + " [Ms] " + TicksToMs(naLoad),
                     "NativeAudio Play : [Avg. Ticks] " + naTicks.Average() + " [Avg. Ms] " + TicksToMs(naTicks.Average()) + " [Avg. FPS] " + (1000/TicksToMs(naTicks.Average())),
                     "NativeAudio Play : [SD Ticks] " + StdDev(naTicks) + " [SD Ms] " + (TicksToMs( StdDev(naTicks))),
                };
                return String.Join("\n", a);
            }
        }
    }

    private bool running;
    public void StartPerformanceTest()
    {
		Application.targetFrameRate = 60;
		if(!running)
		{
			StartCoroutine(PerformanceTest());
		}
    }

    private const float secondsOfPlay = 1.5f;
    private const int framesOfPlay = (int)(60 * secondsOfPlay);
    public IEnumerator PerformanceTest()
    {
        running = true;
        sw = new Stopwatch();
        pr = new PerformanceResult();
        audioSource.clip.UnloadAudioData();
        UnityEngine.Debug.Log("Performance test : AudioSource..");
        UnityEngine.Debug.Log("Performance test : AudioSource loading..");

        sw.Start();
        audioSource.clip.LoadAudioData();
        sw.Stop();
        pr.asLoad = sw.ElapsedTicks;
        sw.Reset();

        yield return new WaitForSeconds(0.5f);

        UnityEngine.Debug.Log("Performance test : AudioSource playing..");

        for (int i = 0; i < framesOfPlay; i++)
        {
            UnityEngine.Debug.Log("Performance test : AudioSource frame " + i);
            sw.Start();
            audioSource.Play();
            yield return null;
            sw.Stop();
            pr.asTicks.Add(sw.ElapsedTicks);
            sw.Reset();
        }

        yield return new WaitForSeconds(0.5f);

        if (!Application.isEditor)
        {

            UnityEngine.Debug.Log("Performance test : NativeAudio initializing..");

            sw.Start();
            //This force the "crash prevention mechanism" to activates. If it crashes then... it is a bug.
            NativeAudio.Initialize(new NativeAudio.InitializationOptions { androidAudioTrackCount = 3 });
            sw.Stop();
            pr.naInitialize = sw.ElapsedTicks;
            sw.Reset();

            yield return new WaitForSeconds(0.5f);

            UnityEngine.Debug.Log("Performance test : NativeAudio silently analyzing..");

            NativeAudioAnalyzer analyzer =  NativeAudio.SilentAnalyze();
            yield return new WaitUntil( () => analyzer.Analyzed );
            pr.silenceAnalysis = analyzer.AnalysisResult.averageFps;

            UnityEngine.Debug.Log("Performance test : NativeAudio loading..");

            sw.Start();
            loaded = NativeAudio.Load("NativeAudioDemo1.wav");
			yield return null;
            sw.Stop();
            pr.naLoad= sw.ElapsedTicks;
            sw.Reset();

            yield return new WaitForSeconds(0.5f);

            UnityEngine.Debug.Log("Performance test : NativeAudio playing..");

            for (int i = 0; i < framesOfPlay; i++)
            {
                UnityEngine.Debug.Log("Performance test : NativeAudio frame " + i);
                sw.Start();
                NativeAudio.GetNativeSourceAuto().Play(loaded);
                yield return null;
                sw.Stop();
                pr.naTicks.Add(sw.ElapsedTicks);
                sw.Reset();
            }

            yield return new WaitForSeconds(0.5f);
        }

        UnityEngine.Debug.Log("Performance test : Ending..");

        resultText.text = pr.AnalysisText;

        running = false;
    }
}
