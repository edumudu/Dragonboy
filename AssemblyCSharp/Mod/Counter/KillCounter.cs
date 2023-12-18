using Mod.PickMob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace Mod.Counter
{
    public class KillCounter
    {
        public const int INITIAL_COUNT = 0;
        private static uint killCount = INITIAL_COUNT;
        public static KillCounter _Instance = null;
        private static bool isEnabled = false;

        public static int x = 15 - 9;
        public static int y = 60;
        public static int distanceBetweenLines = 8;

        public void handleGameScrUpdate()
        {
            y = GetNewY(Boss.isEnabled, Boss.isCollapsed, Boss.listBosses.Count, Boss.distanceBetweenLines, Boss.MAX_BOSS_DISPLAY, Boss.y);
            y = GetNewY(ListCharsInMap.isEnabled, ListCharsInMap.isCollapsed, ListCharsInMap.listChars.Count, ListCharsInMap.distanceBetweenLines, ListCharsInMap.MAX_CHAR, ListCharsInMap.y);
        }

        public void handleMobStartDie(Mob mob)
        {
            if (mob.status == 1 || mob.status == 0 || !isEnabled) return;

            increaseCounter();
        }

        public static KillCounter getInstance()
        {
            _Instance ??= new KillCounter();

            return _Instance;
        }

        public void increaseCounter()
        {
            killCount += 1;
        }

        public static void resetCounter()
        {
            killCount = 0;
        }

        public static void setEnabledState(bool state)
        {
            isEnabled = state;

            if (!state) resetCounter();
        }

        public void Paint(mGraphics g)
        {
            if (!isEnabled) return;

            int titleWidth = 0;
            int scrollBarWidth = 0;
            int offsetX = scrollBarWidth;

            GUIStyle style = new GUIStyle(GUI.skin.label)
            {
                fontSize = 7 * mGraphics.zoomLevel,
                fontStyle = FontStyle.Bold,
                alignment = TextAnchor.UpperRight,
                richText = true
            };

            style.normal.textColor = Color.white;
            titleWidth = Utilities.getWidth(style, "Kill Count");
            g.setColor(new Color(.2f, .2f, .2f, .7f));
            g.fillRect(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8);

            if (GameCanvas.isMouseFocus(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - distanceBetweenLines, titleWidth, 8))
            {
                g.setColor(style.normal.textColor);
                g.fillRect(GameCanvas.w - (x + offsetX) - titleWidth + scrollBarWidth, y - 1, titleWidth - 1, 1);
            }

            g.drawString($"Kill Count: {killCount}", -(x + offsetX) + scrollBarWidth, y - distanceBetweenLines - 2, style);
        }

        private static int GetNewY(bool isEnabled, bool isCollapsed, int count, int distanceBetweenLines, int maxDisplayCount, int baseY)
        {
            if (!isEnabled) return y;

            int _y = y;
            int uncollapsedExpectedY = baseY + 5 + distanceBetweenLines * Mathf.Clamp(count, 0, maxDisplayCount) + 10;
            int collapsedExpectedY = baseY + distanceBetweenLines + 3;

            if (isCollapsed)
                _y = _y != collapsedExpectedY ? collapsedExpectedY : _y;
            else if (_y != uncollapsedExpectedY)
                _y = uncollapsedExpectedY;

            return _y;
        }
    }
}
