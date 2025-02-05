namespace PrimitiveTextGame.Telegram.Modules.Games.Constants;

public static class TemplateConstants
{
	public static readonly Dictionary<string, string> TemplateDictionary = new()
	{
		{nameof(UserCreated), "Уважаемый {{userName}}. Ваш персонаж {{characterName}} создан. Что хотите сделать?"}
	};

	public const string UserCreated = nameof(UserCreated);
	public const string Templates = nameof(Templates);
}