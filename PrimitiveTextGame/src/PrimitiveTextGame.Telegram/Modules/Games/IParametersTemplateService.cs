namespace PrimitiveTextGame.Telegram.Modules.Games
{
    public interface IParametersTemplateService
    {
        Task<Dictionary<string, object>> GetParametersAsync(string templateName, params object[] userDataObjects);
    }
}
