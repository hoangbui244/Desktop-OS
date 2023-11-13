//
//  LibraryAnalytics.mm
//  Unity-iPhone
//
//  Created by 张聪 on 2017/11/3.
//

#import <Foundation/Foundation.h>
#import "../ChannelPlugin/PluginWrapper.h"

//export c language interface to unity3d
#if defined(__cplusplus)
extern "C"{
#endif
    //事件追踪
    extern void track_event(const char * eventType, const char * eventName, const char * eventParam)
    {
        [PluginWrapper track_event:[NSString stringWithUTF8String:eventType] eventName:[NSString stringWithUTF8String:eventName] eventParam:[NSString stringWithUTF8String:eventParam]];
    }
#if defined(__cplusplus)
}
#endif
