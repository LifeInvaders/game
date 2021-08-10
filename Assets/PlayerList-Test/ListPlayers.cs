using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class ListPlayers : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private Transform layout;
    [SerializeField] private GameObject template;

    private List<GameObject> _listings;

    void Start()
    {
        _listings = new List<GameObject>();
        foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            UpdateListing(PhotonNetwork.PlayerList.ToList(), player);
    }

    void UpdateListing(List<Photon.Realtime.Player> scoreBoard, Photon.Realtime.Player player,GameObject listing = null)
    {
        bool isNew = false;
        if (listing == null)
        {
            listing = Instantiate(template, layout);
            _listings.Add(listing);
            isNew = true;
        }
        var templateInterface = listing.GetComponent<TemplateList>();
        templateInterface.name.text = player.NickName;
        templateInterface.deaths.text = ((int) player.CustomProperties["deathCount"]).ToString();
        templateInterface.kills.text = ((int) player.CustomProperties["killCount"]).ToString();
        templateInterface.deaths.text = ((int) player.CustomProperties["deathCount"]).ToString();
        templateInterface.score.text = player.GetScore().ToString();
        if (!isNew) listing.GetComponent<RectTransform>().SetSiblingIndex(scoreBoard.IndexOf(player));
    }
    

    public void UpdateList(List<Photon.Realtime.Player> players)
    {
        if (enabled == false) return;
        List<GameObject> destroyedListing = new List<GameObject>();
        foreach (GameObject listing in _listings)
        {
            var template = listing.GetComponent<TemplateList>();
            bool matched = false;
            foreach (var player in players)
            {
                if (player.NickName == template.name.text)
                {
                    UpdateListing(players, player, listing);
                    matched = true;
                    break;
                }
            }
            if (!matched)
                destroyedListing.Add(listing);
        }
        foreach (var listing in destroyedListing)
        {
            _listings.Remove(listing);
            Destroy(listing);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            scoreBoard.SetActive(true);
        }
        if (Keyboard.current.tabKey.wasReleasedThisFrame)
        {
            scoreBoard.SetActive(false);
        }
    }
}


