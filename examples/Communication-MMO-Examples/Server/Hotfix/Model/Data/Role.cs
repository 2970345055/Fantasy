using MongoDB.Bson.Serialization.Attributes;
using Fantasy;
namespace BestGame;

/// <summary>
/// 玩家角色信息
/// </summary>
public class Role : Entity
{
    public int UnitConfigId;
    /// <summary>
    /// 账户ID
    /// </summary>
    public long AccountId;

    /// <summary>
    /// 等级
    /// </summary>
    public int Level;

    /// <summary>
    /// 创建时间
    /// </summary>
    public long CreatedTime;

    /// <summary>
    /// 最后一次进入角色时间
    /// </summary>
    public long LastEnterRoleTime;

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName;

    /// <summary>
    /// 1:男,2:女
    /// </summary>
    public int Sex;

    public long Experience;

    public string Class; // 职业

    public int LastMap;

    [BsonIgnore]
    public long sessionRuntimeId;

    // 角色状态
    [BsonIgnore]
    public RoleState State;
}

public enum RoleState
{
    None = 0,
    Online = 1, // 在线
    Stay = 2, // 挂机
}

public static class RoleSystem
{
    public static bool IsInMap(this Role role)
    {
        return role.State == RoleState.Online || role.State == RoleState.Stay;
    }

    public static RoleInfo ToProto(this Role role)
    {
        var proto = new RoleInfo
        {
            UnitConfigId = role.UnitConfigId,
            AccountId = role.AccountId,
            RoleId = role.Id,
            Sex = role.Sex,
            NickName = role.NickName,
            Level = role.Level,
            Experience = role.Experience,
            Class = role.Class,
            LastMap = role.LastMap,
        };

        return proto;
    }
}