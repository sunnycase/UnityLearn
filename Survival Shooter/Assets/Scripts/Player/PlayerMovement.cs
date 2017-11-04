using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6.0f;

    private Vector3 _movement;
    private Animator _animator;
    private Rigidbody _playerRigidbody;
    private int _floorMask;
    private float _rayCameraLength = 100f;

    private void Awake()
    {
        _floorMask = LayerMask.GetMask("Floor");
        _animator = GetComponent<Animator>();
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        Move(horizontal, vertical);
        Turning();
        Animating(horizontal, vertical);
    }

    private void Move(float horizontal, float vertical)
    {
        _movement.Set(horizontal, 0, vertical);
        _movement = _movement.normalized * speed * Time.deltaTime;
        _playerRigidbody.MovePosition(_playerRigidbody.position + _movement);
    }

    private void Turning()
    {
        var cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit floorHit;
        if (Physics.Raycast(cameraRay, out floorHit, _rayCameraLength, _floorMask))
        {
            var playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0;

            var newRotation = Quaternion.LookRotation(playerToMouse);
            _playerRigidbody.MoveRotation(newRotation);
        }
    }

    private void Animating(float horizontal, float vertical)
    {
        var walking = horizontal != 0 || vertical != 0;
        _animator.SetBool("IsWalking", walking);
    }
}
