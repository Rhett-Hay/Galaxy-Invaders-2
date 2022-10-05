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
    // Is Ammo powerup active
    /*[SerializeField] private bool _isAmmoActive;
    // Maximum ammo amount for the Player
    [SerializeField] private int _maxAmmo;
    // Current amount of ammo for Player
    [SerializeField] int _currentAmmo;*/

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
            _canFire = Time.time + _fireRate;
            FireLaser();
        }
    }

    private void FireLaser()
    {
        // IF space key is pressed,
        // IF tripleShotActive is True
        // fire TripleShot lasers prefab
        // Else, fire one laser shot        
        if (_isTripleShotActive)
        {
            Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            //_currentAmmo--;
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            //_currentAmmo--;
        }

        // Set the Audio Source's Clip setting to the Laser Sound clip
        _audioSource.clip = _laserSoundClip;
        // Play Laser Sound clip
        _audioSource.Play();
    }

    void PlayerMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        transform.Translate(direction * _speed * Time.deltaTime);

        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -4.3f, 0), 0);
        
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

        _lives -= 1;

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

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
        // begin the power down coroutine for the Shield
        StartCoroutine(ShieldPowerDownRoutine());
    }

    IEnumerator ShieldPowerDownRoutine()
    {
        // Wait 5 seconds 
        yield return new WaitForSeconds(5f);
        // Set isTripleShotActive to False
        _isShieldActive = false;
        _shield.SetActive(false);
    }

    /*public void AmmoActive()
    {
        _isAmmoActive = true;
        // Current Ammo set to the Max Ammo amount
        _currentAmmo += _maxAmmo;
        // Adjust Current Ammo amount to each laser shot
        if (_currentAmmo > _maxAmmo)
        {
            _currentAmmo = _maxAmmo;
        }

        StartCoroutine(AmmoPowerDownRoutine());
    }

    IEnumerator AmmoPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isAmmoActive = false;
    }*/
}
