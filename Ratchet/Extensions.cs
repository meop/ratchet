namespace Ratchet
{
    using Microsoft.Web.XmlTransform;
    using System;
    using System.IO;
    using static System.FormattableString;

    internal static class Extensions
    {
        public static void Print(this Arguments args)
        {
            Console.WriteLine(Invariant($"Source: {args.SourceFilename}"));
            Console.WriteLine(Invariant($"Transform: {args.TransformFilename}"));
            Console.WriteLine(Invariant($"Target: {args.TargetFilename}"));
        }

        public static void Transform(this Arguments args)
        {
            // safer to write to a new stream
            // in case the source and target are same
            // and let the source close first
            using (var stream = new MemoryStream())
            {
                using (var source = new XmlTransformableDocument
                {
                    PreserveWhitespace = true
                })
                {
                    source.Load(args.SourceFilename);

                    using (var transform = new XmlTransformation(args.TransformFilename))
                    {
                        if (transform.Apply(source))
                        {
                            source.Save(stream);
                        }
                    }
                }

                using (var target = new FileStream(args.TargetFilename, FileMode.OpenOrCreate))
                {
                    stream.WriteTo(target);
                }
            }
        }
    }
}
