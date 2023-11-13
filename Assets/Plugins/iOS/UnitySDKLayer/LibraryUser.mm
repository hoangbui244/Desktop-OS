//
//  LibraryUser.mm
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/20.
//

#import <Foundation/Foundation.h>
#import "../ChannelPlugin/PluginWrapper.h"

//export c language interface to unity3d
#if defined(__cplusplus)
extern "C"{
#endif
    //初始化(监听回调的GameObject Name)
    extern void init(const char * listener)
    {
        [PluginWrapper init:[NSString stringWithUTF8String:listener]];
    }
    //设置Debug模式
    extern void setDebugLog(bool enable)
    {
        [PluginWrapper setDebugMode:enable];
    }
    //获取当前渠道
    extern const char * get_channel()
    {
        NSString* result = [PluginWrapper getChannel];
        return strdup([result UTF8String]);

    }
    //登陆
    extern void login()
    {
        [PluginWrapper login];
    }
    //是否已经登录
    extern bool is_logined()
    {
        return [PluginWrapper is_logined];
    }
    //登录验证(服务器ID, 登录验证地址)
    extern void auth_login(int serverID, const char * authLoginURL)
    {
        [PluginWrapper auth_login:serverID authLoginURL:[NSString stringWithUTF8String:authLoginURL]];
    }
    //登出
    extern void logout()
    {
        [PluginWrapper logout];
    }
    //切换账号
    extern void switch_account()
    {
        [PluginWrapper switch_account];
    }
    //进入游戏(服务器ID, 服务器名, 用户ID, 角色ID, 角色名, 角色等级, 角色VIP等级, 是否为新建角色)
    extern void enter_game(int serverID, const char * serverName, const char * userID, const char * roleID,
                           const char * roleName, int roleLevel, int roleVipLevel, bool isCreate)
    {
        [PluginWrapper enter_game:serverID
                           serverName:[NSString stringWithUTF8String:serverName]
                           userID:[NSString stringWithUTF8String:userID]
                           roleID:[NSString stringWithUTF8String:roleID]
                           roleName:[NSString stringWithUTF8String:roleName]
                           roleLevel:roleLevel roleVipLevel:roleVipLevel
                           isCreate:isCreate];
    }
    //更新角色信息
    extern void update_role_info(const char * roleName, int roleLevel, int roleVipLevel)
    {
        [PluginWrapper update_role_info:[NSString stringWithUTF8String:roleName] roleLevel:roleLevel roleVipLevel:roleVipLevel];
    }
    //退出游戏
    extern void exit_game()
    {
        [PluginWrapper exit_game];
    }
    //用户中心
    extern void user_platform()
    {
        [PluginWrapper user_platform];
    }
    //客服中心(扩展字段)
    extern void customer_service(const char * ext)
    {
        [PluginWrapper customer_service:[NSString stringWithUTF8String:ext]];
    }
    //显示悬浮窗(悬浮窗位置)
    extern void show_float_view(int location)
    {
        [PluginWrapper show_float_view:location];
    }
    //隐藏悬浮窗
    extern void hide_float_view()
    {
        [PluginWrapper hide_float_view];
    }
    
#if defined(__cplusplus)
}
#endif
