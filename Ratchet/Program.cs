namespace Ratchet
{
    using System;
    using System.IO;
    using System.Linq;
    using static System.FormattableString;

    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length < 2 || args.Take(2).Any(arg => !File.Exists(arg)))
            {
                Console.WriteLine("Usage:");
                Console.WriteLine(Invariant($"{AppDomain.CurrentDomain.FriendlyName}") +
                    " sourceFilename" +
                    " transformFilename" +
                    " [targetFilename]");
                Console.WriteLine("Note: target arg is optional; if not passed, source will be overwritten.");
                Console.WriteLine("Please try again..");
                return;
            }

            Console.WriteLine("Applying transform..");
            var arguments = args.Length > 2
                ? new Arguments(args[0], args[1], args[2])
                : new Arguments(args[0], args[1], args[0]);
            arguments.Print();
            arguments.Transform();
            Console.WriteLine("..Done!");
        }
    }
}
