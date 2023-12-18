﻿using Mod.ModHelper.CommandMod.Chat;
using Mod.ModHelper.CommandMod.Hotkey;
using Mod.ModHelper.Menu;
using System.Collections.Generic;
using System.Linq;

namespace Mod.Xmap
{
    public class Pk9rXmap
    {
        public static bool isMapTransAsXmap = false;
        public static bool isShowPanelMapTrans = true;
        public static bool isUseCapsuleNormal = false;
        public static bool isUseCapsuleVip = true;

        [ChatCommand("xcsdb")]
        public static void toggleUseCapsuleVip()
        {
            isUseCapsuleVip = !isUseCapsuleVip;
            GameScr.info1.addInfo("Sử dụng capsule đặc biệt Xmap: " + (isUseCapsuleVip ? "Bật" : "Tắt"), 0);
        }

        [ChatCommand("xcsb")]
        public static void toggleUseCapsuleNormal()
        {
            isUseCapsuleNormal = !isUseCapsuleNormal;
            GameScr.info1.addInfo("Sử dụng capsule thường Xmap: " + (isUseCapsuleNormal ? "Bật" : "Tắt"), 0);
        }

        [ChatCommand("xmp"), HotkeyCommand('x')]
        public static void showXmapMenu()
        {
            if (XmapController.gI.IsActing)
            {
                LogMod.writeLine("[xmap][info] Người chơi yêu cầu hủy xmap");

                XmapController.finishXmap();
                GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                return;
            }

            XmapData.loadGroupMapsFromFile("TextData\\GroupMapsXmap.txt");

            string[] popupText = [
                "XmapNRO by Phucprotein",
                $"{LangHelper.GetString("CurrentMap")} {TileMap.mapName}, ID: {TileMap.mapID}",
                LangHelper.GetString("SelectDestination")
            ];

            new MenuBuilder()
                .setChatPopup(string.Join("\n", popupText))
                .map(XmapData.groups, groupMap =>
                {
                    return new(groupMap.nameGroup, new(() =>
                    {
                        showXmapPanel(groupMap.maps);
                    }));
                })
                .start();

            //OpenMenu.start(new(menuItems =>
            //{
            //    foreach (var groupMap in XmapData.groups)
            //        menuItems.Add(new(groupMap.nameGroup, new(() =>
            //        {
            //            showXmapPanel(groupMap.maps);
            //        })));
            //}), $"XmapNRO by Phucprotein\nMap hiện tại: {TileMap.mapName}, ID: {TileMap.mapID}\nVui lòng chọn nơi muốn đến");
        }

        public static void showXmapPanel(List<int> maps)
        {
            isMapTransAsXmap = true;
            int len = maps.Count;
            GameCanvas.panel.mapNames = new string[len];
            GameCanvas.panel.planetNames = new string[len];
            for (int i = 0; i < len; i++)
            {
                var mapId = maps[i];
                var nameMap = TileMap.mapNames[maps[i]];
                GameCanvas.panel.mapNames[i] = $"{mapId}: {nameMap}";
                GameCanvas.panel.planetNames[i] = "Xmap by Phucprotein";
            }
            GameCanvas.panel.setTypeMapTrans();
            GameCanvas.panel.show();
        }

        public static void Info(string text)
        {
            if (XmapController.gI.IsActing)
            {
                var keywords = new List<string>
                {
                    "Bạn chưa thể đến khu vực này",
                    "Bang hội phải có từ 5 thành viên mới được tham gia",
                    "Chỉ tiếp các bang hội, miễn tiếp khách vãng lai",
                    "Gia nhập bang hội trên 2 ngày mới được tham gia",
                };

                if (keywords.Contains(text))
                {
                    XmapController.finishXmap();
                    GameScr.info1.addInfo("Đã huỷ Xmap", 0);
                }
                else if (text.Equals("Có lỗi xảy ra vui lòng thử lại sau."))
                {
                    Utilities.teleportMyChar(XmapUtils.getX(2), XmapUtils.getY(2));
                }
            }
        }

