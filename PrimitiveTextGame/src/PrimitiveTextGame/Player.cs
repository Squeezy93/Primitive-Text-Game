namespace PrimitiveTextGame
{
    public record Player
    {
        public string PlayerName { get; private set; }

        public Player(string playerName)
        {
            IsValidNickname(playerName);
            PlayerName = playerName;
        }

        private string IsValidNickname(string playerName)
        {
            if (string.IsNullOrEmpty(playerName))
            {
                throw new ArgumentNullException(nameof(playerName), "PlayerName cannot be null or empty.");
            }
            return playerName;
        }
    }
}
