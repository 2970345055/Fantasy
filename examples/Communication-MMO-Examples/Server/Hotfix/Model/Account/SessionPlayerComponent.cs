using Fantasy;
namespace BestGame;
public class SessionPlayerComponentDestroySystem : DestroySystem<SessionPlayerComponent>
{
    protected override void Destroy(SessionPlayerComponent self)
    {
        self.playerId = 0;
    }
}
/// 这是一个在网关缓存玩家基本信息的组件，添加给客户端与网关的连接session
public class SessionPlayerComponent : Entity
{
    /// 玩家playerId
    public long playerId;

    /// 缓存角色网关账号
    public GateAccount gateAccount;
    /// 缓存session状态
    public SessionState EnterState;
}

public enum SessionState
{
    None = 0,
    Entering = 1, // 登陆中
    Enter = 2, // 进入游戏
}