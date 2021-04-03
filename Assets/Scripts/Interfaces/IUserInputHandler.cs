using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventBusSystem;

public interface IUserInputHandler : IGlobalSubscriber
{
    void PrimaryActionHandler(UserInputArgs args);
    void SecondaryActionHandler(UserInputArgs args);
}
