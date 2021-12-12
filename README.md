# TOKMAK Universal Event

![GitHub Page](https://user-images.githubusercontent.com/79500078/137570424-1749e4d4-dc0f-444f-b02f-52cbcd56baf6.png)

This is a universal event system for Unity that is easy to use, easy to configure, and really flexible. You can event build up your own MVC framework using this package.

# Prerequisite

```
"com.dbrizov.naughtyattributes": "https://github.com/dbrizov/NaughtyAttributes.git#upm",
"com.hextantstudios.utilities": "https://github.com/hextantstudios/com.hextantstudios.utilities.git",
```

# Install

Use the Package Manager to install from url

`https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event.git`

or add in the manifest.json

`"com.fintokmak.universaleventsystem": "https://github.com/Fangjun-Zhou/TOKMAK-Universal-Event.git"`

# Documentation

[Here](https://fangjun-zhou.github.io/TOKMAK-Universal-Event-Release/) is the documentation of TOKMAK Universal Event System.

# Usage of Global Event (a build-in implementation of universal event)

## 1. TOKMAK Global Event Config

Before you start using the global event system, you need to have a config file prepare for later use.

Pick any place in your project, right click and choose: Create/FinTOKMAK/Universal Event/Create Config

![image](https://user-images.githubusercontent.com/79500078/137571850-65e4ff12-0a40-4818-93fc-a4593054ca2d.png)

## 2. TOKMAK Global Event Project Settings

Open the Project Settings, find `FinTOKMAK Global Event`. Pick the config you just create in `Global Event Config` field.

![image](https://user-images.githubusercontent.com/79500078/137571966-fbae7419-d926-49b6-8030-fb70d9c04689.png)

## 3. Create some events!

In the config file, you can see a file-structure-like events and folders.

Clicking on the "+" you can add an event or an event directory. Click on "Current: Add Event(Directory)" to switch betweening adding events and adding directory.

![image](https://user-images.githubusercontent.com/79500078/137572035-01c8ee9b-f8e2-4ff2-80df-7a2ec40147cd.png)

Using the fold/expand button, you can fold and expand the directory.

This event config system is pretty easy to use, you should be very familiar with it in just a few minutes!

## 4. IEventData

All the event should pass in an `IEventData`, even if there's no value to pass through event.

In the TOKMAK Universal Event System, there are already 3 build in Event data wrapper class (maybe more in the future):

```C#
/// <summary>
/// The GlobalEventData with 0 parameter
/// </summary>
public struct EventData : IEventData
{

}

/// <summary>
/// The GlobalEventData with 1 parameter
/// </summary>
/// <typeparam name="T1">the generic type of first parameter</typeparam>
public struct EventData<T1> : IEventData
{
    public T1 data1;
}

/// <summary>
/// The GlobalEventData with 2 parameter
/// </summary>
/// <typeparam name="T1">the generic type of first parameter</typeparam>
/// <typeparam name="T2">the generic type of second parameter</typeparam>
public struct EventData<T1, T2> : IEventData
{
    public T1 data1;
    public T2 data2;
}
```

These three class provides the event data with no parameter, 1 parameter, and 2 parameter, you can also easilly extend the event data to enable it passing more parameters.

To extand an event data, you just need to create a struct inheriting IGlobalEventData:

```C#
/// <summary>
/// The sample global event data with a string message and a int message.
/// </summary>
public struct DemoGlobalEventData : IEventData
{
    public string strMsg;
    public int intMsg;
}

/// <summary>
/// The sample global event data with two generic type.
/// </summary>
/// <typeparam name="T1">The generic type 1.</typeparam>
/// <typeparam name="T2">The generic type 2.</typeparam>
public struct DemoGlobalGenericData<T1, T2> : IEventData
{
    public T1 data1;
    public T2 data2;
}
```

Here, I created 2 event data. `DemoGlobalEventData` has string data and an int data inside, DemoGlobalGenericData can have two flexible generic type data inside.

## 5. GlobalEvent attribute

Global Event System access the event internally by string, so you also need to pass a string to system so that the system can find the event to register/invoke.

With `GlobalEvent` attribute, you can change the editor UI of a string to a dropdown. And you can easily pick the event you configed in the config file.

```C#
/// <summary>
/// The name of event with string and int data.
/// </summary>
[GlobalEvent]
public string eventDataName;

/// <summary>
/// The name of event with generic data.
/// </summary>
[GlobalEvent]
public string eventGenericName;
```

![image](https://user-images.githubusercontent.com/79500078/137572996-b61473ae-97c1-42da-b965-3378e4b4cf2f.png)

## 6. Invoke the event

To invoke the event, simply call the `InvokeEvent` method in the singleton of TOKMAK Global Event System.

```C#
/// <summary>
/// The sample method to call a Global Event with string and int data.
/// </summary>
public void CallDataEvent()
{
    GlobalEventManager.Instance.InvokeEvent(eventDataName, new DemoGlobalEventData()
    {
        strMsg = "Nana7mi",
        intMsg = 010
    });
}

/// <summary>
/// The sample method to call a Global Event with generic type data.
/// </summary>
public void CallGenericEvent()
{
    GlobalEventManager.Instance.InvokeEvent(eventGenericName, new DemoGlobalGenericData<float, string>()
    {
        data1 = 0.10f,
        data2 = "ybb"
    });
}
```

## 7. Listen to the event

As the listener, a monobehaviour need to register and unregister the event in its own life cycle.

Normally, we will register the event in Start, and unregister it in OnDestroy

``` C#
private void Start()
{
    // Register the event when Start
    // GlobalEventManager.Instance.RegisterEvent(eventDataName, ResponseEventData);
    // GlobalEventManager.Instance.RegisterEvent(eventGenericName, ResponseGenericData);
}

private void OnDestroy()
{
    // Unregister the event when Destroy
    GlobalEventManager.Instance.UnRegisterEvent(eventDataName, ResponseEventData);
    GlobalEventManager.Instance.UnRegisterEvent(eventGenericName, ResponseGenericData);
}
```

But here, I want to demonstrate a special case that you need to register the event in Awake.

Since TOKMAK Global Event initialize itself in Awake, it's not always the case that the Global Event System will initialize the first. So it's possible that you cannot get the singleton in Awake.

To solve the problem, we use a `finishInitializeEvent` Action inside the Global Event Manager to do the initialize.

```C#
private void Awake()
{
    // If the GlobalEventManager is already initialized
    // Register the event directly
    if (GlobalEventManager.initialized)
    {
        GlobalEventManager.Instance.RegisterEvent(eventDataName, ResponseEventData);
        GlobalEventManager.Instance.RegisterEvent(eventGenericName, ResponseGenericData);
    }
    // If not initialized, register the operation when GlobalEventManager is initialized
    else
    {
        GlobalEventManager.finishInitializeEvent += () =>
        {
            GlobalEventManager.Instance.RegisterEvent(eventDataName, ResponseEventData);
            GlobalEventManager.Instance.RegisterEvent(eventGenericName, ResponseGenericData);
        };
    }
}
```

The `ResponseEventData` method and `ResponseGenericData` are responsible for the main logic in response to the global event.

Inside these two methods, you need to cast the raw IGlobalEventData into specific type first. Then, you are able to read the data in side the global event data.

``` C#
/// <summary>
/// The listener of the string and int data event.
/// </summary>
/// <param name="data">The unconverted IGlobalEventData.</param>
private void ResponseEventData(IEventData data)
{
    // Convert data to the DemoGlobalEventData
    DemoGlobalEventData eventData = (DemoGlobalEventData) data;
    Debug.Log($"String message: {eventData.strMsg}; Int message: {eventData.intMsg}.");
}

/// <summary>
/// The listener of the generic data event.
/// </summary>
/// <param name="data">The unconverted IGlobalEventData.</param>
private void ResponseGenericData(IEventData data)
{
    // Convert the data to DemoGlobalGenericData<float, string>
    DemoGlobalGenericData<float, string> eventData = (DemoGlobalGenericData<float, string>) data;
    Debug.Log($"Float message: {eventData.data1}; String message: {eventData.data2}.");
}
```

## 8. Add the Global Event Manager

Finally, we can use the global event system to check our work. To make all this works, a Global Event Manager is needed.

![image](https://user-images.githubusercontent.com/79500078/137573059-ff87dbeb-026e-4620-bbb6-238c1e463fc8.png)

Added an empty GameObject into the scene and add the Global Event Manager to it. By default, it will be moved into Dont Destroy On Load.
