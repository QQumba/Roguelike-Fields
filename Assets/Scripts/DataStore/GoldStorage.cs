using System.IO;

namespace DataStore
{
    public class GoldStorage
    {
        private const string Path = "gold.txt";

        public int GoldCount { get; private set; }

        public void AddGold(int amount)
        {
            GoldCount += amount;
            SaveGold(GoldCount.ToString());
        } 

        public void LoadGold()
        {
            if (!File.Exists(Path))
            {
                return;
            }

            var text = File.ReadAllText(Path);
            if (!int.TryParse(text, out var count))
            {
                return;
            }

            GoldCount = count;
        }

        private void SaveGold(string goldCount)
        {
            if (File.Exists(Path))
            {
                File.WriteAllText(Path, goldCount);
                return;
            }

            using var sw = File.CreateText(Path);
            sw.Write(goldCount);
        }
    }
}