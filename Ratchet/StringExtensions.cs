using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.FormattableString;

[assembly: InternalsVisibleTo("Ratchet.Tests")]
namespace Ratchet {
    internal static class StringExtensions {
        private static bool IsFileBasedOn (
            this string transform,
            string source,
            Func<IReadOnlyList<string>, IReadOnlyList<string>, bool> comparison
        ) =>
            !string.IsNullOrWhiteSpace(transform)
            && !string.IsNullOrWhiteSpace(source)
            && comparison(transform.ToFilenameParts(), source.ToFilenameParts());

        public static bool IsFileUltimatelyBasedOn (
            this string transform,
            string source
        ) =>
            transform.IsFileBasedOn(source, AreFilenamePartsUltimatelyBasedOn);

        public static bool IsFileDirectlyBasedOn (
            this string transform,
            string source
        ) =>
            transform.IsFileBasedOn(source, AreFilenamePartsDirectlyBasedOn);

        public static bool AreFilenamePartsUltimatelyBasedOn (
            this IReadOnlyList<string> transform,
            IReadOnlyList<string> source
        ) =>
            source?.Count >= 2
            && transform?.Count > source.Count
            && string.Equals(source[0], transform[0], StringComparison.InvariantCultureIgnoreCase)
            && string.Equals(source.Last(), transform.Last(), StringComparison.InvariantCultureIgnoreCase);

        public static bool AreFilenamePartsDirectlyBasedOn (
            this IReadOnlyList<string> transform,
            IReadOnlyList<string> source
        ) =>
            transform.AreFilenamePartsUltimatelyBasedOn(source)
            && source
                .Take(source.Count - 1)
                .Select(s => s.ToLowerInvariant())
                .SequenceEqual(transform
                    .Take(transform.Count - 2)
                    .Select(t => t.ToLowerInvariant()));

        public static string[] ToFilenameParts (
            this string filename
        ) =>
            filename.Split('.');

        public static bool Validate (
            this string path,
            Func<string, bool> exists
        ) {
            if (exists(path)) { return true; }

            Console.WriteLine(Invariant($" Path does not exist: {path}"));
            Console.WriteLine("Bailing..");
            return false;
        }
    }
}
