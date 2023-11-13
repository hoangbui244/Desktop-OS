//
//  AuthLoginHelper.h
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/23.
//
#ifndef AuthLoginHelper_h
#define AuthLoginHelper_h
@interface AuthLoginHelper : NSObject <NSURLConnectionDataDelegate>
@property (nonatomic, strong) NSMutableData* responseData;
+ (void) request_auth_login:(NSString*)url param:(NSData*)body;
@end
#endif /* AuthLoginHelper_h */
