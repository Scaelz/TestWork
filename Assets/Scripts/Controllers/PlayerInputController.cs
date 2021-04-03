using System;
using UnityEngine;
using EventBusSystem;

public class PlayerInputController : IExecuteable, IUserInput
{
    private Camera _camera;

    public PlayerInputController(Camera camera)
    {
        _camera = camera;
    }

    public event Action<UserInputArgs> OnPrimaryAction;
    public event Action<UserInputArgs> OnSecondaryAction;

    public void Execute(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0))
        {
            var coords = _camera.ScreenToWorldPoint(Input.mousePosition);
            var args = new UserInputArgs()
            {
                MousePosition = coords,
            };
            EventBus.RaiseEvent<IUserInputHandler>(x => x.PrimaryActionHandler(args));
        }
        if (Input.GetMouseButtonDown(1))
        {
            EventBus.RaiseEvent<IUserInputHandler>(x => x.SecondaryActionHandler(new UserInputArgs()));
        }
    }
}
