using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using TaoBaoAPIHelper;
using CusServiceAchievements.DAL;
using LogHelper;

namespace GetTalkRecods
{
    public class TalkRecod
    {
        public void GetTalkRecordContent()
        {

            IList<TopNickSessionInfo> list = new NickSessionService().GetAllNickSession(Enum.TopTaoBaoService.Temporary);
            TalkRecodService trDal = new TalkRecodService();
            SubUserService userDal = new SubUserService();
            foreach (TopNickSessionInfo info in list)
            {
                DateTime now = trDal.GetMaxTime(info.Nick);

                ServiceLog.RecodeLog(info.Nick + "开始获取时间" + now);

                trDal.CreateTable(DBHelp.DataHelper.Encrypt(info.Nick));
                //List<GroupMember> memberList = TaoBaoAPI.GetNickGroupList(info.Nick, info.Session);

                List<SubUserInfo> hasuserList = userDal.GetAllChildNick(info.Nick);
                List<string> childNicks = new List<string>();

                //直接获取子帐号列表
                //if (memberList.Count == 0)
                //{
                IList<SubUserInfo> userList = TaoBaoAPI.GetChildNick(info.Nick, info.Session);

                foreach (SubUserInfo uinfo in userList)
                {
                    childNicks.Add(uinfo.nick);
                    if (hasuserList.Where(o => o.nick == uinfo.nick).ToList().Count == 0)
                        userDal.InsertSubUserInfo(uinfo);
                }
                //}
                //else
                //{
                //    foreach (GroupMember minfo in memberList)
                //    {
                //        foreach (string cnick in minfo.MemberIdList)
                //        {
                //            if (childNicks.Contains(cnick))
                //                continue;
                //            childNicks.Add(cnick);
                //        }
                //    }
                //}

                //foreach (GroupMember minfo in memberList)
                //{

                DateTime rnow = DateTime.Now;

                List<TalkContent> allcontent = trDal.GetAllContent(now.AddHours(-16), now, info.Nick);

                foreach (string fromNick in childNicks)
                {
                    List<TalkObj> objList = TaoBaoAPI.GetTalkObjList(fromNick.Replace("cntaobao", ""), info.Session, now, rnow);
                    foreach (TalkObj obj in objList)
                    {
                        List<TalkContent> contents = TaoBaoAPI.GetTalkContentNow(info.Session, fromNick.Replace("cntaobao", ""), obj.uid.Replace("cntaobao", ""), now, rnow);

                        for (int i = 0; i < contents.Count; i++)
                        {
                            contents[i].FromNick = fromNick.Replace("cntaobao", "");
                            contents[i].ToNick = obj.uid.Replace("cntaobao", "");
                            if (allcontent.Contains(contents[i]))
                            {
                                continue;
                            }
                            trDal.InsertContent(contents[i], info.Nick);
                        }
                    }
                }
                //}
            }
        }
    }
}