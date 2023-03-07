using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{   
    // Speed movement of the ship
    [SerializeField] private float _speed = 3.5f;
    // Is the Laser allowed to fire?
    [SerializeField] private float _canFire = -1f;
    [SerializeField] private float _fireRate = 0.3f;
    [SerializeField] private int _lives = 3;
    // Create variable to communicate with the Spawn Manager script
    private SpawnManager _spawnManager;

    // Is TripleShot lasers active?
    private bool _isTripleShotActive;
    // Get TripleShot prefab game object
    [SerializeField] GameObject _tripleShotPrefab;
    // Is Speed Boost active?
    private bool _isSpeedBoostActive;
    // Double the performance output
    [SerializeField] private float _speedMultiplier = 2f;
    // Is Shield active?
    private bool _isShieldActive;
    // Get Shield prefab game object
    [SerializeField] GameObject _shield;
    // Amount of hits the Player received
    private int _hits = 0;
    // Is Health boost active
    private bool _isHealthBoostActive;
    

    // Retrieve the laser prefab
    [SerializeField] GameObject _laserPrefab;
    // Player score
    [SerializeField] private int _score;
    // Handle to UI Manager
    [SerializeField] private UIManager _uiManager;
    [SerializeField] GameObject _leftEngine;
    [SerializeField] GameObject _rightEngine;
    // Player's explosion effect
    [SerializeField] GameObject _playerExplosionPrefab;
    // Handle to Audio Source component
    AudioSource _audioSource;
    // Laser sound effects
    [SerializeField] AudioClip _laserSoundClip;
    private bool _fireLaser = true;
    private bool _isLaserBoostActive = true;

    /*[SerializeField] AudioClip _noAmmoClip;
    [SerializeField] int _laserFire = 15;
    [SerializeField] int _laserMax = 15;
    [SerializeField] int _laserMin = 0;
    */

    // Homing Missile prefab
    /*[SerializeField] Transform _target;
    private Rigidbody2D _rb;
    [SerializeField] float _rotateSpeed;
    private bool _isHomingMissileActive;*/

    // Is Ammo powerup active
    /*[SerializeField] private bool _isAmmoActive;
    // Maximum ammo amount for the Player
    [SerializeField] private int _maxAmmo;
    // Current amount of ammo for Player
    [SerializeField] int _currentAmmo;*/

    // Start is called before the first frame update
    void Start()
    {
        // Current amount of ammo the Player will start the game off with
        //_currentAmmo = _maxAmmo;

        // starting position of the player when the game starts.
        transform.position = new Vector3(0, -3, 0);
        // Get the SpawnManager script component from Spawn Manager object
        _spawnManager = GameObject.FindObjectOfType<SpawnManager>().GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.LogError("Spawn Manager is NULL!");
        }

        // Use handle to access the UI Manager component
        _uiManager = GameObject.FindObjectOfType<UIManager>().GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL!");
        }

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();

        // If the space key is pressed AND the laser can fire,
        // instantiate the laser from the player at an offset of 0.8 meters above the Player.
        // Turn off canFire, and make the Player wait 0.3 seconds to shoot again
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {            
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        // Set the laserFire to be Clamped between the minimum and maximum amount allowed
        //_laserFire = Mathf.Clamp((int)_laserFire, (int)_laserMin, (int)_laserMax);

        if (_canFire > Time.time && _fireLaser) // Check if canFire is greater + fireLaser is True
        {
            if (_isTripleShotActive)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                //_laserFire -= 3;
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
                //_laserFire--;
            }

            // Use UI Manager's LaserUpdate() to display lasers amount
            //_uiManager.LaserUpdate(_laserFire);
            // Set the Audio Source's Clip setting to the Laser Sound clip
            _audioSource.clip = _laserSoundClip;
            // Play Laser Sound clip
            _audioSource.Play();

            // Update UI Manager Laser Update text field

            /*if (_laserFire < 1)
            {
                StopShooting();
                _audioSource.Stop();
                _audioSource.clip = _noAmmoClip;
                //_audioSource.loop = _noAmmoClip;
                _audioSource.Play();
            }
            else
            {
                _audioSource.loop = false;
                _audioSource.clip = _laserSoundClip;
            }*/
        }              
    }

    void StopShooting()
    {
        _fireLaser = false;
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -5.2f, 0), 0);
        
        if (transform.position.x >= 11f)
        {
            transform.position = new Vector3(-11f, transform.position.y, 0);
        }
        else if (transform.position.x <= -11f)
        {
            transform.position = new Vector3(11f, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        // If shield is active
        // Do Nothing, then set shield to not active
        // Return to stop right here
        if (_isShieldActive)
        {
            return;
        }

        _hits += 1;

        if (_hits == 1)
        {
            _rightEngine.SetActive(true);
            //_shield.GetComponent<SpriteRenderer>().material.color = Color.yellow;
        }

        if (_hits == 2)
        {
            _leftEngine.SetActive(true);
            //_shield.GetComponent<SpriteRenderer>().material.color = Color.blue;
        }

        if (_hits == 3)
        {
            _lives -= 1;
            _hits = 0;
            _rightEngine.SetActive(false);
            _leftEngine.SetActive(false);
        }

        /*_lives -= 1;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }*/

        _uiManager.UpdateLives(_lives);

        // If player has zero lives, destroy player
        if (_lives < 1)
        {
            // Stop spawning enemies from SpawnManager OnPlayerDeath() method
            _spawnManager.OnPlayerDeath();
            Instantiate(_playerExplosionPrefab, transform.position, Quaternion.identity);            
            Destroy(this.gameObject);
        }
    }

    // Method to add 20 points to the score after each enemy killed
    // Communicate with the UI Manager to update the score
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        // begin the power down coroutine for the Triple Shot
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        // Wait 5 seconds 
        yield return new WaitForSeconds(5.0f);
        // Set isTripleShotActive to False
        _isTripleShotActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        // If Speed Boost is active, double the speed of the player's movement
        _speed *= _speedMultiplier;
        // begin the power down coroutine for the Triple Shot
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        // Wait 5 seconds 
        yield return new WaitForSeconds(5.0f);
        // Return the player's speed back to normal
        _speed /= _speedMultiplier;
        // Set isTripleShotActive to False
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {        
        _isShieldActive = true;
        _shield.SetActive(true);

        switch (_hits)
        {
            case 0:
                _shield.GetComponent<SpriteRenderer>().material.color = Color.white;
                break;
            case 1:
                _shield.GetComponent<SpriteRenderer>().material.color = Color.yellow;
                break;
            case 2:
                _shield.GetComponent<SpriteRenderer>().material.color = Color.blue;
                break;
            case 3:
                _shield.GetComponent<SpriteRenderer>().material.color = Color.red;
                break;
            default:
                Debug.Log("No shields activated!");
                break;
        }

        // begin the power down coroutine for the Shield
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        // Wait 5 seconds 
        yield return new WaitForSeconds(15f);
        // Set isTripleShotActive to False
        _isShieldActive = false;
        _shield.SetActive(false);
    }

    public void HealthActive()
    {
        //float spawnDelay = Time.time + 60f;
        _isHealthBoostActive = true;
        _lives += 1;
        _hits = 0;
        _leftEngine.SetActive(false);
        _rightEngine.SetActive(false);

        if (_lives > 3)
        {
            _lives = 3;
        }
        _uiManager.UpdateLives(_lives);
    }

    /*public void LaserBoostActive()
    {
        _isLaserBoostActive = true;
        _fireLaser = true;
        _laserFire = _laserMax;
        _uiManager.LaserUpdate(_laserFire);
        _audioSource.Stop();

        StartCoroutine(LaserBoostPowerDownRoutine());
    }

    private IEnumerator LaserBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _isLaserBoostActive = false;
    }*/
}
