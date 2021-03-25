using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity;
using System.Threading.Tasks;
using CursorBot.RiotSharpFunctions;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading;

namespace CursorBot.Commands
{
    public class TeamCommands : BaseCommandModule
    {
        public static List<JoinedMember> ResultTeam = new List<JoinedMember>();

        //[Command("join")]
        //public async Task Join(CommandContext ctx)
        //{
        //    var joinEmbed = new DiscordEmbedBuilder
        //    {
        //        Title = "Would you like to join?",
        //        //ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl,
        //        Color = DiscordColor.Green
        //    };

        //    var joinMessage = await ctx.Channel.SendMessageAsync(embed: joinEmbed).ConfigureAwait(false);

        //    var thumbsUpEmoji = DiscordEmoji.FromName(ctx.Client, ":+1:");
        //    var thumbsDownEmoji = DiscordEmoji.FromName(ctx.Client, ":-1:");

        //    await joinMessage.CreateReactionAsync(thumbsUpEmoji).ConfigureAwait(false);
        //    await joinMessage.CreateReactionAsync(thumbsDownEmoji).ConfigureAwait(false);

        //    var interactivity = ctx.Client.GetInteractivity();

        //    var reactionResult = await interactivity.WaitForReactionAsync(
        //        x => x.Message == joinMessage &&
        //        //x.Message.Author.Id == ctx.Member.Id &&
        //        x.User == ctx.User &&
        //        (x.Emoji == thumbsUpEmoji || x.Emoji == thumbsDownEmoji)).ConfigureAwait(false);

        //    if (reactionResult.Result.Emoji == thumbsUpEmoji)
        //    {
        //        //var role = ctx.Guild.GetRole(746571082353475595);
        //        //await ctx.Member.GrantRoleAsync(role).ConfigureAwait(false);
        //    }
        //    else if (reactionResult.Result.Emoji == thumbsDownEmoji)
        //    {
        //        // Do Nothing
        //    }
        //    {
        //        // Something went wrong
        //    }

        //    await joinMessage.DeleteAsync().ConfigureAwait(false);
        //}

        [Command("내전")]
        [Description("내전 팀 구성")]
        public async Task GenerateTeam(CommandContext ctx,
                      [Description("밸런스/그냥")] string balance = "밸런스",
                      [Description("소환사명(띄워쓰기 구분), 공백은 _넣을것")] params string[] summonerNames)
        {
            var gt = new GenerateTeamMember();
            var listTeam = new List<JoinedMember>();
            var balTeam = new List<JoinedMember>();

            var teamEmbed = new DiscordEmbedBuilder
            {
                Title = "내전 참가자",
                Description = gt.GetJoinMember(summonerNames),
                //ThumbnailUrl = ctx.Client.CurrentUser.AvatarUrl,
                Color = DiscordColor.Green
            };

            await ctx.Channel.SendMessageAsync(embed: teamEmbed).ConfigureAwait(false);

            if (summonerNames.Length != 10)
            {
                await ctx.Channel.SendMessageAsync("참가자를 다시 확인해주세요!").ConfigureAwait(false);
                return;
            }

            listTeam = gt.ResultTeam(summonerNames);
            PrintTeamResult ptr;

            if (balance == "밸런스")
            {
                //balTeam = gt.BalancedTeamList(listTeam);
                //int stackSize = 1024 * 1024 * 100;
                //Thread thread = new Thread(() =>
                //{
                //    gt.BalancedTeam(listTeam);
                //}, stackSize);
                //thread.Start();
                //thread.Join();
                var i = 0;
                while (i < 300)
                {
                    gt.BalancedTeam(listTeam);
                    i++;
                }

                //ptr = new PrintTeamResult(gt.resultTeamMember);
                ptr = new PrintTeamResult(ResultTeam, true);
            }
            else
            {
                ptr = new PrintTeamResult(listTeam, false);
            }

            await ctx.Channel.SendMessageAsync(ptr.ToString()).ConfigureAwait(false);

        }
    }
}
