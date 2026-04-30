using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Vector3 min, max;
    Vector3 destination;
    bool playerDetected = false;

    private void Start()
    {
        RandomDestination();
        StartCoroutine("Patrol");
    }

    void Update()
    {
        if (playerDetected)
        {
            GetComponent<NavMeshAgent>().SetDestination(GameObject.FindWithTag("Player").transform.position);
            GetComponent<Animator>().SetFloat("Velocity", 2);
        }
    }

    IEnumerator Patrol()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, destination) < 1.5f)
            {
                GetComponent<Animator>().SetFloat("Velocity", 0);
                yield return new WaitForSeconds(Random.Range(1f, 3f));
                RandomDestination();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void RandomDestination()
    {
        destination = new Vector3(
            Random.Range(min.x, max.x), 0,
            Random.Range(min.z, max.z)
        );
        GetComponent<NavMeshAgent>().SetDestination(destination);
        GetComponent<Animator>().SetFloat("Velocity", 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerDetected = true;
            StopCoroutine("Patrol");
            transform.LookAt(other.transform);
            print("personaje detectado");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerDetected = false;
            RandomDestination();
            StartCoroutine("Patrol");
            print("personaje fuera de la detección");
        }
    }
}
