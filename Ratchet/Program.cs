using System;
using static System.FormattableString;

namespace Ratchet
{
    internal static class Program
    {
        private static void Main(
            string[] args
        )
        {
            if (args == null
                || args.Length < 2
            )
            {
                Console.WriteLine("Usage:");
                Console.WriteLine(Invariant($"{AppDomain.CurrentDomain.FriendlyName}") +
                    " sourceFilePath" +
                    " transformFilePath" +
                    " [targetFilePath]");
                Console.WriteLine("Note: Target arg is optional; if not passed, Source will be overwritten.");
                return;
            }

            if (args[0] == args[1])
            {
                Console.WriteLine("Requested Source and Transform were the same:");
                Console.WriteLine(args[0]);
                Console.WriteLine(args[1]);
                Console.WriteLine("Exiting..");
                return;
            }

            var paths = new Paths
            {
                Source = args[0],
                Transform = args[1],
                Target = args.Length > 2 ? args[2] : args[0]
            };

            if (!paths.Validate()) { return; }

            paths.Print();
            Console.WriteLine("Starting transform..");
            paths.Transform();
            Console.WriteLine("..Done!");
        }
    }
}
