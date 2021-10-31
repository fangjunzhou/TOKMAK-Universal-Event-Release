namespace FinTOKMAK.EventSystem.Runtime
{
    /// <summary>
    /// The data pass into the global event
    /// </summary>
    public interface IGlobalEventData
    {
        
    }

    /// <summary>
    /// The GlobalEventData with 0 parameter
    /// </summary>
    public struct GlobalEventData : IGlobalEventData
    {
        
    }

    /// <summary>
    /// The GlobalEventData with 1 parameter
    /// </summary>
    /// <typeparam name="T1">the generic type of first parameter</typeparam>
    public struct GlobalEventData<T1> : IGlobalEventData
    {
        public T1 data1;
    }

    /// <summary>
    /// The GlobalEventData with 2 parameter
    /// </summary>
    /// <typeparam name="T1">the generic type of first parameter</typeparam>
    /// <typeparam name="T2">the generic type of second parameter</typeparam>
    public struct GlobalEventData<T1, T2> : IGlobalEventData
    {
        public T1 data1;
        public T2 data2;
    }
}