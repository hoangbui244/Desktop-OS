//
//  PluginAppController.m
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/20.
//

#import <Foundation/Foundation.h>
#import "PluginAppController.h"

@implementation PluginAppController

+ (BOOL)application:(UIApplication * _Nonnull)application didFinishLaunchingWithOptions:(nullable NSDictionary *)launchOptions controller:(UnityAppController * _Nonnull)controller
{
    NSLog(@"[PluginAppController application:%@ didFinishLaunchingWithOptions:%@]", application, launchOptions);
    return YES;
}

+ (void)application:(UIApplication * _Nonnull)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData * _Nullable)deviceToken
{

}

+ (BOOL)application:(UIApplication * _Nonnull)application openURL:(NSURL * _Nullable)url sourceApplication:(nullable NSString *)sourceApplication annotation_Nullable:(id _Nullable)annotation
{
    return YES;
}

+ (BOOL)application:(UIApplication * _Nonnull)app openURL:(NSURL * _Nullable)url options:(NSDictionary<UIApplicationOpenURLOptionsKey,id> * _Nullable)options
{
    return YES;
}

@end
