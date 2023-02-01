using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private Rigidbody targetRb;
    private float minSpeed = 12;
    private float maxSpeed = 16;
    private float maxTorque = 10;
    private float xRange = 6;
    private float ySpawnPos = -2;
    private GameManager gameManager;

    public int pointValue;
    public ParticleSystem explosionParticle;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Get target rigidbody compenent
        targetRb = GetComponent<Rigidbody>();

        // Apply force and torque to send object spinning upwards
        targetRb.AddForce(RandomForce(), ForceMode.Impulse);
        targetRb.AddTorque(RandomTorque(), RandomTorque(), RandomTorque(), ForceMode.Impulse);

        // spawn below play area
        transform.position = RandomSpawnPos();
    }

    // Update is called once per frame
    void Update()
    {

    }

    //private void OnMouseDown()
    //{
    //    if(gameManager.isGameActive)
    //    {
    //        // Destroy target object 
    //        Destroy(gameObject);

    //        // Spawn particle
    //        Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);

    //        // Update score
    //        gameManager.UpdateScore(pointValue);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        // Destroy object when it hits sensor
        Destroy(gameObject);

        // Update lives when a good object hits the sensor
        if (!gameObject.CompareTag("Bad") && gameManager.isGameActive)
        {
            // Update number of player lives
            gameManager.UpdateLives();
        }
    }

    Vector3 RandomForce()
    {
        return Vector3.up * Random.Range(minSpeed, maxSpeed);
    }

    float RandomTorque()
    {
        return Random.Range(-maxTorque, maxTorque);
    }

    Vector3 RandomSpawnPos()
    {
        return new Vector3(Random.Range(-xRange, xRange), ySpawnPos);
    }

    // Replaces OnMouseDown with the ClickAndSwipe script
    public void DestroyTarget()
    {
        if (gameManager.isGameActive)
        {
            gameManager.PlayTargetExplosionSound();
            Destroy(gameObject);
            Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            gameManager.UpdateScore(pointValue);
        }
    }
}
