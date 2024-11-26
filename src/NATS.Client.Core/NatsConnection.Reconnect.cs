namespace NATS.Client.Core;

public partial class NatsConnection
{
    public async ValueTask ReconnectAsync()
    {
        var connectionOpenedTcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        // Event handler to set the TaskCompletionSource result
        ValueTask OnConnectionOpened(object? sender, NatsEventArgs e)
        {
            connectionOpenedTcs.TrySetResult();
            return default;
        }

        ConnectionOpened += OnConnectionOpened;
        await _socket.AbortConnectionAsync(CancellationToken.None).ConfigureAwait(false);
        try
        {
            await connectionOpenedTcs.Task.ConfigureAwait(false);
        }
        finally
        {
            // Unsubscribe from the ConnectionOpened event
            ConnectionOpened -= OnConnectionOpened;
        }
    }

    // public async ValueTask ReconnectAsync()
    // {
    //     switch (ConnectionState)
    //     {
    //     case NatsConnectionState.Connecting:
    //     case NatsConnectionState.Reconnecting:
    //         return;
    //     case NatsConnectionState.Closed:
    //         await ConnectAsync().ConfigureAwait(false);
    //         return;
    //     case NatsConnectionState.Open:
    //         if (_socket == null) return;
    //         CancellationTokenSource cts = new();
    //
    //         // Abort the current connection to force reconnect loop to start
    //         await _socket!.AbortConnectionAsync(cts.Token).ConfigureAwait(false);
    //         return;
    //     default:
    //         throw new ArgumentOutOfRangeException();
    // }

    private async ValueTask ReconnectAndWaitAsync()
    {
        // Todo: Cleanup method above
    }
}
