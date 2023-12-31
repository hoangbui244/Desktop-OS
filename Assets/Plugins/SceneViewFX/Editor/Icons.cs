﻿using UnityEngine;

public class Icons : MonoBehaviour {


	private static Texture2D iconRefresh;

	public static Texture2D IconRefresh {
		get {
			if (iconRefresh == null) {
				iconRefresh = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
				iconRefresh.LoadImage(refreshIcon);
			}
			return iconRefresh;
		}
	}

	private static Texture2D iconRefreshPersonal;
	public static Texture2D IconRefreshPersonal {
		get {
			if (iconRefreshPersonal == null) {
				iconRefreshPersonal = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
				iconRefreshPersonal.LoadImage(refreshPersonalSkin);
			}
			return iconRefreshPersonal;
		}
	}
	private static Texture2D iconGoto;
	public static Texture2D IconGoto {
		get {
			if (iconGoto == null) {
				iconGoto = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
				iconGoto.LoadImage(gotoIcon);
			}
			return iconGoto;
		}
	}
	private static Texture2D iconGotoPersonal;
	public static Texture2D IconGotoPersonal {
		get {
			if (iconGotoPersonal == null) {
				iconGotoPersonal = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
				iconGotoPersonal.LoadImage(gotoPersonalSkin);
			}
			return iconGotoPersonal;
		}
	}


