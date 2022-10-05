using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 3f;
    [SerializeField] GameObject _explosionPrefab;
    SpawnManager _spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * _rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.CompareTag("Laser"))
        {
            // Instantiate explosion at the Asteroid's position
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            // Destroy Laser
            Destroy(other.gameObject);
            // Use the StartSpawning() method to spawn enemies and powerups
            _spawnManager.StartSpawning();
            // Destroy Asteroid
            Destroy(this.gameObject, 0.5f);
        }
    }
}
