using System;
using Common.Tools;
using WeDoItBot.Tools;

namespace WeDoItBot
{
	class Settings : IBotStarterSettings
    {
		private Properties.Settings Config => Properties.Settings.Default;

        //public string BotToken => "1107856285:AAHcDAuZg-BikqjCUAIb6ro8z3prriYR3sg";
        public string BotToken => "1209926618:AAFWtxyuuDISBKikVwgxYA7ucpinWOYDb9Y";
            
        //public Uri ProxyHost => new Uri("http://51.15.157.135:5836");
        public Uri ProxyHost => new Uri("http://201.249.190.235:3128");
        public string BotFile => "bot.json";
    }
}