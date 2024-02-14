using Core;
using StarterAssets;
using UnityEngine;

public class UnitMono : CharacterMono
{
    private new Unit Actual => (Unit)base.Actual;
    private Animator? _animator;
    public float SpeedChangeRate = 10.0f;

    private int _animIDSpeed;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDFreeFall;
    private int _animIDMotionSpeed;

    public override void Setup(Entity entity)
    {
        base.Setup(entity);
        _animator = GetComponent<Animator>();
        AssignAnimationIDs();
        _animator.SetFloat(_animIDMotionSpeed, 1f);
    }

    public override void Tick(float deltaTime)
    {
        base.Tick(deltaTime);
        Vector3 velocity = Actual.Velocity.ToVector3();
        this.transform.position += velocity * deltaTime;

        if (_animator != null)
        {
            _animator.SetFloat(_animIDSpeed, velocity.magnitude);
        }

        if (velocity != Vector3.zero)
        {
            velocity.y = 0;
            this.transform.rotation = Quaternion.Lerp(
                this.transform.rotation,
                Quaternion.LookRotation(velocity),
                deltaTime * 10);
        }

        if ((transform.position - Actual.Location.ToVector3()).sqrMagnitude > 0.2f)
        {
            transform.position = Actual.Location.ToVector3();
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
    }
}