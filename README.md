# UnityEngineTimers

Referenced UnityEngine.CoreModule.dll, UnityEngine.UI.dll, Unity.TextMeshPro.dll are removed from repository! Use your own while rebuilding solution.

* **[Core](https://github.com/MrCroller/UnityEngineTimers/tree/master/TimersCore)**
  * Coroutines Manager
  * Timer class
  * Timers pool manager
* **[Extension](https://github.com/MrCroller/UnityEngineTimers/tree/master/Extension)**
  * Extension alpha dinamic
  * AdapterFabric
      * ImageAdapter
      * SpriteRendererAdapter
      * TextAdapter
* **[Collections](https://github.com/MrCroller/UnityEngineTimers/tree/master/Collections)**
  * DeadNoteList

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
IStop timer = timersPool.StartTimer(EndMethod, time)

void EndMethod()
{
   Debug.Log("End Time")
}

void Restart()
{
   timer.Stop();
}
```

However, you may not need a pool of timers and want to use a single timer when needed...
In this case, you can manually initialize an object of the timer class and work exclusively with it

```csharp
float time = 10 //seconds
Timer timer = new();

timer.Start(EndMethod, time);

void EndMethod()
{
   Debug.Log("End Time")
}
```
When this timer is started again, the current ticker will reset and all dependencies will be unsubscribed

```csharp
float time = 10 //seconds
Timer timer = new();

void Start()
{
   timer.Start(() => Debug.Log("End Start"), time);
}

void Handler()
{
    timer.Start(() => Debug.Log("End HandlerEvent"), time);
}

```

### Extension

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

### Collection

#### DeadNoteList

This list works by adding an element to it and setting the time after which it will disappear.

```csharp
DeadNoteList<Player> deadList = new(length);
Player player = new();

void StartMethod()
{
  player.Sleep()
}

void EndMethod() => player.Awake()

deadList.Add(player, StartMethod, EndMethod, 3f)

foreach(Player player in players)
{
    deadList.Add(player, () => player.Dead(), 15f)
}
```

If you add an object that is already present in the sheet, it will automatically reset the timer and start it again

You can also:

* Stop the timer and remove the object
* Self-stop
* Clear all timers inside

```csharp
deadList.Remove(player); //stop and dell

deadList.StopAll(); // stop all, don't clear

deadList.Dispose(); // stop all and clear
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

## If you want to expand the supported components for extensions

They must have a Color property and inherit from Component, IColor

Otherwise, if you do not have direct access to objects, but they have a Color property and are inherited from the Component, then you need to add the corresponding adapters to the "Adapter" folder and the wrapper constructor to the "AdapterFabric":

```csharp
public class ExampleClassAdapter : Component, IColor
    {
        public Color Color { get => _component.color; set => _component.color = value; }

        private SomeClass _component;

        public ExampleClassAdapter(SomeClass com) => _component = com;
    }
```

```csharp
public static class AdapterFabric
    {
        public static ExampleClassAdapter Ext(this SomeClass spriteRenderer) => new ExampleClassAdapter(spriteRenderer);
    }
```

Now you can use the extension method:

```csharp
ExampleClassAdapter.Ext().SetAlpha(0.5f);
```

>If you want to use extension methods without the "Ext()" transformation >packaging method, then you need to package each extension method you need >from the "Extension" class.
>
>```csharp
>public static Color SetAlpha(this SomeClass spriteRenderer, float value) => >spriteRenderer.Ext().SetAlpha(value);
>```
>
> See the "AdapterFabric" class for examples.
