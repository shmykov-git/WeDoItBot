using Common;
using Common.Logs;
using WeDoItBot.Tools;

namespace WeDoItBot
{
	class Program
	{
		static Program()
		{
			IoC.Configure(IoCWeDoIt.Register);
		}

		static void Main(string[] args)
		{
			var log = IoC.Get<ILog>();
			log.Debug("### Start ###");

            IoC.Get<BotStarter>().Start();

            log.Debug("### End ###");
		}
	}
}
