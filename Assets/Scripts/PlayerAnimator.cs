using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Update()
    {
        var dirx = _player.DirectionalInput.x;
        
        if (_spriteRenderer.flipX ? (dirx > 0.01f) : (dirx < -0.01f))
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }

        _animator.SetBool("grounded", _controller.collisions.below);
        _animator.SetFloat("velocityX", Mathf.Abs(dirx) / _player.moveSpeed);
    }

    public void Plugin(Energy energy)
    {
        if (!_isPlayingBlockingAnimation)
        {
            StartCoroutine(PluginAnimation(energy));
        }
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
        _playerInput.enabled = false;
        await new WaitForSeconds(spawnDelay);
        _playerInput.enabled = true;
    }
}
