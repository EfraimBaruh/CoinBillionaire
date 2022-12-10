using UnityEngine;

[RequireComponent(typeof(Animator))]
public class LockButton : PressAbleButton
{
    private Animator _animator;
    protected override void Start()
    {
        base.Start();

        _animator = GetComponent<Animator>();
    }

    protected override void OnButtonPress()
    {
        _animator.SetTrigger("unlock");

        _button.enabled = false;
    }
}
