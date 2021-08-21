using System;
using System.Collections;
using System.Collections.Generic;
using FinTOKMAK.GlobalEventSystem.Runtime;
using NUnit.Framework;
using NUnit.Framework.Internal;
using UnityEngine;
using UnityEngine.TestTools;
using Object = UnityEngine.Object;

public class GlobalEventTest
{
    private GlobalEventManager _manager;

    [SetUp]
    public void SetUp()
    {
        _manager = new GameObject().AddComponent<GlobalEventManager>();
    }

    /// <summary>
    /// Test the initialization of global event system singleton
    /// </summary>
    [Test]
    public void GlobalEventManagerSingletonTest()
    {
        // Check if the global event manager instance has been initialized
        Assert.IsNotNull(GlobalEventManager.Instance);
    }

    /// <summary>
    /// Test the listen method of global event manager
    /// </summary>
    [Test]
    public void GlobalEventManagerListenTest()
    {
        // Register a new global event
        GlobalEventManager.Instance.RegisterEvent("/TEST_EVENT_1", data =>
        {
            
        });
        Assert.Pass("Register event listener success.");
    }

    /// <summary>
    /// Test the invoke method of global event manager
    /// </summary>
    [Test]
    public void GlobalEventManagerInvokeTest()
    {
        // Invoke a new global event
        GlobalEventManager.Instance.InvokeEvent("/TEST_EVENT_1", new GlobalEventData());
        Assert.Pass("Invoke event listener success.");
    }

    /// <summary>
    /// Test the invoke of global event with 1 bool value
    /// </summary>
    [Test]
    public void GlobalEventSingleParameterBoolTest()
    {
        // the test bool value
        bool testBool = false;
        // Register the event to change the bool value
        GlobalEventManager.Instance.RegisterEvent("/TEST_EVENT_1", data =>
        {
            testBool = ((GlobalEventData<bool>) data).data1;
        });
        
        // Check if the testBool is still false before the event
        Assert.IsFalse(testBool);
        // Invoke the event, pass in a bool value: true
        GlobalEventManager.Instance.InvokeEvent("/TEST_EVENT_1", new GlobalEventData<bool>()
        {
            data1 = true
        });
        // Check if the testBool is true after the event
        Assert.IsTrue(testBool);
    }

    /// <summary>
    /// Test the invoke of global event with 1 string value
    /// </summary>
    [Test]
    public void GlobalEventSingleParameterStringTest()
    {
        // the test string value
        string testString = "false";
        // Register the event to change the string value
        GlobalEventManager.Instance.RegisterEvent("/TEST_EVENT_1", data =>
        {
            testString = ((GlobalEventData<string>) data).data1;
        });
        
        // Check if the testString is still false before the event
        Assert.AreEqual(testString, "false");
        // Invoke the event, pass in a string value: true
        GlobalEventManager.Instance.InvokeEvent("/TEST_EVENT_1", new GlobalEventData<string>()
        {
            data1 = "true"
        });
        // Check if the testString is true after the event
        Assert.AreEqual("true", testString);
    }

    /// <summary>
    /// Test the invoke of global event with 1 bool and 1 string value
    /// </summary>
    [Test]
    public void GlobalEventTwoParameterBoolStringTest()
    {
        // the test bool value and the test string value
        bool testBool = false;
        string testString = "false";
        // Register the event to change the bool and string value
        GlobalEventManager.Instance.RegisterEvent("/TEST_EVENT_1", data =>
        {
            testBool = ((GlobalEventData<bool, string>) data).data1;
            testString = ((GlobalEventData<bool, string>) data).data2;
        });
        
        // Check if the testBool and testString is still false before the event
        Assert.IsFalse(testBool);
        Assert.AreEqual("false", testString);
        // Invoke the event, pass in a bool value and a string value: true
        GlobalEventManager.Instance.InvokeEvent("/TEST_EVENT_1", new GlobalEventData<bool, string>()
        {
            data1 = true,
            data2 = "true"
        });
        // Check if the testBool and testString is true after the event
        Assert.IsTrue(testBool);
        Assert.AreEqual("true", testString);
    }

    /// <summary>
    /// Test the case that the global event system failed to parse the event data
    /// </summary>
    [Test]
    public void GlobalEventParseFailedTest()
    {
        // the test bool value and string value
        bool testBool = false;
        string testString = "false";
        // Register the event to change the bool value
        GlobalEventManager.Instance.RegisterEvent("/TEST_EVENT_1", data =>
        {
            // Test if the System can correctly throw the exception when listening to the event
            try
            {
                testBool = ((GlobalEventData<bool, string>) data).data1;
                testString = ((GlobalEventData<bool, string>) data).data2;
                Assert.Fail("Failed to throw the exception.");
            }
            catch (InvalidCastException ICE)
            {
                Assert.Pass("Successfully throw the exception.");
            }
        });
        
        // Check if the testBool and testString is still false before the event
        Assert.IsFalse(testBool);
        Assert.AreEqual("false", testString);
        
        // Invoke the event, pass in a bool value: true
        GlobalEventManager.Instance.InvokeEvent("/TEST_EVENT_1", new GlobalEventData<bool>()
        {
            data1 = true
        });
        
        // Check if the testBool is true after the event
        Assert.IsFalse(testBool);
        Assert.AreEqual("false", testString);
    }

    [TearDown]
    public void TearDown()
    {
        Object.Destroy(_manager.gameObject);
    }
}
