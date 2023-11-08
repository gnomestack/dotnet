using System.Diagnostics;
using System.Security.Claims;

using GnomeStack.Functional;

using Microsoft.DevTunnels.Ssh;
using Microsoft.DevTunnels.Ssh.Algorithms;
using Microsoft.DevTunnels.Ssh.Keys;
using Microsoft.DevTunnels.Ssh.Messages;
using Microsoft.DevTunnels.Ssh.Tcp;

namespace GnomeStack.Run.Tasks;

public class SshTask : BaseTask
{
    public SshTask(string id)
        : base(id)
    {
    }

    public IReadOnlyCollection<Target> Targets { get; set; }

    public string Command { get; set; } = string.Empty;

    public override async Task<Result<object, Error>> RunAsync(ITaskContext context, CancellationToken cancellationToken = default)
    {
        foreach (Target target in this.Targets)
        {
            var client = new SshClient(
                SshSessionConfiguration.Default,
                new TraceSource(nameof(SshClient)));
            SshClientSession session = await client.OpenSessionAsync(target.Host, target.Port, cancellationToken)
                .NoCap();

            SshClientCredentials credentials;

            if (!target.Key.IsNullOrWhiteSpace())
            {
                var key = KeyPair.ImportKeyFile(target.Key, target.Password, KeyFormat.OpenSsh);
                credentials = new SshClientCredentials(target.User, key);
            }
            else
            {
                credentials = new SshClientCredentials(target.User, target.Password);
            }

            // Handle server public key authentication.
            session.Authenticating += (_, e) =>
            {
                e.AuthenticationTask = Task.Run(
                    () =>
                        {
                            // TODO: Validate the server's public key.
                            // Return null if validation failed.
                            IKeyPair hostKey = e.PublicKey;
                            Console.WriteLine(hostKey);

                            var serverIdentity = new ClaimsIdentity();
                            return new ClaimsPrincipal(serverIdentity);
                        },
                    cancellationToken);
            };

            var isAuthenticated = await session.AuthenticateAsync(credentials, cancellationToken)
                .NoCap();

            if (!isAuthenticated)
            {
                return new Error($"Authentication failed for {target.Host}.");
            }
             

            SshChannel channel = await session.OpenChannelAsync(cancellationToken).NoCap();
            channel.RequestAsync(new ChannelSignalMessage()
            {
                Signal = 
            })
            bool commandAuthorized = await channel.RequestAsync(
                new CommandRequestMessage("test"),
                cancellationToken);
            if (commandAuthorized)
            {
                using var channelStream = new SshStream(channel);
                var result = await new StreamReader(channelStream).ReadToEndAsync();
                Console.WriteLine(result);
            }

            await channel.CloseAsync(cancellationToken).NoCap();

        }
        
    }
}