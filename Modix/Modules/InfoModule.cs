﻿using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Modix.Modules
{
    [Name("Info"), Summary("General helper module")]
    public sealed class InfoModule : ModuleBase
    {
        private readonly CommandService _commandService;

        public InfoModule(CommandService cs)
        {
            _commandService = cs;
        }

        [Command("help"), Summary("Prints a neat list of all commands.")]
        public async Task HelpAsync()
        {
            var eb = new EmbedBuilder();
            var userDm = await Context.User.GetOrCreateDMChannelAsync();

            foreach (var module in _commandService.Modules)
            {
                eb = eb.WithTitle($"Module: {module.Name ?? "Unknown"}")
                       .WithDescription(module.Summary ?? "Unknown");

                foreach(var command in module.Commands)
                {
                    eb.AddField(new EmbedFieldBuilder().WithName($"Command: !{module.Name ?? ""} {command.Name ?? ""} {GetParams(command)}").WithValue(command.Summary ?? "Unknown"));
                }

                await userDm.SendMessageAsync(string.Empty, embed: eb.Build());
                eb = new EmbedBuilder();
            }
            await ReplyAsync($"Check your private messages, {Context.User.Mention}");
        }

        private string GetParams(CommandInfo info)
        {
            var sb = new StringBuilder();
            info.Parameters.ToList().ForEach(x =>
            {
                if (x.IsOptional)
                    sb.Append("[Optional(" + x.Name + ")]");
                else
                    sb.Append("[" + x.Name + "]");
            });
            return sb.ToString();
        }
    }
}
