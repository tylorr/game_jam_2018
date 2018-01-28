using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Controller2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerAnimator : MonoBehaviour
{
    public float chargeDuration = 5.0f;
    public float pluginDuration = 1.5f;
    
    public float spawnDelay = 1.0f;

    public new DeadzoneCamera camera;
    public Transform carryLocator;
    public Transform dropLocator;
    
    private PlayerInput _playerInput;
    private Controller2D _controller;
    private Animator _animator;
    private Player _player;
    private SpriteRenderer _spriteRenderer;

    private bool _isPlayingBlockingAnimation = false;
    
    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _player = GetComponent<Player>();
        _controller = GetComponent<Controller2D>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Settle();
    }

    private void Settle()
    {
        _player.Update();
        while (!_controller.collisions.below)
        {
            _player.Update();
        }
    }

    private void Update()
    {
        //var dirx = _player.DirectionalInput.x;
        //var shouldFlip = !_spriteRenderer.flipX ? dirx < -0.001f : dirx > 0.001f;
        //if (shouldFlip)
        //{
        //    _spriteRenderer.flipX = !_spriteRenderer.flipX;
        //}
        _spriteRenderer.flipX = _controller.collisions.faceDir > 0 ? false : true;
        _animator.SetBool("grounded", _controller.collisions.below);
        _animator.SetFloat("velocityX", Mathf.Abs(_player.DirectionalInput.x) / _player.moveSpeed);
    }

    public bool Plugin(Energy energy)
    {
        if (!_isPlayingBlockingAnimation)
        {
            StartCoroutine(PluginAnimation(energy));
            return true;
        }
        return false;
    }
    
    public IEnumerator PluginAnimation(Energy energy)
    {
        _isPlayingBlockingAnimation = true;
        _playerInput.enabled = false;
        _player.Stop();
        
        _animator.SetTrigger("plugin");
        
        yield return new WaitForSeconds(pluginDuration);

        // TODO: Animate energy
        energy.AddEnergy(energy.maxEnergy);
        yield return new WaitForSeconds(chargeDuration - pluginDuration);

        _animator.SetTrigger("unplug");
        var unplugDuration = _animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(unplugDuration);

        _playerInput.enabled = true;
        _isPlayingBlockingAnimation = false;
    }

    public void EnterRoom(Room room)
    {
        camera.limits.Add(room.GetBoundingRect());
        Respawn(room.spawnLocation);
    }

    public async void Respawn(Transform location)
    {
        _player.Stop();
        transform.position = location.position;
        Settle();
        _playerInput.enabled = false;
        await new WaitForSeconds(spawnDelay);
        _playerInput.enabled = true;
    }
    
    public void Pickup(Carryable carryable)
    {
        carryable.transform.SetParent(carryLocator);
        carryable.transform.localPosition = Vector3.zero;
        _animator.SetTrigger("pickup");
    }
    
    public void Putdown(Carryable carryable)
    {
        _animator.SetTrigger("putdown");
        carryable.transform.SetParent(null);
        carryable.transform.position = dropLocator.position;
    }
}
