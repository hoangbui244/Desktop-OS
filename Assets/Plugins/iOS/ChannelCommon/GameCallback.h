//
//  GameCallback.h
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/23.
//
#ifndef GameCallback_h
#define GameCallback_h
typedef NS_ENUM(NSInteger, GameCallbackStatus) {
    GameCallbackStatus_Success,
    GameCallbackStatus_Failed,
    GameCallbackStatus_Cancel,
};
NSString* const GAME_CALLBACK_METHOD_INIT = @"InitCallback";
NSString* const GAME_CALLBACK_METHOD_LOGIN = @"LoginCallback";
NSString* const GAME_CALLBACK_METHOD_AUTHLOGIN = @"AuthLoginCallback";
NSString* const GAME_CALLBACK_METHOD_SWITCHACCOUNT = @"SwitchAccountCallback";
NSString* const GAME_CALLBACK_METHOD_LOGOUT = @"LogoutCallback";
NSString* const GAME_CALLBACK_METHOD_PAY = @"PayCallback";
NSString* const GAME_CALLBACK_METHOD_EXITGAME = @"ExitGameCallback";
NSString* const GAME_AUTH_LOGIN_SUCCESS_NOTIFICATION = @"AuthLoginSuccessNotification";
NSString* const GAME_AUTH_LOGIN_FAILED_NOTIFICATION = @"AuthLoginFailedNotification";
@interface GameInfo : NSObject
@property (nonatomic) int ServerID;
@property (copy, nonatomic) NSString *ServerName;
@property (copy, nonatomic) NSString *UserID;
@property (copy, nonatomic) NSString *RoleID;
@property (copy, nonatomic) NSString *RoleName;
@property (nonatomic) int RoleLevel;
@property (nonatomic) int RoleVipLevel;
+ (GameInfo *)shareGameInfo;
@end
@interface GameAdapter : NSObject
+ (void) set_unity_listener:(NSString*)listener;
+ (void) send_untiy_message:(NSString*)method arg:(NSDictionary*)arg;
@end
#endif /* GameCallback_h */
