﻿namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        // "Технические сохранения" для работы плагина (Не удалять)
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        // Тестовые сохранения для демо сцены
        // Можно удалить этот код, но тогда удалите и демо (папка Example)
        public int money = 1;                       // Можно задать полям значения по умолчанию
        public string newPlayerName = "Hello!";
        public bool[] openLevels = new bool[3];

        ////////////////////////////////////////////////////////////////////////////////////
        
        public string[] UnlockedItems = new string[] {"Pistol"};
        public int HighScores;
        public int StartHealth = 3;
        public int Money;

        public SavesYG()
        {
            openLevels[1] = true;
        }
    }
}
