//
//  LibraryIAP.mm
//  Unity-iPhone
//
//  Created by 张聪 on 2017/10/20.
//

#import <Foundation/Foundation.h>
#import "../ChannelPlugin/PluginWrapper.h"

//export c language interface to unity3d
#if defined(__cplusplus)
extern "C"{
#endif
    //支付(商品ID, 商品名, 商品描述, 价格, 扩展字段)
    extern void pay(const char * productID, const char * productName, const char * productDesc, float price, const char * ext)
    {
        [PluginWrapper pay:[NSString stringWithUTF8String:productID]
                             productName:[NSString stringWithUTF8String:productName]
                             productDesc:[NSString stringWithUTF8String:productDesc]
                             price:price
                             ext:[NSString stringWithUTF8String:ext]];
    }
#if defined(__cplusplus)
}
#endif
