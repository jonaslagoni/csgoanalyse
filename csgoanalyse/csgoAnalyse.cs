using DemoInfo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace csgoanalyse
{
    class csgoAnalyse
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (File.Exists(args[0]))
                {
                    try
                    {
                        //open the file to read
                        DemoParser parser = new DemoParser(File.OpenRead(args[0]));
                        parser.ParseHeader();
                        string map = parser.Map;
                        Console.WriteLine("map:" + map);

                        //get the score from the two teams
                        int ct_score = 0, t_score = 0;
                        parser.RoundEnd += (sender, e) => {
                            if(ct_score + t_score >= 15){
                                if (e.Winner.ToString() == "Terrorist"){
                                    ct_score++;
                                }
                                else if (e.Winner.ToString() == "CounterTerrorist"){
                                    t_score++;
                                }
                            }
                            else{
                                if (e.Winner.ToString() == "Terrorist"){
                                    t_score++;
                                }
                                else if (e.Winner.ToString() == "CounterTerrorist"){
                                    ct_score++;
                                }
                            }
                        };

                        //get the two team players
                        parser.MatchStarted += (sender, e) => {
                            //get players t start info
                            Console.WriteLine("terrorist players");
                            foreach (var player in parser.PlayingParticipants.Where(a => a.Team == Team.Terrorist))
                            {
                                Console.WriteLine("name:" + player.Name);
                                Console.WriteLine("steamid:" + player.SteamID);
                                Console.WriteLine("player end");
                            }

                            Console.WriteLine("counter-t players");
                            //get players ct start info
                            foreach (var player in parser.PlayingParticipants.Where(a => a.Team == Team.CounterTerrorist))
                            {
                                Console.WriteLine("name:" + player.Name);
                                Console.WriteLine("steamid:" + player.SteamID);
                                Console.WriteLine("player end");
                            }
                            Console.WriteLine("end");
                        };

                        //Parse to end to get all data.
                        parser.ParseToEnd();

                        Console.WriteLine("tscore:" + t_score);
                        Console.WriteLine("ctscore:" + ct_score);
                    }
                    catch(ArgumentOutOfRangeException e){
                        Console.WriteLine("error");
                    }
                }
                else
                {
                    Console.WriteLine("File not found");
                }
            }else
            {
                Console.WriteLine("no args found");
            }
        }
    }
}
