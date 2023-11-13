//
//  PluginWrapper.m
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/20.
//

#import <Foundation/Foundation.h>
#import "PluginWrapper.h"
#import "../ChannelCommon/GameCallback.h"
#import "../ChannelCommon/AuthLoginHelper.h"
#import "../ChannelCommon/UtilHelper.h"

@implementation AuthInfo

+ (AuthInfo *) shareAuthInfo
{
    static AuthInfo* info = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        info = [[AuthInfo alloc] init];
    });
    return info;
}

@end

@implementation PluginWrapper

static bool isInit = false;
static bool isLogin = false;
static bool isSwitchAccount = false;

+ (void)init:(NSString * _Nonnull)listener
{
    if (isInit)
    {
        NSLog(@"[PluginWrapper init repeated called]");
        return;
    }
    NSLog(@"[PluginWrapper init listener: %@]", listener);
    
    [GameAdapter set_unity_listener:listener];
    [self register_notification];
}

+ (void)setDebugMode:(bool)debug
{
    
}

+(NSString*_Nonnull)getChannel
{
    NSDictionary *dict = [[NSBundle mainBundle] infoDictionary];
    NSString *sdkid = [dict objectForKey:@"com.cong.channel"];
    return sdkid;
}

+ (void)login
{
    if (isLogin)
    {
        NSLog(@"[PluginWrapper login repeated called]");
        return;
    }
    NSLog(@"[PluginWrapper login called]");
}

+ (bool)is_logined
{
    return isLogin;
}

+ (void)auth_login:(int)serverID authLoginURL:(NSString * _Nonnull)authLoginURL
{
    if (false == isLogin)
    {
        NSLog(@"[PluginWrapper auth_login need login]");
        return;
    }
    NSLog(@"[PluginWrapper auth_login serverID: %d, authLoginURL: %@]", serverID, authLoginURL);
}

+ (void)logout
{
    if (false == isLogin)
    {
        NSLog(@"[PluginWrapper logout need login]");
        return;
    }
    NSLog(@"[PluginWrapper logout called]");
}

+ (void)switch_account
{
    NSLog(@"[PluginWrapper switch_account called]");
    isSwitchAccount = true;
    if (isLogin)
    {
        [self logout];
    }
    else
    {
        [self login];
    }
}

+ (void)enter_game:(int)serverID serverName:(NSString * _Nullable)serverName userID:(NSString * _Nonnull)userID roleID:(NSString * _Nonnull)roleID roleName:(NSString * _Nonnull)roleName roleLevel:(int)roleLevel roleVipLevel:(int)roleVipLevel isCreate:(bool)isCreate
{
    NSLog(@"[PluginWrapper enter_game serverID: %d, serverName: %@, userID: %@, roleID: %@, roleName: %@, roleLevel: %d, roleVipLevel: %d, isCreate: %d]", serverID, serverName, userID, roleID, roleName, roleLevel, roleVipLevel, isCreate);;
    
    [GameInfo shareGameInfo].ServerID = serverID;
    [GameInfo shareGameInfo].ServerName = serverName;
    [GameInfo shareGameInfo].UserID = userID;
    [GameInfo shareGameInfo].RoleID = roleID;
    [GameInfo shareGameInfo].RoleName = roleName;
    [GameInfo shareGameInfo].RoleLevel = roleLevel;
    [GameInfo shareGameInfo].RoleVipLevel = roleVipLevel;
}

+ (void)update_role_info:(NSString * _Nonnull)roleName roleLevel:(int)roleLevel roleVipLevel:(int)roleVipLevel
{
    [GameInfo shareGameInfo].RoleName = roleName;
    [GameInfo shareGameInfo].RoleLevel = roleLevel;
    [GameInfo shareGameInfo].RoleVipLevel = roleVipLevel;
}

+ (void)exit_game
{
    NSLog(@"[PluginWrapper exit_game called]");
}

+ (void)user_platform
{
    NSLog(@"[PluginWrapper user_platform called]");
}

+ (void)customer_service:(NSString * _Nullable)ext
{
    NSLog(@"[PluginWrapper customer_service ext: %@]", ext);
}

+ (void)show_float_view:(int)location
{
    NSLog(@"[PluginWrapper show_float_view location: %d]", location);
}

+ (void)hide_float_view
{
    NSLog(@"[PluginWrapper hide_float_view called]");
}

+ (void)pay:(NSString * _Nonnull)productID productName:(NSString * _Nullable)productName productDesc:(NSString * _Nullable)productDesc price:(float)price ext:(NSString * _Nullable)ext
{
    NSLog(@"[PluginWrapper pay productID: %@, productName: %@, productDesc: %@, price: %f, ext: %@]", productID, productName, productDesc, price, ext);
}

+ (void)track_event:(NSString * _Nonnull)eventType eventName:(NSString * _Nonnull)eventName eventParam:(NSString * _Nullable)eventParam
{
    
}

+ (void)callFuncWithParam:(NSString * _Nonnull)method param:(NSString * _Nonnull)param
{
    
}

+ (int)callIntFuncWithParam:(NSString * _Nonnull)method param:(NSString * _Nonnull)param
{
    return 0;
}

+ (float)callFloatFuncWithParam:(NSString * _Nonnull)method param:(NSString * _Nonnull)param
{
    return 0.0f;
}

+ (bool)callBoolFuncWithParam:(NSString * _Nonnull)method param:(NSString * _Nonnull)param
{
    return false;
}

+ (NSString * _Nonnull)callStringFuncWithParam:(NSString * _Nonnull)method param:(NSString * _Nonnull)param
{
    return @"";
}

//Facebook interface
+ (void) auth_social_account
{

}

+ (void) share:(int)shareType shareURL:(NSString* _Nonnull)shareURL sharePictureURL:(NSString* _Nonnull)sharePictureURL shareTitle:(NSString* _Nonnull)shareTitle shareContent:(NSString* _Nonnull)shareContent
{

}

+ (void) share_local_picture:(int)shareType picturePath:(NSString* _Nonnull)picturePath
{

}

+ (void) request_invitable_friends
{

}

+ (NSString * _Nonnull) get_invitable_friend_info:(int)index
{
    return @"";
}

+ (void) request_playing_friends
{

}

+ (NSString * _Nonnull) get_playing_friend_info:(int)index
{
    return @"";
}


+ (void)register_notification
{

}

@end
