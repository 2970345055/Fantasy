﻿using System;
using Fantasy;
using Fantasy.Core;
using Fantasy.Helper;
using Fantasy.Core.Network;

namespace BestGame
{
    /// 检查角色性别，名字等
    /// 网关帐号有效，可创建角色数量
    /// 创建角色，存库
    public class H_G2C_RoleCreateHandler : MessageRPC<C2G_RoleCreateRequest, G2C_RoleCreateResponse>
    {
        protected override async FTask Run(Session session, C2G_RoleCreateRequest request, G2C_RoleCreateResponse response,
            Action reply)
        {
            response.ErrorCode = await Check(session, request, response);

            await FTask.CompletedTask;
        }

        private async FTask<uint> Check(Session session, C2G_RoleCreateRequest request, G2C_RoleCreateResponse response)
        {
            var name = request.NickName?.Trim() ?? "";
            if (string.IsNullOrEmpty(name))
                return ErrorCode.H_C2G_RoleCreate_Handler_NickNameNull;
            
            var err = name.IsNameValid();
            if (err != ErrorCode.Success)
                return err;

            // 性别无效
            if (request.Sex != 1 && request.Sex != 2)
                return ErrorCode.RoleCreate_SexInvalid;
            
            var sessionPlayer = session.GetComponent<SessionPlayerComponent>();
            var gateAccountManager = session.Scene.GetComponent<GateAccountManager>();

            long accountId = sessionPlayer.playerId;
            
            try
            {
                // 锁住账号和名字
                var _LockGateAccountLock = new CoroutineLockQueueType("LockGateAccountLock");
                using (await _LockGateAccountLock.Lock(accountId))
                {
                    // 没有账号信息
                    if (!gateAccountManager.TryGetValue(accountId, out GateAccount gateAccount))
                    {
                        return ErrorCode.Error_CreateRoleNotFindAccountInfo;
                    }
                    
                    // 检查是否超过单个玩家最大角色数量限制。
                    if ((await gateAccount.GetRoles()).Count >= ConstValue.CharacterCountLimit)
                    {
                        return ErrorCode.H_C2G_RoleCreate_Handler_ExceedLimit;
                    }

                    // 检查名字是否可用
                    var nameCheckComponent = session.Scene.GetComponent<NameCheckComponent>();
                    err = await nameCheckComponent.CanUse(name);

                    if (err != ErrorCode.Success)
                    {
                        return err;
                    }
                    
                    // 创建新角色
                    Role role = Entity.Create<Role>(session.Scene);
                    role.UnitConfigId = 12012; // 从unit配置表取得
                    role.AccountId = accountId;
                    role.Sex = request.Sex;
                    role.NickName = name;
                    role.CreatedTime = TimeHelper.Now;
                    role.Level = 1;
                    role.Experience = 0;
                    role.Class = ""; // 从unit配置表取得

                    // 创建一个新角色并保存到数据库中。
                    await gateAccount.CreateRole(role);

                    response.RoleInfo = role.ToProto();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"H_G2C_RoleCreateHandler Error \n{ex}");
                return ErrorCode.H_C2G_RoleCreate_Handler_UnknownError;
            }

            return ErrorCode.Success;
        }
    }
}