        public static void selectMapTrans(int selected)
        {
            if (isMapTransAsXmap)
            {
                InfoDlg.hide();
                string mapName = GameCanvas.panel.mapNames[selected];
                int mapId = getMapIdFromPanelXmap(mapName);
                XmapController.start(mapId);
                return;
            }
            Utilities.mapCapsuleReturn = TileMap.mapID;
            Service.gI().requestMapSelect(selected);
        }

        public static void showPanelMapTrans()
        {
            isMapTransAsXmap = false;
            if (isShowPanelMapTrans)
            {
                GameCanvas.panel.setTypeMapTrans();
                GameCanvas.panel.show();
                return;
            }
            isShowPanelMapTrans = true;
        }

        public static void fixBlackScreen()
        {
            Controller.gI().loadCurrMap(0);
            Service.gI().finishLoadMap();
            Char.isLoadingMap = false;
        }

        public static bool canUseCapsuleNormal()
        {
            return isUseCapsuleNormal && !Utilities.isMyCharDied() && Utilities.hasItemCapsuleNormal();
        }

        public static bool canUseCapsuleVip()
        {
            return isUseCapsuleVip && !Utilities.isMyCharDied() && Utilities.hasItemCapsuleVip();
        }

        public static int getMapIdFromPanelXmap(string mapName)
        {
            return int.Parse(mapName.Split(':')[0]);
        }

        public static bool isWaitInfoMapTrans()
        {
            return !isShowPanelMapTrans;
        }

        public static void nextMap(MapNext mapNext)
        {
            switch (mapNext.type)
            {
                case TypeMapNext.AutoWaypoint:
                    nextMapAutoWaypoint(mapNext);
                    break;
                case TypeMapNext.NpcMenu:
                    nextMapNpcMenu(mapNext);
                    break;
                case TypeMapNext.NpcPanel:
                    nextMapNpcPanel(mapNext);
                    break;
                case TypeMapNext.Position:
                    nextMapPosition(mapNext);
                    break;
                case TypeMapNext.Capsule:
                    nextMapCapsule(mapNext);
                    break;
            }
        }

        public static void nextMapAutoWaypoint(MapNext mapNext)
        {
            var waypoint = Utilities.findWaypoint(mapNext.to);
            Utilities.changeMap(waypoint);
        }

        public static void nextMapNpcMenu(MapNext mapNext)
        {
            var npcId = mapNext.info[0];
            if (npcId == 38)
            {
                var flag = false;
                int vNpcSize = GameScr.vNpc.size();
                for (int i = 0; i < vNpcSize; i++)
                {
                    var npc = (Npc)GameScr.vNpc.elementAt(i);
                    if (npc.template.npcTemplateId == npcId)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    Waypoint waypoint;
                    if (TileMap.mapID == 27 || TileMap.mapID == 29)
                        waypoint = Utilities.findWaypoint(28);
                    else
                    {
                        if (Utilities.random.Next(27, 29) == 27)
                            waypoint = Utilities.findWaypoint(27);
                        else
                            waypoint = Utilities.findWaypoint(29);
                    }

                    Utilities.changeMap(waypoint);
                    return;
                }
            }
            Service.gI().openMenu(npcId);
            for (int i = 1; i < mapNext.info.Length; i++)
            {
                int select = mapNext.info[i];
                Service.gI().confirmMenu((short)npcId, (sbyte)select);
            }
        }

        public static void nextMapNpcPanel(MapNext mapNext)
        {
            var idNpc = mapNext.info[0];
            var selectMenu = mapNext.info[1];
            var selectPanel = mapNext.info[2];
            Service.gI().openMenu(idNpc);
            Service.gI().confirmMenu((short)idNpc, (sbyte)selectMenu);
            Service.gI().requestMapSelect(selectPanel);
        }

        public static void nextMapPosition(MapNext mapNext)
        {
            var xPos = mapNext.info[0];
            var yPos = mapNext.info[1];
            Utilities.teleportMyChar(xPos, yPos);
            Service.gI().requestChangeMap();
            Service.gI().getMapOffline();
        }

        public static void nextMapCapsule(MapNext mapNext)
        {
            Utilities.mapCapsuleReturn = TileMap.mapID;
            var select = mapNext.info[0];
            Service.gI().requestMapSelect(select);
        }
    }
}
