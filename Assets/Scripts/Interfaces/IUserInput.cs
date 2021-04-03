using System;

public interface IUserInput
{
    event Action<UserInputArgs> OnPrimaryAction;
    event Action<UserInputArgs> OnSecondaryAction;
}
