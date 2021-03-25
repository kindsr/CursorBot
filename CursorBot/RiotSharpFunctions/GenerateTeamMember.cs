using CursorBot.Enums;
using RiotSharp;
using RiotSharp.Endpoints.ChampionMasteryEndpoint;
using RiotSharp.Endpoints.LeagueEndpoint;
using RiotSharp.Endpoints.StaticDataEndpoint.Champion;
using RiotSharp.Endpoints.SummonerEndpoint;
using RiotSharp.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursorBot.RiotSharpFunctions
{
    public class GenerateTeamMember
    {
        private static readonly RiotApi Api = RiotApi.GetDevelopmentInstance(RiotApiKey.Instance.RiotKey);
        int reCount = 0;
        
        int resultTeamScoreGap = 0;
        string[] tier = { "챌", "그마", "마", "다", "플", "골", "실", "브", "아", "언" };

        public List<JoinedMember> resultTeamMember = new List<JoinedMember>();


        public string GetJoinMember(string[] summonerNames)
        {
            var result = String.Empty;

            foreach (var s in summonerNames)
            {
                result += s.Replace('_', ' ') + " * ";
            }

            return result;
        }
        public string GetSummonerID(string summonerName)
        {
            return Api.Summoner.GetSummonerByNameAsync(Region.Kr, summonerName).Result.Id ?? summonerName;
            //Task<Summoner> task = Task.Run<Summoner>(async () => await Api.Summoner.GetSummonerByNameAsync(Region.Kr, summonerName));
            //return task.Result.Id;
        }

        public List<LeagueEntry> GetSummonerPosition(string summonerID)
        {
            Task<List<LeagueEntry>> task = Task.Run<List<LeagueEntry>>(async () => await Api.League.GetLeagueEntriesBySummonerAsync(Region.Kr, summonerID));
            return task.Result;
        }

        public List<ChampionMastery> GetMostChampions(string summonerID)
        {
            Task<List<ChampionMastery>> task = Task.Run<List<ChampionMastery>>(async () => await Api.ChampionMastery.GetChampionMasteriesAsync(Region.Kr, summonerID));
            return task.Result;
        }

        public ChampionStatic GetChampionName(int championID)
        {
            Task<ChampionStatic> task = Task.Run<ChampionStatic>(async () => await Api.DataDragon.Champions.GetByIdAsync(championID, RiotApiKey.Instance.Version, Language.ko_KR));
            return task.Result;
        }

        public string ChampionsID2Name(List<ChampionMastery> champs, int count)
        {
            var result = string.Empty;

            for (int i = 0; i < count; i++)
            {
                //result += champs[i].ChampionId.ToString();
                result += GetChampionName((int)champs[i].ChampionId).Name;

                if (i < count - 1) result += ",";
            }

            return result;
        }

        public int GetTierScore(string tier)
        {
            switch (tier.ToUpper())
            {
                case "IRON":
                case "아":
                    return (int)Tier.Iron;
                case "BRONZE":
                case "브":
                    return (int)Tier.Bronze;
                case "SILVER":
                case "실":
                    return (int)Tier.Silver;
                case "GOLD":
                case "골":
                    return (int)Tier.Gold;
                case "PLATINUM":
                case "플":
                    return (int)Tier.Platinum;
                case "DIAMOND":
                case "다":
                    return (int)Tier.Diamond;
                case "MASTER":
                case "마":
                    return (int)Tier.Master;
                case "GRANDMASTER":
                case "그마":
                    return (int)Tier.GrandMaster;
                case "CHALLENGER":
                case "챌":
                    return (int)Tier.Challenger;
                default:
                    return 80;
            }
        }

        public int GetRankScore(string rank)
        {
            switch (rank.ToUpper())
            {
                case "IV":
                    return (int)Rank.IV;
                case "III":
                    return (int)Rank.III;
                case "II":
                    return (int)Rank.II;
                case "I":
                    return (int)Rank.I;
                default:
                    return 0;
            }
        }

        public List<JoinedMember> ResultTeam(string[] summonerNames)
        {
            var list = new List<JoinedMember>();
            var sortedList = new List<JoinedMember>();
            JoinedMember member;

            try
            {
                // Summoner 정보
                foreach (var s in summonerNames)
                {
                    member = new JoinedMember();
                    member.SummonerName = s.Replace('_', ' ');

                    // 티어만입력
                    if (Array.Exists(tier, x => x == member.SummonerName))
                    {
                        member.Tier = "";
                        member.Rank = "";
                        member.LeaguePoints = 0;
                        member.TierScore = GetTierScore(member.SummonerName);
                        member.MostChampions = "";
                    }
                    else
                    {
                        member.SummonerID = GetSummonerID(member.SummonerName);

                        member.Tier = "Unranked";
                        member.Rank = "";
                        member.LeaguePoints = 0;
                        var position = GetSummonerPosition(member.SummonerID);

                        foreach (var pos in position)
                        {
                            if (pos.QueueType.Contains("RANKED_SOLO_5x5"))
                            {
                                member.Tier = pos.Tier;
                                member.Rank = pos.Rank;
                                member.LeaguePoints = pos.LeaguePoints;
                                member.TierScore = GetTierScore(pos.Tier)
                                                 + GetRankScore(pos.Rank)
                                                 + (pos.LeaguePoints / 10);
                            }
                        }

                        var mostChamps = GetMostChampions(member.SummonerID);
                        var mostChampsName = ChampionsID2Name(mostChamps, 3);

                        member.MostChampions = mostChampsName;
                    }

                    list.Add(member);
                }

                // List sort here
                sortedList = list.OrderByDescending(x => x.TierScore).ToList();

                // Separate team here
                for (int i = 0; i < sortedList.Count; i++)
                {
                    sortedList[i].Team = i % 2 + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            var team1TierScore = 0;
            var team2TierScore = 0;

            foreach (var l in list)
            {
                if (l.Team == 1)
                {
                    team1TierScore += l.TierScore;
                }
                else if (l.Team == 2)
                {
                    team2TierScore += l.TierScore;
                }
            }

            resultTeamMember = sortedList;
            resultTeamScoreGap = Math.Abs(team1TierScore - team2TierScore);

            return sortedList;
        }

        public List<JoinedMember> BalancedTeamList(List<JoinedMember> list)
        {
            var tmpList = new List<JoinedMember>();
            var team1List = new List<JoinedMember>();
            var team2List = new List<JoinedMember>();
            var tmpSwapMember = new JoinedMember();
            var team1TierScore = 0;
            var team2TierScore = 0;

            var team1Order = 0;
            var team2Order = 0;


            foreach (var l in list)
            {
                if (l.Team == 1)
                {
                    l.TeamOrder = team1Order++;
                    team1List.Add(l);
                    team1TierScore += l.TierScore;
                }
                else if (l.Team == 2)
                {
                    l.TeamOrder = team2Order++;
                    team2List.Add(l);
                    team2TierScore += l.TierScore;
                }
            }

            Console.WriteLine(string.Format("1팀 : {0} // 2팀 : {1}", team1TierScore, team2TierScore));

            Random rand = new Random();
            int swapPos1 = rand.Next(0, 5);
            int swapPos2 = rand.Next(0, 5);
            

            if (Math.Abs(team1TierScore - team2TierScore) > 100)
            {
                if (reCount > 100)
                {
                    reCount = 0;
                    return team1List.Concat(team2List).ToList();
                }
                reCount++;
                // 난수를 발생하여 각 팀의 같은 order에 해당하는 사람을 변경
                tmpSwapMember = team1List[swapPos1];
                team1List[swapPos1] = team2List[swapPos2];
                team1List[swapPos1].Team = 1;
                team2List[swapPos2] = tmpSwapMember;
                team2List[swapPos2].Team = 2;

                //team별 오더 재정리
                team1List = team1List.OrderByDescending(x => x.TierScore).ToList();
                team1Order = 0;
                
                foreach (var t in team1List)
                {
                    t.TeamOrder = team1Order++;
                }

                team2List = team2List.OrderByDescending(x => x.TierScore).ToList();
                team2Order = 0;
                foreach (var t in team2List)
                {
                    t.TeamOrder = team2Order++;
                }

                // 다시 계산
                BalancedTeamList(team1List.Concat(team2List).ToList());
            }

            return team1List.Concat(team2List).ToList();
        }

        public void BalancedTeam(List<JoinedMember> list)
        {
            var team1List = new List<JoinedMember>();
            var team2List = new List<JoinedMember>();
            var tmpSwapMember = new JoinedMember();
            var team1TierScore = 0;
            var team2TierScore = 0;

            var team1Order = 0;
            var team2Order = 0;

            if (reCount >= 300)
            {
                //CursorBot.Commands.TeamCommands.ResultTeam = resultTeamMember;
                reCount = 0;
            }


            try
            {
                foreach (var l in list)
                {
                    if (l.Team == 1)
                    {
                        team1List.Add(l);
                    }
                    else if (l.Team == 2)
                    {
                        team2List.Add(l);
                    }
                }

                Random rand = new Random();
                int swapPos1 = rand.Next(0, 5);
                int swapPos2 = rand.Next(0, 5);

                // 난수를 발생하여 각 팀의 같은 order에 해당하는 사람을 변경
                tmpSwapMember = team1List[swapPos1];
                team1List[swapPos1] = team2List[swapPos2];
                team1List[swapPos1].Team = 1;
                team2List[swapPos2] = tmpSwapMember;
                team2List[swapPos2].Team = 2;

                //team별 오더 재정리
                team1List = team1List.OrderByDescending(x => x.TierScore).ToList();

                foreach (var t in team1List)
                {
                    t.TeamOrder = team1Order++;
                    team1TierScore += t.TierScore;
                }

                team2List = team2List.OrderByDescending(x => x.TierScore).ToList();
                foreach (var t in team2List)
                {
                    t.TeamOrder = team2Order++;
                    team2TierScore += t.TierScore;
                }

                int currentGap = Math.Abs(team1TierScore - team2TierScore);

                Console.WriteLine(string.Format("{4} :::  1팀 : {0} // 2팀 : {1} // 현재차이 : {2} // 최저차이 : {3}",
                team1TierScore, team2TierScore, currentGap, resultTeamScoreGap, reCount));

                if (currentGap < resultTeamScoreGap)
                {
                    foreach (var t in team1List)
                    {
                        t.Team = 1;
                        Console.WriteLine(string.Format("1팀 : {0} / {1}", t.SummonerName, t.TierScore));
                    }
                    foreach (var t in team2List)
                    {
                        t.Team = 2;
                        Console.WriteLine(string.Format("2팀 : {0} / {1}", t.SummonerName, t.TierScore));
                    }
                    
                    resultTeamMember = team1List.Concat(team2List).ToList();
                    CursorBot.Commands.TeamCommands.ResultTeam = resultTeamMember;
                    resultTeamScoreGap = currentGap;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            reCount++;

            // 다시 계산
            //BalancedTeam(team1List.Concat(team2List).ToList());
        }
    }
}
