using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour {
    [SerializeField] private float jumpForce = 1f;
    [SerializeField] private float fastFallForce = 1f;
    [SerializeField] private float velocity = 1f;
    private readonly float FAST_FALL_MULTIPLIER = -600f;
    private readonly float JUMP_MULTIPLIER = 400f;
    private readonly float MAX_HORIZONTAL_SPEED = 1f;
    private readonly float MAX_VERTICAL_SPEED = 15f;
    private readonly float SPEED_MULTIPLIER = 0.4f;

    private Vector2 _inputDirs;
    private bool _isFastFalling;
    private bool _isJumping;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    private void Start() {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    private void Update() {
        _inputDirs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space)) _isJumping = true;
        if (_inputDirs.y < 0) _isFastFalling = true;
    }

    private void FixedUpdate() {
        var verticalForce = 0f;
        if (_isFastFalling) {
            verticalForce = fastFallForce * FAST_FALL_MULTIPLIER;
            _isFastFalling = false;
        }

        if (_isJumping) {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            verticalForce = jumpForce * JUMP_MULTIPLIER;
            _isJumping = false;
        }

        _rb.AddForce(new Vector2(0, verticalForce));

        transform.Translate(_inputDirs.x * SPEED_MULTIPLIER * velocity, 0, 0);

        _rb.velocity = CapVelocity(_rb.velocity);
    }

    private Vector2 CapVelocity(Vector2 vel) {
        return new Vector2(Math.Clamp(vel.x, -MAX_HORIZONTAL_SPEED, MAX_HORIZONTAL_SPEED),
            Math.Clamp(vel.y, -MAX_VERTICAL_SPEED, MAX_VERTICAL_SPEED));
    }
}