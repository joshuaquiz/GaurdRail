using System;
using System.Device.Gpio;
using GuardRail.DoorClient.Interfaces;
using Microsoft.Extensions.Logging;

namespace GuardRail.DoorClient.Implementation;

public sealed class Gpio : IGpio, IDisposable
{
    private readonly GpioController _gpio;
    private readonly ILogger<Gpio> _logger;

    public Gpio(GpioController gpio, ILogger<Gpio> logger)
    {
        _gpio = gpio;
        _logger = logger;
        _logger.LogInformation("GPIO wrapper setup");
    }

    /// <inheritdoc />
    public void OpenPin(int pinNumber, PinMode mode)
    {
        try
        {
            _gpio.OpenPin(pinNumber, mode);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    /// <inheritdoc />
    public void OpenPin(int pinNumber)
    {
        try
        {
            _gpio.OpenPin(pinNumber);
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    /// <inheritdoc />
    public void ClosePin(int pinNumber)
    {
        try
        {
            _gpio.ClosePin(pinNumber);
        }
        catch (Exception e) when (e.Message == $"Can not close pin {pinNumber} because it is not open.")
        {
            _logger.LogInformation($"Pin {pinNumber} is already closed");
        }
        catch (Exception e)
        {
            _logger.LogError(e, e.Message);
        }
    }

    /// <inheritdoc />
    public PinValue Read(int pinNumber) =>
        _gpio.Read(pinNumber);

    /// <inheritdoc />
    public void Write(int pinNumber, PinValue value) =>
        _gpio.Write(pinNumber, value);

    /// <inheritdoc />
    public void SetPinMode(int pinNumber, PinMode mode) =>
        _gpio.SetPinMode(pinNumber, mode);

    /// <inheritdoc />
    public void RegisterCallbackForPinValueChangedEvent(
        int pinNumber,
        PinEventTypes eventTypes,
        PinChangeEventHandler callback) =>
        _gpio.RegisterCallbackForPinValueChangedEvent(
            pinNumber,
            eventTypes,
            callback);

    /// <inheritdoc />
    public void Dispose() =>
        _gpio.Dispose();
}