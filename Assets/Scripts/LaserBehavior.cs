using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBehavior : MonoBehaviour
{
    // Speed of the laser
    [SerializeField] private float _speed = 8.0f;
    // True or False, is this the Enemy's laser
    bool _isEnemyLaser = false;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isEnemyLaser == false)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    public void MoveUp()
    {
        // Moving the lasers upward with a speed of 8 and in real-time
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // If laser moves greater than 8 on Y-axis
        // destroy game object
        if (transform.position.y >= 8f)
        {
            // Check if the object has a parent
            // If so, destroy the parent with the game object
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    private void MoveDown()
    {
        // Moving the lasers downward with a speed of 8 and in real-time
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // If laser moves less than -8 on Y-axis
        // destroy game object
        if (transform.position.y <= -8f)
        {
            // Check if the object has a parent
            // If so, destroy the parent with the game object
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && _isEnemyLaser == true)
        {
            PlayerBehavior player = other.GetComponent<PlayerBehavior>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
