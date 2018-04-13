using System.IO.Abstractions;
using TSQLLint.Common;
using TSQLLint.Console.CommandLineOptions;
using TSQLLint.Console.Interfaces;
using TSQLLint.Lib.Config;
using TSQLLint.Lib.Config.Interfaces;
using TSQLLint.Lib.Parser;
using TSQLLint.Lib.Parser.Interfaces;
using TSQLLint.Lib.Parser.RuleExceptions;
using TSQLLint.Lib.Plugins;
using TSQLLint.Lib.Plugins.Interfaces;
using TSQLLint.Lib.Reporters;
using TSQLLint.Lib.Reporters.Interfaces;

namespace TSQLLint.Console
{
    public class Application
    {
        private readonly ICommandLineOptionHandler commandLineOptionHandler;
        private readonly CommandLineOptions.CommandLineOptions commandLineOptions;
        private readonly IConfigReader configReader;
        private readonly IReporter reporter;
        private readonly IConsoleTimer timer;
        private IPluginHandler pluginHandler;
        private ISqlFileProcessor fileProcessor;

        public Application(string[] args, IReporter reporter)
        {
            timer = new ConsoleTimer();
            timer.Start();

            this.reporter = reporter;
            commandLineOptions = new CommandLineOptions.CommandLineOptions(args);
            configReader = new ConfigReader(reporter);
            commandLineOptionHandler = new CommandLineOptionHandler(commandLineOptions, new ConfigFileGenerator(), configReader, reporter);
        }

        public void Run()
        {
            configReader.LoadConfig(commandLineOptions.ConfigFile);

            var fragmentBuilder = new FragmentBuilder(configReader.CompatabilityLevel);
            var ruleVisitorBuilder = new RuleVisitorBuilder(configReader, this.reporter);
            IRuleVisitor ruleVisitor = new SqlRuleVisitor(ruleVisitorBuilder, fragmentBuilder, reporter);
            pluginHandler = new PluginHandler(reporter);
            fileProcessor = new SqlFileProcessor(ruleVisitor, pluginHandler, reporter, new FileSystem());

            pluginHandler.ProcessPaths(configReader.GetPlugins());
            commandLineOptionHandler.HandleCommandLineOptions(commandLineOptions);
            fileProcessor.ProcessList(commandLineOptions.LintPath);

            if (fileProcessor.FileCount > 0)
            {
                reporter.ReportResults(timer.Stop(), fileProcessor.FileCount);
            }
        }
    }
}
