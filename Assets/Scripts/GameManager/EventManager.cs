
/// <summary>This static class will be used to stock all Event bytes.
    ///    <para>Event bytes are numbers between 1-99 that are used to condition outcome of OnEvent() callbacks.</para>
    ///    <para>For more info see https://doc.photonengine.com/en-us/pun/v2/gameplay/rpcsandraiseevent#raiseevent</para>
    /// </summary>
public static class EventManager
{
    //Static unique event definitions
       
    /// <summary>
    /// Event raised when a player is killed.
    /// </summary>
    public const byte KilledEventCode = 1;
    
    
    
    //You can also implement template RaiseEvent methods for specific Events here.
    
    
}