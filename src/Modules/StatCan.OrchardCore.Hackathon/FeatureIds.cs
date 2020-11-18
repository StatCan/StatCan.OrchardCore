namespace StatCan.OrchardCore.Hackathon
{
    public static class FeatureIds
    {
        private const string FeatureIdPrefix = Hackathon + ".";
        public const string Hackathon = "StatCan.OrchardCore.Hackathon";
        public const string Team = FeatureIdPrefix + nameof(Team);
        public const string Judging = FeatureIdPrefix + nameof(Judging);
    }
}
