namespace WPFCommon
{
    public interface IConfigService
    {
        string ConnectionStrings { get; }

        string SystemLanguage { get; }

        string DefaultFont { get; }
    }
}