	#region byte arrays for image data.
	public static byte[] refreshPersonalSkin = new byte[] {
	                                                      	0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A,0x00,0x00,0x00,0x0D,0x49,0x48,0x44,0x52,0x00,0x00,0x00,0x0B,0x00,0x00,0x00,0x0D,0x08,0x06,0x00,0x00,0x00,0x7F,0xF5,0x94,0x3B,0x00,0x00,0x00,0xFA,0x49,0x44,0x41,0x54,0x28,0x15,0x8D,0xD1,0xBF,0x4B,0x42,0x51,0x18,0x87,0xF1,0x63,0xA9,0x50,0x20,0xB8,0xD4,0x7C,0x89,0xA0,0x3D,0x6A,0x3F,0x82,0xFF,0x82,0x6B,0xD4,0x52,0xA3,0x81,0x50,0x43,0x4B,0xB4,0x36,0x04,0x2D,0xD1,0x76,0x57,0x57,0xE7,0x5A,0x1C,0x6C,0xA8,0x5C,0x02,0x27,0xF7,0xA6,0x5A,0x24,0x87,0xA4,0xF2,0x79,0x2E,0x5D,0xC8,0x42,0xE8,0x0B,0x1F,0xCF,0xBD,0xF7,0x7D,0x3D,0x9C,0x1F,0x85,0x18,0x63,0xF8,0x4E,0x81,0x71,0x15,0x27,0x38,0xC0,0x05,0x2E,0xF1,0x8C,0x2F,0x84,0xA2,0x3F,0x64,0x09,0x0D,0xEC,0x61,0x0B,0x65,0x1C,0xE3,0x13,0xA7,0x78,0x47,0x58,0x40,0x09,0x47,0xB8,0xC6,0x08,0x8F,0xB0,0xE9,0x16,0x37,0xC8,0x1A,0x19,0xC3,0x62,0x92,0x24,0x75,0xC6,0x2B,0x9C,0x63,0x1F,0xFE,0xE1,0x05,0x87,0x78,0x82,0x4B,0x5B,0xC6,0xD8,0x65,0xEC,0x60,0x80,0x33,0x98,0x1E,0x3A,0x98,0xF8,0x42,0x5C,0x8E,0x69,0xD9,0xBC,0x81,0x3B,0xE4,0x45,0x37,0xF4,0x33,0x6B,0xBC,0xAC,0xFB,0xC1,0x35,0x7F,0xC0,0x0D,0xCD,0xCB,0x2B,0x85,0x37,0x8B,0x36,0x3F,0xA0,0x86,0x15,0xFC,0x8E,0xEB,0x8D,0xB8,0xB7,0x60,0x73,0x8A,0x0A,0xDC,0xA4,0xC5,0x3C,0x3E,0xFB,0xCD,0x5A,0x8A,0xEC,0x9C,0x3D,0xAA,0x26,0xBC,0x84,0x2E,0xB2,0x59,0x18,0xB7,0x51,0x85,0xB5,0x3E,0xB2,0x66,0x6F,0xA7,0x8D,0x21,0x76,0xB1,0x09,0xE3,0x39,0xA7,0x70,0xB2,0x99,0x1B,0xF4,0x12,0x9C,0x31,0x9F,0x95,0xC7,0xBF,0x71,0xCD,0xFF,0xCE,0x14,0x7D,0x73,0x33,0x2C,0xC4,0x84,0x4D,0x06,0x00,0x00,0x00,0x00,0x49,0x45,0x4E,0x44,0xAE,0x42,0x60,0x82,
	                                                      };
	public static byte[] refreshIcon = new byte[] {
	                                              	0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A,0x00,0x00,0x00,0x0D,0x49,0x48,0x44,0x52,0x00,0x00,0x00,0x0B,0x00,0x00,0x00,0x0D,0x08,0x06,0x00,0x00,0x00,0x7F,0xF5,0x94,0x3B,0x00,0x00,0x00,0xF8,0x49,0x44,0x41,0x54,0x28,0x15,0x8D,0xD1,0xB1,0x4B,0x02,0x61,0x18,0xC7,0xF1,0x53,0x52,0x70,0x10,0x5A,0xEC,0x0F,0x88,0xA0,0x5D,0x72,0xD7,0xBF,0x41,0x5C,0xA3,0xA6,0x46,0x87,0xA0,0xA0,0x10,0xA2,0xB6,0x96,0xC0,0x45,0xDC,0x6E,0x6D,0x6D,0x56,0x0E,0x1A,0x6C,0xD0,0xDA,0x6C,0x6A,0x2F,0x08,0x5A,0xC2,0xC5,0x28,0xFB,0x7E,0x5F,0x3A,0xD0,0x44,0xF0,0x07,0x9F,0x7B,0xEF,0xEE,0x79,0x78,0x79,0xEE,0xBD,0x4C,0x92,0x24,0xD1,0x5F,0x32,0xAC,0x5B,0x38,0xC7,0x11,0x6E,0xD0,0xC6,0x2B,0x66,0x88,0x36,0xBC,0x90,0x02,0x1A,0x38,0xC4,0x1E,0xF2,0x38,0xC5,0x0F,0x2E,0x30,0x45,0x94,0x45,0x0E,0x27,0xE8,0xE2,0x13,0x8F,0xB0,0xA9,0x8F,0x1E,0x42,0x23,0x6B,0xD8,0xB9,0xC6,0x7A,0x86,0x6B,0xB4,0x50,0xC7,0x18,0x57,0x78,0x83,0xA3,0x39,0xC6,0xBB,0x63,0xEC,0xE3,0x19,0x97,0x30,0x03,0xDC,0xE1,0xCB,0x07,0xE2,0x38,0xE6,0xD8,0xE6,0x5D,0x3C,0x20,0x2D,0xFA,0x41,0xF3,0xD9,0xE6,0x61,0xC7,0x17,0xCE,0xFC,0x0D,0x3F,0x68,0x55,0x3E,0x28,0x4C,0x2C,0xDA,0x3C,0x82,0x73,0x97,0xF0,0x3F,0xCE,0x5B,0xC5,0xD0,0x82,0xCD,0x31,0x8A,0xE8,0xC0,0x62,0x1A,0xEF,0x7D,0x67,0x2D,0x46,0x38,0x0D,0x8F,0xAA,0x09,0x7F,0xC2,0x3D,0xC2,0x2E,0xAC,0x15,0x6C,0xC2,0xDA,0x13,0x42,0xB3,0xC7,0x72,0x8B,0x17,0x1C,0xA0,0x0C,0xE3,0x39,0xC7,0x70,0xB3,0x85,0x3F,0xE8,0x4F,0x70,0xC7,0x74,0x57,0x6E,0x97,0xE3,0xCC,0x6B,0xE7,0x17,0xAC,0xE5,0x34,0x9A,0xF6,0x88,0x15,0x9A,0x00,0x00,0x00,0x00,0x49,0x45,0x4E,0x44,0xAE,0x42,0x60,0x82,
	                                              };

