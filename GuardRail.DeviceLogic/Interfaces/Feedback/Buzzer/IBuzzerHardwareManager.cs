using System.Threading;
using System.Threading.Tasks;

namespace GuardRail.DeviceLogic.Interfaces.Feedback.Buzzer;

/// <summary>
/// The low level API for interacting with a feedback buzzer.
/// </summary>
public interface IBuzzerHardwareManager : IAsyncInit
{
    /// <summary>
    /// Turns on a buzzer.
    /// </summary>
    /// <param name="address">The hardware address.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="ValueTask"/> representing the work to turn the buzzer on.</returns>
    public ValueTask TurnBuzzerOnAsync(
        byte[] address,
        CancellationToken cancellationToken);

    /// <summary>
    /// Turns off a buzzer.
    /// </summary>
    /// <param name="address">The hardware address.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <returns>A <see cref="ValueTask"/> representing the work to turn the buzzer off.</returns>
    public ValueTask TurnBuzzerOffAsync(
        byte[] address,
        CancellationToken cancellationToken);

    /// <summary>
    /// Disposes, closes, or resets the hardware.
    /// </summary>
    /// <param name="address">The hardware address.</param>
    /// <returns>A <see cref="ValueTask"/> representing the work to dispose/close the hardware.</returns>
    public ValueTask DisposeAddressAsync(
        byte[] address);
}