using UnityEngine;
using System.Collections;

public class Done_DestroyByBoundary : MonoBehaviour
{
    private Done_GameController gameController;

    void Start()
    {
        GameObject gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<Done_GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    void OnTriggerExit (Collider other) 
	{
        if (other.name == "Done_Enemy Ship(Clone)")
        {
            gameController.RemoveScore(10);
        }        
        Destroy(other.gameObject);
	}
}