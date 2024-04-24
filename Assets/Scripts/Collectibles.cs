using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Collectibles : MonoBehaviour
{
    public List<string> pickups; // List to hold colleted items
    public int collectiblesCount; // Interger to store amount of circles collected
    private int collectiblesCollected; // Interger to store amount of boxes collected
    public TextMeshProUGUI cCollected; // Reference for text object
    public PlayerController playerController;
    public List<GameObject> collectiblesUI;

    private void Start()
    {
        pickups = new List<string>(); // Initialise the list 
        cCollected.text = "Collectibles: " + collectiblesCollected + " / " + collectiblesCount; // Set intial value
    }

    /// <summary>
    /// Function that checks for trigger overlaps 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectable")) // Compares the tag of trigger
        {
            playerController.audioManager.PlayPickupClip();
            string itemName = other.gameObject.GetComponent<PickupsScript>().itemType; // Gets the string of the item collected
            
            print("Collected a: "+ itemName); // Debug to see what was collected 

            pickups.Add(itemName); // Adds collected item to a list 
            print("Items count: "+ pickups.Count); // Debugs the list count 
            Destroy(other.gameObject); // Destroys the collected item in the scene 

            collectiblesCollected = pickups.Count;

            cCollected.text = "Collectibles: " + collectiblesCollected + " / " + collectiblesCount; 
            
            UpdateCollectibles(itemName);
            
        }
    }

   public void UpdateCollectibles(string itemName)
    {
        foreach (var collectible in collectiblesUI)
        {
            if (collectible.CompareTag(itemName))
            {
                Debug.Log("found the right item");
                collectible.SetActive(true);
                collectiblesUI.Remove(collectible);
            }
        }
    }
}
