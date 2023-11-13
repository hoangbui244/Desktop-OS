//
//  LibraryFacebook.mm
//  Unity-iPhone
//
//  Created by 郭鹏杰 on 2017/12/1.
//

#import <Foundation/Foundation.h>
#import "../ChannelPlugin/PluginWrapper.h"

//export c language interface to unity3d
#if defined(__cplusplus)
extern "C"{
#endif
    //
    extern void auth_social_account()
    {
        [PluginWrapper auth_social_account];
    }
    extern void share(int shareType, const char * shareURL, const char * sharePictureURL, const char * shareTitle, const char * shareContent)
    {
        [PluginWrapper share:shareType
                            shareURL:[NSString stringWithUTF8String:shareURL]
                            sharePictureURL:[NSString stringWithUTF8String:sharePictureURL]
                            shareTitle:[NSString stringWithUTF8String:shareTitle]
                            shareContent:[NSString stringWithUTF8String:shareContent]];
    }
    extern void share_local_picture(int shareType, const char * picturePath)
    {
         [PluginWrapper share_local_picture:shareType
                            picturePath:[NSString stringWithUTF8String:picturePath]];
    }
    extern void request_invitable_friends()
    {
        [PluginWrapper request_invitable_friends];
    }
    extern const char * get_invitable_friend_info(int index)
    {
        NSString* result = [PluginWrapper get_invitable_friend_info:index];
        return strdup([result UTF8String]);
    }
    extern void request_playing_friends()
    {
        [PluginWrapper request_playing_friends];
    }
    extern const char * get_playing_friend_info(int index)
    {
        NSString* result = [PluginWrapper get_playing_friend_info:index];
        return strdup([result UTF8String]);
    }
    
#if defined(__cplusplus)
}
#endif
