#nullable enable
using System;
using System.Text;
using Collections.Pooled;
using EvoSC.Utility.Remotes.Structs;
using U8Xml;
using UnsafeGbxConnector.Serialization;
using UnsafeGbxConnector.Serialization.Readers;

namespace EvoSC.Modules.Remotes.UnsafeGbxConnector.Impl
{
    public partial class UnsafeLowLevelGbxRemote
    {
        private static void pretty(StringBuilder sb, XmlNode node, int i = 0)
        {
            if (node.IsNull)
                return;

            sb.Append("\n");
            try
            {
                for (var l = 0; l < i; l++)
                    sb.Append("  ");
                sb.Append($"<{node.Name}>");
            }
            catch
            {
                for (var l = 0; l < i; l++)
                    sb.Append("  ");
                sb.Append("<???>");
            }

            if (node.HasChildren)
            {
                foreach (var child in node.Children)
                {
                    pretty(sb, child, i + 1);
                }
                
                sb.Append("\n");
                for (var l = 0; l < i; l++)
                    sb.Append("  ");
                sb.Append($"</{node.Name}>");
            }
            else
            {
                sb.Append(node.InnerText);
                sb.Append($"</{node.Name}>");
            }
        }

        private static void pretty(XmlNode node)
        {
            var sb = new StringBuilder();
            pretty(sb, node);
            Console.WriteLine("xml: " + sb.ToString());
        }

        private static void pretty(GbxReader reader)
        {
            var parent = reader.Root;
            while (!parent.IsNull && parent.Parent.HasValue)
            {
                parent = parent.Parent.Value;
            }

            var sb = new StringBuilder();
            pretty(sb, parent);
            Console.WriteLine("xml: " + sb.ToString());
        }

        private static readonly Func<GbxResponse, bool> s_BooleanResult = gbx =>
        {
            if (gbx.IsError || gbx.Reader.Root.IsNull)
                return false;
            
            //pretty(gbx.Reader);

            return new GbxMemberReader(gbx.Reader.Root).ReadBool();
        };

        private static readonly Func<GbxResponse, GbxPlayerInfo?> s_GetPlayerInfoResult = gbx =>
        {
            if (gbx.IsError)
                return default;

            var gbxStruct = gbx.Reader[0].ReadStruct();
            return GetGbxPlayerInfo(gbxStruct);
        };

        private static readonly Func<GbxResponse, GbxPlayerInfo[]?> s_GetPlayerInfoArrayResult = gbx =>
        {
            if (gbx.IsError)
                return default;

            using var playerList = new PooledList<GbxPlayerInfo>();
            
            var gbxArray = gbx.Reader[0].ReadArray();
            for (var i = 0; gbxArray.TryReadAt(out var element, i); i++)
            {
                playerList.Add(GetGbxPlayerInfo(element.AsStruct()));
            }

            return playerList.ToArray();
        };

        private static GbxPlayerInfo GetGbxPlayerInfo(GbxStructReader gbxStruct)
        {
            GbxPlayerInfo info = default;
            if (gbxStruct.TryGet(nameof(info.Login), out var loginMember))
                info.Login = loginMember.ReadString();
            if (gbxStruct.TryGet(nameof(info.NickName), out var nickNameMember))
                info.NickName = nickNameMember.ReadString();
            if (gbxStruct.TryGet(nameof(info.PlayerId), out var playerIdMember))
                info.PlayerId = playerIdMember.ReadInt();
            if (gbxStruct.TryGet(nameof(info.TeamId), out var teamIdMember))
                info.TeamId = teamIdMember.ReadInt();
            if (gbxStruct.TryGet(nameof(info.LadderRanking), out var ladderRankingMember))
                info.LadderRanking = ladderRankingMember.ReadInt();
            if (gbxStruct.TryGet(nameof(info.Flags), out var flagsMember))
                info.Flags = flagsMember.ReadInt();

            return info;
        }
    }
}