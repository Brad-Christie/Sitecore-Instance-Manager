namespace SIM.Client.Commands
{
  using System;
  using System.IO;
  using System.Threading;
  using CommandLine;
  using JetBrains.Annotations;
  using SIM.Core;
  using SIM.Core.Commands;
  using SIM.IO.Real;

  [Verb("login", HelpText = "Log in user as admin to an instance.")]
  public class LoginCommandFacade : LoginCommand
  {
    [UsedImplicitly]
    public LoginCommandFacade()
      : base(new RealFileSystem())
    {
    }

    [Option('n', "name", Required = true)]
    public override string Name { get; set; }

    protected override void WaitAndDelete(string destFileName)
    {
      var deadline = DateTime.UtcNow.AddSeconds(CoreInstanceAuth.LifetimeSeconds);
      if (File.Exists(destFileName))
      {
        Console.WriteLine("The file has been created and opened in browser:");
        Console.WriteLine(destFileName.Replace("/", "\\"));
        Console.WriteLine();
        Console.WriteLine("[Recommended] Wait until it is destroyed automatically.");
        Console.WriteLine("[Optional]    Press any key to abort and delete it instantly.");
        Console.WriteLine();
      }

      while (File.Exists(destFileName))
      {
        if (Console.KeyAvailable || deadline < DateTime.UtcNow)
        {
          File.Delete(destFileName);
        }

        Thread.Sleep(250);
      }
    }
  }
}