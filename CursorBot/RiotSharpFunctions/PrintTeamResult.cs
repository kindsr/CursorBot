using System;
using System.Collections.Generic;
using System.Text;

namespace CursorBot.RiotSharpFunctions
{
    public class PrintTeamResult
    {
        public PrintTeamResult()
        {

        }

        public PrintTeamResult(List<JoinedMember> team, bool balance)
        {
            int team1cnt = 0;
            int team2cnt = 0;

            team1 = "1팀 : ";
            team2 = "2팀 : ";

            foreach (var l in team)
            {
                if (balance)
                {
                    if (team1cnt < 5)
                    {
                        team1 += l.ToString();
                        team1Detail += string.Format("{0} ({1})", l.SummonerName, l.MostChampions) + Environment.NewLine;
                        team1cnt++;
                        if (team1cnt < 5) team1 += ",";
                        continue;
                    }
                    else
                    {
                        team2 += l.ToString();
                        team2Detail += string.Format("{0} ({1})", l.SummonerName, l.MostChampions) + Environment.NewLine;
                        team2cnt++;
                        if (team2cnt < 5) team2 += ",";
                        continue;
                    }
                }
                else
                {
                    switch (l.Team)
                    {
                        case 1:
                            team1 += l.ToString();
                            team1Detail += string.Format("{0} ({1})", l.SummonerName, l.MostChampions) + Environment.NewLine;
                            team1cnt++;
                            if (team1cnt < 5) team1 += ",";
                            continue;
                        case 2:
                            team2 += l.ToString();
                            team2Detail += string.Format("{0} ({1})", l.SummonerName, l.MostChampions) + Environment.NewLine;
                            team2cnt++;
                            if (team2cnt < 5) team2 += ",";
                            continue;
                        default:
                            continue;
                    }
                }
            }
        }

        private string team1;
        private string team1Detail;
        private string team2;
        private string team2Detail;

        public override string ToString()
        {
            return team1 + Environment.NewLine + Environment.NewLine +
                   team1Detail + Environment.NewLine +
                   team2 + Environment.NewLine + Environment.NewLine +
                   team2Detail;
        }
    }
}