	public static byte[] gotoIcon = new byte[] {
	                                         	0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A,0x00,0x00,0x00,0x0D,0x49,0x48,0x44,0x52,0x00,0x00,0x00,0x0D,0x00,0x00,0x00,0x0E,0x08,0x06,0x00,0x00,0x00,0xF4,0x7F,0x96,0xD2,0x00,0x00,0x01,0x42,0x49,0x44,0x41,0x54,0x28,0x15,0x7D,0xD2,0x4B,0x2B,0x84,0x61,0x14,0xC0,0x71,0x23,0xA1,0xDC,0x36,0x48,0x2E,0x25,0x16,0x36,0x2E,0xB1,0x30,0x59,0x59,0xD8,0x89,0x6C,0x24,0x91,0xA5,0x8F,0x60,0x63,0x63,0x65,0x27,0x1F,0x42,0xB1,0xB1,0x42,0x76,0xEA,0xB5,0x91,0x9D,0x72,0x57,0x43,0x44,0xB9,0x94,0xA4,0x91,0x28,0x33,0x5E,0xFF,0xFF,0x9B,0x47,0xB2,0x70,0xEA,0xD7,0xF3,0x3E,0x6F,0xE7,0xCC,0x9C,0x73,0x66,0x52,0x51,0x14,0x15,0xFC,0x89,0x14,0xF7,0x2A,0x14,0x22,0x8B,0x1C,0x8C,0x5E,0x8C,0x60,0xBE,0xC8,0xDB,0x77,0x14,0x73,0xF6,0xA1,0x13,0xBE,0x8F,0x61,0xA1,0x9F,0xFA,0x8E,0x15,0xB4,0x22,0x0E,0x45,0x15,0x5C,0x26,0x61,0xD2,0x16,0x1E,0x60,0x51,0x19,0x7A,0xB0,0x08,0x0B,0x2E,0xB0,0x6E,0x91,0x89,0x63,0x78,0xC6,0x2A,0x4C,0xFE,0x84,0x51,0x83,0x05,0x58,0x70,0x8E,0x41,0x64,0x2C,0xEA,0x42,0x39,0x96,0x90,0xC7,0x2C,0xAE,0xB1,0x87,0x0D,0xB4,0xE0,0x0C,0x33,0xB0,0xA3,0xA4,0xF7,0x36,0xCE,0x43,0x7C,0xF8,0x82,0xE8,0xC6,0x1C,0x1E,0x51,0x8F,0x0C,0x86,0xF1,0x8A,0x09,0x1C,0xD8,0x9A,0xD5,0xB7,0x30,0xBC,0xD7,0xC2,0xA5,0x58,0x70,0x83,0x21,0xD8,0xDA,0x1B,0xEC,0xA4,0xD2,0x24,0x67,0x08,0x0B,0x71,0xD5,0x8D,0x08,0xD1,0xC0,0x83,0xAB,0xF6,0x67,0x30,0x3C,0xF3,0x26,0x3F,0xA1,0x19,0x47,0xA8,0x46,0x29,0x9C,0xE1,0x0E,0xF7,0x68,0x82,0x79,0x75,0xC8,0x21,0xEB,0x65,0x1F,0xA3,0xD8,0xC1,0x15,0xFA,0xE1,0x3C,0x6E,0x33,0x84,0xDF,0x30,0x00,0x73,0x63,0xDB,0x73,0xF7,0xA7,0x98,0x82,0xF3,0xD9,0xFF,0xEF,0x82,0x12,0xEE,0xE3,0x70,0x9E,0x5D,0xFC,0xCC,0xB2,0xC9,0xB3,0x03,0x4F,0xE3,0x04,0x97,0x30,0xC9,0x99,0xDA,0xE1,0xDF,0x69,0x19,0xC9,0x86,0xC3,0x02,0x4C,0x58,0xC3,0x31,0x3A,0x90,0x86,0x5D,0xBC,0x60,0x1B,0x49,0x5B,0x9C,0x49,0x84,0xA2,0x70,0xB7,0x35,0xFD,0x1B,0x5F,0x3A,0xD3,0x4E,0x6C,0x7A,0x54,0x51,0xAF,0x00,0x00,0x00,0x00,0x49,0x45,0x4E,0x44,0xAE,0x42,0x60,0x82,
	                                         };
	public static byte[] gotoPersonalSkin = new byte[] {
	                                                   	0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A,0x00,0x00,0x00,0x0D,0x49,0x48,0x44,0x52,0x00,0x00,0x00,0x0D,0x00,0x00,0x00,0x0E,0x08,0x06,0x00,0x00,0x00,0xF4,0x7F,0x96,0xD2,0x00,0x00,0x01,0x44,0x49,0x44,0x41,0x54,0x28,0x15,0x7D,0xD1,0xCB,0x2B,0x44,0x71,0x14,0xC0,0xF1,0x19,0x84,0xF2,0x2A,0xEF,0x3C,0xEA,0xC6,0xC2,0xCA,0x73,0x41,0x56,0x6E,0x59,0x28,0x8F,0x6C,0x24,0x91,0x3F,0xC3,0xC6,0xC6,0xCA,0x4E,0xFE,0x08,0x0B,0x29,0x2B,0x6C,0x15,0x1B,0xD9,0x29,0xAF,0xA8,0xA1,0x44,0x79,0x94,0xA4,0x91,0x28,0x33,0xAE,0xEF,0x77,0x72,0x25,0x0B,0xA7,0x3E,0xFD,0xE6,0xDC,0xCE,0x99,0x7B,0x7E,0xE7,0x26,0xC3,0x30,0x4C,0xFC,0x89,0x24,0x79,0x05,0xF2,0x90,0x46,0x06,0x46,0x2F,0xC6,0xB1,0x58,0x60,0xF6,0x1D,0x85,0x9C,0xFD,0xE8,0x80,0xCF,0x23,0xD8,0xB8,0x83,0x77,0xAC,0xA2,0x15,0x51,0xDC,0x54,0x46,0x32,0x03,0x8B,0xB6,0xF1,0x00,0x9B,0x4A,0xD0,0x83,0x65,0xD8,0x70,0x89,0x4D,0x9B,0x2C,0x9C,0xC4,0x33,0xD6,0x61,0xF1,0x27,0x8C,0x1A,0x2C,0xC1,0x86,0x0B,0x0C,0x23,0x65,0x53,0x27,0x4A,0xB1,0x82,0x2C,0xE6,0x71,0x8D,0x03,0x6C,0xA1,0x05,0xE7,0x98,0x83,0x13,0xE5,0x66,0x6F,0xE3,0x3C,0xC6,0x87,0x0F,0x88,0x6E,0x2C,0xE0,0x11,0x0D,0x48,0x61,0x0C,0xAF,0x98,0xC6,0x91,0xA3,0xD9,0x7D,0x0B,0xC3,0xBC,0x16,0x2E,0xC5,0x86,0x1B,0x8C,0xC2,0xD1,0xDE,0xE0,0x24,0xE5,0x16,0x79,0x87,0x78,0x21,0xAE,0xBA,0x09,0x71,0x34,0xF2,0xC3,0x55,0xFB,0x19,0x0C,0xCF,0xAC,0xC5,0x4F,0x08,0x70,0x82,0x6A,0x14,0xC3,0x3B,0xDC,0xE1,0x1E,0xCD,0xB0,0xAE,0x1E,0x19,0xA4,0x4D,0x0E,0x31,0x81,0x3D,0x5C,0x61,0x00,0xDE,0xC7,0x6D,0xC6,0xE1,0x1B,0x06,0x61,0x6D,0xE4,0x78,0xEE,0xFE,0x0C,0xB3,0xF0,0x7E,0xCE,0xFF,0xBB,0xA1,0x88,0x7C,0x0A,0xDE,0x67,0x1F,0x89,0xFC,0x20,0x08,0x3C,0x2D,0xAC,0xC3,0x10,0x2A,0xE1,0x22,0xAA,0xD0,0x85,0x11,0xF8,0xDD,0xD6,0xE0,0x32,0x7E,0x16,0xE0,0xBF,0x6C,0xE0,0x14,0xED,0xE8,0x83,0x53,0xBC,0x60,0x17,0xB9,0xB1,0x38,0x73,0x11,0x6F,0x2D,0xCE,0x7D,0xA3,0xFE,0x8D,0x2F,0xD9,0x1E,0x4A,0xDA,0xBC,0x0F,0xF7,0x22,0x00,0x00,0x00,0x00,0x49,0x45,0x4E,0x44,0xAE,0x42,0x60,0x82,
	                                                   };
	#endregion
}