using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Done_EnableShield : MonoBehaviour
{
    public GameObject explosion;
    public GameObject enableShield;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boundary" || other.tag == "Enemy")
        {
            return;
        }

        if (explosion != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
        }

        if (other.tag == "Player")
        {
            Instantiate(enableShield, other.transform.position, other.transform.rotation);
            gameController.shieldEnabled = true;
            gameController.showShiedImage(true);
        }

        Destroy(gameObject);
    }
}
