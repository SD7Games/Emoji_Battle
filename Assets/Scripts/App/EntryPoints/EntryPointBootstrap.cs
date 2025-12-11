using System;
using UnityEngine;

public class EntryPointBootstrap : MonoBehaviour
{
    [SerializeField]
    private BootstrapView _bootstrapView;

    private BootstrapController _controller;

    private void Awake()
    {
        _controller = new BootstrapController(_bootstrapView);
    }

    private async void Start()
    {
        try
        {
            await _controller.StartAsynk();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}