
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

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
    public const byte KilledPlayerEventCode = 1;
    /// <summary>
    /// Event raised when an NPC is killed.
    /// </summary>
    public const byte KilledNpcEventCode = 2;
    /// <summary>
    /// Event raised when a round ends.
    /// </summary>
    public const byte EndRoundEventCode = 3;
    //You can also implement template RaiseEvent methods for specific Events here.

    public static void RaisePlayerKilled(GameObject playerKilled,int anim)
    {
        Debug.Log("Raising player killed event.");
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; 
        PhotonNetwork.RaiseEvent(KilledPlayerEventCode,playerKilled.GetPhotonView().ViewID,raiseEventOptions,SendOptions.SendReliable);
    }

    public static void RaiseNpcKilled(GameObject npcKilled,int anim)
    {
        Debug.Log("Raising NPC killed event.");
        RaiseEventOptions raiseEventOptions= new RaiseEventOptions {Receivers = ReceiverGroup.All};
        PhotonNetwork.RaiseEvent(KilledNpcEventCode, npcKilled.GetPhotonView().ViewID, raiseEventOptions, SendOptions.SendReliable);
    }

    public static void RaiseEndRoundEvent()
    {
        Debug.Log("Raising end round event.");
        RaiseEventOptions raiseEventOptions= new RaiseEventOptions {Receivers = ReceiverGroup.All};
        PhotonNetwork.RaiseEvent(EndRoundEventCode,null, raiseEventOptions, SendOptions.SendReliable);
    }
}