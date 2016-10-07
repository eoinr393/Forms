using UnityEngine;
using System.Threading;
using System.Text;
using System.Collections;

public class CreatureManager : MonoBehaviour {

    public int threadCount = 0;

    GUIStyle style = new GUIStyle();

    [HideInInspector]
    public static float threadTimeDelta;

    Thread thread;
    Boid[] boids;

    StringBuilder message = new StringBuilder();

    static CreatureManager instance;

    [Header("Debugging")]
    public bool showMessages;

    public static void PrintMessage(string message)
    {
        if (instance != null)
        {
            Instance.message.Append(message + "\n");
        }
    }

    public static void PrintFloat(string message, float f)
    {
        if (instance != null)
        {
            Instance.message.Append(message + ": " + f + "\n");
        }
    }

    public static void PrintVector(string message, Vector3 v)
    {
        if (instance != null)
        {
            Instance.message.Append(message + ": (" + v.x + ", " + v.y + ", " + v.z + ")\n");
        }
    }

    CreatureManager()
    {
        instance = this;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "" + message, style);
        if (Event.current.type == EventType.Repaint)
        {
            message.Length = 0;
        }

        if (Event.current.type == EventType.KeyDown)
        {            
            if (Event.current.keyCode == KeyCode.F4)
            {
                showMessages = !showMessages;
            }

            if (Event.current.keyCode == KeyCode.Escape)
            {
                Application.Quit();
            }
        }
    }

    float minFPS = float.MaxValue;
    float maxFPS = float.MinValue;
    float avgFPS = 0;
    float sumFPS = 0;
    int frameCount = 0;

    public static CreatureManager Instance
    {
        get
        {
            return instance;
        }
    }

    void Awake()
    {

        instance = this;
        style.fontSize = 18;
        style.normal.textColor = Color.white;
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start ()
    {
        
        boids = FindObjectsOfType<Boid>(); // Find all the boids
        foreach (Boid boid in boids)
        {
            boid.multiThreaded = true;
        }

        StartUpdateThreads();
        StartCoroutine("UpdateThreadTimeDelta");
        
        

    }

    void Update()
    {
        frameCount++;
        float fps = (1.0f / Time.deltaTime);
        if (fps < minFPS)
        {
            minFPS = fps;
        }
        if (fps > maxFPS)
        {
            maxFPS = fps;
        }
        sumFPS += fps;
        avgFPS = sumFPS / frameCount;
        PrintFloat("FPS: ", (int)fps);
        PrintFloat("Avg FPS: ", (int)avgFPS);
        PrintFloat("Min FPS: ", (int)minFPS);
        PrintFloat("Max FPS: ", (int)maxFPS);

        if (!thread.IsAlive)
        {
            StartUpdateThreads();
        }
        PrintFloat("Boid FPS: ", threadFPS);
        PrintFloat("ThreadCount: ", (int)threadCount);
        PrintFloat("Thread TimeDelta", threadTimeDelta);
    }

    long lastThreadCount = 0;
    float threadFPS;

    void UpdateThread()
    {
        while (running)
        {
            for (int i = 0; i < boids.Length; i++)
            {
                Boid boid = boids[i];
                boid.force = boid.CalculateForce();
            }
            threadCount++;
            //Thread.Sleep(30);
        }
    }

    System.Collections.IEnumerator UpdateThreadTimeDelta()
    {
        while (true)
        {
            long newThreadCount = threadCount;
            threadFPS = newThreadCount - lastThreadCount;
            if (threadFPS > float.Epsilon)
            {
                threadTimeDelta = 1.0f / threadFPS;
            }
            lastThreadCount = newThreadCount;
            yield return new WaitForSeconds(1.0f);
        }
    }

    bool running = false;

    void OnApplicationQuit()
    {
        running = false;
    }

    void StartUpdateThreads()
    {
        running = true;
        Debug.Log("Starting thread...");

        for (int i = 0; i < boids.Length; i++)
        {
            Boid boid = boids[i];
            boid.multiThreaded = true;
            boid.UpdateLocalFromTransform();
        }
        thread = new Thread(UpdateThread);
        thread.Start();
    }
}
