//
//  LibraryExtension.mm
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/25.
//

#import <Foundation/Foundation.h>
#import "../ChannelPlugin/PluginWrapper.h"

//export c language interface to unity3d
#if defined(__cplusplus)
extern "C"{
#endif
    //根据函数名及相关参数列表, 尝试调用渠道插件中符合相应函数签名的函数, 如果无此函数功能, 则不做处理返回默认值即可
    extern void callFuncWithParam(const char * method, const char * param)
    {
        [PluginWrapper callFuncWithParam:[NSString stringWithUTF8String:method] param:[NSString stringWithUTF8String:param]];
    }
    extern int callIntFuncWithParam(const char * method, const char * param)
    {
        return [PluginWrapper callIntFuncWithParam:[NSString stringWithUTF8String:method]
                                             param:[NSString stringWithUTF8String:param]];
    }
    extern float callFloatFuncWithParam(const char * method, const char * param)
    {
        return [PluginWrapper callFloatFuncWithParam:[NSString stringWithUTF8String:method] param:[NSString stringWithUTF8String:param]];
    }
    extern bool callBoolFuncWithParam(const char * method, const char * param)
    {
        return [PluginWrapper callBoolFuncWithParam:[NSString stringWithUTF8String:method] param:[NSString stringWithUTF8String:param]];
    }
    extern const char * callStringFuncWithParam(const char * method, const char * param)
    {
        NSString* result = [PluginWrapper callStringFuncWithParam:[NSString stringWithUTF8String:method] param:[NSString stringWithUTF8String:param]];
        return strdup([result UTF8String]);
    }
#if defined(__cplusplus)
}
#endif

