using Microsoft.Web.XmlTransform;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using static System.FormattableString;

[assembly: InternalsVisibleTo("Ratchet.Tests")]
namespace Ratchet {
    internal static class PathsExtensions {
        public static void Print (
            this Paths paths
        ) {
            Console.WriteLine("Paths:");
            Console.WriteLine(Invariant($" Source: {paths.Source}"));
            Console.WriteLine(Invariant($" Transform: {paths.Transform}"));
            Console.WriteLine(Invariant($" Target: {paths.Target}"));
        }

        public static bool Validate (
            this Paths paths
        ) {
            if (paths.Source.Validate(File.Exists)) {
                paths.Source = Path.GetFullPath(paths.Source);
            } else {
                return false;
            }

            if (paths.Transform.Validate(File.Exists)) {
                paths.Transform = Path.GetFullPath(paths.Transform);
            } else {
                return false;
            }

            if (new DirectoryInfo(paths.Target).Parent.FullName.Validate(Directory.Exists)) {
                paths.Target = Path.GetFullPath(paths.Target);
            } else {
                return false;
            }

            return true;
        }

        public static void Transform (
            this Paths paths
        ) {
            var transformFiles = paths.GetTransformFiles();

            // preserving whitespace seems buggy when not
            // letting the library handle streams itself
            // looked through the MS source, did not see a reason
            // why this was this case
            using var document = new XmlTransformableDocument {
                PreserveWhitespace = paths.Source == paths.Target
            };
            document.Load(paths.Source);

            foreach (var transformFile in transformFiles) {
                using var transform = new XmlTransformation(transformFile);
                if (transform.Apply(document)) {
                    Console.WriteLine(Invariant($" Applied: {transformFile}"));
                } else {
                    Console.WriteLine(Invariant($" Failed: {transformFile}"));
                    Console.WriteLine("Exiting..");
                    return;
                }
            }

            // the Save function does not Dispose its underlying Stream right away
            // but, using custom Stream objects and calling dispose on them
            // cannot be done because of issue above with whitespace
            // retries are not limited, since users can kill this program if they want
            var saved = false;
            Console.Write(" Saving..");
            while (!saved) {
                try {
                    document.Save(paths.Target);
                    saved = true;
                } catch (IOException) {
                    Console.Write(".");
                }
                Thread.Sleep(100);
            }
            Console.WriteLine();

            Console.WriteLine(Invariant($" Saved: {paths.Target}"));
        }

        public static Stack<string> GetTransformFiles (
            this Paths paths
        ) {
            Stack<string> transforms = new Stack<string>();

            var transform = Path.GetFileName(paths.Transform);
            var source = Path.GetFileName(paths.Source);

            if (transform.IsFileUltimatelyBasedOn(source)) {
                transforms.Push(paths.Transform);

                var files = new DirectoryInfo(paths.Transform)
                    .Parent
                    .GetFiles()
                    .ToList();

                var done = false;

                while (!done) {
                    done = true;

                    foreach (var file in files) {
                        if (!string.Equals(file.Name, source, StringComparison.InvariantCultureIgnoreCase)
                            && transform.IsFileDirectlyBasedOn(file.Name)
                        ) {
                            transforms.Push(file.FullName);
                            transform = file.Name;

                            done = false;
                            break;
                        }
                    }
                }
            } else {
                transforms.Push(paths.Transform);
            }

            return transforms;
        }
    }
}
