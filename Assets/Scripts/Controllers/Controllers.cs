using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Controllers : IExecuteable, ICleanupable
{

    private List<IExecuteable> _executeables;
    private List<ICleanupable> _cleanupable;

    public Controllers()
    {
        _executeables = new List<IExecuteable>();
        _cleanupable = new List<ICleanupable>();
    }

    public void Add(IController controller)
    {
        if (controller is IExecuteable executeableController)
        {
            if (!_executeables.Contains(executeableController))
            {
                _executeables.Add(executeableController);
            }
        }
        if (controller is ICleanupable cleanupableController)
        {
            if (!_cleanupable.Contains(cleanupableController))
            {
                _cleanupable.Add(cleanupableController);
            }
        }
    }

    public void Cleanup()
    {
        for (int i = 0; i < _cleanupable.Count; i++)
        {
            _cleanupable[i].Cleanup();
        }
    }

    public void Execute(float deltaTime)
    {
        for (int i = 0; i < _executeables.Count; i++)
        {
            _executeables[i].Execute(deltaTime);
        }
    }
}
