namespace DevJournal.Models
{
    public static class MoodOptions
    {
        public static List<string> GetAllMoods()
        {
            return new List<string>
            {
                "Happy",
                "Sad",
                "Anxious",
                "Calm",
                "Excited",
                "Angry",
                "Tired",
                "Energetic",
                "Stressed",
                "Relaxed",
                "Grateful",
                "Frustrated"
            };
        }

        public static string GetMoodEmoji(string mood)
        {
            return mood switch
            {
                "Happy" => "😊",
                "Sad" => "😢",
                "Anxious" => "😰",
                "Calm" => "😌",
                "Excited" => "🤩",
                "Angry" => "😠",
                "Tired" => "😴",
                "Energetic" => "⚡",
                "Stressed" => "😫",
                "Relaxed" => "😎",
                "Grateful" => "🙏",
                "Frustrated" => "😤",
                _ => "😐"
            };
        }
    }
}