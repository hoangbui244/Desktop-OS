using System;
using System.Runtime.InteropServices;
public class ChannelLibrary 
{
#if UNITY_IOS && !UNITY_EDITOR
	const string libFile = "__Internal";
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void init ([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string listener);
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void setDebugLog (bool enable);
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern string get_channel ();
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void login ();
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool is_logined();
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool auth_login(int serverID, [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string authLoginURL);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void switch_account ();
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void logout ();
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void enter_game (int serverID, 
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string serverName, 
		[InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string userID,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string roleID,
		[InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string roleName, 
        int roleLevel, int roleVipLevel, bool isCreate);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void update_role_info([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string roleName,
        int roleLevel, int roleVipLevel);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void exit_game ();
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void user_platform ();
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void customer_service ([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string ext);
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void show_float_view (int location);
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void hide_float_view ();
	[DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
	public static extern void pay ([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string productID,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string productName,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string productDesc,
        float price,  
		[InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string ext);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern string track_event([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string eventType,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string eventName,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string eventParam);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern string get_social_system();
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void callFuncWithParam([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string method,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string param);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern int callIntFuncWithParam([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string method,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string param);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern float callFloatFuncWithParam([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string method,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string param);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern bool callBoolFuncWithParam([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string method,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string param);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern string callStringFuncWithParam([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string method,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string param);
    //SDKFaceBook
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void auth_social_account();
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void share(int shareType,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string shareURL,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string sharePictureURL,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string shareTitle,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string shareContent);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void share_local_picture(int shareType,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string picturePath);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void request_invitable_friends();
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern string get_invitable_friend_info(int index);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void invite_friend([InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string inviteMessage,
        [InAttribute()] [MarshalAsAttribute(UnmanagedType.LPStr)] string indexList);
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern void request_playing_friends();
    [DllImport(libFile, CallingConvention = CallingConvention.Cdecl)]
    public static extern string get_playing_friend_info(int index);
#endif
}
