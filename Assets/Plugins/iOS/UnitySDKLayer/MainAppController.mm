//
//  MainAppController.mm
//  Unity-iPhone
//
//  Created by 张聪 on 2017/9/15.
//
//

#import "MainAppController.h"
#import "../ChannelPlugin/PluginAppController.h"

IMPL_APP_CONTROLLER_SUBCLASS(MainAppController)

@implementation MainAppController

- (BOOL) application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    NSLog(@"[MainAppController application:%@ didFinishLaunchingWithOptions:%@]", application, launchOptions);

    if ([super application:application didFinishLaunchingWithOptions:launchOptions])
    {
        return [PluginAppController application:application didFinishLaunchingWithOptions:launchOptions controller:self];
    }
    return NO;
}

- (void) application:(UIApplication*)application didRegisterForRemoteNotificationsWithDeviceToken:(nonnull NSData *)deviceToken
{
    NSLog(@"[MainAppController application:%@ didRegisterForRemoteNotificationsWithDeviceToken:%@]", application, deviceToken);
    
    [super application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
    
    [PluginAppController application:application didRegisterForRemoteNotificationsWithDeviceToken:deviceToken];
}

- (BOOL) application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation
{
    NSLog(@"[MainAppController application:%@ openURL:%@]", application, url);
    
    if ([super application:application openURL:url sourceApplication:sourceApplication annotation:annotation])
    {
        return [PluginAppController application:application openURL:url sourceApplication:sourceApplication annotation_Nullable:annotation];
    }
    return NO;
}

- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(NSDictionary<UIApplicationOpenURLOptionsKey, id> *)options
{
    NSLog(@"[MainAppController app:%@ openURL:%@]", app, url);
    
    if ([super application:app openURL:url options:options])
    {
        return [PluginAppController application:app openURL:url options:options];
    }
    return NO;
}

@end

