//
//  GameCallback.mm
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/23.
//

#import <Foundation/Foundation.h>
#import "GameCallback.h"

@implementation GameInfo

@synthesize ServerID = _ServerID;
@synthesize ServerName = _ServerName;
@synthesize UserID = _UserID;
@synthesize RoleID = _RoleID;
@synthesize RoleName = _RoleName;
@synthesize RoleLevel = _RoleLevel;
@synthesize RoleVipLevel = _RoleVipLevel;

+ (GameInfo *) shareGameInfo
{
    static GameInfo* info = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        info = [[GameInfo alloc] init];
    });
    return info;
}

@end

@implementation GameAdapter

static NSString* UnityListener = @"";

+ (void) set_unity_listener:(NSString*)listener
{
    self:UnityListener = listener;
}

+ (void) send_untiy_message:(NSString*)method arg:(NSDictionary*)arg
{
    NSString* jsonString = nil;
    NSError* error;
    NSData* jsonData = [NSJSONSerialization dataWithJSONObject:arg options:NSJSONWritingPrettyPrinted error:&error];
    if (!jsonData)
    {
        NSLog(@"send to unity message method: %@, error: %@", method, error);
    }
    else
    {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
    }
    UnitySendMessage([UnityListener UTF8String], [method UTF8String], [jsonString UTF8String]);
}

@end
