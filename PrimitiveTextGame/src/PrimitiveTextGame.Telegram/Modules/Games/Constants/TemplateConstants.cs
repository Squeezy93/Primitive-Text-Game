namespace PrimitiveTextGame.Telegram.Modules.Games.Constants;

public static class TemplateConstants
{
    public static readonly Dictionary<string, string> MessagesTemplateDictionary = new()
    {
        {nameof(CreateNewUser), "Выбери героя!" },
        {nameof(UserReturned), "Добро пожаловать обратно {{UserName}}. Ваш персонаж {{CharacterName}}. Что хотите сделать?"},
        {nameof(UserCreated), "Уважаемый {{UserName}}. Ваш персонаж {{CharacterName}} создан. Что хотите сделать?"},
        {nameof(StartSearching), "Вы встали в поиск. Скоро противник найдется :)"},
        {nameof(GameInvitationForUser), "Противник найден. Вы играете против {{OpponentUserName}}. Готовы к битве?"},
        {nameof(GameInvitationForOpponent), "Противник найден. Вы играете против {{UserUserName}}. Готовы к битве?"},
        {nameof(WaitingForOpponent), "Вы подтвердили участие. Ожидаем подтверждения от вашего оппонента" },
        {nameof(FirstTurn),"Ваш ход. Выберите оружие" }
    };

    public static readonly Dictionary<string, List<string>> ParametersTemplatesDictionary = new()
    {
        {nameof(UserReturned), new List<string> {"UserName", "Character.Name"}},
        {nameof(UserCreated), new List<string> {"UserName", "Character.Name"}},
        {nameof(StartSearching), new List<string>{"UserTelegramId"}},
        {nameof(GameInvitationForUser), new List<string> {"User.UserName", "Opponent.UserName", "User.UserTelegramId", "Opponent.UserTelegramId"}},
        {nameof(FirstTurn), new List<string> { "User.UserTelegramId", "Opponent.UserTelegramId", "User.Weapons"}}
    };

    public static readonly Dictionary<string, Dictionary<string, string>> InlineMarkupTemplatesDictionary = new()
    {
        {nameof(UserReturned), new Dictionary<string, string>
        {
            {"Найти соперника", "search"},
            {"Поменять персонажа", "choose_player_character"},
            {"Покинуть игру", "quit_game" }
        }},
        {nameof(CreateNewUser), new Dictionary<string, string>
        {
            {"Рыцарь", "create_player_Knight"},
            {"Маг", "create_player_Mage"},
            {"Лесоруб", "create_player_Lumberjack"}

        }},
        {nameof(UserCreated), new Dictionary<string, string>
        {
            {"Найти соперника", "search" },
            {"Поменять персонажа", "choose_player_character"},
            {"Покинуть игру", "quit_game" }
        }},
        {nameof(StartSearching), new Dictionary<string, string>
        {
            {"Выйти из поиска", "stop_searching_{{UserTelegramId}}" }
        }},
        {nameof(GameInvitationForUser), new Dictionary<string, string>
        {
            {"Да", "accept_game_{{UserUserTelegramId}}_{{OpponentUserTelegramId}}" },
            {"Нет", "decline_game_{{UserUserTelegramId}}_{{OpponentUserTelegramId}}" }
        }},
        {nameof(GameInvitationForOpponent), new Dictionary<string, string>
        {
            {"Да", "accept_game_{{OpponentUserTelegramId}}_{{UserUserTelegramId}}" },
            {"Нет", "decline_game_{{OpponentUserTelegramId}}_{{UserUserTelegramId}}" }
        }},
        {nameof(FirstTurn), new Dictionary<string, string>
        {
            {"{{UserWeaponName}}", "attack_{{UserWeaponName}}_{{UserUserTelegramId}}_{{OpponentUserTelegramId}}" }
        }},
    };

    public const string MessagesTemplate = nameof(MessagesTemplate);
    public const string ParametersTemplate = nameof(ParametersTemplate);
    public const string InlineMarkupTemplate = nameof(InlineMarkupTemplate);
    public const string UserReturned = nameof(UserReturned);
    public const string CreateNewUser = nameof(CreateNewUser);
    public const string UserCreated = nameof(UserCreated);
    public const string StartSearching = nameof(StartSearching);
    public const string GameInvitationForUser = nameof(GameInvitationForUser);
    public const string GameInvitationForOpponent = nameof(GameInvitationForOpponent);
    public const string WaitingForOpponent = nameof(WaitingForOpponent);
    public const string FirstTurn = nameof(FirstTurn);
}