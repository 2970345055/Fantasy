using Fantasy;
using UnityEngine;

namespace BestGame;
public class EventSystemStruct
{
    public struct StartMove
    {
        public Unit Unit;
        public MoveInfo MoveInfo;
        public StartMove()
        {
            Unit = null;
            MoveInfo = null;
        }
    }

    public struct NoticeUnitAdd
    {
        public Unit Unit;
        public RoleInfo RoleInfo;
        public NoticeUnitAdd()
        {
            Unit = null;
            RoleInfo = null;
        }
    }

    public struct NoticeUnitRemove
    {
        public Unit Unit;
        public NoticeUnitRemove()
        {
            Unit = null;
        }
    }
}
