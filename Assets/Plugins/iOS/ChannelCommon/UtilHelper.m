//
//  UtilHelper.m
//  Unity-iPhone
//
//  Created by 张聪 on 2017/11/3.
//

#import <Foundation/Foundation.h>
#import "UtilHelper.h"

@implementation UtilHelper

+ (NSMutableDictionary * _Nonnull)parse_func_param:(NSString * _Nonnull)param
{
    NSLog(@"UtilHelper parse_func_param param: %@", param);
    NSArray *paramSplit = [param componentsSeparatedByString:@"&"];
    NSMutableDictionary *paramMap = [NSMutableDictionary dictionary];
    
    for (int index = 0 ; index < paramSplit.count; index++)
    {
        NSArray *kvParam = [paramSplit[index] componentsSeparatedByString:@"="];
        if (2 <= kvParam.count)
        {
            [paramMap setObject:kvParam[1] forKey:kvParam[0]];
        }
        else
        {
            NSLog(@"UtilHelper parse_func_param invalid key-value param: %@", paramSplit[index]);
        }
    }
    NSLog(@"UtilHelper parse_func_param result: %@", paramMap);
    return paramMap;
}

@end
