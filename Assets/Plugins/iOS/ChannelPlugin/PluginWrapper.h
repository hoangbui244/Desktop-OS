//
//  PluginWrapper.h
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/20.
//
#ifndef PluginWrapper_h
#define PluginWrapper_h
@interface AuthInfo : NSObject
@property (copy, nonatomic) NSString * _Nonnull UserID;
@property (copy, nonatomic) NSString * _Nonnull Timestamp;
@property (copy, nonatomic) NSString * _Nonnull Sign;
+ (AuthInfo *_Nonnull)shareAuthInfo;
@end
@interface PluginWrapper : NSObject
//provide common interface
//user interface
+ (void) init:(NSString*_Nonnull)listener;
+ (void) setDebugMode:(bool)debug;
+ (NSString*_Nonnull) getChannel;
+ (void) login;
+ (bool) is_logined;
+ (void) auth_login:(int)serverID authLoginURL:(NSString*_Nonnull)authLoginURL;
+ (void) logout;
+ (void) switch_account;
+ (void) enter_game:(int)serverID serverName:(NSString*_Nullable)serverName userID:(NSString*_Nonnull)userID roleID:(NSString*_Nonnull)roleID roleName:(NSString*_Nonnull)roleName roleLevel:(int)roleLevel roleVipLevel:(int)roleVipLevel isCreate:(bool)isCreate;
+ (void) update_role_info:(NSString*_Nonnull)roleName roleLevel:(int)roleLevel roleVipLevel:(int)roleVipLevel;
+ (void) exit_game;
+ (void) user_platform;
+ (void) customer_service:(NSString*_Nullable)ext;
+ (void) show_float_view:(int)location;
+ (void) hide_float_view;
//iap interface
+ (void) pay:(NSString*_Nonnull)productID productName:(NSString*_Nullable)productName productDesc:(NSString*_Nullable)productDesc price:(float)price ext:(NSString*_Nullable)ext;
//analytics interface
+ (void) track_event:(NSString* _Nonnull)eventType eventName:(NSString* _Nonnull)eventName eventParam:(NSString *_Nullable)eventParam;
//extension interface
+ (void) callFuncWithParam:(NSString* _Nonnull)method param:(NSString* _Nonnull)param;
+ (int) callIntFuncWithParam:(NSString* _Nonnull)method param:(NSString* _Nonnull)param;
+ (float) callFloatFuncWithParam:(NSString* _Nonnull)method param:(NSString* _Nonnull)param;
+ (bool) callBoolFuncWithParam:(NSString* _Nonnull)method param:(NSString* _Nonnull)param;
+ (NSString*_Nonnull) callStringFuncWithParam:(NSString* _Nonnull)method param:(NSString* _Nonnull)param;
//Facebook interface
+ (void) auth_social_account;
+ (void) share:(int)shareType shareURL:(NSString* _Nonnull)shareURL sharePictureURL:(NSString* _Nonnull)sharePictureURL shareTitle:(NSString* _Nonnull)shareTitle shareContent:(NSString* _Nonnull)shareContent;
+ (void) share_local_picture:(int)shareType picturePath:(NSString* _Nonnull)picturePath;
+ (void) request_invitable_friends;
+ (NSString*_Nonnull) get_invitable_friend_info:(int)index;
+ (void) request_playing_friends;
+ (NSString*_Nonnull) get_playing_friend_info:(int)index;
//channel private interface
+ (void) register_notification;
@end
#endif /* PluginWrapper_h */
