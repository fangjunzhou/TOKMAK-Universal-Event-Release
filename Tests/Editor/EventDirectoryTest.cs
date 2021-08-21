using NUnit.Framework;
using Package.Editor;

public class EventDirectoryTest
{
    #region Private Field

    /// <summary>
    /// The root directory
    /// </summary>
    private PathDirectory _root;

    #endregion

    /// <summary>
    /// Test the constructor of a root directory
    /// </summary>
    [Test]
    public void RootTest()
    {
        // create a root directory
        _root = new PathDirectory();
        
        Assert.Pass("Root directory constructed.");
    }

    /// <summary>
    /// Test the add event method in the root directory
    /// </summary>
    [Test]
    public void RootAddEvent()
    {
        // create a root directory
        _root = new PathDirectory();
        
        // add an event to the root directory
        _root.AddEvent("EVENT_ROOT");
        
        // check if the event's path is "/EVENT_ROOT"
        Assert.AreEqual("/EVENT_ROOT", _root.events["EVENT_ROOT"].path);
    }

    /// <summary>
    /// Test the add directory method in the root directory
    /// </summary>
    [Test]
    public void RootAddDirectory()
    {
        // create a root directory
        _root = new PathDirectory();
        
        // add an new directory to the root directory
        _root.AddDirectory("DIRECTORY_ROOT");
        
        // check if the event's path is "/DIRECTORY_ROOT"
        Assert.AreEqual("/DIRECTORY_ROOT", _root.subDirectories["DIRECTORY_ROOT"].path);
    }

    /// <summary>
    /// Test the remove event method in the root directory
    /// </summary>
    [Test]
    public void RootRemoveEvent()
    {
        // create a root directory
        _root = new PathDirectory();
        
        // add an event to the root directory
        _root.AddEvent("EVENT_ROOT");
        
        // remove the EVENT_ROOT event from root directory
        _root.RemoveEvent("EVENT_ROOT");
        
        // check if the EVENT_ROOT event still exist
        Assert.IsFalse(_root.events.ContainsKey("EVENT_ROOT"));
    }

    /// <summary>
    /// Test the remove directory method in the root directory
    /// </summary>
    [Test]
    public void RootRemoveDirectory()
    {
        // create a root directory
        _root = new PathDirectory();
        
        // add an new directory to the root directory
        _root.AddDirectory("DIRECTORY_ROOT");
        
        // remove the added directory
        _root.RemoveDirectory("DIRECTORY_ROOT");

        // check if the event's path is "/DIRECTORY_ROOT"
        Assert.IsFalse(_root.subDirectories.ContainsKey("DIRECTORY_ROOT"));
    }
    
    /// <summary>
    /// Test the add event method in a sub directory
    /// </summary>
    [Test]
    public void SubDirectoryAddEvent()
    {
        // create a root directory
        _root = new PathDirectory();
        
        // add an new directory to the root directory
        _root.AddDirectory("DIRECTORY_ROOT");
        
        // add a new event to the new sub directory
        _root.subDirectories["DIRECTORY_ROOT"].AddEvent("EVENT_SUB");
        
        // check if the event's path is "/DIRECTORY_ROOT"
        Assert.AreEqual("/DIRECTORY_ROOT/EVENT_SUB", _root.subDirectories["DIRECTORY_ROOT"].events["EVENT_SUB"].path);
    }

    /// <summary>
    /// Test the add directory method in a sub directory
    /// </summary>
    [Test]
    public void SubDirectoryAddDirectory()
    {
        // create a root directory
        _root = new PathDirectory();
        
        // add an new directory to the root directory
        _root.AddDirectory("DIRECTORY_ROOT");
        
        // add a new event to the new sub directory
        _root.subDirectories["DIRECTORY_ROOT"].AddDirectory("DIRECTORY_SUB");
        
        // check if the event's path is "/DIRECTORY_ROOT"
        Assert.AreEqual("/DIRECTORY_ROOT/DIRECTORY_SUB",
            _root.subDirectories["DIRECTORY_ROOT"].subDirectories["DIRECTORY_SUB"].path);
    }

    /// <summary>
    /// Test the get event method in the root directory with no events
    /// </summary>
    [Test]
    public void RootDirectoryGetEmptyEventsTest()
    {
        // create a root directory
        _root = new PathDirectory();

        string[] res = _root.GetAllEvents();
        
        Assert.AreEqual(0, res.Length);
    }

    /// <summary>
    /// Test the get event method in the root directory with some events
    /// </summary>
    [Test]
    public void RootDirectoryGetNonEmptyEventsTest()
    {
        // create a root directory
        _root = new PathDirectory();
        
        _root.AddEvent("EVENT_ROOT_1");
        _root.AddEvent("EVENT_ROOT_2");

        string[] res = _root.GetAllEvents();
        
        Assert.AreEqual(2, res.Length);
        Assert.Contains("/EVENT_ROOT_1", res);
        Assert.Contains("/EVENT_ROOT_2", res);
    }

    /// <summary>
    /// Test the get event method in a sub directory with no events
    /// </summary>
    [Test]
    public void SubDirectoryGetEmptyEventsTest()
    {
        // create a root directory
        _root = new PathDirectory();
        
        // add an new directory to the root directory
        _root.AddDirectory("DIRECTORY_ROOT");
        
        string[] res = _root.GetAllEvents();
        
        Assert.AreEqual(0, res.Length);
    }

    /// <summary>
    /// Test the get event method in a sub directory with some events
    /// </summary>
    [Test]
    public void SubDirectoryGetNonEmptyEventsTest()
    {
        // create a root directory
        _root = new PathDirectory();
        
        // add an new directory to the root directory
        _root.AddDirectory("DIRECTORY_ROOT");
        
        // add 2 events to the sub directory
        _root.subDirectories["DIRECTORY_ROOT"].AddEvent("EVENT_SUB_1");
        _root.subDirectories["DIRECTORY_ROOT"].AddEvent("EVENT_SUB_2");
        
        string[] res = _root.GetAllEvents();
        
        Assert.AreEqual(2, res.Length);
        Assert.Contains("/DIRECTORY_ROOT/EVENT_SUB_1", res);
        Assert.Contains("/DIRECTORY_ROOT/EVENT_SUB_2", res);
    }
}
