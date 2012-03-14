using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using TaoBaoAPIHelper;

namespace GetTalkRecods
{
    public class TalkRecod
    {
        public void GetTalkRecordContent()
        {
            IList<TopNickSessionInfo> list = new CusServiceAchievements.DAL.NickSessionService().GetAllNickSession(Enum.TopTaoBaoService.KeFuJiXiao);

            foreach (TopNickSessionInfo info in list)
            {
                List<GroupMember> memberList = TaoBaoAPI.GetNickGroupList(info.Nick, info.Session);

                foreach (GroupMember minfo in memberList)
                {

                    foreach (string tonick in minfo.MemberIdList)
                    {

                    }
                }
            }
        }
    }
}
