namespace Ratchet
{
    internal class Arguments
    {
        public string SourceFilename { get; }
        public string TransformFilename { get; }
        public string TargetFilename { get; }

        public Arguments(
            string sourceFilename,
            string transformFilename,
            string targetFileName
        )
        {
            SourceFilename = sourceFilename;
            TransformFilename = transformFilename;
            TargetFilename = targetFileName;
        }
    }
}
