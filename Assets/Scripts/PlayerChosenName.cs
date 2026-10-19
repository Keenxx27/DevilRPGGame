namespace RPG
{
    public static class PlayerChosenName
    {
        private static string value;

        public static string Value => string.IsNullOrWhiteSpace(value) ? "主角" : value;

        public static void Set(string value)
        {
            PlayerChosenName.value = string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }

        public static string ResolveDialogueSpeaker(string speaker)
        {
            return speaker == "主角" ? Value : speaker;
        }
    }
}
