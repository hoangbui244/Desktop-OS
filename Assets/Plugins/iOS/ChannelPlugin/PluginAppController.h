//
//  PluginAppController.h
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/20.
//
#ifndef PluginAppController_h
#define PluginAppController_h
#import "UnityAppController.h"
@interface PluginAppController : NSObject 
+ (BOOL)application:(UIApplication *_Nonnull)application didFinishLaunchingWithOptions:(nullable NSDictionary *)launchOptions controller:(UnityAppController*_Nonnull)controller;
+ (void)application:(UIApplication *_Nonnull)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *_Nullable)deviceToken;
+ (BOOL)application:(UIApplication *_Nonnull)application openURL:(NSURL *_Nullable)url sourceApplication:(nullable NSString *)sourceApplication annotation_Nullable:(id _Nullable )annotation;
+ (BOOL)application:(UIApplication *_Nonnull)app openURL:(NSURL *_Nullable)url options:(NSDictionary<UIApplicationOpenURLOptionsKey, id> *_Nullable)options;
@end
#endif /* PluginAppController_h */
