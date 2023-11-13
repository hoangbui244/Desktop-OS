//
//  PluginSDK.mm
//  Unity-iPhone
//
//  Created by GAME on 16/5/26.
//
//

#import <Foundation/Foundation.h>
#import "PluginSDK.h"


@implementation PluginSDK

-(void)SetNumber:(int)num
{
    [UIApplication sharedApplication].applicationIconBadgeNumber = num;
}
-(void)SubIconBadgeNumber
{
    int iconBadgeNumber = [UIApplication sharedApplication].applicationIconBadgeNumber;
    iconBadgeNumber --;
    if (iconBadgeNumber < 0)
    {
        iconBadgeNumber = 0;
    }
    [UIApplication sharedApplication].applicationIconBadgeNumber = iconBadgeNumber;
}

@end

#if defined(__cplusplus)  
extern "C"{  
#endif  
    static PluginSDK * myPluginSDK;
	float doTestSelector(const char* info, int num)
	{
	    //UIViewController *vc = [[YourViewController alloc] initWithNibName:@"yourViewControllerName" //bundle:nil];
	    //[[UnityGetMainWindow() rootViewController] presentViewController:vc animated:YES completion:nil];
	    printf_console("-> doTestSelector()\n");
        // if (myPluginSDK == nil)
        // {
        //     myPluginSDK = [[PluginSDK alloc]init];
        // }
        // [myPluginSDK SetNumber:num];
	    return 10.0f;
	}
    void subIconBadgeNumber()
    {
        //UIViewController *vc = [[YourViewController alloc] initWithNibName:@"yourViewControllerName" //bundle:nil];
        //[[UnityGetMainWindow() rootViewController] presentViewController:vc animated:YES completion:nil];
        printf_console("-> subIconBadgeNumber()\n");
        if (myPluginSDK == nil)
        {
            myPluginSDK = [[PluginSDK alloc]init];
        }
        [myPluginSDK SubIconBadgeNumber];
    }
#if defined(__cplusplus)
}
#endif

