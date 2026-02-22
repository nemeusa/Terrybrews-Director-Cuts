using System.Collections;
using UnityEngine;

public class Pump : MonoBehaviour
{
    [Header("conditions")]
    [SerializeField] private int _corduraMinEscopeta = 100;
    [SerializeField] private float _fallDistance = 2f;
    [SerializeField] private Vector3 _fallOffset = new Vector3(0, 2f, 0);
    [SerializeField] private float _fallDuration = 0.5f;
    [SerializeField] private GameObject _objetoAActivar;
    [SerializeField] private GameObject _objetoADesactivar;
    [SerializeField] private AudioClip _soundDesbloqueoEscopeta;
    [SerializeField] private AudioSource _audioSourceFeedback;
    [HideInInspector] private GameObject _shotgunUnlockFeedback;
    private bool _hasPlayedShotgunSound = false;
    private bool _shotgunUsable = false;

    [Header("ShootGun")]
    [SerializeField] MeshRenderer meshPumpBar;
    [SerializeField] MeshRenderer meshPumpHand;
    bool _usePump;
    PumpGun _pumpCode = null;
    //[SerializeField] ParticleSystem _smokeParticle;
    [SerializeField] private GameObject[] _ammoVisuals;
    public int _currentAmmo = 10;
    public int _maxAmmo = 10;
    private bool _isBlocked = false;
    [SerializeField] private float _blockDurationAfterShot = 1f;
    [SerializeField] private float _doubleShotCooldown = 5f;
    [SerializeField] private int _corduraPenalty = 10;
    private float _lastShotTime = -Mathf.Infinity;


    [Header("Audio")]
    [SerializeField] AudioSource _shotgunAudioSource;
    [SerializeField] AudioClip _shotgunSound;
    [SerializeField] private AudioClip _emptyShotSound;

    [Header("Bebidas y clientes")]
    [SerializeField] LayerMask _beverageLayer;
    [SerializeField] LayerMask _clientLayer;
    MoveDrinks _moveDrinks;
    [SerializeField] ShowShop _showShop;
    public string currentRequest;
    Player _player;
    [HideInInspector] public Client _client;


    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Start()
    {
        UpdateAmmoVisuals();
    }

    private void Update()
    {
        PumpConditions();
    }

    void PumpConditions()
    {
        if (_isBlocked) return;
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, 100f, _beverageLayer))
            {
                MoveDrinks moveDrinks = hit.collider.GetComponent<MoveDrinks>();
                _moveDrinks = moveDrinks;
                PumpGun pump = hit.collider.GetComponent<PumpGun>();
                _pumpCode = pump;

                if (moveDrinks != null)
                    if (moveDrinks.isDraggingDrink)
                    {
                        PumpOff();
                        _pumpCode = null;
                    }

                if (!_showShop.useShop)
                    if (_pumpCode != null)
                    {
                        if (!_usePump)
                        {
                            if (_currentAmmo > 0)
                            {
                                PumpOn();
                            }
                            else
                            {
                                if (_shotgunAudioSource != null && _emptyShotSound != null)
                                {
                                    _shotgunAudioSource.PlayOneShot(_emptyShotSound);
                                }
                            }
                        }
                        else
                        {
                            PumpOff();
                        }
                    }
                if (!_shotgunUsable && _player.cordura <= _corduraMinEscopeta)
                {
                    _shotgunUsable = true;
                    StartCoroutine(ShowShotgunUnlockFeedback());
                }
            }
            else if (Physics.Raycast(ray, out hit, 100f, _clientLayer))
            {
                Client client = hit.collider.GetComponentInParent<Client>();


                _client = client;
                PumpLogic();

            }
        }
    }

    private void UpdateAmmoVisuals()
    {
        for (int i = 0; i < _ammoVisuals.Length; i++)
        {
            _ammoVisuals[i].SetActive(i < _currentAmmo);
        }
    }



    void PumpLogic()
    {
        if (_usePump)
        {
            if (_currentAmmo > 0)
            {

                _currentAmmo--;
                UpdateAmmoVisuals();
                //urp.StartCoroutine(urp.ShootURP());
                if(_client != null) _client.isDeath = true;

                if (_shotgunAudioSource != null && _shotgunSound != null)
                    _shotgunAudioSource.PlayOneShot(_shotgunSound);

                if (_client.imposter)
                {
                    //GameStats.impostoresEliminados++;
                    _player.MoreMoney(100);
                    StartCoroutine(correct());
                }
                else
                {
                    _player.LessMoney(200);
                    _player.cordura -= _player._corduraMatarCliente;
                    StartCoroutine(Incorrect());
                }

                StartCoroutine(Shoot());
            }
            else
            {
                if (_shotgunAudioSource != null && _emptyShotSound != null)
                {
                    _shotgunAudioSource.PlayOneShot(_emptyShotSound);
                }
            }
        }
    }

    void PumpOn()
    {

        Debug.Log("escopeta agarrada");

        //_pumpHandAni.SetBool("Hand gets it", true);
        //_pumpBarAni.SetBool("Bar gets it", false);

        meshPumpBar.enabled = false;
        meshPumpHand.enabled = true;

        _usePump = true;

        if (!_hasPlayedShotgunSound && _audioSourceFeedback != null && _soundDesbloqueoEscopeta != null)
        {
            _audioSourceFeedback.PlayOneShot(_soundDesbloqueoEscopeta);
            _hasPlayedShotgunSound = true;

            if (_objetoAActivar != null)
                _objetoAActivar.SetActive(true);

            if (_objetoADesactivar != null)
                _objetoADesactivar.SetActive(false);
        }
    }

    void PumpOff()
    {

        Debug.Log("Escopeta dejada");
        //_pumpHandAni.SetBool("Hand gets it", false);
        //_pumpBarAni.SetBool("Bar gets it", true);

        meshPumpBar.enabled = true;
        meshPumpHand.enabled = false;

        _usePump = false;
        _pumpCode = null;
    }



    IEnumerator ShowShotgunUnlockFeedback()
    {
        if (_shotgunUnlockFeedback == null) yield break;

        _shotgunUnlockFeedback.SetActive(true);

        Vector3 startPos = _shotgunUnlockFeedback.transform.position + Vector3.down * _fallDistance;
        Vector3 targetPos = _shotgunUnlockFeedback.transform.position;
        _shotgunUnlockFeedback.transform.position = startPos;

        float elapsed = 0f;
        while (elapsed < _fallDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / _fallDuration);
            _shotgunUnlockFeedback.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        _shotgunUnlockFeedback.SetActive(false);
    }

    IEnumerator Shoot()
    {
        //_smokeParticle.Play();
        yield return new WaitForSeconds(0.1f);
        //ActivateDepthOfField(3f, 0.5f);
        PumpOff();
    }


    private IEnumerator Activate(float duration, float startDistance)
    {
        //depthOfField.active = true;
        //depthOfField.focusDistance.value = startDistance;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            //depthOfField.focusDistance.value = Mathf.Lerp(startDistance, 9f, elapsed / duration);
            yield return null;
        }
        //depthOfField.active = false;
        //depthOfField.focusDistance.value = 9f;
    }
    IEnumerator Incorrect()
    {
        //_client.visor.GetComponent<MeshRenderer>().material.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        //_client.visor.GetComponent<MeshRenderer>().material.color = Color.white;
    }

    IEnumerator correct()
    {
        //_client.visor.GetComponent<MeshRenderer>().material.color = Color.green;
        yield return new WaitForSeconds(0.3f);
        //_client.visor.GetComponent<MeshRenderer>().material.color = Color.white;
    }
}
