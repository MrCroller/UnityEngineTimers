# UnityEngineTimers:

Referenced UnityEngine.CoreModule.dll, UnityEngine.UI.dll, Unity.TextMeshPro.dll are removed from repository! Use your own while rebuilding solution.

* **[Core](https://github.com/MrCroller/UnityEngineTimers/tree/master/Core)**
    - Coroutines Manager
    - Timer class
    - Timers pool manager
* **[UI Extension](https://github.com/MrCroller/UnityEngineTimers/tree/master/UIExtension)**
    - Image Extension
    - Text Mesh Pro Extension

## Getting Started

### TimersPool

Timers are received and managed by the TimersPool class
```csharp
var timersPool = TimersPool.GetInstance();
```

Although we can create a timer separately and work with it, the access methods are already present (through encapsulation) in the class
```csharp
timersPool.StartTimer(UnityAction method, float time);
timersPool.StartTimer(UnityAction<float> timeTickMethod, float time);
timersPool.StartTimer(UnityAction method, UnityAction<float> timeTickMethod, float time);
```

Example of use:
```csharp
float time = 10 //seconds
IStop timer = timersPool.StartTimer(EndMethod, time))

void EndMethod()
{
   Debug.Log("End Time")
}

void Restart()
{
   timer.Stop();
}
```
### UI Extension

In the UI extensions there are different methods for smooth animation of the alpha channel change
```csharp
Image img;

img.SetAlpha(0.5f);

```

For example, it is well variable to use as a parameter the AnimationCurve
```csharp
AnimationCurve easing;
float time = 2.0f;

img.SetAplhaDynamic(time, easing, isChangeActive: true);
```
the isChangeActive parameter is responsible for changing the gameobject activity

You can do the same with the text
```csharp
TMP_Text text;
float timeToVisable = 1.0f;
float timeVisible = 2.0f;
float timeToInvisable = 1.5f;

text.SetAplhaDynamic(timeToVisable, timeVisible, timeToInvisable);
```
It is also worth noting that the text has an extension to change the Y coordinate
```csharp
text.SetTransformYDynamic(time, easing);
```

### Coroutines

The Coroutines class provides the use of coroutines outside of MonoBehaviour. It is auxiliary to the main timer system. Accessed via singleton
```csharp
var coroutines = Coroutines.Instance;
```
It has methods of starting and stopping the coroutine
```csharp
var routine = StartRoutine(IEnumerator enumerator);
StopRoutine(routine);
```