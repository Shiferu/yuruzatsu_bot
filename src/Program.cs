using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using YamlDotNet.RepresentationModel;

namespace auto_roll_controller
{
    class Program
    {
        static void Main(string[] args)
        {
            Bot bot = new Bot();
            bot.keep().GetAwaiter().GetResult();

        }
    }
    class Bot
    {
        DiscordSocketClient client;

        ulong annotation;

        ulong male;

        ulong female;

        ulong lgbtq;

        public Bot()
        {
            client = new DiscordSocketClient();

            client.Log += log;

            var yaml = new YamlStream();
            yaml.Load(new StreamReader("data/data.yml", System.Text.Encoding.UTF8));

            var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;

            var token = (YamlScalarNode)mapping.Children[new YamlScalarNode("token")];

            var male_ = (YamlScalarNode)mapping.Children[new YamlScalarNode("male_profile")];

            var female_ = (YamlScalarNode)mapping.Children[new YamlScalarNode("female_profile")];

            var lgbtq_ = (YamlScalarNode)mapping.Children[new YamlScalarNode("LGBTQ_profile")];

            var annotation_ = (YamlScalarNode)mapping.Children[new YamlScalarNode("annotation")];

            male = ulong.Parse(male_.Value);

            female = ulong.Parse(female_.Value);

            lgbtq = ulong.Parse(lgbtq_.Value);

            annotation = ulong.Parse(annotation_.Value);

            client.LoginAsync(TokenType.Bot, token.Value);

            client.MessageReceived += onMessage;

            client.UserJoined += join;
        }
        public async Task join(SocketGuildUser user)
        {
            await user.SendMessageAsync("ようこそ多目的雑談サーバー【ゆる雑】へ\n" +

                                        "ご参加いただきありがとうございます！！\n" +
                                        "まず必読・ルールを読んでいただき、プロフィールの記入よろしくお願いします\n" +
                                        "\n" +
                                        "ここでは雑談、作業、ゲーム、カラオケ、お絵描き様々なコンテンツを揃えております\n" +
                                        "\n" +
                                        "どうぞ楽しんでいってくださいね\n" +
                                        "よろしくおねがいします");
        }
        public Task log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
        public async Task keep()
        {
            await client.StartAsync();
            await Task.Delay(-1);
        }
        public async Task onMessage(SocketMessage message)
        {
            if(message.Author.IsBot == true)
            {
                return ;
            }
            var guild = client.GetGuild(558484910256422912);
            var male_roll = guild.Roles.FirstOrDefault(roles => roles.Name == "男性");
            var female_roll = guild.Roles.FirstOrDefault(roles => roles.Name == "女性");
            var lgbtq_roll = guild.Roles.FirstOrDefault(roles => roles.Name == "LGBTQ");

            var user = message.Author as SocketGuildUser;

            if (user.Roles.Contains(male_roll) || user.Roles.Contains(female_roll) || user.Roles.Contains(lgbtq_roll))
            {
                return;
            }

            if (message.Channel.Id == male)
            {
                await user.AddRoleAsync(male_roll);

                var annotation_ = client.GetChannel(annotation) as SocketTextChannel;
                await annotation_.SendMessageAsync(message.Author.Mention + "さんに" + male_roll.Name + "を付与しました。");

                var text = client.GetChannel(male) as SocketTextChannel;
                await text.SendMessageAsync(
                    "※あくまでテンプレです。参考までにどうぞ。\n" +
                    "\n" +
                    "\n" +
                    "【名前】\n" +
                    "【性別】\n" +
                    "【年齢】\n" +
                    "【住み】\n" +
                    "【時間】\n" +
                    "【趣味】\n" +
                    "【一言】\n"
                );
            }

            if (message.Channel.Id == female)
            {
                await user.AddRoleAsync(female_roll);
                var annotation_ = client.GetChannel(annotation) as SocketTextChannel;
                await annotation_.SendMessageAsync(message.Author.Mention + "さんに" + female_roll.Name + "を付与しました。");
                var text = client.GetChannel(female) as SocketTextChannel;
                await text.SendMessageAsync(
                    "※あくまでテンプレです。参考までにどうぞ。\n" +
                    "\n" +
                    "\n" +
                    "【名前】\n" +
                    "【性別】\n" +
                    "【年齢】\n" +
                    "【住み】\n" +
                    "【時間】\n" +
                    "【趣味】\n" +
                    "【一言】\n"
                );
            }
            if (message.Channel.Id == lgbtq)
            {
                await user.AddRoleAsync(lgbtq_roll);
                var annotation_ = client.GetChannel(annotation) as SocketTextChannel;
                await annotation_.SendMessageAsync(message.Author.Mention + "さんに" + lgbtq_roll.Name + "を付与しました。");
                var text = client.GetChannel(lgbtq) as SocketTextChannel;
                await text.SendMessageAsync(
                    "※あくまでテンプレです。参考までにどうぞ。\n" +
                    "\n" +
                    "\n" +
                    "【名前】\n" +
                    "【性別】\n" +
                    "【年齢】\n" +
                    "【住み】\n" +
                    "【時間】\n" +
                    "【趣味】\n" +
                    "【一言】\n"
                );
            }
        }
    }
}