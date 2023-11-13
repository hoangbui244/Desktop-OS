//
//  AuthLoginHelper.m
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/23.
//

#import <Foundation/Foundation.h>
#import "GameCallback.h"
#import "AuthLoginHelper.h"

@implementation AuthLoginHelper

+ (AuthLoginHelper *) shareAuthLoginHelper
{
    static AuthLoginHelper* helper = nil;
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        helper = [[AuthLoginHelper alloc] init];
    });
    return helper;
}

+ (void)request_auth_login:(NSString *)url param:(NSData *)body
{
    NSLog(@"auth login helper send request url: %@", url);
    NSLog(@"auth login helper send request body: %@", [[NSString alloc] initWithData:body encoding:NSUTF8StringEncoding]);
    
    url = [url stringByAddingPercentEscapesUsingEncoding:NSUTF8StringEncoding];
    NSURL* netURL = [NSURL URLWithString:url];
    NSMutableURLRequest* request = [NSMutableURLRequest requestWithURL:netURL cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:5];
    [request setHTTPMethod:@"POST"];
    [request setHTTPBody:body];
    
    NSURLConnection* conn = [[NSURLConnection alloc] initWithRequest:request delegate:[AuthLoginHelper shareAuthLoginHelper]];
    [conn start];
}

- (void) connection:(NSURLConnection*)connection didReceiveResponse:(nonnull NSURLResponse *)response
{
    self.responseData = [NSMutableData data];
}

- (void) connection:(NSURLConnection*)connection didReceiveData:(nonnull NSData *)data
{
    [self.responseData appendData:data];
}

- (void) connectionDidFinishLoading:(NSURLConnection*)connection
{
    NSString* data = [[NSString alloc] initWithData:self.responseData encoding:NSUTF8StringEncoding];
    NSLog(@"connectionDidFinishLoading %@", data);
    
    NSError* error = nil;
    NSDictionary* jsonData = [NSJSONSerialization JSONObjectWithData:[data dataUsingEncoding:NSUTF8StringEncoding]
                                                             options:NSJSONReadingMutableContainers error:&error];
    if (!jsonData || error)
    {
        NSLog(@"auth login helper parse http reponse: %@, error: %@", data, error);
        [[NSNotificationCenter defaultCenter] postNotificationName:GAME_AUTH_LOGIN_FAILED_NOTIFICATION object:nil];
    }
    else
    {
        [[NSNotificationCenter defaultCenter] postNotificationName:GAME_AUTH_LOGIN_SUCCESS_NOTIFICATION object:nil userInfo:jsonData];
    }
}

- (void) connection:(NSURLConnection*)connection didFailWithError:(nonnull NSError *)error
{
    NSLog(@"auth login helper net error: %@", [error localizedDescription]);
    [[NSNotificationCenter defaultCenter] postNotificationName:GAME_AUTH_LOGIN_FAILED_NOTIFICATION object:nil];
}

@end
